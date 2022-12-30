using API.Models;

namespace API.Data.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams) 
            : base(p => (!productParams.CategoryId.HasValue || p.CategoryId == productParams.CategoryId) &&
                        (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)))
        {
        }
    }
}