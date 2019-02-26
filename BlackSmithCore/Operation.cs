using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using BlackSmithDBConnect;
using System.Linq.Expressions;
using System.Reflection;

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

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public T UpdateColumn(T entity, params Expression<Func<T, object>>[] properties)
        {
            return _repository.UpdateColumn(entity, properties);
        }

        public IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T FetchIdUsingExpression(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeExpressions)
        {
            dynamic objectBO = null;
            objectBO = _repository.GetByIdUsingExpression(predicate, includeExpressions);
            return objectBO;
        }

        public IList<T> GetAllUsingExpression(out int totalItemCount, int recordsPerPage = 1, int currentPage = 0, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, T>> columns = null, params Expression<Func<T, object>>[] includeExpressions)
        {
            totalItemCount = 0;
            dynamic list = null;
            list = _repository.GetAllUsingExpression(out totalItemCount, recordsPerPage, currentPage, predicate, orderBy, columns, includeExpressions);
            return list;
        }

        public int ExecuteRawSqlCommand(string spName, params object[] parameters)
        {
            return _repository.ExecuteSqlCommand(spName, parameters);
        }

        public T GetUsingId(long id)
        {
            dynamic entity = null;
            entity = _repository.GetById(id);
            return entity;
        }

        #region privateMethods
        private dynamic InsertTrace(dynamic entity)
        {
            try
            {
                PropertyInfo creationDate = entity.GetType().GetProperty("CreationDate");
                PropertyInfo createdBy = entity.GetType().GetProperty("FK_CreatedBy");
                PropertyInfo userId = entity.GetType().GetProperty("ActiveUserId");

                if (creationDate != null)
                {
                    creationDate.SetValue(entity, DateTime.Now, null);
                }
                if (createdBy != null && userId != null)
                {
                    createdBy.SetValue(entity, entity.GetType().GetProperty("ActiveUserId").GetValue(entity, null), null);
                }
            }
            catch (Exception)
            {
            }
            return entity;
        }

        private dynamic UpdateTrace(dynamic entity)
        {
            try
            {
                PropertyInfo modificationDate = entity.GetType().GetProperty("ModificationDate");
                PropertyInfo modifiedBy = entity.GetType().GetProperty("FK_ModifiedBy");

                if (modificationDate != null)
                {
                    modificationDate.SetValue(entity, DateTime.Now, null);
                }

                //if (modifiedBy != null)
                //{
                //    modifiedBy.SetValue(entity, userid, null);
                //}
            }
            catch (Exception)
            {
            }
            return entity;
        }
        #endregion
    }
}





