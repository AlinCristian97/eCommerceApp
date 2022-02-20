namespace Core.Specifications;

public class ProductSpecificationParams
{
    private const int MAX_PAGE_SIZE = 50;
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
    }
    
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public string Sort { get; set; }
}