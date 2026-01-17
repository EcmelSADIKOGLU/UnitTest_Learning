using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnitTest.MVC.Web.Models;
using UnitTest.MVC.Web.Repository;

namespace xUnitTest.Test.UnitTest.MVC
{
    public class TestAppFactory : WebApplicationFactory<Program>
    {
        private readonly IRepository<Product> _repo;

        public TestAppFactory(IRepository<Product> repo)
            => _repo = repo;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IRepository<Product>));
                services.AddSingleton(_repo);
            });
        }
    }
}
