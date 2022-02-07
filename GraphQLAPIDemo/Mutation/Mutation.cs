using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLAPIDemo.Mutation
{
    public partial class Mutation
    {
        private readonly IDbContextFactory<BooksContext> dbContextFactory;

        public Mutation(IDbContextFactory<BooksContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
        public record InputBookPayLoad(string Isbn, string Title, string Author, Guid AddressId, Guid PressId);
        public async Task<Book> AddBook(InputBookPayLoad input)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Isbn = input.Isbn,
                Title= input.Title,
                Author = input.Author,
                AddressId = input.AddressId,
                PressId = input.PressId
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();
            return book;
        }
    }
}
