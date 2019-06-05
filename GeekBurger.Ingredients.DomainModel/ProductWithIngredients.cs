using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Ingredients.DomainModel
{
    public class ProductWithIngredients
    {
        public string ProductId { get; set; }

        public string StoreId { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
