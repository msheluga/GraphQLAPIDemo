using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;

namespace GraphQLAPIDemo.Mutation
{
    public partial class Mutation
    {
        private readonly BooksContext context;

        public Mutation(BooksContext booksContext)
        {
            this.context = booksContext;
        }
        public record InputBookPayLoad(string Isbn, string Title, string Author, Guid AddressId, Guid PressId);

        public record CreateBookPayLoad(Book book);
        public async Task<CreateBookPayLoad> AddBook(InputBookPayLoad input)
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

            context.Book.Add(book);
            await context.SaveChangesAsync();
            return new CreateBookPayLoad(book);
        }
    }
}
