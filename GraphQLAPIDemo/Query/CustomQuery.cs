using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using System.Diagnostics;

public partial class Query
{
    public static readonly ActivitySource MyActivitySource = new("GraphQLAPIDemo");

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Tables> GetTables([Service] BooksContext context)
    {
        using var myActivity = MyActivitySource.StartActivity("Books");
        myActivity?.AddEvent(new("Custom Log Event Books"));
        return context.Tables;
    }
}