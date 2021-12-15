using Microsoft.AspNetCore.Mvc;
using ReportSTP.Models.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ReportSTP.Controllers.Report
{
    public class PurchaseAndSaleContractController : Controller
    {
        public IActionResult Index(int page=1)
        {
            var result = GetList().ToPagedList(page, 10);
            return View("Index", result);
        }

        public IEnumerable<PurchaseAndSaleContract> GetList()
        {
            string connectionString = "Server=172.16.1.48;Database=TGR3;User Id=m.aliyev;Password=@a123123;";
            string query = @"SELECT 
CASE WHEN A.TRCODE=2 THEN 'PURCHASE_CONTRACT'
WHEN A.TRCODE=1 THEN 'SALES_CONTRACT' END AS CONTRACT_TYPE
,A.FICHENO AS CONTRACT
,A.DOCODE AS CONTRACT_DOC_NO 
,CL.CODE AS ACCOUNT_CODE
,CL.DEFINITION_ AS ACCOUNT
, CT.LINK1 AS LINK1,
CT.LINK2 AS LINK2,
CT.LINK3 AS LINK3,
CT.tarix AS LINK_LOAD_DATE,
A.POFFERBEGDT CONTRAKTBEGDATE,
A.POFFERENDDT CONTRAKTENDDATE,
A.CAPIBLOCK_CREADEDDATE AS CONTRACT_CREATE_DATE
,CAP.NAME AS CREATE_BY,
DEF.NUMFLDS1   AVANS,
DEF.NUMFLDS2 ODEMEGUN,
DEF.NUMFLDS3 ODEME ,
A.NETTOTAL AS NETTOTAL,
A.TOTALVAT AS KDV,
A.SPECODE OZELKOD,
PRO.CODE PROJEKOD,
PRO.NAME PROJEAD,
WH.NAME ANBAR,
DIV.NAME ZAVOD,
CASE WHEN A.TRCURR=0 THEN 'AZN' ELSE (SELECT CURCODE FROM L_CURRENCYLIST WITH(NOLOCK)  WHERE FIRMNR=1 AND CURTYPE=A.TRCURR) END VALUTE
,(SELECT FICHENO FROM  LG_001_PURCHOFFER WHERE TYP=1 AND LOGICALREF=A.OFFALTREF) AS TEKLIF_NO

FROM LG_001_PURCHOFFER A WITH(NOLOCK)
LEFT OUTER JOIN LG_001_CLCARD CL WITH(NOLOCK) ON  CL.LOGICALREF=A.CLIENTREF
LEFT OUTER JOIN [dbo].[CTE] CT  WITH(NOLOCK)  ON CT.GU=A.GUID
LEFT OUTER JOIN L_CAPIUSER CAP WITH(NOLOCK) ON CAP.NR=A.CAPIBLOCK_MODIFIEDBY AND CAP.FIRMNR=1
LEFT OUTER JOIN     LG_001_01_DEFNFLDSTRANV DEF ON DEF.MODULENR=262 AND DEF.PARENTREF=A.LOGICALREF
LEFT OUTER JOIN L_CAPIWHOUSE WH ON WH.NR=A.SOURCEINDEX AND WH.FIRMNR=1
LEFT OUTER JOIN L_CAPIDIV  DIV ON DIV.NR=A.BRANCH AND DIV.FIRMNR=1
LEFT OUTER JOIN LG_001_PROJECT PRO ON PRO.LOGICALREF=A.PROJECTREF
";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            List<PurchaseAndSaleContract> reportlist = new List<PurchaseAndSaleContract>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                reportlist.Add
                    (
                        new PurchaseAndSaleContract
                        {
                            ContractType = dataTable.Rows[i]["CONTRACT_TYPE"].ToString(),
                            Contract = dataTable.Rows[i]["CONTRACT"].ToString(),
                            ContractDocNo = dataTable.Rows[i]["CONTRACT_DOC_NO"].ToString(),
                            AccountCode = dataTable.Rows[i]["ACCOUNT_CODE"].ToString(),
                            Account = dataTable.Rows[i]["ACCOUNT"].ToString(),

                            Link1 = dataTable.Rows[i]["LINK1"].ToString(),
                            Link2 = dataTable.Rows[i]["LINK2"].ToString(),
                            Link3 = dataTable.Rows[i]["LINK3"].ToString(),
                            LinkLoadDate = dataTable.Rows[i]["LINK_LOAD_DATE"].ToString(),

                            ContractBeginDate = dataTable.Rows[i]["CONTRAKTBEGDATE"].ToString(),
                            ContractEndDate = dataTable.Rows[i]["CONTRAKTENDDATE"].ToString(),
                            ContractCreateDate = dataTable.Rows[i]["CONTRACT_CREATE_DATE"].ToString(),
                            CreateBy = dataTable.Rows[i]["CREATE_BY"].ToString(),

                            Advancepayment = !string.IsNullOrEmpty(dataTable.Rows[i]["AVANS"].ToString()) ? Convert.ToDouble(dataTable.Rows[i]["AVANS"]) : 0,
                            PaymentDay = !string.IsNullOrEmpty(dataTable.Rows[i]["ODEMEGUN"].ToString()) ? Convert.ToInt32(dataTable.Rows[i]["ODEMEGUN"]) : 0,
                            Payment = !string.IsNullOrEmpty(dataTable.Rows[i]["ODEME"].ToString()) ? Convert.ToDouble(dataTable.Rows[i]["ODEME"]) : 0.0,
                            NetTotal = !string.IsNullOrEmpty(dataTable.Rows[i]["NETTOTAL"].ToString()) ? Convert.ToDouble(dataTable.Rows[i]["NETTOTAL"]) : 0.0,
                            ValueAddedTax = !string.IsNullOrEmpty(dataTable.Rows[i]["KDV"].ToString()) ? Convert.ToDouble(dataTable.Rows[i]["KDV"]) : 0.0,

                            SpecialCode = dataTable.Rows[i]["OZELKOD"].ToString(),
                            ProjectCode = dataTable.Rows[i]["PROJEKOD"].ToString(),
                            ProjectName = dataTable.Rows[i]["PROJEAD"].ToString(),
                            Warehouse = dataTable.Rows[i]["ANBAR"].ToString(),
                            Factory = dataTable.Rows[i]["ZAVOD"].ToString(),
                            Currency = dataTable.Rows[i]["VALUTE"].ToString(),
                            OfferNo = dataTable.Rows[i]["TEKLIF_NO"].ToString(),



                        }
                    );
            }

            return reportlist;
        }

        public ActionResult GetPdf(string filePath)
        {
            var fileStream = new FileStream(filePath,
                                             FileMode.Open,
                                             FileAccess.Read
                                           );
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }
    }
}
