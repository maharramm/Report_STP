using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportSTP.Models.Report
{
    public class CurrentAccountBalance
    {

        public string Definition { get; set; } 
        public string Code { get; set; }
        public double? Remainder { get; set; }
        public string Currency { get; set; }
        public string Personnel { get; set; }
    }
}
