using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using GraphQLAPIDemo.Listener;
using System.Diagnostics;

namespace GraphQLAPIDemo.Query
{
    public class Query
    {
        

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Book> GetBooks([Service] BooksContext context) =>
            context.Book;
    }
}
