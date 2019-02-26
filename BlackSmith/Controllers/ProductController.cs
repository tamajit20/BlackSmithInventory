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
    [Route("api/Product/[action]")]
    public class ProductController : Controller
    {
        private IOperation<Product> _baseOpp;

        public ProductController(IOperation<Product> baseOpp)
        {
            _baseOpp = baseOpp;
        }

        [HttpPost]
        public Product Save([FromBody]Product input)
        {
            if (input.Id == 0)
            {
                input = _baseOpp.Add(input);
            }
            else
            {
                input = _baseOpp.Update(input);
            }
            return input;
        }

        [HttpPost]
        public IList<Product> GetAll()
        {
            List<Product> result = new List<Product>();
            try
            {
                var predicate = PredicateBuilder.True<Product>();
                predicate = predicate.And(x=>!x.IsDeleted);

                result = _baseOpp.GetAllUsingExpression(out int totalCount,1,0, predicate).ToList();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        [HttpPost]
        public bool Delete([FromBody]Product input)
        {
            try
            {
                Expression<Func<Product, object>>[] properties =
                    new Expression<Func<Product, object>>[] { x => x.IsDeleted, x => x.ModifiedBy, x => x.ModifiedOn };

                input.IsDeleted = true;
                _baseOpp.UpdateColumn(input, properties);
            }
            catch (Exception ex)
            {
            }
            return true;
        }
    }
}