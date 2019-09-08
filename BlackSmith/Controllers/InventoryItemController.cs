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
    [Route("api/InventoryItem/[action]")]
    public class InventoryItemController : BaseController
    {
        private IOperation<InventoryItem> _baseOpp;

        public InventoryItemController(IOperation<InventoryItem> baseOpp)
        {
            _baseOpp = baseOpp;
        }

        [HttpPost]
        public InventoryItem Save([FromBody]InventoryItem input)
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
        public IList<InventoryItem> GetAll()
        {
            List<InventoryItem> result = new List<InventoryItem>();
            try
            {
                var predicate = PredicateBuilder.True<InventoryItem>();
                predicate = predicate.And(x => !x.IsDeleted);

                result = _baseOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate).ToList();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        [HttpPost]
        public bool Delete([FromBody]InventoryItem input)
        {
            try
            {
                Expression<Func<InventoryItem, object>>[] properties =
                    new Expression<Func<InventoryItem, object>>[] { x => x.IsDeleted, x => x.ModifiedBy, x => x.ModifiedOn };

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