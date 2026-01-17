using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitTest.MVC.Web.Models;
using UnitTest.MVC.Web.Repository;


namespace UnitTest.MVC.Web.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {

            var group = routes.MapGroup("/api/Products").WithTags(nameof(Product));



            // Get All

            group.MapGet("/", async (IRepository<Product> repository) =>
            {
                return await repository.GetAllAsync();
            })
            .WithName("GetAllProducts");


            // GetById

            group.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (int id, IRepository<Product> repository) =>
            {
                var product = await repository.GetByIdAsync(id);

                return product != null ? TypedResults.Ok(product) : TypedResults.NotFound();
                
            })
            .WithName("GetProductById");

            // Edit

            group.MapPut("/", async Task<Results<Ok, NotFound>> ([FromBody]Product product, [FromServices]IRepository<Product> repository) =>
            {
                var existProduct = await repository.GetByIdAsync(product.Id);

                if (existProduct is null)
                {
                    return TypedResults.NotFound();
                }

                existProduct.Name = product.Name;
                existProduct.Price = product.Price;
                existProduct.Stock = product.Stock;
                existProduct.Color = product.Color;

                await repository.UpdateAsync(existProduct);

                return TypedResults.Ok();
            })
            .WithName("UpdateProduct");

            // Add

            group.MapPost("/", async ([FromBody] Product product, [FromServices] IRepository<Product> repository) =>
            {

                await repository.AddAsync(product);
                return TypedResults.Created($"/api/Product/{product.Id}", product);
            })
            .WithName("CreateProduct");


            // Delete

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, IRepository<Product> repository) =>
            {

                var product = await repository.GetByIdAsync(id);

                if (product is null)
                {
                    return TypedResults.NotFound();
                }

                await repository.DeleteAsync(product);

                return TypedResults.Ok();
            })
            .WithName("DeleteProduct");
        }
    }
}
