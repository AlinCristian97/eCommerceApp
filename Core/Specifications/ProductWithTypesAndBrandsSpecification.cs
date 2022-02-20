using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductWithTypesAndBrandsSpecification(ProductSpecificationParams specificationParams)
        : base(x => 
                (string.IsNullOrEmpty(specificationParams.Search) || x.Name.ToLower()
                    .Contains(specificationParams.Search)) &&
                (!specificationParams.BrandId.HasValue || x.ProductBrandId == specificationParams.BrandId) &&
                (!specificationParams.TypeId.HasValue || x.ProductTypeId == specificationParams.TypeId)
            )
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
        AddOrderBy(x => x.Name);
        ApplyPaging(
            specificationParams.PageSize * (specificationParams.PageIndex - 1), 
            specificationParams.PageSize);

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
    
    public ProductWithTypesAndBrandsSpecification(int id) 
        : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
    }
}