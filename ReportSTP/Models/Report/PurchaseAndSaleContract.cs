using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportSTP.Models.Report
{
    public class PurchaseAndSaleContract
    {
        public string ContractType { get; set; }
        public string Contract { get; set; }
        public string ContractDocNo { get; set; }
        public string AccountCode { get; set; }
        public string Account { get; set; }
        public string Link1 { get; set; }
        public string Link2 { get; set; }
        public string Link3 { get; set; }
        public string LinkLoadDate { get; set; }
        public string ContractBeginDate { get; set; }
        public string ContractEndDate { get; set; }
        public string ContractCreateDate { get; set; }
        public string CreateBy { get; set; }
        public double Advancepayment { get; set; }
        public int PaymentDay { get; set; }
        public double Payment { get; set; }
        public double NetTotal { get; set; }
        public double ValueAddedTax { get; set; }
        public string SpecialCode { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Warehouse { get; set; }
        public string Factory { get; set; }
        public string Currency { get; set; }
        public string OfferNo { get; set; }
    }
}
