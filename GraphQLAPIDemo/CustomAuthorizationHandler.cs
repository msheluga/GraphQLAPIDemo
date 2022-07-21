using GraphQLAPIDemo.Data;
using GraphQLAPIDemo.Data.Models;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Language;
using HotChocolate.Resolvers;

namespace GraphQLAPIDemo
{
    public class CustomAuthorizationHandler : IAuthorizationHandler
    {
        private static List<ObjectField>? ObjectFields = null;
        public ValueTask<AuthorizeResult> AuthorizeAsync(IMiddlewareContext context, AuthorizeDirective directive)
        {
            if(ObjectFields == null)
            {
                ObjectFields = context.Schema.Types
                                    .Where(t => EntityTypes.Types.Contains(t.ToRuntimeType())
                                                && t.GetType().IsAssignableTo(typeof(ObjectType)))
                                    .Select(t => (ObjectType)t)
                                    .SelectMany(o => o.Fields).Where(f => f.ExecutableDirectives
                                                                          .Any(d => d.Type.RuntimeType == typeof(AuthorizeDirective))).ToList();
            }
            
            var httpContext = context.ContextData["HttpContext"] as DefaultHttpContext;
            var principal = httpContext?.User;
            if(context.Operation.Operation == OperationType.Mutation)
            {
                var typeName = directive.Policy.Contains('.')
                                ? directive.Policy[..directive.Policy.IndexOf('.')]
                                : directive.Policy;
                var fieldNames = ObjectFields.Where(o => o.Coordinate.TypeName == typeName).Select(f => f.Coordinate.FieldName.Value).ToList();

                if (context.FieldSelection.Arguments != null 
                    && context.FieldSelection.Arguments.Count > 0 
                    && context.FieldSelection.Name.Value.ToLower().Contains("update"))
                {
                    var objectValuesNode = context.FieldSelection.Arguments.FirstOrDefault()?.Value as ObjectValueNode;
                    var arguments = objectValuesNode.Fields.Select(f => f.Name.Value).ToList();

                    var claim = principal?.Claims.FirstOrDefault(c => c.Type == $"{typeName}.Update");
                    foreach (var argument in arguments)
                    {
                        if(fieldNames.Contains(argument))
                        {     
                            if (claim == null
                                || !claim.Value.ToLower().Contains(argument.ToLower()))
                            {
                                return new ValueTask<AuthorizeResult>(AuthorizeResult.NotAllowed);
                            }
                        }
                    }
                }
                else
                {
                    if (fieldNames.Contains(context.FieldSelection.Name.Value))
                    {
                        var claim = principal?.Claims.FirstOrDefault(c => c.Type == $"{typeName}.Read");
                        if (claim == null
                                || !claim.Value.ToLower().Contains(context.FieldSelection.Name.Value.ToLower()))
                        {
                            return new ValueTask<AuthorizeResult>(AuthorizeResult.NotAllowed);
                        }
                    }
                }
            }
            
            return new ValueTask<AuthorizeResult>(AuthorizeResult.Allowed);
        }
    }
}
