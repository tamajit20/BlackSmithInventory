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
    [Route("api/Purchase/[action]")]
    public class PurchaseController : BaseController
    {
        private IOperation<Purchase> _purchaseOpp;
        private IOperation<PurchaseDetail> _purchaseDetailOpp;
        private IOperation<InventoryItem> _itemOpp;
        private IOperation<PurchasePayment> _purchasePaymentOpp;
        private IConfiguration _configuration;

        public PurchaseController(
            IOperation<Purchase> purchaseOpp,
            IOperation<PurchaseDetail> purchaseDetailOpp,
            IOperation<PurchasePayment> purchasePaymentOpp,
           IOperation<InventoryItem> itemOpp,
            IConfiguration configuration
            )
        {
            _purchaseOpp = purchaseOpp;
            _purchaseDetailOpp = purchaseDetailOpp;
            _purchasePaymentOpp = purchasePaymentOpp;
            _configuration = configuration;
            _itemOpp = itemOpp;
        }

        public IActionResult GetPurchaseListOnExcel([FromQuery]string searchObject)
        {
            dynamic ret = null;
            try
            {
                SearchObject input = JsonConvert.DeserializeObject<SearchObject>(searchObject);
                PurchaseList result = GetPurchaseList(input);

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
                    foreach (var each in result.Purchases)
                    {
                        row = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        row.Append(
                            ConstructCell(each.PurchaseId, CellValues.Number),
                            //ConstructCell(each.Customer.Name, CellValues.String),
                            ConstructCell(each.FinalTotal.ToString(), CellValues.Number),
                            ConstructCell(each.PurchaseDate.ToString("dd/MM/yyyy"), CellValues.String));

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

        public PurchaseList GetPurchaseListOnScreen([FromBody]SearchObject input)
        {
            return GetPurchaseList(input);
        }


        public PurchasePayment GetOne([FromBody] PurchasePayment input)
        {
            try
            {
                if (input != null)
                {
                    Expression<Func<Purchase, object>>[] exp = new Expression<Func<Purchase, object>>[] { x => x.PurchasePayments, x => x.PurchaseDetails, x => x.Suplier };
                    var predicate = PredicateBuilder.True<Purchase>();
                    predicate = predicate.And(x => !x.IsDeleted);

                    if (!string.IsNullOrWhiteSpace(input.BillId))
                        predicate = predicate.And(x => x.PurchaseId.ToUpper().Trim() == input.BillId.ToUpper().Trim());

                    if (input.FK_PurchaseId > 0)
                    {
                        predicate = predicate.And(x => x.Id == input.FK_PurchaseId);
                    }

                    var result = _purchaseOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate, null, null, exp).FirstOrDefault();

                    if (result != null)
                    {
                        if (result.PurchasePayments != null)
                        {
                            foreach (var each in result.PurchasePayments)
                            {
                                result.TotalPaid = result.TotalPaid + each.Amount;
                            }
                        }

                        result.Due = Math.Round(result.RoundOffTotal - result.TotalPaid, 2);
                        //nullifying to avoid object chain
                        if (result.PurchasePayments != null)
                            result.PurchasePayments.ForEach(x => x.Purchase = null);

                        if (result.Suplier != null)
                            result.Suplier.Purchases = null;

                        if (result.PurchaseDetails != null)
                            result.PurchaseDetails.ForEach(x => x.Purchase = null);

                        input.Purchase = result;
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

        public PurchasePayment Payment([FromBody] PurchasePayment input)
        {
            try
            {
                if (input != null)
                {
                    input.Purchase = null;

                    if (input.Id <= 0)
                    {
                        input.Id = 0;
                        _purchasePaymentOpp.Add(input);
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

        public FileStreamResult DownloadBill([FromQuery] string id)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var input = _purchaseOpp.GetUsingId(Convert.ToInt64(id));

                    using (FileStream file = new FileStream(_configuration["Configuration:BillStorePath"] + input.Id + "_" + input.PurchaseId + ".pdf", FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        memoryStream.Write(bytes, 0, (int)file.Length);
                    }
                    memoryStream.Position = 0;
                    return new FileStreamResult(memoryStream, "application/pdf");
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        public Purchase Save([FromBody] Purchase input)
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

                    if (input != null && input.PurchaseDetails != null)
                    {

                        List<PurchaseDetail> purchaseDetails = input.PurchaseDetails;
                        input.PurchaseDetails = null;

                        input.Id = 0;
                        input = _purchaseOpp.Add(input);

                        foreach (var each in purchaseDetails)
                        {
                            each.FK_PurchaseId = input.Id;
                            each.CreatedBy = input.CreatedBy;
                            each.Id = 0;
                            _purchaseDetailOpp.Add(each);

                            var inventoryItem = _itemOpp.GetUsingId(each.Fk_InventoryItemId);

                            Expression<Func<InventoryItem, object>>[] properties =
                                new Expression<Func<InventoryItem, object>>[] { x => x.Availibility };

                            inventoryItem.Availibility = inventoryItem.Availibility + each.Quantity;
                            _itemOpp.UpdateColumn(inventoryItem, properties);
                        }
                        input.PurchaseDetails = purchaseDetails;
                    }

                    input.FinalTotalInWords = "Rupees " + GetNumberInWords(input.RoundOffTotal) + " Only";

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

        private PurchaseList GetPurchaseList(SearchObject input)
        {
            PurchaseList result = new PurchaseList();
            try
            {
                Expression<Func<Purchase, object>>[] exp = new Expression<Func<Purchase, object>>[] { x => x.PurchasePayments, x => x.PurchaseDetails, x => x.Suplier };
                var predicate = PredicateBuilder.True<Purchase>();
                predicate = predicate.And(x => !x.IsDeleted);


                if (input.PurchaseIds != null && input.PurchaseIds.Count > 0)
                {
                    predicate = predicate.And(x => input.SaleIds.Contains(x.Id));
                }
                else
                {
                    if (input.FromDate != null)
                        predicate = predicate.And(x => x.PurchaseDate.Date >= input.FromDate.Date);

                    if (input.ToDate != null)
                        predicate = predicate.And(x => x.PurchaseDate.Date <= input.ToDate.Date);
                }

                if (!string.IsNullOrWhiteSpace(input.BillIds))
                {
                    var billIds = input.BillIds.Split(',').ToList().ConvertAll(c => c.ToUpper().Trim());
                    predicate = predicate.And(x => billIds.Contains(x.PurchaseId.ToUpper().Trim()));
                }

                result.Purchases = _purchaseOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate, null, null, exp).OrderByDescending(x => x.PurchaseDate).ToList();

                var predicateProd = PredicateBuilder.True<InventoryItem>();
                predicateProd = predicateProd.And(x => !x.IsDeleted);
                var itemList = _itemOpp.GetAllUsingExpression(out int total, 1, 0, predicateProd).ToList();

                //nullifying to avoid object chain

                if (result.Purchases != null)
                {
                    result.Purchases.ForEach(x =>
                    {
                        if (x.PurchaseDetails != null)
                            x.PurchaseDetails.ForEach(y =>
                            {
                                y.Purchase = null;
                                y.Item = itemList.Where(p => p.Id == y.Fk_InventoryItemId).FirstOrDefault();
                            });

                        if (x.PurchasePayments != null)
                            x.PurchasePayments.ForEach(y => { y.Purchase = null; x.TotalPaid = x.TotalPaid + y.Amount; });

                        if (x.Suplier != null)
                            x.Suplier.Purchases = null;

                        x.Due = Math.Round(x.RoundOffTotal - x.TotalPaid, 2);

                    });
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private bool SaveBillInBillStore(Purchase input)
        {
            return true;
        }

        private bool HardDelete(Purchase input)
        {
            try
            {
                if (input != null && input.Id > 0)
                {
                    var predicate = PredicateBuilder.True<PurchaseDetail>();
                    predicate = predicate.And(x => x.FK_PurchaseId == input.Id);

                    var saleDetails = _purchaseDetailOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate);

                    if (saleDetails != null)
                    {
                        foreach (var each in saleDetails)
                        {
                            if (each.Id > 0)
                                _purchaseDetailOpp.Delete(each.Id);
                        }
                    }
                    _purchaseOpp.Delete(input.Id);
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

        private Purchase Calculate(ref Purchase input)
        {
            try
            {
                if (input != null)
                {
                    // input.Total = 0; input.SGSTTax = 0; input.CGSTTax = 0; input.FinalTotal = 0;

                    if (input.PurchaseDetails != null)
                    {
                        foreach (var each in input.PurchaseDetails)
                        {
                            each.Total = Math.Round(each.Quantity * each.Price, 2);
                            input.Total = Math.Round(input.Total + each.Total, 2);
                        }

                        //   input.CGSTTax = Math.Round(input.Total * input.CGSTRate / 100, 2);
                        //   input.SGSTTax = Math.Round(input.Total * input.SGSTRate / 100, 2);

                        input.FinalTotal = input.Total + input.CGSTTax + input.SGSTTax - input.Discount;
                        input.RoundOffTotal = Math.Round(input.FinalTotal,0,MidpointRounding.AwayFromZero);
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


        private string GetNumberInWords(decimal input)
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
