using GeekBurger.Ingredients.DomainModel;

namespace GeekBurger.Ingredients.DataLayer.Repositories
{
    public interface IProductRepository
    {
        void Save(Product product);
    }
}