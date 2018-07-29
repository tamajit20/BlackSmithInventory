using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using BlackSmithDBConnect;
using System.Linq.Expressions;

namespace BlackSmithCore
{
    public class Operation<T> : IOperation<T> where T : class
    {
        public IRepository<T> _repository;
        public Operation()
        {
            _repository = new Repository<T>();
        }

        public T Add(T c)
        {
            _repository.Add(c);
            return c;
        }

        public T Update(T c)
        {
            _repository.Update(c);
            return c;
        }

        public T UpdateColumn(T model, params Expression<Func<T, object>>[] properties)
        {
            return _repository.UpdateColumn(model, properties);
        }

        public IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _repository.FindBy(predicate);
        }
    }
}
