using BlackSmithCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private IOperation<SalePayment> _salePaymentOpp;
        private IConfiguration _configuration;



        public SaleController(
            IOperation<Sale> saleOpp,
            IOperation<SaleDetail> saleDetailOpp,
            IOperation<SalePayment> salePaymentOpp,
            IConfiguration configuration
            )
        {
            _saleOpp = saleOpp;
            _saleDetailOpp = saleDetailOpp;
            _salePaymentOpp = salePaymentOpp;
            _configuration = configuration;
        }

        public SalePayment GetOne([FromBody] SalePayment input)
        {
            try
            {
                if (input != null)
                {
                    Expression<Func<Sale, object>>[] exp = new Expression<Func<Sale, object>>[] { x => x.SalePayments, x => x.SaleDetails, x => x.Customer };
                    var predicate = PredicateBuilder.True<Sale>();
                    predicate = predicate.And(x => !x.IsDeleted);

                    if (!string.IsNullOrWhiteSpace(input.BillId))
                        predicate = predicate.And(x => x.BillId.ToUpper().Trim() == input.BillId.ToUpper().Trim());

                    if (input.FK_SaleId > 0)
                    {
                        predicate = predicate.And(x => x.Id == input.FK_SaleId);
                    }

                    var result = _saleOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate, null, null, exp).FirstOrDefault();

                    if (result != null)
                    {
                        if (result.SalePayments != null)
                        {
                            foreach (var each in result.SalePayments)
                            {
                                result.TotalPaid = result.TotalPaid + each.Amount;
                            }
                        }
                        //nullifying to avoid object chain
                        if (result.SalePayments != null)
                            result.SalePayments.ForEach(x => x.Sale = null);

                        if (result.Customer != null)
                            result.Customer.Sales = null;

                        if (result.SaleDetails != null)
                            result.SaleDetails.ForEach(x => x.Sale = null);

                        input.Sale = result;
                    }
                }
            }
            catch (Exception ex)
            {
                input.IsFailure = true;
                input.Msg = "";
            }
            return input;
        }

        public SalePayment Payment([FromBody] SalePayment input)
        {
            try
            {
                if (input != null)
                {
                    input.Sale = null;

                    if (input.Id <= 0)
                    {
                        input.Id = 0;
                        _salePaymentOpp.Add(input);
                     }

                    input = GetOne(input);
                }
            }
            catch (Exception ex)
            {
                input.Msg = "";
                input.IsFailure = true;
            }
            return input;
        }
        public dynamic Download([FromBody]Sale input)
        {
            try
            {
                Expression<Func<Sale, object>>[] exp = new Expression<Func<Sale, object>>[] { x => x.SaleDetails, x => x.Customer };
                var predicate = PredicateBuilder.True<Sale>();
                predicate = predicate.And(x => x.Id == input.Id);

                var result = _saleOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate, null, null, exp).FirstOrDefault();

                PdfDocument document = new PdfDocument();

            }
            catch (Exception ex)
            {
                input.Msg = "";
                input.IsFailure = true;
            }
            return true;
        }

        public Sale Save([FromBody] Sale input)
        {
            try
            {
                if (input != null)
                {
                    Calculate(ref input);

                    if (input.Id > 0)
                    {
                        HardDelete(input);
                    }
                    if (string.IsNullOrWhiteSpace(input.BillId))
                    {
                        input.BillId = GenerateBillId();
                    }

                    input.BillDate = DateTime.Now;
                    if (input != null && input.SaleDetails != null)
                    {

                        List<SaleDetail> saleDetails = input.SaleDetails;
                        input.SaleDetails = null;

                        input.Id = 0;
                        input = _saleOpp.Add(input);

                        foreach (var each in saleDetails)
                        {
                            each.FK_SaleId = input.Id;
                            each.CreatedBy = input.CreatedBy;
                            each.Id = 0;
                            _saleDetailOpp.Add(each);
                        }
                        input.SaleDetails = saleDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                input.Msg = "";
                input.IsFailure = true;
            }
            return input;
        }

        #region private methods

        private bool HardDelete(Sale input)
        {
            try
            {
                if (input != null && input.Id > 0)
                {
                    var predicate = PredicateBuilder.True<SaleDetail>();
                    predicate = predicate.And(x => x.FK_SaleId == input.Id);

                    var saleDetails = _saleDetailOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate);

                    if (saleDetails != null)
                    {
                        foreach (var each in saleDetails)
                        {
                            if (each.Id > 0)
                                _saleDetailOpp.Delete(each.Id);
                        }
                    }
                    _saleOpp.Delete(input.Id);
                }
            }
            catch (Exception ex)
            {
                input.Msg = "";
                input.IsFailure = true;
                throw ex;
            }
            return true;
        }

        private Sale Calculate(ref Sale input)
        {
            try
            {
                if (input != null)
                {
                    input.Total = 0; input.SGSTTax = 0; input.CGSTTax = 0; input.FinalTotal = 0;

                    if (input.SaleDetails != null)
                    {
                        foreach (var each in input.SaleDetails)
                        {
                            each.Total = Math.Round(each.Quantity * each.Price, 2);
                            input.Total = Math.Round(input.Total + each.Total, 2);
                        }

                        input.CGSTTax = Math.Round(input.Total * input.CGSTRate / 100, 2);
                        input.SGSTTax = Math.Round(input.Total * input.SGSTRate / 100, 2);

                        input.FinalTotal = Math.Round(input.Total + input.CGSTTax + input.SGSTTax - input.Discount, 2);
                        input.FinalTotalInWords = "Rupees " + GetNumberInWords(input.FinalTotal) + " Only";
                    }
                }
            }
            catch (Exception ex)
            {
                input.Msg = "";
                input.IsFailure = true;
                throw ex;
            }
            return input;
        }

        private string GenerateBillId()
        {
            string billId = string.Empty;
            string billSeperator = string.Empty;
            string billInitial = string.Empty;
            try
            {
                billInitial = _configuration["Configuration:BillInitial"];
                billSeperator = _configuration["Configuration:BillSeperator"];
                billId = billInitial + billSeperator + 1;//default

                var predicate = PredicateBuilder.True<Sale>();
                if (_configuration["Configuration:IgnoreDeleted"].ToLower().Trim() == "false")
                {
                    predicate = predicate.And(x => !x.IsDeleted);
                }
                var result = _saleOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate, x => x.OrderByDescending(y => y.Id)).FirstOrDefault();
                if (result != null)
                {
                    if (!string.IsNullOrWhiteSpace(result.BillId))
                    {
                        string[] splitted = result.BillId.Split(billSeperator);
                        if (splitted.Length > 1)
                        {
                            billId = billInitial + billSeperator + (Convert.ToDouble(splitted[1]) + 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return billId;
        }

        private string GetNumberInWords(double input)
        {
            string isNegative = "";
            string number = string.Empty;
            string ret = "Zero";
            try
            {
                number = input.ToString();
                if (Math.Sign(input) == -1)
                {
                    isNegative = "Minus ";
                    number = number.Substring(1, number.Length - 1);
                }
                if (number != "0")
                {
                    ret = isNegative + ConvertToWords(number);
                }
            }
            catch (Exception ex)
            {
                ret = "Failed";
            }
            return ret;
        }

        private static string ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents  
                        endStr = "Paisa " + endStr;//Cents  
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val.Trim();
        }

        private static string ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros
                        //if (beginsZero) word = " and " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }

        private static string tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private static string ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private static string ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        #endregion
    }
}
