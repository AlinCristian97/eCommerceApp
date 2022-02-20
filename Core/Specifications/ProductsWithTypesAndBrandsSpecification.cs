using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecificationParams specificationParams)
        : base(x => 
                (!specificationParams.BrandId.HasValue || x.ProductBrandId == specificationParams.BrandId) &&
                (!specificationParams.TypeId.HasValue || x.ProductTypeId == specificationParams.TypeId)
            )
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
        AddOrderBy(x => x.Name);

        if (!string.IsNullOrEmpty(specificationParams.Sort))
        {
            switch (specificationParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(n => n.Name);
                    break;
            }
        }
    }
    
    public ProductsWithTypesAndBrandsSpecification(int id) 
        : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
    }
}