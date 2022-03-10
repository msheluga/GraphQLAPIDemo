using GraphQLAPIDemo.Data;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;

namespace GraphQLAPIDemo.Extensions
{
    public class RequestValidationMiddleware
    {
        private readonly HotChocolate.Execution.RequestDelegate _next;       
        private const string auth = "authorization";
        public RequestValidationMiddleware(HotChocolate.Execution.RequestDelegate next)
        {
            _next = next;          
        }

        public async Task InvokeAsync(HttpContext httpContext, IDbContextFactory<BooksContext> dbContextFactory)
        {
            var appId = httpContext?.Request?.Headers?.FirstOrDefault(x => x.Key.Trim().ToLower().Equals(auth)).Value.ToString();
            if (appId == null)
            {
                return;
            }
            var context = dbContextFactory.CreateDbContext();
            await _next((IRequestContext)httpContext);
        }

      

    }
}