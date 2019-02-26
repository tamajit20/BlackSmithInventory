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
    [Route("api/Customer/[action]")]
    public class CustomerController : Controller
    {
        private IOperation<Customer> _custOpp;

        public CustomerController(IOperation<Customer> custOpp)
        {
            _custOpp = custOpp;
        }

        [HttpPost]
        public Customer Save([FromBody]Customer input)
        {
            try
            {
                if (input.Id == 0)
                {
                    input = _custOpp.Add(input);
                }
                else
                {
                    input = _custOpp.Update(input);
                }
            }
            catch (Exception ex)
            {
            }
            return input;
        }

        [HttpPost]
        public IList<Customer> GetAll()
        {
            List<Customer> result = new List<Customer>();
            try
            {
                var predicate = PredicateBuilder.True<Customer>();
                predicate = predicate.And(x => !x.IsDeleted);

                result = _custOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate).ToList();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        [HttpPost]
        public bool Delete([FromBody]Customer input)
        {
            try
            {
                Expression<Func<Customer, object>>[] properties =
                    new Expression<Func<Customer, object>>[] { x => x.IsDeleted, x => x.ModifiedBy, x => x.ModifiedOn };

                input.IsDeleted = true;
                _custOpp.UpdateColumn(input, properties);
            }
            catch (Exception ex)
            {
            }
            return true;
        }
    }
}