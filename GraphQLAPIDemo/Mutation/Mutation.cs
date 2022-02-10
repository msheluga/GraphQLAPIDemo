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
        public record InputBookPayLoad(string Isbn, string Title, string Author, decimal Price, Guid AddressId, Guid PressId);
        public record EditBookPayload(Guid Id, string Isbn, string Title, string Author, decimal Price, Guid AddressId, Guid PressId);       
        public async Task<Book> AddBook(InputBookPayLoad input)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Isbn = input.Isbn,
                Title= input.Title,
                Author = input.Author,
                Price = input.Price,
                AddressId = input.AddressId,
                PressId = input.PressId
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> EditBook(EditBookPayload edit)
        {
            var context = dbContextFactory.CreateDbContext();
            if (edit.Id == new Guid() || edit.Id == Guid.Empty)
            {
                throw new ArgumentException("Id not valid");
            }

            var book = await context.Books.FindAsync(edit.Id);
            if (book != null)
            {
                book.Isbn = edit.Isbn;
                book.Title = edit.Title;
                book.Author = edit.Author;
                book.Price = edit.Price;
                book.AddressId = edit.AddressId;
                book.PressId = edit.PressId;
            }
            context.Books.Update(book);            
            await context.SaveChangesAsync();
            return book;
        }
    }
}
