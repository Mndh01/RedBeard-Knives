using System;
using System.Linq;
using System.Linq.Expressions;
using API.Data.Specifications;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            foreach(Expression<Func<TEntity, bool>> criteria in spec.Criteria)
            {    
                if (criteria != null)
                {
                    query = query.Where(criteria);
                }    
            }

            if(spec.OrderBy != null) 
            {
                query = query.OrderBy(spec.OrderBy);
            }
            
            if(spec.OrderByDescending != null) 
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }   
    }
}