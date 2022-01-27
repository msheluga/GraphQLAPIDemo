using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Query;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Hosting;
using OpenTelemetry.Instrumentation.AspNetCore;
using GraphQLAPIDemo.Listener;

var builder = WebApplication.CreateBuilder(args);
   
var dbString = builder.Configuration.GetConnectionString("BookDatabase");

//builder.Host.UseSerilog((ctx, lc) => lc
//    .MinimumLevel.Information()
//.WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day));

builder.Logging.ClearProviders();

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
    .AddInstrumentation();




builder.Services.AddOpenTelemetryTracing(
    b =>
    {
        b.AddHttpClientInstrumentation();
        b.AddAspNetCoreInstrumentation();
        b.AddHotChocolateInstrumentation();
        //b.AddJaegerExporter();
        b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GraphQLAPIDemo"));
        //b.AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:6831"));
        b.AddConsoleExporter();
        b.AddFileExporter();
    }); 

builder.Logging.AddOpenTelemetry(
    b =>
    {   
        b.IncludeFormattedMessage = true;
        b.IncludeScopes = true;
        b.ParseStateValues = true;        
    });


builder.Services.Configure<AspNetCoreInstrumentationOptions>(options =>
{
    options.RecordException = true;
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