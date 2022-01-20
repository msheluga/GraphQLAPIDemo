using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Query;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);
var dbString = builder.Configuration.GetConnectionString("BookDatabase");
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
        b.AddConsoleExporter();
    });

builder.Logging.AddOpenTelemetry(
    b =>
    {
        b.IncludeFormattedMessage = true;
        b.IncludeScopes = true;
        b.ParseStateValues = true;
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