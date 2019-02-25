using BlackSmithCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using ViewModels;

namespace BlackSmithAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Sale/[action]")]
    public class SaleController : Controller
    {
        private IOperation<Sale> _saleOpp;
        private IOperation<SaleDetail> _saleDetailOpp;
        private IConfiguration _configuration;



        public SaleController(
            IOperation<Sale> saleOpp,
            IOperation<SaleDetail> saleDetailOpp,
            IConfiguration configuration
            )
        {
            _saleOpp = saleOpp;
            _saleDetailOpp = saleDetailOpp;
            _configuration = configuration;
        }

        public Sale Save([FromBody] Sale input)
        {
            try
            {
                if (input!=null && input.SaleDetails != null)
                {
                    input.BillId = GenerateBillId();
                    List<SaleDetail> saleDetails = input.SaleDetails;
                    input.SaleDetails = null;

                    input = _saleOpp.Add(input);

                    foreach(var each in saleDetails)
                    {
                        each.FK_SaleId = input.Id;
                        each.CreatedBy = input.CreatedBy;
                        _saleDetailOpp.Add(each);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return input;
        }


        private string GenerateBillId()
        {
            string billId = string.Empty;
            try
            {
              
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return billId;
        }
    }
}
