using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Redefine.Broker.Repository
{
    public class RepositoryBase<TDomain, TDbContext> : IRepositoryBase<TDomain>
        where TDomain : class
        where TDbContext: DbContext 
    {
        protected readonly TDbContext _dbContext;

        public RepositoryBase(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual void Remove(TDomain entity)
        {
            GetSet().Remove(entity);
        }

		public virtual void Add(TDomain entity)
		{
			GetSet().Add(entity);
		}

		public virtual void Update(TDomain entity)
		{
			GetSet().Update(entity);
		}

		public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async virtual Task<TDomain> FindAsync(Expression<Func<TDomain, bool>> filterExpression, CancellationToken cancellationToken = default)
		{
			return await QueryInternal(filterExpression).SingleOrDefaultAsync<TDomain>(cancellationToken);
		}

		public async virtual Task<List<TDomain>> FindAllAsync(CancellationToken cancellationToken = default)
		{
			return await QueryInternal(x => true).ToListAsync<TDomain>(cancellationToken);
		}

		public async virtual Task<List<TDomain>> FindAllAsync(Expression<Func<TDomain, bool>> filterExpression, CancellationToken cancellationToken = default)
		{
			return await QueryInternal(filterExpression).ToListAsync<TDomain>(cancellationToken);
		}

		public async virtual Task<List<TDomain>> FindAllAsync(Expression<Func<TDomain, bool>> filterExpression, Func<IQueryable<TDomain>, IQueryable<TDomain>> linq, CancellationToken cancellationToken = default)
		{
			return await QueryInternal(filterExpression, linq).ToListAsync<TDomain>(cancellationToken);
		}

		public async virtual Task<IPagedResult<TDomain>> FindAllAsync(int pageNo, int pageSize, CancellationToken cancellationToken = default)
		{
			var query = QueryInternal(x => true);
			return await PagedList<TDomain>.CreateAsync(
				query,
				pageNo,
				pageSize);
		}

		public async virtual Task<IPagedResult<TDomain>> FindAllAsync(Expression<Func<TDomain, bool>> filterExpression, int pageNo, int pageSize, CancellationToken cancellationToken = default)
		{
			var query = QueryInternal(filterExpression);
			return await PagedList<TDomain>.CreateAsync(
				query,
				pageNo,
				pageSize,
				cancellationToken);
		}

		public async virtual Task<IPagedResult<TDomain>> FindAllAsync(Expression<Func<TDomain, bool>> filterExpression, int pageNo, int pageSize, Func<IQueryable<TDomain>, IQueryable<TDomain>> linq, CancellationToken cancellationToken = default)
		{
			var query = QueryInternal(filterExpression, linq);
			return await PagedList<TDomain>.CreateAsync(
				query,
				pageNo,
				pageSize,
				cancellationToken);
		}
		public virtual IEnumerable<TDomain> FindAllEnumerable(Expression<Func<TDomain, bool>> filterExpression)
		{
			return QueryInternal(filterExpression).AsEnumerable();
		}

		public virtual IAsyncEnumerable<TDomain> FindAllAsyncEnumerable(Expression<Func<TDomain, bool>> filterExpression)
		{
			return QueryInternal(filterExpression).AsAsyncEnumerable();
		}

		public async virtual Task<int> CountAsync(Expression<Func<TDomain, bool>> filterExpression, CancellationToken cancellationToken = default)
		{
			return await QueryInternal(filterExpression).CountAsync(cancellationToken);
		}

		public virtual bool Any(Expression<Func<TDomain, bool>> filterExpression)
		{
			return QueryInternal(filterExpression).Any();
		}

		public async virtual Task<bool> AnyAsync(Expression<Func<TDomain, bool>> filterExpression, CancellationToken cancellationToken = default)
		{
			return await QueryInternal(filterExpression).AnyAsync(cancellationToken);
		}

		protected virtual IQueryable<TDomain> QueryInternal(Expression<Func<TDomain, bool>> filterExpression)
		{
			var queryable = CreateQuery();
			if (filterExpression != null)
			{
				queryable = queryable.Where(filterExpression);
			}
			return queryable;
		}
		protected virtual IQueryable<TDomain> QueryInternal(Expression<Func<TDomain, bool>> filterExpression, Func<IQueryable<TDomain>, IQueryable<TDomain>> linq)
		{
			var queryable = CreateQuery();
			if (filterExpression != null)
			{
				queryable = queryable.Where(filterExpression);
			}
			var result = linq(queryable);

			return result;
		}

		protected virtual IQueryable<TResult> QueryInternal<TResult>(Expression<Func<TDomain, bool>> filterExpression, Func<IQueryable<TDomain>, IQueryable<TResult>> linq)
		{
			var queryable = CreateQuery();
			queryable = queryable.Where(filterExpression);

			var result = linq(queryable);
			return result;
		}
		protected virtual IQueryable<TDomain> CreateQuery()
		{
			return GetSet();
		}

		protected virtual DbSet<TDomain> GetSet()
        {
            return _dbContext.Set<TDomain>();
        }
    }
}