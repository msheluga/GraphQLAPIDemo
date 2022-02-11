using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Mvc.Filters;

public class MinimumPermissionHandler : IAuthorizationHandler
{
    public ValueTask<AuthorizeResult> AuthorizeAsync(IMiddlewareContext context, AuthorizeDirective directive)
    {
        if(context.Operation.Operation.ToString().ToLower() == "query")
        {

        }
        throw new NotImplementedException();
    }
}