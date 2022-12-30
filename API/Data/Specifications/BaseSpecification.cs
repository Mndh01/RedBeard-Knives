using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace API.Data.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria.Add(criteria);
        }

        public BaseSpecification(List<Expression<Func<T, bool>>> criteria)
        {
            AddCriteria(criteria);
        }

        public List<Expression<Func<T, bool>>> Criteria {get; } = new List<Expression<Func<T, bool>>>();

        public List<Expression<Func<T, object>>> Includes {get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy {get; private set;}

        public Expression<Func<T, object>> OrderByDescending {get; private set;}

        public int Take {get; private set;}

        public int Skip {get; private set;}

        public bool IsPagingEnabled {get; private set;}

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        
        protected void AddOrderBy(Expression<Func<T, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }
        
        protected void AddOrderByDescending(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrderByDescending = OrderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
        
        protected void AddCriteria(List<Expression<Func<T, bool>>> criteria) 
        {
            foreach(Expression<Func<T, bool>> crit in criteria) 
            {
                Criteria.Add(crit);
            }
        }
    }
}