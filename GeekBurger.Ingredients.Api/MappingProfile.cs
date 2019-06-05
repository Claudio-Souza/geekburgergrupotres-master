using AutoMapper;
using GeekBurger.Ingredients.DomainModel;

namespace GeekBurger.Ingredients.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<LabelImageAddedMessage, Product>()
                .ForMember(destiny => destiny.Name, src => src.MapFrom(l => l.ItemName))
                .ReverseMap();
        }
    }
}