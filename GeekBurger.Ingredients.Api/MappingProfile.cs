using AutoMapper;
using GeekBurger.Ingredients.DomainModel;
using GeekBurger.Products.Contract;

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

            this.CreateMap<ItemToGet, Ingredient>()
                .ForMember(destiny => destiny.Name, src => src.MapFrom(itg => itg.Name));

            this.CreateMap<ProductChangedMessage, ProductWithIngredients>()
                .ForMember(destiny => destiny.StoreId, src=> src.MapFrom(pcm => pcm.Product.StoreId))
                .ForMember(destiny => destiny.ProductId, src => src.MapFrom(pcm => pcm.Product.ProductId))
                .ForMember(destiny => destiny.Ingredients, src => src.MapFrom(pcm => pcm.Product.Items))
                .ReverseMap();
        }
    }
}