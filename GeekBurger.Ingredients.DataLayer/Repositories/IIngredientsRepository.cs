using System.Threading.Tasks;
using GeekBurger.Ingredients.DomainModel;

namespace GeekBurger.Ingredients.DataLayer.Repositories
{
    public interface IIngredientRepository
    {
        Task SaveAsync(Ingredient product);
    }
}