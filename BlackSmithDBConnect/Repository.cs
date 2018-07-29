using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using System.Linq.Expressions;

namespace BlackSmithDBConnect
{
    public class Repository<T> : IRepository<T>
        where T : class
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

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
            _entities.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
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
            return entity;
        }


    }
}