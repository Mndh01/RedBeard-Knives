using System;
using System.Linq.Expressions;
using API.Models;

namespace API.Data.Specifications
{
    public class ProductsWithTypesSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesSpecification()
        {
            AddInclude(p => p.Category);
        }

        public ProductsWithTypesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.Category);
        }
    }
}