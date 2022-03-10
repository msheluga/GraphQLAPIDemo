using HotChocolate.Execution;



namespace GraphQLAPIDemo.Authorization
{
    public class MutationAuthorizeHandler 
    {
       
        private readonly HotChocolate.Execution.RequestDelegate _next;

        public MutationAuthorizeHandler(HotChocolate.Execution.RequestDelegate next)
        {
            _next = next;  
        }
        

        public async Task InvokeAsync(IRequestContext requestContext)
        {

            if (requestContext == null || requestContext.Document == null)
            {
                return;
            }
            if (requestContext.Document.Definitions.Count > 1)
            {
                foreach (var definition in requestContext.Document.Definitions)
                {

                }
            }
            else
            {
                if (requestContext.Operation?.Definition.Name.ToString() == "Mutation")
                {

                }
            }
            
            await _next(requestContext);
        }

    }
}