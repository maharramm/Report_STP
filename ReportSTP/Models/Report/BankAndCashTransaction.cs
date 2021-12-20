using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportSTP.Models.Report
{
    public class BankAndCashTransaction
    {
        public int Id { get; set; }
        public string EntryPlace { get; set; }
        public DateTime? Date { get; set; }
        public string ContragentCode { get; set; }
        public string ContragentName { get; set; }
        public string UpperCode { get; set; }
        public string UpperCodeName { get; set; }
        public string Code { get; set; }
        public string CodeName { get; set; }
        public string FicheNo { get; set; }
        public string FicheType { get; set; }
        public string WorkPlace { get; set; }
        public string Division { get; set; }
        public string XERCMADDE { get; set; }
        public string Contract { get; set; }
        public string Employee { get; set; }
        public string ProjectCode { get; set; }
        public double? DebitAZN { get; set; }
        public double? CreditAZN { get; set; }
        public string Currency { get; set; }
        public double? CurrencyAmount { get; set; }
        public string Description { get; set; }
        public string ContractLink1 { get; set; }
        public string ContractLink2 { get; set; }
        public string ContractLink3 { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Adder { get; set; }
        public string Modifier { get; set; }
        public string Locker { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string SpecificCode { get; set; }
        public string SpecificCodeDescription { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }

    }
}
