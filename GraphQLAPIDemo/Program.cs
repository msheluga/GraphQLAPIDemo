using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Query;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Hosting;
using OpenTelemetry.Instrumentation.AspNetCore;
using GraphQLAPIDemo.Listener;
using HotChocolate.Diagnostics;
using GraphQLAPIDemo.Mutation;

var builder = WebApplication.CreateBuilder(args);

var dbString = builder.Configuration.GetConnectionString("BookDatabase");

builder.Logging.ClearProviders();
builder.Services.AddLogging();
builder.Services.AddHealthChecks();

builder.Services.AddPooledDbContextFactory<BooksContext>(opt =>
                opt.UseSqlServer(dbString));
builder.Services.AddScoped<BooksContext>(sp =>
    sp.GetRequiredService<IDbContextFactory<BooksContext>>().CreateDbContext());
builder.Services.AddHealthChecks();
builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddInstrumentation(
    o =>
    {
        o.Scopes = ActivityScopes.All;
    })
    .AddDiagnosticEventListener<MyListener>()
    .AddDefaultTransactionScopeHandler()
    .AddMutationConventions();

builder.Services.AddOpenTelemetryTracing(
    b =>
    {
        b.AddHttpClientInstrumentation();
        b.AddAspNetCoreInstrumentation();
        b.AddHotChocolateInstrumentation();
        b.AddSource("GraphQLAPIDemo");
        b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GraphQLAPIDemo"));
        b.AddConsoleExporter();
        b.AddFileExporter();
    });

builder.Logging.AddOpenTelemetry(
    b =>
    {
        b.IncludeFormattedMessage = true;
        b.IncludeScopes = true;
        b.ParseStateValues = true;
        b.AddFileExporter();
    });


builder.Services.Configure<AspNetCoreInstrumentationOptions>(options =>
{
    options.RecordException = true;
    options.Enrich = (activity, eventName, rawObject) =>
    {
        if (eventName.Equals("OnStartActivity"))
        {
            if (rawObject is HttpRequest httpRequest)
            {
                activity.SetTag("requestProtocol", httpRequest.Protocol);
                activity.SetTag("remote", httpRequest.HttpContext.Connection.RemoteIpAddress);
                activity.AddEvent(new(string.Format("Http remote {0}", httpRequest.HttpContext.Connection.RemoteIpAddress)));
                activity.AddEvent(new(String.Format("Http request {0}", httpRequest.HttpContext.Request.ToString())));
            }
        }
        else if (eventName.Equals("OnStopActivity"))
        {
            if (rawObject is HttpResponse httpResponse)
            {
                activity.SetTag("responseLength", httpResponse.ContentLength);
                activity.SetTag("response", httpResponse.HttpContext.Response.ToString());
                activity.AddEvent(new(string.Format("Http response length {0}", httpResponse.ContentLength)));
                activity.AddEvent(new(String.Format("Http response {0}", httpResponse.HttpContext.Response.ToString())));
            }
        }    
    };
});


// Add services to the container.

var app = builder.Build();
app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});


app.Run();