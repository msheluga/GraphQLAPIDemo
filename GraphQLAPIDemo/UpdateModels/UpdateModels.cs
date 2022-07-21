using GraphQLAPIDemo.Data.Models;

namespace GraphQLAPIDemo.UpdateModels
{
    public class AuthorUpdate
    {
        public Guid Id { get; set; }
        public Optional<string?> Name { get; set; }
    }

    public class BookUpdate
    {
        public Guid Id { get; set; }
        public Optional<string?> Title { get; set; }
        public Optional<string?> Isbn { get; set; }
        public Optional<Guid?> AuthorId { get; set; }
        
    }
}
