using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using GraphQLAPIDemo.Listener;
using System.Diagnostics;

namespace GraphQLAPIDemo.Query
{
    public partial class Query
    {
        public static readonly ActivitySource MyActivitySource = new("GraphQLAPIDemo");

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Book> GetBooks([Service] BooksContext context)
        {
            using var myActivity = MyActivitySource.StartActivity("Books");           
            myActivity?.AddEvent(new("Custom Log Event Books"));
            return context.Books; 
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Address> GetAddresses([Service] BooksContext context)
        {
            using var myActivity = MyActivitySource.StartActivity("Books");
            myActivity?.AddEvent(new("Custom Log Event Addresses"));
            return context.Addresses;
        }
    }
}
