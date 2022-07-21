using AutoMapper;
using GraphQLAPIDemo.Data.Models;
using GraphQLAPIDemo.UpdateModels;

namespace GraphQLAPIDemo.Mapping
{
    public class MyProfile : MyBaseProfile
    {
        public MyProfile()
            :base()
        {
            CreateMap<BookUpdate, Book>()
                .ForMember(b => b.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition(MappingCondition));

            CreateMap<AuthorUpdate, Author>()
                .ForMember(a => a.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition(MappingCondition)); ;
        }
    }
}
