using AutoMapper;
using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using GraphQLAPIDemo.UpdateModels;
using HotChocolate.AspNetCore.Authorization;

namespace GraphQLAPIDemo
{
    public class Mutation : MutationBase
    {
        [Authorize(Policy = "Book")]
        public async Task<Book?> UpdateBook([Service] BooksContext context, [Service] IMapper mapper, BookUpdate input)
        {
            return await UpdateEntity<Book>(context, mapper, input, input.Id);
        }

        [Authorize(Policy = "Author")]
        public async Task<Author?> UpdateAuthor([Service] BooksContext context, AuthorUpdate input)
        {
            return await UpdateEntity<Author>(context, input, input.Id);
        }
    }
}
