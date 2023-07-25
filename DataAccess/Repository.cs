using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class Repository
{
    
    private readonly CodeFlyDbContext _dbContext;

    public Repository(CodeFlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> FirstOrDefaultAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        IQueryable<TEntity> query = this._dbContext.Set<TEntity>().AsQueryable<TEntity>();
        Expression<Func<TEntity, object>>[] expressionArray = includes;
        for (int index = 0; index < expressionArray.Length; ++index)
        {
            Expression<Func<TEntity, object>> include = expressionArray[index];
            query = query.Include<TEntity>(include.AsPath());
            include = (Expression<Func<TEntity, object>>)null;
        }

        expressionArray = (Expression<Func<TEntity, object>>[])null;
        TEntity data =
            await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TEntity>(query, predicate,
                new CancellationToken());
        return data;
    }


    public async Task<IList<TEntity>> ListAsNoTrackingAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate,
        PagingModel model,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsNoTracking().Where(predicate).AsQueryable();
        Expression<Func<TEntity, object>>[] expressionArray = includes;
        for (int index = 0; index < expressionArray.Length; ++index)
        {
            Expression<Func<TEntity, object>> include = expressionArray[index];
            query = query.Include(include.AsPath());
            include = null;
        }
        expressionArray = null;
        List<TEntity> result = await query.Skip(model.PageNumber * model.PageSize).Take<TEntity>(model.PageSize).ToListAsync<TEntity>();
        IEnumerable<TEntity> items = result;
        int totalCount = await query.CountAsync();
        return (IList<TEntity>)items;
    }

}