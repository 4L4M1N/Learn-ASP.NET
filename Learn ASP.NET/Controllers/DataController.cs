using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
namespace Learn_ASP.NET.Controllers
{
    public class DataController : Controller 
    {
        // GET: Data
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Result()
        {
            if(Request.Files["fileupload"].ContentLength > 0)
            {
                string extention = Path.GetExtension(Request.Files["fileupload"].FileName.ToLower());
                string query = null;
                string connectionString = "";
                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                //Set file save path
                string path = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["fileupload"].FileName);

                //Create Directory if not Exists
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                }

                if (validFileTypes.Contains(extention))
                {
                    if (validFileTypes.Contains(extention))
                    {
                        if(System.IO.File.Exists(path))
                        { System.IO.File.Delete(path);}
                        Request.Files["fileupload"].SaveAs(path);
                        if (extention == ".csv")
                        {
                            DataTable dataTable = Extentions.Extentions.ConvertCsVtoDataTable(path);
                            ViewBag.Data = dataTable;
                        }
                        else if (extention.Trim() == ".xls")
                        {
                            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + path +
                                               ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            DataTable dataTable = Extentions.Extentions.CovertXslXtoDataTable(path, connectionString);
                        }
                        else if (extention.Trim() == ".xlsx")
                        {
                            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            DataTable dt = Extentions.Extentions.CovertXslXtoDataTable(path, connectionString);
                            ViewBag.Data = dt;
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Please Upload fils in .xls .xlsx or .csv format";
                    }
                }

               
            }
            return PartialView("Index");
        }
    }
}