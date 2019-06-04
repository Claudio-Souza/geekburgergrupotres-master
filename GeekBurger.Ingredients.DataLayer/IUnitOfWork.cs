using GeekBurger.Ingredients.DataLayer.Repositories;
using System;

namespace GeekBurger.Ingredients.DataLayer
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
    }
}
