using Esim7.Models;
using Esim7.ReturnMessage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.Http;
using static Esim7.Action.Action_haringGetExcel;

namespace Esim7.Controllers
    
{    [RoutePrefix("DataToCoustom")]  
    public class CharingController : ApiController
    {

        ///// <summary>
        ///// 导入客户数据
        ///// </summary>
        ///// <param name="Json">
        ///// {
        /////"Card":[{"ICCID":"","ActivateDate":"2018-12-20 12:25:38","EndDate":"2018-12-20 12:25:38"}],
        /////"CompanyID":""}
        ///// 
        ///// 
        ///// </param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("ImportToCustom")]
        //public IHttpActionResult ImportToCustom(object Json)
        //{

        //    string s = Json.ToString();

        //    ResultCurrency r = new ResultCurrency();
        //    try
        //    {



        //        r.MSg = Action.Action_haringGetExcel.GetExcels(s);

        //        r.flg = "1";



        //    }
        //    catch (Exception ex)
        //    {

        //        r.MSg = "接口失败" + ex.ToString(); ;
        //        r.flg = "0";
        //    }




        //    return Json<ResultCurrency>(r);





        //}

        //[HttpPost]
        //[Route("ImportToCustom2")]
        //public IHttpActionResult ImportToCustom2([FromBody]string Json)
        //{



        //    ResultCurrency r = new ResultCurrency();
        //    try
        //    {



        //        r.MSg = Action.Action_haringGetExcel.GetExcels(Json);

        //        r.flg = "1";



        //    }
        //    catch (Exception ex)
        //    {

        //        r.MSg = "接口失败" + ex.ToString(); ;
        //        r.flg = "0";
        //    }




        //    return Json<ResultCurrency>(r);





        //}



        ///// <summary>
        ///// 导入
        ///// </summary>
        ///// <param name="organizationId"></param>
        ///// <returns></returns>
        //[HttpPost, Route("ImportExcel235t56y765")]
        //public V_ResultModel ImportExcel(string CompanyID)
        //{


        //    var result = new V_ResultModel();
        //    List<Card2> List_Card = new List<Card2>();


        //    try
        //    {
        //        System.Web.HttpFileCollection file = System.Web.HttpContext.Current.Request.Files;
        //        if (file.Count > 0)
        //        {
        //            //文件名  
        //            string name = file[0].FileName;
        //            //保存文件  
        //            string path = HttpContext.Current.Server.MapPath("~/UpLoad/") + name;
        //            file[0].SaveAs(path);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.data = "接口错误+" + ex.ToString();
        //        result.successMessage = false;
        //    }


        //    try
        //    {

        //        HttpPostedFile file = HttpContext.Current.Request.Files[0];
        //        // HttpPostedFile file = HttpContext.Current.Request.Files[0];
        //        string FileName;
        //        string savePath;
        //        if (file == null || file.ContentLength <= 0)
        //        {
        //            result.successMessage = false;
        //            result.data = "文件为空！";
        //            return result;
        //        }
        //        else
        //        {
        //            string filename = Path.GetFileName(file.FileName);
        //            int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
        //            string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
        //            string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
        //            int Maxsize = 4000 * 1024;//定义上传文件的最大空间大小为4M
        //            string FileType = ".xls,.xlsx";//定义上传文件的类型字符串

        //            FileName = DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
        //            if (!FileType.Contains(fileEx))
        //            {
        //                result.successMessage = false;
        //                result.data = "文件类型不对，只能导入xls和xlsx格式的文件！";
        //                return result;
        //            }
        //            if (filesize >= Maxsize)
        //            {
        //                result.successMessage = false;
        //                result.data = "上传文件超过4M，不能上传！";
        //                return result;
        //            }
        //            string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/excel/";
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            savePath = Path.Combine(path, FileName);
        //            file.SaveAs(savePath);

        //            string strConn;
        //            if (fileEx == ".xls")
        //            {
        //                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
        //            }
        //            else
        //            {
        //                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + savePath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
        //            }

        //            OleDbConnection conn = new OleDbConnection(strConn);
        //            try
        //            {
        //                conn.Open();
        //                OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
        //                DataSet myDataSet = new DataSet();
        //                myCommand.Fill(myDataSet, "ExcelInfo");
        //                conn.Close();
        //                DataTable table = myDataSet.Tables["ExcelInfo"].DefaultView.ToTable();

        //                var sqlList = new List<string>();

        //                foreach (DataRow item in table.Rows)
        //                {

        //                    Card2 c = new Card2();
        //                    c.ActivateDate = item["ActivateDate"].ToString();
        //                    c.EndDate = item["EndDate"].ToString();
        //                    c.ICCID = item["ICCID"].ToString();
        //                    List_Card.Add(c);

        //                }
        //                Molde_ForCustom f = new Molde_ForCustom();
        //                f.Card = List_Card;
        //                f.CompanyID = CompanyID;
        //                string Json = JsonConvert.SerializeObject(f);

        //            }
        //            catch
        //            {
        //                conn.Close();
        //                result.successMessage = false;
        //                result.data = "导入失败！";

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        result.successMessage = false;
        //        result.data = ex.ToString();
        //        return result;
        //    }
        //    return result;
        //}


        //[HttpPost, Route("Importexcel")]

        //public IHttpActionResult import([FromBody]string  CompanyID) {




        //    return Json<V_ResultModel>(Action.Action_haringGetExcel.ImportExcel(CompanyID));


        //}


        //       [HttpPost, Route("Importexcel2")]
        //public string PostSaveData()
        //{
        //   // LogHelper.WriteLog("接口请求：" + Request.RequestUri.ToString());
        //    HttpRequest request = HttpContext.Current.Request;
        //    Stream postData = request.InputStream;
        //    StreamReader sRead = new StreamReader(postData);
        //    string postContent = sRead.ReadToEnd();
        //    sRead.Close();
        //   // LogHelper.WriteLog("接收到的数据：" + postContent);
        //    return postContent;
        //}


        //[HttpPost, Route("Importexcel_ForCustom2")]
        //public IHttpActionResult ImportExcel_forJson2()  {


        //    return Json<V_ResultModel>(Action.Action_haringGetExcel.ImportExcel_forJson());

        //}

        /// <summary>
        /// 客户数据导入
        /// {"CompanyID":"1564385200122","list":[{"ICCID":"898602B8261890501425","ActivateDate":"2019-07-10 16:20:07","EndDate":"2019-07-01 16:20:07"}]}
        /// </summary>
        /// <returns></returns>
        //[HttpPost, Route("Importexcel_ForCustom")]
        //public IHttpActionResult im()
        //{

           
        //   return Json<V_ResultModel>(Action.Action_haringGetExcel.ImportExcel_forJson());
           
        //}
    }
}
