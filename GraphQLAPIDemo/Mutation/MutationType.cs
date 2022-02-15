using GraphQLAPIDemo.Data;

namespace GraphQLAPIDemo.Mutation
{
    public partial class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(f => f.AddBook(default!)).UseMutationConvention();
            descriptor.Field(f => f.EditBook(default!)).UseMutationConvention();
            descriptor.Field(f => f.PatchBook(default!, default!)).UseMutationConvention();
        }
    }

}
