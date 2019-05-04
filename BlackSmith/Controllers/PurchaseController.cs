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
    public class PurchaseController : Controller
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
                    Expression<Func<Purchase, object>>[] exp = new Expression<Func<Purchase, object>>[] { x => x.PurchasePayments, x => x.PurchaseDetails , x=>x.Suplier};
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

                        result.Due = Math.Round(result.FinalTotal - result.TotalPaid, 2);
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

                    input.PurchaseDate = DateTime.Now;
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
                        }
                        input.PurchaseDetails = purchaseDetails;
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

        private PurchaseList GetPurchaseList(SearchObject input)
        {
            PurchaseList result = new PurchaseList();
            try
            {
                Expression<Func<Purchase, object>>[] exp = new Expression<Func<Purchase, object>>[] { x => x.PurchasePayments, x => x.PurchaseDetails,x=>x.Suplier };
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

                        x.Due = Math.Round(x.FinalTotal - x.TotalPaid, 2);

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
                    input.Total = 0; input.SGSTTax = 0; input.CGSTTax = 0; input.FinalTotal = 0;

                    if (input.PurchaseDetails != null)
                    {
                        foreach (var each in input.PurchaseDetails)
                        {
                            each.Total = Math.Round(each.Quantity * each.Price, 2);
                            input.Total = Math.Round(input.Total + each.Total, 2);
                        }

                        input.CGSTTax = Math.Round(input.Total * input.CGSTRate / 100, 2);
                        input.SGSTTax = Math.Round(input.Total * input.SGSTRate / 100, 2);

                        input.FinalTotal = Math.Round(input.Total + input.CGSTTax + input.SGSTTax - input.Discount, 2);
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

        #endregion
    }
}
