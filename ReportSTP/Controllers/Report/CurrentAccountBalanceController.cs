using Microsoft.AspNetCore.Mvc;
using ReportSTP.Models.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ReportSTP.Controllers.Report
{
    public class CurrentAccountBalanceController : Controller
    {
        public IActionResult Index(int page=1)
        {
            var result = GetList().ToPagedList(page, 10);
            return View("Index", result);
        }

        public IEnumerable<CurrentAccountBalance> GetList()
        {
            string connectionString = "Server=172.16.1.48;Database=TGR3;User Id=m.aliyev;Password=@a123123;";
            string query = @"SELECT CLC.DEFINITION_,CLC.CODE,A.Qaliq,A.VALYUTA,A.ELEMAN FROM (
                             SELECT 
                                    CLCARD.CODE AS KODU,
                                    CLCARD.DEFINITION_ AS ÜNVANI, 
                                    SUM((1 - CLFLINE.SIGN) * CLFLINE.AMOUNT) - SUM(CLFLINE.SIGN * CLFLINE.AMOUNT) AS Qaliq,
                                    CASE 
                                         WHEN CLCARD.CCURRENCY=0   THEN 'AZN' 
                                         WHEN CLCARD.CCURRENCY=1   THEN 'USD'
                                         WHEN CLCARD.CCURRENCY=20  THEN 'EURO'
                                         WHEN CLCARD.CCURRENCY=53  THEN 'TRY'
                                         WHEN CLCARD.CCURRENCY=51  THEN 'RUBL'
                                         WHEN CLCARD.CCURRENCY=162 THEN 'AZN'
                                         WHEN CLCARD.CCURRENCY=17  THEN 'GBP'
                                    END AS VALYUTA,
                                    [dbo].[azh](MAN.DEFINITION_) ELEMAN,
                                    CLCARD.LOGICALREF,CLCARD.CARDTYPE
                             FROM TGR3.dbo.LG_001_01_CLFLINE AS CLFLINE WITH(NOLOCK) 
                             LEFT OUTER JOIN dbo.LG_001_CLCARD AS CLCARD WITH(NOLOCK) ON CLFLINE.CLIENTREF = CLCARD.LOGICALREF AND CLFLINE.CANCELLED = 0  AND CLCARD.CARDTYPE=3
                             LEFT OUTER JOIN TGR3.dbo.LG_001_SLSCLREL SL  WITH(NOLOCK)  ON CLCARD.LOGICALREF=SL.CLIENTREF
                             LEFT OUTER JOIN TGR3.DBO.LG_SLSMAN MAN WITH(NOLOCK)  ON MAN.LOGICALREF=SL.SALESMANREF
                             WHERE (CLCARD.ACTIVE = 0)   
                             GROUP BY CLCARD.CARDTYPE,CLCARD.CODE, CLCARD.DEFINITION_, CLCARD.ACTIVE,CLCARD.CCURRENCY,MAN.DEFINITION_,CLCARD.LOGICALREF)A
                             RIGHT OUTER JOIN LG_001_CLCARD CLC WITH(NOLOCK)  ON CLC.LOGICALREF=A.LOGICALREF 
                             WHERE CLC.CARDTYPE=3 ";

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            List<CurrentAccountBalance> reportlist = new List<CurrentAccountBalance>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                reportlist.Add
                    (
                        new CurrentAccountBalance
                        {
                            Definition = dataTable.Rows[i]["DEFINITION_"].ToString(),
                            Code = dataTable.Rows[i]["CODE"].ToString(),
                            Remainder = !string.IsNullOrEmpty(dataTable.Rows[i]["Qaliq"].ToString()) ? Convert.ToDouble(dataTable.Rows[i]["Qaliq"]) : 0.0,
                            Currency = dataTable.Rows[i]["VALYUTA"].ToString(),
                            Personnel = dataTable.Rows[i]["ELEMAN"].ToString()
                        }
                    );
            }

            return reportlist;
        }
    }
}
