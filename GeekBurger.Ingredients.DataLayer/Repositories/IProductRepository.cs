using System.Threading.Tasks;
using GeekBurger.Ingredients.DomainModel;

namespace GeekBurger.Ingredients.DataLayer.Repositories
{
    public interface IProductRepository
    {
        Task SaveAsync(Product product);
    }
}