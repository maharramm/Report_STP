using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using ReportSTP.Extensions;
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
    public class BankAndCashTransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetList(IDataTablesRequest request)
        {
            string connectionString = "Server=172.16.1.48;Database=TGR3;User Id=m.aliyev;Password=@a123123;";
            string query = @"SELECT 
D.LOGICALREF,
GIRILMEYERI GirilmeYeri,
D.DATE_ Tarix,
CL.CODE Cari,
CL.DEFINITION_ CariAdi,
UstKod,
UstKodAdi,
Kodu,
KodADI,
D.FICHENO SenedNo,
D.FisTip,
D.NAME AS ISH_YERI,
D.BOLUM,
D.XERCMADDE,
D.SOZLESHME,
D.ELEMAN,
D.PROJEKODU,
(CASE WHEN D.SIGN=0 THEN D.AMOUNT END) DEBET_AZN,
(CASE WHEN D.SIGN=1 THEN D.AMOUNT*-1 END) KREDIT_AZN,
CASE WHEN D.TRCURR=0 THEN 'AZN' ELSE (SELECT CURCODE FROM L_CURRENCYLIST WHERE FIRMNR=1 AND CURTYPE=D.TRCURR) END Valyuta,
(CASE WHEN D.SIGN=0 AND D.TRNET!=0 THEN D.TRNET 
WHEN  D.SIGN=1 AND D.TRNET!=0  THEN D.TRNET*-1 
WHEN D.SIGN=1 AND D.TRNET=0 THEN D.AMOUNT*-1 
WHEN D.SIGN=0 AND D.TRNET=0 THEN D.AMOUNT 
 END) ValyutaliMebleg,
D.LINEEXP Aciqlama,
D.LINK1 AS MUQAVILE_LINK1,
D.LINK2 AS MUQAVILE_LINK2,
D.LINK3 AS MUQAVILE_LINK3,
D.CAPIBLOCK_CREADEDDATE AS CREATEDATE,
D.EKLEYEN,
D.SON_DEYISHEN,
D.SON_DEYISHEN AS KLITLEYEN,
D.CAPIBLOCK_MODIFIEDDATE AS DEYISHDIRILME_TARIXI,
D.SPECODE AS OZELKOD,
D.SPECODE1 AS OZELKODTANIM,
D.Y1_ACIKLAMA AS Y1_ACIKLAMA,
D.Y2_ACIKLAMA AS Y2_ACIKLAMA
FROM
(
SELECT 
GIRILMEYERI='KASSA',
A.DATE_,
C.CLIENTREF,
'Kassa' UstKod,
'Kassa' UstKodAdi,
S.CODE  Kodu,
S.NAME  KodADI,
A.FICHENO,
A.TRCODE,
(CASE A.TRCODE
WHEN 11 THEN 'CH TAHSILAT'
WHEN 12 THEN 'CH ODEME'
WHEN 21 THEN 'BANKAYA YATIRILAN'
WHEN 22 THEN 'BANKADAN CEKILEN'
WHEN 31 THEN 'SATINALAMA'
WHEN 32 THEN 'SATIS IADE'
WHEN 33 THEN 'SATIS IADE'
WHEN 34 THEN 'ALINAN HIZMET'
WHEN 35 THEN 'SATINALMA IADE'
WHEN 36 THEN 'SATIS'
WHEN 37 THEN 'SATIS'
WHEN 38 THEN 'VERILEN HIZMET'
WHEN 71 THEN 'ACILIS BORC'
WHEN 72 THEN 'ACILIS ALACAK'
WHEN 73 THEN 'VIRMAN BORC'
WHEN 74 THEN 'VIRMAN ALACAK'
WHEN 79 THEN 'KUR FARKI BORC'
WHEN 80 THEN 'KUR FARKI ALACAK' 
END) FisTip,
A.SIGN,
A.AMOUNT,
A.TRCURR,
A.TRNET,
A.LINEEXP,
CAP.NAME,
DEPT.NAME AS BOLUM,
SPE.DEFINITION_  AS XERCMADDE,
PURCH.FICHENO AS SOZLESHME,
SLS.DEFINITION_ AS ELEMAN,
PRO.NAME AS PROJEKODU,
CT.LINK1,
CT.LINK2,
CT.LINK3,
A.CAPIBLOCK_CREADEDDATE,
SER.NAME AS EKLEYEN,
SER1.NAME AS SON_DEYISHEN,
A.CAPIBLOCK_MODIFIEDDATE,
SPE.SPECODE AS SPECODE1,
SPE.DEFINITION_ AS SPECODE,
a.Y1_ACIKLAMA as Y1_ACIKLAMA,
A.Y2_ACIKLAMA AS Y2_ACIKLAMA,
A.LOGICALREF
FROM 
(SELECT 
DATE_,
LOGICALREF LOG,
FICHENO,
TRCODE,
BRANCH,
SIGN,
AMOUNT,
TRCURR,
TRNET,
LINEEXP,
CARDREF,
SPECODE,
DEPARTMENT,
OFFERREF,
SALESMANREF,
CAPIBLOCK_CREADEDDATE,
PROJECTREF,
CAPIBLOCK_CREATEDBY,
CAPIBLOCK_MODIFIEDBY,
CAPIBLOCK_MODIFIEDDATE,
'' AS Y1_ACIKLAMA,
''AS Y2_ACIKLAMA,
LOGICALREF
FROM  TGR3..LG_001_01_KSLINES WITH(NOLOCK)
--WHERE  DATE_ BETWEEN {FLTDATEBEG(1)} AND {FLTDATEEND(1)
) A
LEFT OUTER JOIN L_CAPIUSER  SER WITH(NOLOCK)  ON SER.NR=A.CAPIBLOCK_CREATEDBY
LEFT OUTER JOIN L_CAPIUSER SER1 WITH(NOLOCK)  ON SER1.NR=A.CAPIBLOCK_MODIFIEDBY
INNER JOIN  TGR3..LG_001_KSCARD S WITH(NOLOCK)  ON S.LOGICALREF=A.CARDREF
LEFT OUTER JOIN  TGR3..LG_001_01_CLFLINE C WITH(NOLOCK)  ON C.MODULENR=10 AND C.SOURCEFREF=A.LOG
LEFT OUTER JOIN TGR3..L_CAPIDIV CAP WITH(NOLOCK)  ON CAP.NR=A.BRANCH AND CAP.FIRMNR=1
LEFT  OUTER JOIN TGR3..L_CAPIDEPT DEPT WITH(NOLOCK)  ON DEPT.NR=A.DEPARTMENT AND DEPT.FIRMNR=1
LEFT OUTER JOIN LG_001_SPECODES SPE WITH(NOLOCK)  ON SPE.SPECODE=A.SPECODE
LEFT OUTER JOIN LG_001_PURCHOFFER PURCH WITH(NOLOCK)  ON PURCH.LOGICALREF=A.OFFERREF
LEFT OUTER JOIN LG_SLSMAN SLS WITH(NOLOCK)  ON SLS.LOGICALREF=A.SALESMANREF
LEFT OUTER JOIN LG_001_PROJECT PRO WITH(NOLOCK)  ON PRO.LOGICALREF=A.PROJECTREF
LEFT OUTER JOIN [dbo].[CTE] CT WITH(NOLOCK)  ON CT.GU=PURCH.GUID
UNION ALL
SELECT 
GIRILMEYERI='BANKA',
L.DATE_,
L.CLIENTREF,
C.CODE UstKod,
C.DEFINITION_ UstKodAdi,
B.CODE  Kodu,
B.DEFINITION_  KodADI,
L.TRANNO,
L.TRCODE,
(CASE L.TRCODE
WHEN 1 THEN 'ISLEM'
WHEN 2 THEN 'VIRMAN'
WHEN 3 THEN 'GELEN HEVALE'
WHEN 4 THEN 'GONDERILEN HEVALE'
WHEN 5 THEN 'ACILIS'
WHEN 16 THEN 'BANKA ALINAN HIZMET'
WHEN 17 THEN 'BANKA VERILEN HIZMETI'
END) FisTip,
L.SIGN,
L.AMOUNT+L.COSTTOT AS AMOUNT ,
L.TRCURR,
L.TRNET,
L.LINEEXP,
CAP.NAME,
DEPT.NAME AS BOLUM,
SPE.DEFINITION_  AS XERCMADDE,
PURCH.FICHENO AS SOZLESHME,
SLS.DEFINITION_ AS ELEMAN,
PRO.NAME AS PROJEKODU,
CT.LINK1,
CT.LINK2,
CT.LINK3,
L.CAPIBLOCK_CREADEDDATE AS CREATEDATE,
SER.NAME AS EKLEYEN,
SER1.NAME AS SONDEYISHEN,
L.CAPIBLOCK_MODIFIEDDATE,
SPE.DEFINITION_ AS SPECODE,
SPE.SPECODE  AS SPECODE1,
XT.Y1DESC AS Y1_ACIKLAMA,
XT.Y2DESC AS Y2_ACIKLAMA,
L.LOGICALREF
FROM  TGR3..LG_001_01_BNFLINE L WITH(NOLOCK) 
LEFT OUTER JOIN LG_XT001001_001 XT ON XT.PARLOGREF=L.LOGICALREF
LEFT OUTER JOIN L_CAPIUSER SER WITH(NOLOCK)  ON SER.NR=L.CAPIBLOCK_CREATEDBY
LEFT OUTER JOIN L_CAPIUSER SER1 WITH(NOLOCK)  ON SER1.NR=L.CAPIBLOCK_MODIFIEDBY
LEFT JOIN  TGR3..LG_001_BANKACC B WITH(NOLOCK)  ON B.LOGICALREF=L.BNACCREF and L.CANCELLED=0
LEFT JOIN  TGR3..LG_001_BNCARD C WITH(NOLOCK)  ON C.LOGICALREF=L.BANKREF
LEFT JOIN TGR3..L_CAPIDIV CAP WITH(NOLOCK)  ON CAP.NR=L.BRANCH AND CAP.FIRMNR=1
LEFT JOIN TGR3..L_CAPIDEPT DEPT WITH(NOLOCK)  ON DEPT.NR=L.DEPARTMENT AND DEPT.FIRMNR=1
LEFT OUTER JOIN LG_001_SPECODES SPE WITH(NOLOCK)  ON SPE.SPECODE=L.SPECODE 
LEFT OUTER JOIN LG_001_PURCHOFFER PURCH WITH(NOLOCK)  ON PURCH.LOGICALREF=L.OFFERREF
LEFT OUTER JOIN LG_SLSMAN SLS WITH(NOLOCK)  ON SLS.LOGICALREF=L.SALESMANREF
LEFT OUTER JOIN LG_001_PROJECT PRO WITH(NOLOCK)  ON PRO.LOGICALREF=L.PROJECTREF
LEFT OUTER JOIN [dbo].[CTE] CT WITH(NOLOCK) ON CT.GU=PURCH.GUID
--WHERE L. DATE_ BETWEEN {FLTDATEBEG(1)} AND {FLTDATEEND(1)
) D
LEFT JOIN  TGR3..LG_001_CLCARD CL WITH(NOLOCK)  ON CL.LOGICALREF=D.CLIENTREF
";

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            List<BankAndCashTransaction> reportlist = new List<BankAndCashTransaction>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                reportlist.Add
                    (
                        new BankAndCashTransaction
                        {
                            EntryPlace = dataTable.Rows[i]["GirilmeYeri"].ToString(),                            
                            Date = !string.IsNullOrEmpty(dataTable.Rows[i]["Tarix"].ToString()) ? Convert.ToDateTime((dataTable.Rows[i]["Tarix"].ToString())) : DateTime.MinValue,
                            ContragentCode = dataTable.Rows[i]["Cari"].ToString(),
                            ContragentName = dataTable.Rows[i]["CariAdi"].ToString(),
                            UpperCode = dataTable.Rows[i]["UstKod"].ToString(),
                            UpperCodeName = dataTable.Rows[i]["UstKodAdi"].ToString(),
                            Code = dataTable.Rows[i]["Kodu"].ToString(),
                            CodeName = dataTable.Rows[i]["KodADI"].ToString(),
                            FicheNo = dataTable.Rows[i]["SenedNo"].ToString(),
                            FicheType = dataTable.Rows[i]["FisTip"].ToString(),
                            WorkPlace = dataTable.Rows[i]["ISH_YERI"].ToString(),
                            Division = dataTable.Rows[i]["BOLUM"].ToString(),
                            XercMadde = dataTable.Rows[i]["XERCMADDE"].ToString(),
                            Contract = dataTable.Rows[i]["SOZLESHME"].ToString(),
                            Employee = dataTable.Rows[i]["ELEMAN"].ToString(),
                            ProjectCode = dataTable.Rows[i]["PROJEKODU"].ToString(),
                            DebitAZN = !string.IsNullOrEmpty(dataTable.Rows[i]["DEBET_AZN"].ToString()) ? Convert.ToDouble(dataTable.Rows[i]["DEBET_AZN"]) : 0,
                            CreditAZN = !string.IsNullOrEmpty(dataTable.Rows[i]["KREDIT_AZN"].ToString()) ? Convert.ToInt32(dataTable.Rows[i]["KREDIT_AZN"]) : 0,
                            Currency = dataTable.Rows[i]["Valyuta"].ToString(),
                            CurrencyAmount = !string.IsNullOrEmpty(dataTable.Rows[i]["ValyutaliMebleg"].ToString()) ? Convert.ToInt32(dataTable.Rows[i]["ValyutaliMebleg"]) : 0,
                            Description = dataTable.Rows[i]["Aciqlama"].ToString(),


                            ContractLink1 = dataTable.Rows[i]["MUQAVILE_LINK1"].ToString(),
                            ContractLink2 = dataTable.Rows[i]["MUQAVILE_LINK2"].ToString(),
                            ContractLink3 = dataTable.Rows[i]["MUQAVILE_LINK3"].ToString(),
                            CreateDate = !string.IsNullOrEmpty(dataTable.Rows[i]["CREATEDATE"].ToString()) ? Convert.ToDateTime(dataTable.Rows[i]["CREATEDATE"].ToString()) : DateTime.MinValue,

                            Adder = dataTable.Rows[i]["EKLEYEN"].ToString(),
                            Modifier = dataTable.Rows[i]["SON_DEYISHEN"].ToString(),
                            Locker = dataTable.Rows[i]["KLITLEYEN"].ToString(),
                            ModifyDate = !string.IsNullOrEmpty(dataTable.Rows[i]["DEYISHDIRILME_TARIXI"].ToString()) ? Convert.ToDateTime(dataTable.Rows[i]["DEYISHDIRILME_TARIXI"].ToString()) : DateTime.MinValue,
                            SpecificCode = dataTable.Rows[i]["OZELKOD"].ToString(),
                            SpecificCodeDescription = dataTable.Rows[i]["OZELKODTANIM"].ToString(),
                            Description1 = dataTable.Rows[i]["Y1_ACIKLAMA"].ToString(),
                            Description2 = dataTable.Rows[i]["Y2_ACIKLAMA"].ToString()
                        }
                    );
            }

            // Filtering
            var filteredRows = reportlist.AsQueryable().GlobalFilterBy(request.Search, request.Columns);

            // Ordering and Paging
            var pagedRows = filteredRows
                .SortBy(request.Columns);

            if (request.Length != -1)
            {
                pagedRows = pagedRows
                    .Skip(request.Start)
                    .Take(request.Length);
            }

            var response = DataTablesResponse.Create(request, reportlist.Count, filteredRows.Count(), pagedRows);

            return new DataTablesJsonResult(response);
        }

        public ActionResult GetPdf(string filePath)
        {
            var fileStream = new FileStream(filePath,
                                             FileMode.Open,
                                             FileAccess.Read
                                           );

            string extension = filePath.Split('.').Last();
            string contentType = string.Empty;

            switch (extension)
            {
                case "pdf":
                    contentType = "application/pdf";
                    break;
                case "docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "jpeg":
                case "jpg":
                    contentType = "image/jpeg";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                case "gif":
                    contentType = "image/gif";
                    break;
                default:
                    break;
            }

            var fsResult = new FileStreamResult(fileStream, contentType);
            return fsResult;
        }
    }
}
