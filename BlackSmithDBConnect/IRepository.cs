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

        void Add(T obj);

        void Delete(long id);

        T Update(T obj);

        T UpdateColumn(T obj, params Expression<Func<T, object>>[] properties);

        T GetById(long id);

        IList<T> GetAllUsingExpression(out int totalItemCount, int recordsPerPage = -1, int currentPage = 0, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, T>> columns = null, params Expression<Func<T, object>>[] includeExpressions);

        T GetByIdUsingExpression(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeExpressions);

        int ExecuteSqlCommand(string query, params object[] parameters);
    }
}
