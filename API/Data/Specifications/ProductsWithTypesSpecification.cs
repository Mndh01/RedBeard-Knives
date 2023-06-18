using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using API.Models;

namespace API.Data.Specifications
{
    public class ProductsWithTypesSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesSpecification(ProductSpecParams productParams, List<Expression<Func<Product, bool>>> criteria)
            : base(criteria)
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Photos); // TODO: Must be removed after adding PhotoUrl property to Product model
            AddOrderBy(p => p.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if(!string.IsNullOrEmpty(productParams.Sort)) 
            {
                switch(productParams.Sort) 
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    case "topSells":
                        AddOrderByDescending(p => p.SoldItems);
                        break;
                    default:
                        AddOrderBy(p => p.Id);
                        break;
                }
            }
        }

        public ProductsWithTypesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Photos);
            AddInclude(p => p.Reviews);
        }

    }
}