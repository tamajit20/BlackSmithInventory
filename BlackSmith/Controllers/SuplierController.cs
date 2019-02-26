using BlackSmithCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using ViewModels;

namespace BlackSmithAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Suplier/[action]")]
    public class SuplierController : Controller
    {
        private IOperation<Suplier> _supOpp;

        public SuplierController(IOperation<Suplier> suppOpp)
        {
            _supOpp = suppOpp;
        }

        [HttpPost]
        public Suplier Save([FromBody]Suplier input)
        {
            try
            {
                if (input.Id == 0)
                {
                    input = _supOpp.Add(input);
                }
                else
                {
                    input = _supOpp.Update(input);
                }
            }
            catch (Exception ex)
            {

            }
            return input;
        }

        [HttpPost]
        public IList<Suplier> GetAll()
        {
            List<Suplier> result = new List<Suplier>();
            try
            {
                var predicate = PredicateBuilder.True<Suplier>();
                predicate = predicate.And(x => !x.IsDeleted);

                result = _supOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate).ToList();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        [HttpPost]
        public bool Delete([FromBody]Suplier input)
        {
            try
            {
                Expression<Func<Suplier, object>>[] properties =
                    new Expression<Func<Suplier, object>>[] { x => x.IsDeleted, x => x.ModifiedBy, x => x.ModifiedOn };

                input.IsDeleted = true;
                _supOpp.UpdateColumn(input, properties);
            }
            catch (Exception ex)
            {
            }
            return true;
        }
    }
}