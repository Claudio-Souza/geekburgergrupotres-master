using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeekBurger.Ingredients.DataLayer;
using GeekBurger.Ingredients.DomainModel;
using GeekBurger.Products.Contract;

namespace GeekBurger.Ingredients.Api.Services
{
    public class MergeService : IMergeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MergeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task MergeProductWithIngredientsAsync(ProductToGet storeProduct)
        {
            var productIngredients = storeProduct.Items.Select(i => i.Name).ToList();
            var ingredients = await _unitOfWork.IngredientsRepository.GetByNamesAsync(productIngredients);

            if (ingredients.Any() == false) { ingredients = _mapper.Map<IList<Ingredient>>(storeProduct.Items); }

            var productWithIngredients = new ProductWithIngredients
            {
                Id = storeProduct.ProductId.ToString(),
                StoreId = storeProduct.StoreId.ToString(),
                Ingredients = ingredients
            };

            await _unitOfWork.MergedProductsRepository.InsertOrUpdate(productWithIngredients);
        }

        public async Task UpdateProductsMergesAsync(Ingredient ingredient)
        {
            await _unitOfWork.IngredientsRepository.InsertOrUpdateAsync(ingredient);

            var products = _unitOfWork.MergedProductsRepository.GetAll();

            foreach (var product in products)
            {
                await _unitOfWork.MergedProductsRepository.InsertOrUpdate(product);
            }
        }
    }
}
