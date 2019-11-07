using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using Products_api;
using System.Threading.Tasks;
using System.Net;
using System;
using Newtonsoft.Json;
using Products_api.Models;
using System.Text;

namespace CRUD.Test
{
    public class ProductApiTest
    {
        private readonly HttpClient client;
        public ProductApiTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            client = server.CreateClient();
        }

        private async Task CreateFakeProduct(Guid TmpId)
        {
            await client.PostAsync($"/Product", new StringContent(
                JsonConvert.SerializeObject(new Product()
                {
                    Id = TmpId,
                    ProductName = "Test product",
                    Price = 22.12M
                }),
                Encoding.UTF8, "application/json")
                );
        }

        [Theory]
        [InlineData("GET")]
        public async Task GetAllProductsOKTest(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/Product");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Theory]
        [InlineData("GET")]
        public async Task GetProductsOKTest(string method,string id = null)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), $"/Product/{id}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Theory]
        [InlineData("GET")]
        public async Task GetProductsBadRequestTest(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), $"/Product/TestID");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task DeleteProductsNotFoundTest()
        {
            Guid id = Guid.NewGuid();
            var response = await client.DeleteAsync($"/Product/{id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task DeleteProductsBadRequestTest()
        {
            var response = await client.DeleteAsync($"/Product/Test");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task DeleteProductsOKTest()
        {
            Guid TmpId = Guid.NewGuid();
            await CreateFakeProduct(TmpId);
            var response = await client.DeleteAsync($"/Product/{TmpId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task PostProductsCreatedTest()
        {
            var response = await client.PostAsync($"/Product",new StringContent(
                JsonConvert.SerializeObject(new Product()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test product",
                    Price = 22.12M
                }),
                Encoding.UTF8, "application/json")
                );
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task PostProductsBadRequestTest()
        {
            var response = await client.PostAsync($"/Product", new StringContent(
                JsonConvert.SerializeObject(new Product()
                {
                    Id = Guid.NewGuid(),
                    Price = 22.12M
                }),
                Encoding.UTF8, "application/json")
                );
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task PutProductsBadRequestTest()
        {
            Guid TmpId = Guid.NewGuid();
            var response = await client.PutAsync($"/Product/{TmpId}", new StringContent(
                JsonConvert.SerializeObject(new Product()
                {
                    Id = TmpId,
                    Price = 22.12M
                }),
                Encoding.UTF8, "application/json")
                );
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task PutProductsOkTest()
        {
            Guid TmpId = Guid.NewGuid();
            await CreateFakeProduct(TmpId);
            var response = await client.PutAsync($"/Product/{TmpId}", new StringContent(
                JsonConvert.SerializeObject(new Product()
                {
                    Id = TmpId,
                    ProductName = "TestProduct",
                    Price = 22.12M
                }),
                Encoding.UTF8, "application/json")
                );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task PutProductsNotFoundTest()
        {
            Guid TmpId = Guid.NewGuid();
            var response = await client.PutAsync($"/Product/{TmpId}", new StringContent(
                JsonConvert.SerializeObject(new Product()
                {
                    Id = TmpId,
                    ProductName = "TestProduct",
                    Price = 22.12M
                }),
                Encoding.UTF8, "application/json")
                );
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}