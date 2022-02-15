using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using System.Security.Claims;

public class HttpRequestInterceptor: DefaultHttpRequestInterceptor
{
    public override ValueTask OnCreateAsync(HttpContext context,
        IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        var identity = context.User.Claims.Select(x=>x.Type == "author");
        //var identity = new ClaimsIdentity();
        //identity.AddClaim(new Claim(ClaimTypes.Country, "us"));

        //context.User.AddIdentity(identity);

        if(identity == null )
        {
            throw new ArgumentNullException(nameof(identity));
        }

        return base.OnCreateAsync(context, requestExecutor, requestBuilder,
            cancellationToken);
    }
}