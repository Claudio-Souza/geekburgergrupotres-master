using System.Threading.Tasks;
using GeekBurger.Ingredients.DomainModel;

namespace GeekBurger.Ingredients.Api.Services
{
    public interface IMergeService
    {
        Task MergeProductWithIngredientsAsync(ProductWithIngredients productWithIngredients);

        Task UpdateProductsMergesAsync(Ingredient ingredient);
    }
}