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
        IQueryable<T> FindBy(Expression<Func<T, bool>> expression);
        T Update(T model);
        T UpdateColumn(T model, params Expression<Func<T, object>>[] properties);
    }
}
