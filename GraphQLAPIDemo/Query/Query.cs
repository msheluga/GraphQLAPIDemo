using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using GraphQLAPIDemo.Listener;
using HotChocolate.Data.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GraphQLAPIDemo.Query
{
    public class Query
    {
        public static readonly ActivitySource MyActivitySource = new("GraphQLAPIDemo");

        [UseProjection]
        [UseFiltering]
        [UseSorting]

        public IQueryable<Book> GetBooks([Service] IDbContextFactory<BooksContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            using var myActivity = MyActivitySource.StartActivity("Books");
            myActivity?.AddEvent(new("Custom Log Event Books"));
            return context.Books;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("This Query gets Addresses")]
        public IQueryable<Address> GetAddresses([Service] IDbContextFactory<BooksContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            using var myActivity = MyActivitySource.StartActivity("Books");
            myActivity?.AddEvent(new("Custom Log Event Addresses"));
            return context.Addresses;
        }

        /// <summary>
        /// example of how to filter by id query
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id">the incoming ID, has to be lowercase due to Graphql mapping</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [UseFiltering]        
        public Address GetAddressesById([Service] IDbContextFactory<BooksContext> dbContextFactory, Guid id)
        {
            if (id == new Guid())
            {
                throw new Exception("ID cannot be null");
            }
            var context = dbContextFactory.CreateDbContext();
            using var myActivity = MyActivitySource.StartActivity("Books");
            myActivity?.AddEvent(new("Custom Log Event Addresses"));
            return context.Addresses.Where(x => id.Equals(x.Id)).SingleOrDefault();
        }
    }
}