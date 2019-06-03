using AutoFixture;
using GeekBurger.Ingredients.Api.Services;
using GeekBurger.Products.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GeekBurger.Ingredients.Api.Tests
{
    public class ProductsServiceTests
    {
        private string _productsServiceUri;

        private HttpClient _client;
        private Fixture _fixture;
        private Mock<HttpMessageHandler> _httpHandlerMock;
        private ProductService _productService;

        public ProductsServiceTests()
        {
            _fixture = new Fixture();

            _httpHandlerMock = new Mock<HttpMessageHandler>();
            _client = new HttpClient(_httpHandlerMock.Object);

            _productsServiceUri = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetValue<string>("ProductsServiceUri");

            _productService = new ProductService(_client, _productsServiceUri);
        }

        [Fact]
        public async void Product_service_should_make_call_to_get_products()
        {
            //Arrange
            _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var storeName = _fixture.Create<string>();

            //Act -> Assert
            await _productService.GetStoreProducts(storeName);
        }

        [Fact]
        public async void Product_service_call_to_get_store_products_should_return_a_list_of_ProductToGet()
        {
            //Arrange
            var content = JsonConvert.SerializeObject(_fixture.Create<IEnumerable<ProductToGet>>());

            _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(content) });


            var storeName = _fixture.Create<string>();

            //Act
            var result = (await _productService.GetStoreProducts(storeName)) as IEnumerable<ProductToGet>;

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
