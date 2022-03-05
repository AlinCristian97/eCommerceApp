using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;

    public ProductsController(
        IMapper mapper,
        IGenericRepository<Product> productsRepo, 
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo)
    {
        _mapper = mapper;
        _productsRepo = productsRepo;
        _productBrandRepo = productBrandRepo;
        _productTypeRepo = productTypeRepo;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
        [FromQuery] ProductSpecificationParams specificationParams)
    {
        var specification = new ProductWithTypesAndBrandsSpecification(specificationParams);

        var countSpecification = new ProductWithFiltersForCountSpecification(specificationParams);

        var totalItems = await _productsRepo.CountAsync(countSpecification);

        IReadOnlyList<Product> products = await _productsRepo.ListEntitiesWithSpecification(specification);

        var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
        
        return Ok(new Pagination<ProductToReturnDto>(
            specificationParams.PageIndex, 
            specificationParams.PageSize,
            totalItems,
            data));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
        var specification = new ProductWithTypesAndBrandsSpecification(id);
        
        Product product = await _productsRepo.GetEntityWithSpecification(specification);

        if (product == null) return NotFound(new ApiResponse(404));

        ProductToReturnDto productToReturnDto = _mapper.Map<Product, ProductToReturnDto>(product);

        return Ok(productToReturnDto);
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