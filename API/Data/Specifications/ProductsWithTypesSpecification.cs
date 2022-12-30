using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using API.Models;

namespace API.Data.Specifications
{
    public class ProductsWithTypesSpecification : BaseSpecification<Product>
    {
        // public ProductsWithTypesSpecification(ProductSpecParams productParams) 
        //     : base((p => (!productParams.CategoryId.HasValue || p.CategoryId == productParams.CategoryId)))
        // {
        //     SetParams(productParams);
        // }

        public ProductsWithTypesSpecification(ProductSpecParams productParams, List<Expression<Func<Product, bool>>> criteria)
            : base(criteria)
        {
            AddInclude(p => p.Category);
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
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.Category);
        }

        // private void SetParams(ProductSpecParams productParams) 
        // {
        //     AddInclude(p => p.Category);
        //     AddOrderBy(p => p.Name);
        //     ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

        //     if(!string.IsNullOrEmpty(productParams.Sort)) 
        //     {
        //         switch(productParams.Sort) 
        //         {
        //             case "priceAsc":
        //                 AddOrderBy(p => p.Price);
        //                 break;
        //             case "priceDesc":
        //                 AddOrderByDescending(p => p.Price);
        //                 break;
        //             default:
        //                 AddOrderBy(p => p.Name);
        //                 break;
        //         }
        //     }
        // }
    }
}