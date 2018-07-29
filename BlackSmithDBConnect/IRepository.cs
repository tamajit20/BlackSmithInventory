using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackSmithDBConnect
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T obj);       
        void Delete(T obj);
        T Update(T obj);
        T UpdateColumn(T obj, params Expression<Func<T, object>>[] properties);
    }
}
