using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using System.Linq.Expressions;

namespace BlackSmithCore
{
    public interface IOperation<T>
    {
        T Add(T model);
        IQueryable<T> GetAll();
        T Update(T model);
        void Delete(long id);
        T UpdateColumn(T model, params Expression<Func<T, object>>[] properties);
        T FetchIdUsingExpression(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeExpressions);
        IList<T> GetAllUsingExpression(out int totalItemCount, int recordsPerPage = 1, int currentPage = 0, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, T>> columns = null, params Expression<Func<T, object>>[] includeExpressions);
        int ExecuteRawSqlCommand(string spName, params object[] parameters);
        T GetUsingId(long id);
    }
}
