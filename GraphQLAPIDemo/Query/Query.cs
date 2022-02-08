using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using GraphQLAPIDemo.Listener;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GraphQLAPIDemo.Query
{
    public partial class Query
    {
        public static readonly ActivitySource MyActivitySource = new("GraphQLAPIDemo");

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        
        public async Task<IQueryable<Book>> GetBooks([Service] IDbContextFactory<BooksContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            using var myActivity = MyActivitySource.StartActivity("Books");           
            myActivity?.AddEvent(new("Custom Log Event Books"));
            return context.Books; 
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        
        public async Task<IQueryable<Address>> GetAddresses([Service] IDbContextFactory<BooksContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            using var myActivity = MyActivitySource.StartActivity("Books");
            myActivity?.AddEvent(new("Custom Log Event Addresses"));
            return context.Addresses;
        }


        [UseProjection]
        [UseFiltering]
        [UseSorting]
       
        public async Task<IQueryable<Address>> GetAddressesById(Guid Id, [Service] IDbContextFactory<BooksContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            using var myActivity = MyActivitySource.StartActivity("Books");
            myActivity?.AddEvent(new("Custom Log Event Addresses"));
            return context.Addresses.Where(x => Id.Equals(x));
        }
    }
}
