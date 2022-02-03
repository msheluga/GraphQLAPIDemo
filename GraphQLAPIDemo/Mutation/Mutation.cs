using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;

namespace GraphQLAPIDemo.Mutation
{
    public partial class Mutation
    {        
        
        public async Task<Book> AddBook([Service] BooksContext context, Book input)
        {
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
