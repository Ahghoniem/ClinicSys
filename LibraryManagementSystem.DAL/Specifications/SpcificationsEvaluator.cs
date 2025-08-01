using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.SpecificationContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Specifications
{
    internal static class SpcificationsEvaluator<TEntity>
     where TEntity : BaseEntity
    { 

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> specifications)
        {
            var query = inputQuery; // => dbContext.Set<TEntity>()
            if (specifications.Criteria is not null)
            {
                query = query.Where(specifications.Criteria);
            }

            if (specifications.OrderByDesc is not null)
                query = query.OrderByDescending(specifications.OrderByDesc);
            else if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);

            if (specifications.IsPaginationEnabled)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }

            query = specifications.Includes.Aggregate(query, (currentQuery, includeExpresion) => currentQuery.Include(includeExpresion));


            return query;
        }
    }

}
