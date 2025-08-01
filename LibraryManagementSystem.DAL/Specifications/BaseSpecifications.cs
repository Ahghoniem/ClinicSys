using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.Products;
using LibraryManagementSystem.DAL.SpecificationContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Specifications
{
    public abstract class BaseSpecifications<TEntity> : ISpecifications<TEntity>
        where TEntity : BaseEntity


    {

        public Expression<Func<TEntity, bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new();
        public Expression<Func<TEntity, object>>? OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
        public BaseSpecifications(Expression<Func<TEntity, bool>>? CriteriaExprrsssion)
        {
            Criteria = CriteriaExprrsssion;
        }

        public BaseSpecifications(int id)
        {
            Criteria = E => E.Id.Equals(id);

        }


        private protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        private protected virtual void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }
        private protected void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
