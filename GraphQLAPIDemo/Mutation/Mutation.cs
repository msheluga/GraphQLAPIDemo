﻿using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GraphQLAPIDemo.Mutation
{
    public partial class Mutation
    {  
        public record InputBookPayLoad(string Isbn, string Title, string Author, decimal Price, Guid AddressId, Guid PressId);
        public record EditBookPayload(Guid Id, string Isbn, string Title, string Author, decimal Price, Guid AddressId, Guid PressId);  

        public record PatchObject(string op, string path, object value);
        
        [Authorize]
        public async Task<Book> AddBook([Service] IDbContextFactory<BooksContext> dbContextFactory, InputBookPayLoad input)
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
        [Authorize]
        public async Task<Book> EditBook([Service] IDbContextFactory<BooksContext> dbContextFactory, EditBookPayload edit)
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
                context.Books.Update(book);            
                await context.SaveChangesAsync();
                return book;
            }

            return new Book();
        }
        [Authorize]
        public async Task<Book> PatchBook([Service] IDbContextFactory<BooksContext> dbContextFactory, string patch, Guid id)
        {
            if (String.IsNullOrEmpty(patch))
            {
                throw new Exception("no patch data present");
            }

            var book = new Book();
            var context = dbContextFactory.CreateDbContext();
            book = await context.Books.FindAsync(id);
            if (book != null)
            {
                List<PatchObject>? patchObjects = JsonConvert.DeserializeObject<List<PatchObject>>(patch);
                if (patchObjects != null)
                {
                    if (patchObjects.Count > 0)
                    {
                        //generate transaction scope amd then commit                        
                        foreach (PatchObject patchObject in patchObjects)
                        {
                            var patchDocument = new JsonPatchDocument();
                            switch (patchObject.op.ToLower())
                            {
                                case "add":
                                    patchDocument.Add(patchObject.path, patchObject.value);
                                    break;
                                case "replace":
                                    patchDocument.Replace(patchObject.path, patchObject.value);
                                    break;
                                case "remove":
                                    patchDocument.Remove(patchObject.path);
                                    break;
                                //not supporting Copy or Test
                                default:
                                    break;
                            }
                                                                
                            patchDocument.ApplyTo(book);
                            await context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        throw new Exception("no data found");
                    }
                }               
            }
            else
            {
                throw new Exception("no data found");
            }
            return book;
        }
    }    
}
