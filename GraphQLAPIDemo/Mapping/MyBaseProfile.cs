using AutoMapper;

namespace GraphQLAPIDemo.Mapping
{
    public class MyBaseProfile : Profile
    {
        public MyBaseProfile()
        {
            CreateMap<Optional<string?>, string>()
                .ConvertUsing(opt => opt.Value);
            CreateMap<Optional<Guid?>, Guid>()
                .ConvertUsing(opt => opt.Value ?? Guid.Empty);
        }

        protected bool MappingCondition(object src, object dest, object srcMember)
        {
            if (srcMember == default
                //|| (srcMember is Guid srcGuid && srcGuid == Guid.Empty)
                || (srcMember is IOptional opt && !opt.HasValue))
            {
                return false;
            }
            return true;
        }
    }
}
