using GeekBurger.Products.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Api.Services
{
    public class ProductService
    {
        private HttpClient _client;
        private string _productsServiceUri;

        public ProductService(HttpClient client, string productsServiceUri)
        {
            _client = client;
            _productsServiceUri = productsServiceUri;
        }

        public async Task<IEnumerable<ProductToGet>> GetStoreProducts(string storeName)
        {
            var getProductsByStoreNameUri = new UriBuilder(_productsServiceUri);
            getProductsByStoreNameUri.Path = "GetProductsByStoreName";
            getProductsByStoreNameUri.Query = storeName;

            var response = await _client.GetAsync(getProductsByStoreNameUri.Uri);

            return JsonConvert.DeserializeObject<IEnumerable<ProductToGet>>(await response.Content.ReadAsStringAsync());
        }
    }
}
