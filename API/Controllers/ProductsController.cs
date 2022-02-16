using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
         IReadOnlyList<Product> products = await _repository.GetProductsAsync();

         return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        Product product = await _repository.GetProductByIdAsync(id);

        return Ok(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        IReadOnlyList<ProductBrand> productBrands = await _repository.GetProductBrandsAsync();

        return Ok(productBrands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        IReadOnlyList<ProductType> productTypes = await _repository.GetProductTypesAsync();

        return Ok(productTypes);
    }
}