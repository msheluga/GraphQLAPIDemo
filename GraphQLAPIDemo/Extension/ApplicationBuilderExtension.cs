

namespace GraphQLAPIDemo.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestValidation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestValidationMiddleware>();
        }
    }
    
}
