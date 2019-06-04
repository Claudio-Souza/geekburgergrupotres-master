using System;
using System.Collections;
using System.Collections.Generic;

namespace GeekBurger.Ingredients.DomainModel
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<string> Ingredients { get; set; }
    }
}
