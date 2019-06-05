using AutoMapper;
using GeekBurger.Ingredients.DomainModel;

namespace GeekBurger.Ingredients.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<LabelImageAddedMessage, Ingredient>()
                .ForMember(destiny => destiny.Name, src => src.MapFrom(l => l.ItemName))
                .ForMember(destiny => destiny.Composition, src => src.MapFrom(l => l.Ingredients))
                .ReverseMap();
        }
    }
}