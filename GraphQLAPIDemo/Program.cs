using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Query;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var dbString = builder.Configuration.GetConnectionString("BookDatabase");
builder.Services.AddDbContextFactory<BooksContext>(opt =>
                opt.UseSqlServer(dbString));
builder.Services.AddScoped<BooksContext>(sp => 
    sp.GetRequiredService<IDbContextFactory<BooksContext>>().CreateDbContext());
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();
// Add services to the container.

var app = builder.Build();
app.UseRouting();
app.MapGraphQL();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Run();