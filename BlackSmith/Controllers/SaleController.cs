using BlackSmithCore;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using ViewModels;

namespace BlackSmithAPI.Controllers
{
    // [Produces("application/json")]
    [Route("api/Sale/[action]")]
    public class SaleController : Controller
    {
        private IOperation<Sale> _saleOpp;
        private IOperation<SaleDetail> _saleDetailOpp;
        private IOperation<Product> _productOpp;
        private IOperation<SalePayment> _salePaymentOpp;
        private IConfiguration _configuration;

        public SaleController(
            IOperation<Sale> saleOpp,
            IOperation<SaleDetail> saleDetailOpp,
            IOperation<SalePayment> salePaymentOpp,
           IOperation<Product> productOpp,
            IConfiguration configuration
            )
        {
            _saleOpp = saleOpp;
            _saleDetailOpp = saleDetailOpp;
            _salePaymentOpp = salePaymentOpp;
            _configuration = configuration;
            _productOpp = productOpp;
        }

        public IActionResult GetSaleListOnExcel([FromQuery]string searchObject)
        {
            dynamic ret = null;
            try
            {
                SearchObject input = JsonConvert.DeserializeObject<SearchObject>(searchObject);
                SaleList result = GetSaleList(input);

                MemoryStream memoryStream = new MemoryStream();

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Report" };

                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    // Constructing header
                    DocumentFormat.OpenXml.Spreadsheet.Row row = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    row.Append(
                       ConstructCell("From", CellValues.String),
                       ConstructCell(input.FromDate.ToString("dd/MM/yyyy"), CellValues.String),
                       ConstructCell("To", CellValues.String),
                       ConstructCell(input.ToDate.ToString("dd/MM/yyyy"), CellValues.String));

                    sheetData.AppendChild(row);
                    sheetData.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Row());

                    DocumentFormat.OpenXml.Spreadsheet.Row heading = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    heading.Append(
                        ConstructCell("BillNo", CellValues.String),
                        ConstructCell("Customer", CellValues.String),
                        ConstructCell("Amount (Rs)", CellValues.String),
                        ConstructCell("Date(DD/MM/YYYY)", CellValues.String));

                    // Insert the header row to the Sheet Data
                    sheetData.AppendChild(heading);

                    // Inserting each employee
                    foreach (var eachSale in result.Sales)
                    {
                        row = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        row.Append(
                            ConstructCell(eachSale.BillId, CellValues.Number),
                            ConstructCell(eachSale.Customer.Name, CellValues.String),
                            ConstructCell(eachSale.FinalTotal.ToString(), CellValues.Number),
                            ConstructCell(eachSale.BillDate.ToString("dd/MM/yyyy"), CellValues.String));

                        sheetData.AppendChild(row);
                    }
                    worksheetPart.Worksheet.Save();
                }
                memoryStream.Position = 0;
                ret = new FileStreamResult(memoryStream, "application/octet-stream");
            }
            catch (Exception ex)
            {

            }
            return ret;
        }

        public SaleList GetSaleListOnScreen([FromBody]SearchObject input)
        {
            return GetSaleList(input);
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

                        result.Due = Math.Round(result.FinalTotal - result.TotalPaid, 2);
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

                            var product = _productOpp.GetUsingId(each.FK_ProductId);

                            Expression<Func<Product, object>>[] properties =
                                new Expression<Func<Product, object>>[] { x => x.Availibility };

                            product.Availibility = product.Availibility - each.Quantity;
                            _productOpp.UpdateColumn(product, properties);
                        }
                        input.SaleDetails = saleDetails;
                    }


                    SaveBillInBillStore(input);
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


        private DocumentFormat.OpenXml.Spreadsheet.Cell ConstructCell(string value, CellValues dataType)
        {
            return new DocumentFormat.OpenXml.Spreadsheet.Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        private SaleList GetSaleList(SearchObject input)
        {
            SaleList result = new SaleList();
            try
            {
                Expression<Func<Sale, object>>[] exp = new Expression<Func<Sale, object>>[] { x => x.SalePayments, x => x.SaleDetails, x => x.Customer };
                var predicate = PredicateBuilder.True<Sale>();
                predicate = predicate.And(x => !x.IsDeleted);

                if (input.SaleIds != null && input.SaleIds.Count > 0)
                {
                    predicate = predicate.And(x => input.SaleIds.Contains(x.Id));
                }
                else
                {
                    if (input.FromDate != null)
                        predicate = predicate.And(x => x.BillDate.Date >= input.FromDate.Date);

                    if (input.ToDate != null)
                        predicate = predicate.And(x => x.BillDate.Date <= input.ToDate.Date);
                }

                if (!string.IsNullOrWhiteSpace(input.BillIds))
                {
                    try
                    {
                        var billIds = input.BillIds.Split(',').ToList().ConvertAll(c => c.ToUpper().Trim());
                        if (billIds.Count == 1)
                        {
                            billIds = input.BillIds.Split(':').ToList().ConvertAll(c => c.ToUpper().Trim());

                            if (billIds.Count == 2)
                            {
                                long billStart = Convert.ToInt64(billIds[0].Split('-')[1]);
                                long billEnd = Convert.ToInt64(billIds[1].Split('-')[1]);
                                billIds.Clear();

                                string billInitial = _configuration["Configuration:BillInitial"];
                                string billSeperator = _configuration["Configuration:BillSeperator"];

                                while (billStart <= billEnd)
                                {
                                    billIds.Add(billInitial + billSeperator + billStart);
                                    billStart++;
                                }
                            }
                        }
                        predicate = predicate.And(x => billIds.Contains(x.BillId.ToUpper().Trim()));
                    }
                    catch (Exception)
                    {
                    }
                }
                if (input.CustomerIds != null && input.CustomerIds.Count > 0)
                    predicate = predicate.And(x => input.CustomerIds.Contains(x.FK_CustomerId));

                result.Sales = _saleOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate, null, null, exp).OrderByDescending(x => x.BillDate).ToList();

                var predicateProd = PredicateBuilder.True<Product>();
                predicateProd = predicateProd.And(x => !x.IsDeleted);
                var productList = _productOpp.GetAllUsingExpression(out int total, 1, 0, predicateProd).ToList();

                //nullifying to avoid object chain

                if (result.Sales != null)
                {
                    result.Sales.ForEach(x =>
                    {
                        if (x.SaleDetails != null)
                            x.SaleDetails.ForEach(y =>
                            {
                                y.Sale = null;
                                y.Product = productList.Where(p => p.Id == y.FK_ProductId).FirstOrDefault();
                            });

                        if (x.SalePayments != null)
                            x.SalePayments.ForEach(y => { y.Sale = null; x.TotalPaid = x.TotalPaid + y.Amount; });
                        if (x.Customer != null)
                            x.Customer.Sales = null;

                        x.Due = Math.Round(x.FinalTotal - x.TotalPaid, 2);

                    });
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private bool SaveBillInBillStore(Sale input)
        {
            try
            {
                SearchObject s = new SearchObject() { SaleIds = new List<long>() { input.Id } };

                var result = GetSaleList(s);

                if (result != null && result.Sales != null)
                {
                    input = result.Sales.FirstOrDefault();
                }
                if (input != null)
                {
                    input.FinalTotalInWords = input.FinalTotalInWords = "Rupees " + GetNumberInWords(input.FinalTotal) + " Only";

                    string fileNameExisting = @_configuration["Configuration:BillFormatTemplatePath"];
                    string fileNameNew = @_configuration["Configuration:BillStorePath"] + input.Id + "_" + input.BillId + ".pdf";

                    using (var existingFileStream = new FileStream(fileNameExisting, FileMode.Open))
                    using (var newFileStream = new FileStream(fileNameNew, FileMode.Create))
                    {
                        var pdfReader = new PdfReader(existingFileStream);
                        var stamper = new PdfStamper(pdfReader, newFileStream);

                        var form = stamper.AcroFields;
                        var keys = form.Fields.Keys;
                        List<string> fieldKeys = new List<string>();
                        foreach (string each in keys)
                        {
                            fieldKeys.Add(each);
                        }

                        var customerName = fieldKeys.Find(x => x.ToUpper() == "CUSTOMERNAME");
                        if (customerName != null && input.Customer != null && input.Customer.Name != null)
                        {
                            form.SetField(customerName, input.Customer.Name);
                        }

                        var customerGSTIN = fieldKeys.Find(x => x.ToUpper() == "CUSTOMERGSTIN");
                        if (customerGSTIN != null && input.Customer != null && input.Customer.GSTIN != null)
                        {
                            form.SetField(customerGSTIN, input.Customer.GSTIN);
                        }

                        var customerPAN = fieldKeys.Find(x => x.ToUpper() == "CUSTOERPAN");
                        if (customerPAN != null && input.Customer != null && input.Customer.PAN != null)
                        {
                            form.SetField(customerPAN, input.Customer.PAN);
                        }

                        var invoiceNo = fieldKeys.Find(x => x.ToUpper() == "INVOICENO");
                        if (invoiceNo != null && input.BillId != null)
                        {
                            form.SetField(invoiceNo, input.BillId);
                        }

                        var invoiceDate = fieldKeys.Find(x => x.ToUpper() == "INVOICEDATE");
                        if (invoiceDate != null && input.BillDate != null)
                        {
                            form.SetField(invoiceDate, input.BillDate.ToString("dd-MM-yyyy"));
                        }

                        var paymentTerm = fieldKeys.Find(x => x.ToUpper() == "PAYMENTTERMS");
                        if (paymentTerm != null && input.PaymentTerm != null)
                        {
                            form.SetField(paymentTerm, input.PaymentTerm);
                        }

                        var dispatchThru = fieldKeys.Find(x => x.ToUpper() == "INVOICETHROUGH");
                        if (dispatchThru != null && input.DispatchThru != null)
                        {
                            form.SetField(dispatchThru, input.DispatchThru);
                        }

                        var total = fieldKeys.Find(x => x.ToUpper() == "TOTAL");
                        if (total != null)
                        {
                            form.SetField(total, input.Total.ToString());
                        }

                        var cgst = fieldKeys.Find(x => x.ToUpper() == "CGST");
                        if (cgst != null)
                        {
                            form.SetField(cgst, input.CGSTTax.ToString());
                        }

                        var sgst = fieldKeys.Find(x => x.ToUpper() == "SGST");
                        if (sgst != null)
                        {
                            form.SetField(sgst, input.SGSTTax.ToString());
                        }

                        var discount = fieldKeys.Find(x => x.ToUpper() == "DISCOUNT");
                        if (discount != null)
                        {
                            form.SetField(discount, input.Discount.ToString());
                        }

                        var finalTotal = fieldKeys.Find(x => x.ToUpper() == "FINALTOTAL");
                        if (finalTotal != null)
                        {
                            form.SetField(finalTotal, input.FinalTotal.ToString());
                        }

                        var billingAddress = fieldKeys.Find(x => x.ToUpper() == "BILLINGADDRESS");
                        if (billingAddress != null && input.Customer != null && input.Customer.Address != null)
                        {
                            form.SetField(billingAddress, input.Customer.Address);
                        }

                        var shippingAddress = fieldKeys.Find(x => x.ToUpper() == "SHIPPINGADDRESS");
                        if (shippingAddress != null)
                        {
                            form.SetField(shippingAddress, input.Customer.Address);
                        }

                        var inWords = fieldKeys.Find(x => x.ToUpper() == "INWORDS");
                        if (inWords != null && input.FinalTotalInWords != null)
                        {
                            form.SetField(inWords, input.FinalTotalInWords);
                        }

                        if (input.SaleDetails != null && input.SaleDetails.Count > 0)
                        {
                            for (int i = 0; i < input.SaleDetails.Count; i++)
                            {
                                var sl = fieldKeys.Find(x => x.ToUpper() == "SL" + (i + 1));
                                if (sl != null)
                                {
                                    form.SetField(sl, (i + 1).ToString());
                                }

                                var hsn = fieldKeys.Find(x => x.ToUpper() == "HSN" + (i + 1));
                                if (hsn != null)
                                {
                                    form.SetField(hsn, _configuration["Configuration:HSN"]);
                                }

                                var desc = fieldKeys.Find(x => x.ToUpper() == "DESC" + (i + 1));
                                if (desc != null)
                                {
                                    form.SetField(desc, input.SaleDetails[i].Product != null ? input.SaleDetails[i].Product.Name : string.Empty);
                                }

                                var qty = fieldKeys.Find(x => x.ToUpper() == "QTY" + (i + 1));
                                if (qty != null)
                                {
                                    form.SetField(qty, input.SaleDetails[i].Quantity.ToString());
                                }

                                var unit = fieldKeys.Find(x => x.ToUpper() == "UNIT" + (i + 1));
                                if (unit != null)
                                {
                                    form.SetField(unit, "Piece");
                                }

                                var rate = fieldKeys.Find(x => x.ToUpper() == "RATE" + (i + 1));
                                if (rate != null)
                                {
                                    form.SetField(rate, input.SaleDetails[i].Price.ToString());
                                }

                                var amount = fieldKeys.Find(x => x.ToUpper() == "AMT" + (i + 1));
                                if (amount != null)
                                {
                                    double amt = Math.Round((input.SaleDetails[i].Price * input.SaleDetails[i].Quantity), 2);
                                    form.SetField(amount, amt.ToString());
                                }
                            }
                        }
                        stamper.FormFlattening = true; // "Flatten" the form so it wont be editable/usable anymore


                        // You can also specify fields to be flattened, which
                        // leaves the rest of the form still be editable/usable
                        //stamper.PartialFormFlattening("field1");

                        stamper.Close();
                        pdfReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

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


        public FileStreamResult DownloadBill([FromQuery] string id)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    id.Replace(" ", string.Empty);
                    List<string> ids = id.Split(',').ToList().ConvertAll(c => c.ToUpper().Trim());
                    List<string> paths = new List<string>();
                    string tempOutPath = _configuration["Configuration:BillStorePath"] + "temp.pdf";

                    System.IO.File.Delete(tempOutPath);

                    foreach (var each in ids)
                    {
                        if (!string.IsNullOrWhiteSpace(each))
                        {
                            var input = _saleOpp.GetUsingId(Convert.ToInt64(each));

                            paths.Add(_configuration["Configuration:BillStorePath"] + input.Id + "_" + input.BillId + ".pdf");
                        }
                    }

                    CombineMultiplePDFs(paths, tempOutPath);

                    using (FileStream file = new FileStream(tempOutPath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        memoryStream.Write(bytes, 0, (int)file.Length);
                    }

                    System.IO.File.Delete(tempOutPath);

                    memoryStream.Position = 0;
                    return new FileStreamResult(memoryStream, "application/pdf");
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        private void CombineMultiplePDFs(List<string> fileNames, string outFile)
        {
            // step 1: creation of a document-object
            Document document = new Document();
            //create newFileStream object which will be disposed at the end
            using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
            {
                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, newFileStream);
                if (writer == null)
                {
                    return;
                }

                // step 3: we open the document
                document.Open();

                foreach (string fileName in fileNames)
                {
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    PrAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        writer.CopyAcroForm(reader);
                    }

                    reader.Close();
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();
            }//disposes the newFileStream object
        }



        #endregion
    }
}
