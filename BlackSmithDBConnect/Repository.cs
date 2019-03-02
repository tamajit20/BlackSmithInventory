using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BlackSmithDBConnect
{
    public class Repository<T> : IRepository<T>  where T : class
    {
        private BlackSmithDBContext _entities = new BlackSmithDBContext();

        public BlackSmithDBContext Context
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entities.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
            _entities.SaveChanges();
        }

        public virtual void Delete(long id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _entities.Set<T>().Remove(entity);
                _entities.SaveChanges();
            }
        }

        public T GetById(long id)
        {
            return _entities.Set<T>().Find(id);
        }

        public virtual T Update(T entity)
        {
            try
            {
                _entities.Set<T>().Update(entity);
                _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        public T UpdateColumn(T entity, params Expression<Func<T, object>>[] properties)
        {
            try
            {
                _entities.Set<T>().Attach(entity);

                var entry = _entities.Entry(entity);

                foreach (var property in properties)
                    entry.Property(property).IsModified = true;

                _entities.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<T> GetAllUsingExpression(out int totalItemCount, int recordsPerPage = -1, int currentPage = 0,
           Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           Expression<Func<T, T>> columns = null,
           params Expression<Func<T, object>>[] includeExpressions)
        {
            try
            {
                recordsPerPage = recordsPerPage <= 0 ? 10 : recordsPerPage;
                IQueryable<T> query = _entities.Set<T>();
                totalItemCount = 0;

                if ((includeExpressions != null && includeExpressions.Any()) || recordsPerPage != -1)
                {
                    query = includeExpressions.Aggregate(query, (current, include) => current.Include(include));
                    if (predicate != null)
                    {
                        query = query.Where(predicate);
                    }
                }
                else
                {
                    if (predicate != null)
                    {
                        query = _entities.Set<T>().Where(predicate);
                    }
                }
                totalItemCount = query.Count();
                if (columns != null)
                {
                    query = query.Select<T, T>(columns).AsQueryable<T>();
                }

                if (orderBy != null && totalItemCount > 0 && currentPage > 0)
                {
                    int recordsRequired = currentPage * recordsPerPage;
                    int skipRecords = recordsPerPage * (currentPage - 1);
                    if (recordsRequired > totalItemCount)
                    {
                        recordsPerPage = totalItemCount - skipRecords;
                    }

                    query = orderBy(query).Skip(skipRecords).Take(recordsPerPage);
                }
                else if (orderBy != null && totalItemCount > 0)
                {
                    query = orderBy(query);
                }

                var result = query.AsNoTracking().ToList<T>();
                return result;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public T GetByIdUsingExpression(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> set = _entities.Set<T>().AsQueryable<T>();

            try
            {
                if (includeExpressions.Any())
                {
                    set = includeExpressions.Aggregate(set, (current, include) => current.Include(include));

                    if (predicate != null)
                    {
                        return set.Where(predicate).FirstOrDefault();
                    }
                    else
                    {
                        return set.FirstOrDefault();
                    }
                }
                else
                {
                    if (predicate != null)
                    {
                        return _entities.Set<T>().Where(predicate).FirstOrDefault(predicate);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public int ExecuteSqlCommand(string query, params object[] parameters)
        {
            return _entities.Database.ExecuteSqlCommand(query, parameters);
        }
    }
}