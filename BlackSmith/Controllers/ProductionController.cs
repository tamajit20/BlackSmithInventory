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
    [Route("api/Production/[action]")]
    public class ProductionController : Controller
    {
        private IOperation<Production> _baseOpp;
        private IOperation<ProductionInventoryItem> _prodInventoryOpp;
        private IOperation<ProductionProduct> _prodProductOpp;
        private IOperation<InventoryItem> _inventoryItemOpp;
        private IOperation<Product> _productOpp;


        public ProductionController(
            IOperation<Production> baseOpp,
            IOperation<ProductionInventoryItem> prodInventoryOpp,
            IOperation<ProductionProduct> prodProducOpp,
            IOperation<InventoryItem> inventoryItemOpp,
            IOperation<Product> productOpp
            )
        {
            _baseOpp = baseOpp;
            _prodInventoryOpp = prodInventoryOpp;
            _prodProductOpp = prodProducOpp;
            _inventoryItemOpp = inventoryItemOpp;
            _productOpp = productOpp;
        }

        [HttpPost]
        public Production Save([FromBody]Production input)
        {
            try
            {
                if (input != null && input.Id == 0)
                {
                    List<ProductionInventoryItem> prodInv = input.ProductionInventoryItems;
                    List<ProductionProduct> prodProd = input.ProductionProducts;

                    input.ProductionInventoryItems = null;
                    input.ProductionProducts = null;
                    input = _baseOpp.Add(input);

                    if (prodInv != null)
                    {
                        foreach (var each in prodInv)
                        {
                            each.FK_ProductionId = input.Id;
                            each.CreatedBy = input.CreatedBy;
                            each.Id = 0;
                            _prodInventoryOpp.Add(each);

                            var inventoryItem = _inventoryItemOpp.GetUsingId(each.FK_InventoryItemId);

                            Expression<Func<InventoryItem, object>>[] properties =
                                new Expression<Func<InventoryItem, object>>[] { x => x.Availibility };

                            inventoryItem.Availibility = inventoryItem.Availibility - each.Quantity;
                            _inventoryItemOpp.UpdateColumn(inventoryItem, properties);
                        }
                    }

                    if (prodProd != null)
                    {
                        foreach (var each in prodProd)
                        {
                            each.FK_ProductionId = input.Id;
                            each.CreatedBy = input.CreatedBy;
                            each.Id = 0;
                            _prodProductOpp.Add(each);

                            var product = _productOpp.GetUsingId(each.FK_ProductId);

                            Expression<Func<Product, object>>[] properties =
                                new Expression<Func<Product, object>>[] { x => x.Availibility };

                            product.Availibility = product.Availibility + each.Quantity;
                            _productOpp.UpdateColumn(product, properties);
                        }
                    }

                    input.ProductionInventoryItems = prodInv;
                    input.ProductionProducts = prodProd;
                }
            }
            catch (Exception ex)
            {
                input.IsFailure = true;
                input.Msg = "";
            }
            return input;
        }

        [HttpPost]
        public IList<Production> GetAll()
        {
            List<Production> result = new List<Production>();
            try
            {
                var predicate = PredicateBuilder.True<Production>();
                predicate = predicate.And(x => !x.IsDeleted);

                result = _baseOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate).ToList();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        [HttpPost]
        public bool Delete([FromBody]Production input)
        {
            try
            {
                Expression<Func<Production, object>>[] properties =
                    new Expression<Func<Production, object>>[] { x => x.IsDeleted, x => x.ModifiedBy, x => x.ModifiedOn };

                input.IsDeleted = true;
                _baseOpp.UpdateColumn(input, properties);
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        public ProductionList GetProductionListOnScreen([FromBody]SearchObject input)
        {
            return GetProductionList(input);
        }


        private ProductionList GetProductionList(SearchObject input)
        {
            ProductionList result = new ProductionList();
            try
            {
                Expression<Func<Production, object>>[] exp = new Expression<Func<Production, object>>[] { x => x.ProductionInventoryItems, x => x.ProductionProducts };
                var predicate = PredicateBuilder.True<Production>();
                predicate = predicate.And(x => !x.IsDeleted);

                if (input.ProductIds != null && input.ProductIds.Count > 0)
                {
                    predicate = predicate.And(x => input.ProductIds.Contains(x.Id));
                }
                else
                {
                    if (input.FromDate != null)
                        predicate = predicate.And(x => x.Date.Date >= input.FromDate.Date);

                    if (input.ToDate != null)
                        predicate = predicate.And(x => x.Date.Date <= input.ToDate.Date);
                }

                result.Productions = _baseOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate, null, null, exp).OrderByDescending(x => x.Date).ToList();

                   //nullifying to avoid object chain

                if (result.Productions != null)
                {
                    result.Productions.ForEach(x =>
                    {
                        if (x.ProductionInventoryItems != null)
                            x.ProductionInventoryItems.ForEach(y => {
                                y.Production = null;
                                y.InventoryItem = _inventoryItemOpp.GetUsingId(y.FK_InventoryItemId);
                            });

                        if (x.ProductionProducts != null)
                            x.ProductionProducts.ForEach(y => {
                                y.Production = null;
                                y.Product = _productOpp.GetUsingId(y.FK_ProductId);
                            });


                    });
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

    }
}