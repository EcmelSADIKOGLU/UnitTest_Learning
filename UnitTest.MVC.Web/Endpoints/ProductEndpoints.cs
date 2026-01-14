using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using UnitTest.MVC.Web.Models;
using UnitTest.MVC.Web.Repository;


namespace UnitTest.MVC.Web.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes, IRepository<Product> _repository)
        {
            var group = routes.MapGroup("/api/Products").WithTags(nameof(Product));

            // Get All

            group.MapGet("/", async () =>
            {
                return await _repository.GetAllAsync();
            })
            .WithName("GetAllProducts");


            // GetById

            group.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (int id) =>
            {
                var product = await _repository.GetByIdAsync(id);

                return product != null ? TypedResults.Ok(product) : TypedResults.NotFound();
                
            })
            .WithName("GetProductById");

            // Edit

            group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Product product) =>
            {
                var isExist = await _repository.GetByIdAsync(id) != null;

                if (!isExist)
                {
                    return TypedResults.NotFound();
                }

                await _repository.UpdateAsync(product);

                return TypedResults.Ok();
            })
            .WithName("UpdateProduct");

            // Add

            group.MapPost("/", async (Product product) =>
            {

                await _repository.AddAsync(product);
                return TypedResults.Created($"/api/Product/{product.Id}", product);
            })
            .WithName("CreateProduct");


            // Delete

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id) =>
            {

                var product = await _repository.GetByIdAsync(id);

                if (product is null)
                {
                    return TypedResults.NotFound();
                }

                await _repository.DeleteAsync(product);

                return TypedResults.Ok();
            })
            .WithName("DeleteProduct");
        }
    }
}
