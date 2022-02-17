using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;

    public ProductsController(
        IGenericRepository<Product> productsRepo, 
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo)
    {
        _productsRepo = productsRepo;
        _productBrandRepo = productBrandRepo;
        _productTypeRepo = productTypeRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
    {
        var specification = new ProductsWithTypesAndBrandsSpecification();
        
        IReadOnlyList<Product> products = await _productsRepo.ListEntitiesWithSpecification(specification);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        Product product = await _productsRepo.GetByIdAsync(id);

        return Ok(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        IReadOnlyList<ProductBrand> productBrands = await _productBrandRepo.ListAllAsync();

        return Ok(productBrands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        IReadOnlyList<ProductType> productTypes = await _productTypeRepo.ListAllAsync();

        return Ok(productTypes);
    }
}