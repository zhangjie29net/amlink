using Dapper;
using Esim7.Dto;
using Esim7.Models;
using Esim7.parameter.User;
using Esim7.UNity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Esim7.Action
{         /// <summary>
/// 客户数据导入
/// </summary>
    public class Action_haringGetExcel
    {


        /// <summary>
        ///将客户的数据导入card——copy1
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static string GetExcels(string Json)
        {
            string CopyID = Unit.GetTimeStamp(DateTime.Now);
            Model_Charing_GetExcel li = new Model_Charing_GetExcel();
            li = JsonConvert.DeserializeObject<Model_Charing_GetExcel>(Json);
            string s = "";
            string CompanyID = li.CompanyID;
            List<Mesage> ListMess = new List<Mesage>();
            ListMess = li.Card;
            List<string> List_ICCID = new List<string>();
            foreach (Mesage item in ListMess)
            {
                List_ICCID.Add(item.ICCID);
            }
            List<Model_Charing_Card> charing_Cards = new List<Model_Charing_Card>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                //  DATE_FORMAT(Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate,
                // IF(Card_ActivationDate = '0000-00-00 00:00:00', 'null', Card_ActivationDate) as Card_ActivationDate ,
                string sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,  
                                Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici, 
                                DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card where status=1 and isout<>1 and  Card_ICCID in (@Card_ICCID) " + s;
                Card card = new Card();
                charing_Cards = conn.Query<Model_Charing_Card>(sql2, new { Card_ICCID = List_ICCID.ToArray() }).ToList();
            }
            if (ListMess.Count != charing_Cards.Count)
            {
                return "数据不相符，请联系管理员！";
            }
            else
            {
                //数据相同证明 没有遗漏或不相符的
                //修改传入的数据  周期—正常  激活-
                foreach (Model_Charing_Card item in charing_Cards)
                {
                    foreach (Mesage items in ListMess)
                    {
                        item.Card_ActivationDate = Convert.ToDateTime(items.ActivateDate);
                        item.Card_OpenDate = Convert.ToDateTime(items.ActivateDate);
                        item.Card_CompanyID = CompanyID;
                        item.Card_jifei_status = "00";
                        item.Card_EndTime = Convert.ToDateTime(items.EndDate);
                        item.CopyID = CopyID;
                    }
                }
                string sqlCommandText = @"INSERT INTO card_copy1 (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,
                                        Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,
                                        Card_Monthlyusageflow,operatorsID,accountsID,pici,Card_testTime,Card_silentTime,Card_EndTime,cardType,OpeningYearsTime,Card_jifei_status,
                                        BatchID,OutofstockID,RenewDate,CopyID)VALUES(@Card_ID,@Card_IMSI, @Card_Type,@Card_IMEI,@Card_State,@Card_WorkState,@Card_OpenDate,@Card_ActivationDate,
                                        @status,@Card_CompanyID,@Card_ICCID,@Card_totalflow,@Card_userdflow,@Card_Residualflow,@Card_Monthlyusageflow,@operatorsID,@accountsID,
                                        @pici,@Card_testTime,@Card_silentTime,@Card_EndTime,@cardType,@OpeningYearsTime,@Card_jifei_status,@BatchID,@OutofstockID,@RenewDate,@CopyID)";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    try
                    {
                        int result = conn.Execute(sqlCommandText, charing_Cards);
                        s = "success";
                        string s2 = "update card set isout=1,Card_CompanyID=@Card_CompanyID where Card_ICCID=@Card_ICCID";
                        using (IDbConnection conns = DapperService.MySqlConnection())
                        {
                            try
                            {
                                int results = conn.Execute(s2, charing_Cards);
                                s = "success";
                            }
                            catch (Exception ex)
                            {
                                s2 = "update card set isout=0,Card_CompanyID='' where Card_ICCID=@Card_ICCID";
                                int results = conn.Execute(s2, charing_Cards);
                                s = "error";
                                throw ex;
                            }
                        }
                        try
                        {
                            string s22 = "INSERT INTO log_cardcopy1 (CopyID,Number,status,CompanyID) VALUES(@CopyID,@Number,@status,@CompanyID)";
                            using (IDbConnection conns = DapperService.MySqlConnection())
                            {
                                try
                                {
                                    int results = conn.Execute(s22, new { CopyID = CopyID, Number = charing_Cards.Count, status = 1, CompanyID = CompanyID });
                                    s = "success";
                                }
                                catch (Exception ex)
                                {
                                    s = "error";
                                    throw ex;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        string s2 = "update card set isout=0 where Card_ICCID=@Card_ICCID";
                        using (IDbConnection conns = DapperService.MySqlConnection())
                        {
                            try
                            {
                                int results = conn.Execute(s2, charing_Cards);
                                s = "success";
                            }
                            catch (Exception exs)
                            {
                                s = exs.ToString(); ;
                                throw exs;
                            }
                        }
                        s = "ex=" + ex.ToString();
                        throw ex;
                    }
                }
                return s;
            }
        }
        public class ListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string ICCID { get; set; }
            public string SN { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ActivateDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string EndDate { get; set; }
        }
        public class Root
        {
            ///<summary>
            ///运营商标识  OperatorsFlg 1：移动 2：电信 3：联通 5:漫游
            /// </summary>
            public string OperatorsFlg { get; set; }
            /// <summary>
            ///分配用户的公司编码 
            /// </summary>
            public string CompanyID { get; set; }
            public DateTime? ActivateDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DateTime? EndDate { get; set; }

            /// <summary>
            /// 测试流量
            /// </summary>
            public string TestFlow { get; set; }
            /// <summary>
            /// 沉默期
            /// </summary>
            public string SilentTime { get; set; }
            /// <summary>
            /// 开卡年限
            /// </summary>
            public string OpeningYearsTime { get; set; }
            ///<summary>
            ///设置规则 0:没有自定义测试流量沉默期的卡 1:有测试流量没有沉默期 2:有测试流量也有沉默期  3:没有测试流量有沉默期
            /// </summary>
            public int SetType { get; set; }
            /// <summary>
            /// 最长期限
            /// </summary>
            public string MaxTime { get; set; }
            /// <summary>
            /// 是否开启流量共享 1:是 2:否
            /// </summary>
            public string IsFlowSharing { get; set; }
            /// <summary>
            /// 应用场景
            /// </summary>
            public string Scene { get; set; }
            /// <summary>
            /// 合同编号
            /// </summary>
            public string ContractNo { get; set; }
            public List<ListItem> Cards { get; set; }
        }
        /// <summary>
        /// 将客户的数据导入card——copy1
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static V_ResultModel ImportExcel(string CompanyID)
        {
            var result = new V_ResultModel();
            Molde_ForCustom f = new Molde_ForCustom();
            List<Card2> List_Card = new List<Card2>();
            try
            {
                System.Web.HttpFileCollection file = System.Web.HttpContext.Current.Request.Files;
                if (file.Count > 0)
                {
                    //文件名  
                    string name = file[0].FileName;
                    //保存文件  
                    string path = HttpContext.Current.Server.MapPath("~/UpLoad/") + name;
                    file[0].SaveAs(path);
                }
            }
            catch (Exception ex)
            {
                result.data = "接口错误+" + ex.ToString();
                result.successMessage = false;
            }
            try
            {

                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                // HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string FileName;
                string savePath;
                if (file == null || file.ContentLength <= 0)
                {
                    result.successMessage = false;
                    result.data = "文件为空！";
                    return result;
                }
                else
                {
                    string filename = Path.GetFileName(file.FileName);
                    int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                    string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                    string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                    int Maxsize = 4000 * 1024;//定义上传文件的最大空间大小为4M
                    string FileType = ".xls,.xlsx";//定义上传文件的类型字符串

                    FileName = DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                    if (!FileType.Contains(fileEx))
                    {
                        result.successMessage = false;
                        result.data = "文件类型不对，只能导入xls和xlsx格式的文件！";
                        return result;
                    }
                    if (filesize >= Maxsize)
                    {
                        result.successMessage = false;
                        result.data = "上传文件超过4M，不能上传！";
                        return result;
                    }
                    string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/excel/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    savePath = Path.Combine(path, FileName);
                    file.SaveAs(savePath);

                    string strConn;
                    if (fileEx == ".xls")
                    {
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                    }
                    else
                    {
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + savePath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
                    }

                    OleDbConnection conn = new OleDbConnection(strConn);
                    try
                    {
                        conn.Open();
                        OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
                        DataSet myDataSet = new DataSet();
                        myCommand.Fill(myDataSet, "ExcelInfo");
                        conn.Close();
                        DataTable table = myDataSet.Tables["ExcelInfo"].DefaultView.ToTable();

                        var sqlList = new List<string>();

                        foreach (DataRow item in table.Rows)
                        {

                            if (item["ICCID"].ToString() != "")
                            {
                                Card2 c = new Card2();
                                c.ActivateDate = item["ActivateDate"].ToString();
                                c.EndDate = item["EndDate"].ToString();
                                c.ICCID = item["ICCID"].ToString();
                                List_Card.Add(c);
                            }


                        }

                        f.Card = List_Card;
                        f.CompanyID = CompanyID;



                        string CopyID = Unit.GetTimeStamp(DateTime.Now);


                        List<Card2> ListMess = new List<Card2>();



                        ListMess = f.Card;



                        List<string> List_ICCID = new List<string>();
                        foreach (Card2 item in ListMess)
                        {

                            List_ICCID.Add(item.ICCID);

                        }


                        List<Model_Charing_Card> charing_Cards = new List<Model_Charing_Card>();



                        using (IDbConnection conns = DapperService.MySqlConnection())


                        {
                            System.String[] Card_ICCID = List_ICCID.ToArray();
                            //  DATE_FORMAT(Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate,
                            // IF(Card_ActivationDate = '0000-00-00 00:00:00', 'null', Card_ActivationDate) as Card_ActivationDate ,
                            string sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                            Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                            DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                            DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                            cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card where status=1 and (ISNULL( isout ) or isout<>1) and  Card_ICCID in (@List_ICCID)";
                            charing_Cards = conns.Query<Model_Charing_Card>(sql2, new { List_ICCID = List_ICCID.ToArray() }).ToList();
                        }
                        if (ListMess.Count != charing_Cards.Count)
                        {
                            result.data = "有数据不符";
                            result.successMessage = false;
                        }
                        else
                        {
                            ///数据相同证明 没有遗漏或不相符的
                            //修改传入的数据  周期—正常  激活-

                            foreach (Model_Charing_Card item in charing_Cards)
                            {
                                foreach (Card2 items in ListMess)
                                {
                                    if (items.ICCID == item.Card_ICCID)
                                    {
                                        item.Card_ActivationDate = Convert.ToDateTime(items.ActivateDate);
                                        item.Card_OpenDate = Convert.ToDateTime(items.ActivateDate);
                                        item.Card_CompanyID = CompanyID;
                                        item.Card_jifei_status = "00";
                                        item.Card_EndTime = Convert.ToDateTime(items.EndDate);
                                        item.CopyID = CopyID;
                                    }


                                }
                            }
                            #region sql
                            string sqlCommandText = @"INSERT INTO card_copy1 (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,
Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,
Card_Monthlyusageflow,operatorsID,accountsID,pici,Card_testTime,Card_silentTime,Card_EndTime,cardType,OpeningYearsTime,Card_jifei_status,
BatchID,OutofstockID,RenewDate,CopyID
)VALUES(
   @Card_ID,
@Card_IMSI, 
@Card_Type,
@Card_IMEI,
@Card_State
,@Card_WorkState,
@Card_OpenDate,
@Card_ActivationDate,
@status,
@Card_CompanyID,
@Card_ICCID,
@Card_totalflow,
@Card_userdflow,
@Card_Residualflow,
@Card_Monthlyusageflow,
@operatorsID,
@accountsID,
@pici,
@Card_testTime,
@Card_silentTime,
@Card_EndTime,
@cardType,
@OpeningYearsTime,
@Card_jifei_status,
@BatchID,
@OutofstockID,
@RenewDate
,@CopyID
)";

                            #endregion

                            using (IDbConnection conns = DapperService.MySqlConnection())
                            {

                                try
                                {
                                    int results = conns.Execute(sqlCommandText, charing_Cards);
                                    string s2 = "update card set isout=1,CopyID=@Card_CompanyID where Card_ICCID=@Card_ICCID";
                                    using (IDbConnection connss = DapperService.MySqlConnection())
                                    {
                                        try
                                        {
                                            int resultss = connss.Execute(s2, charing_Cards);
                                        }
                                        catch (Exception ex)
                                        {
                                            s2 = "update card set isout=0 ,CopyID='' where Card_ICCID=@Card_ICCID";
                                            int resultss = connss.Execute(s2, charing_Cards);
                                            result.data = "接口出现问题1" + ex.ToString();
                                            result.successMessage = false; ;
                                        }


                                    }

                                    try
                                    {
                                        string s22 = "INSERT INTO log_cardcopy1 (CopyID,Number,status,CompanyID) VALUES(@CopyID,@Number,@status,@CompanyID)";

                                        using (IDbConnection connss = DapperService.MySqlConnection())
                                        {

                                            try
                                            {
                                                int resultss = connss.Execute(s22, new { CopyID = CopyID, Number = charing_Cards.Count, status = 1, CompanyID = CompanyID });

                                                result.data = "成功";
                                                result.successMessage = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                result.data = ex.ToString();
                                                result.successMessage = false;
                                            }


                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        throw ex;
                                    }
                                }
                                catch (Exception)
                                {

                                    string s2 = "update card set isout=0 where Card_ICCID=@Card_ICCID";

                                    using (IDbConnection connss = DapperService.MySqlConnection())
                                    {

                                        try
                                        {
                                            int results = connss.Execute(s2, charing_Cards);

                                            result.data = "出现错误请联系管理员";
                                            result.successMessage = false;
                                        }
                                        catch (Exception exs)
                                        {
                                            result.data = exs.ToString();
                                            result.successMessage = false;
                                        }


                                    }


                                }


                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        result.successMessage = false;
                        result.data = "导入失败！" + ex.ToString();

                    }
                }
            }
            catch (Exception ex)
            {

                result.successMessage = false;
                result.data = ex.ToString();
                return result;
            }
            return result;
        }
        /// <summary>
        /// 客户数据导入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static V_ResultModel ImportExcel_forJson()
        {
            V_ResultModel v = new V_ResultModel();
            #region 获取数据
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            #endregion
            Root list = new Root();
            list = JsonConvert.DeserializeObject<Root>(postContent);
            string CompanyID = list.CompanyID;
            DateTime? ActivateDate = list.ActivateDate;
            DateTime? EndDate = list.ActivateDate;
            DateTime Addtime = DateTime.Now;
            string card_status = string.Empty;//卡状态
            if (!string.IsNullOrWhiteSpace(list.ContractNo))//合同编号不为空
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sqlcontract = "select ContractNo from contractorder where ContractNo='"+list.ContractNo+"'";
                    var listcontract = conn.Query<Root>(sqlcontract).ToList();
                    if (listcontract.Count == 0)
                    {
                        v.data = "合同编号不存在,请先创建合同或确认合同编号是否输入正确!";
                        v.successMessage = false;
                        return v;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(list.MaxTime))
            {
                list.MaxTime = "6";
            }
            if (string.IsNullOrWhiteSpace(list.SilentTime) && !string.IsNullOrWhiteSpace(list.TestFlow))//有测试流量没有沉默期的情况
            {
                card_status = "6";//可测试状态
                list.SilentTime = "0";
                list.SetType = 1;
            }
            if (!string.IsNullOrWhiteSpace(list.SilentTime) && !string.IsNullOrWhiteSpace(list.TestFlow))//有测试流量和沉默期的情况
            {
                card_status = "6";//可测试状态
                list.SetType = 2;
            }
            if (!string.IsNullOrWhiteSpace(list.SilentTime) && string.IsNullOrWhiteSpace(list.TestFlow))//有沉默期没有测试流量的情况
            {
                card_status = "1";//待激活
                list.TestFlow = "0";
                list.SetType = 3;
            }
            if (!string.IsNullOrWhiteSpace(list.ActivateDate.ToString()) && !string.IsNullOrWhiteSpace(list.EndDate.ToString()))
            {
                DateTime ActivateDates = list.ActivateDate.Value.AddHours(8);
                DateTime EndDates = list.EndDate.Value.AddHours(8);
                ActivateDate = ActivateDates;
                EndDate = EndDates;
                //ActivateDate = ActivateDate.value.AddHours(8);
                //EndDate = EndDate.AddHours(8);
                list.SetType = 0;
            }
            Molde_ForCustom f = new Molde_ForCustom();          
            List<Card2> List_Card = new List<Card2>();
            int cardnumber = list.Cards.Count;//统计卡数量
            string companycardnum = string.Empty;
            string sqlcompanynum = string.Empty;//获取用户卡数量
            string Company = list.CompanyID;
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                //获取用户卡数量
                sqlcompanynum = "select CompanyTolCardNum as Number from company where CompanyID='" + list.CompanyID + "'";
                companycardnum = conn.Query<Company>(sqlcompanynum).Select(t => t.Number).FirstOrDefault();
                cardnumber = Convert.ToInt32(companycardnum) + cardnumber;
            }
            if (!string.IsNullOrWhiteSpace(list.Cards[0].SN))
            {
                DateTime time = DateTime.Now;
                //Card_ActivationDate,Card_Remarks,,Card_totalflow,pici,,CopyID,SetMealID2,OperatorsFlg,accountsID
                string Card_Remarks = string.Empty;
                string Card_totalflow = string.Empty;
                string pici = string.Empty;
                string SetMealID2 = string.Empty;
                int OperatorsFlg = 0;
                string accountsID = string.Empty;
                string sqlThreeCard = "select * from three_card where SN='" + list.Cards[0].SN + "'";
                using (IDbConnection Conn = DapperService.MySqlConnection())
                {
                    var lis = Conn.Query<Three_Card>(sqlThreeCard).FirstOrDefault();
                    if (lis != null)
                    {
                        Card_Remarks = lis.Card_Remarks;
                        Card_totalflow = lis.Card_totalflow;
                        pici = lis.pici;
                        SetMealID2 = lis.SetMealID2;
                        OperatorsFlg = lis.OperatorsFlg;
                        accountsID = lis.accountsID;
                    }
                }
                try
                {
                    StringBuilder Sql_JudgeRepet = new StringBuilder("select  SN from Three_CardCopy where SN in( ");
                    list.Cards.RemoveAll(x => x.SN == "" || x.SN == null);
                    List<Card2> List_Card1 = new List<Card2>();
                    foreach (ListItem item in list.Cards)
                    {
                        item.SN = Regex.Replace(item.SN, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                        Sql_JudgeRepet.Append("'" + item.SN + "',");
                    }
                    string CopyID = Unit.GetTimeStamp(DateTime.Now);
                    string sql_JD = Sql_JudgeRepet.ToString();
                    sql_JD = sql_JD.Substring(0, sql_JD.Length - 1) + ")";
                    sql_JD = sql_JD + "and status=1";
                    List<ThreeCardCopySN> li = new List<ThreeCardCopySN>();
                    // 查重 去重
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        li = conn.Query<ThreeCardCopySN>(sql_JD).ToList();
                    }
                    if (li.Count != 0)
                    {
                        string s = "";
                        foreach (ThreeCardCopySN item in li)
                        {
                            item.SN = Regex.Replace(item.SN, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                            s += item.SN + ",";
                        }
                        s = s.Substring(0, s.Length - 1);
                        v.data = "数据重复:" + s;
                        v.successMessage = false;
                        return v;
                    }
                    else
                    {
                        //将信息添加到用户三合一卡表中
                        StringBuilder InnerThreeCardCopy = new StringBuilder("insert into three_cardcopy(SN,Card_OpenDate,Card_ActivationDate,Card_Remarks,Card_CompanyID,Card_totalflow," +
                            "pici,Card_EndTime,RenewDate,CopyID,SetMealID2,OperatorsFlg,accountsID,AddTime,Scene,ContractNo)  values");
                        //更新奇迹三合一卡copyid
                        StringBuilder UpdateThreeCard = new StringBuilder("update three_card set CopyID='" + CopyID + "',Scene='"+list.Scene+"' where SN in (");
                        foreach (var items in list.Cards)
                        {
                            items.SN = Regex.Replace(items.SN, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                            InnerThreeCardCopy.Append("('" + items.SN + "','" + ActivateDate + "','" + ActivateDate + "','" + Card_Remarks + "','" + list.CompanyID + "','" + Card_totalflow + "','" + pici + "'," +
                                "'" + EndDate + "','" + EndDate + "','" + CopyID + "','" + SetMealID2 + "','" + OperatorsFlg + "','" + accountsID + "','" + time + "','"+list.Scene+"','"+list.ContractNo+"'),");
                            UpdateThreeCard.Append("'" + items.SN + "',");
                        }
                        string threecardcopy = InnerThreeCardCopy.ToString().Substring(0, InnerThreeCardCopy.ToString().Length - 1);
                        string updatethreecard = UpdateThreeCard.ToString().Substring(0, UpdateThreeCard.ToString().Length - 1) + ")";
                        //修改用户卡数据
                        string updateCompanyCardNmber = "update company set CompanyTolCardNum="+cardnumber+" where CompanyID='"+list.CompanyID+"'";
                        try
                        {
                            using (IDbConnection Conn = DapperService.MySqlConnection())
                            {
                                Conn.Execute(threecardcopy);
                                Conn.Execute(updatethreecard);
                                Conn.Execute(updateCompanyCardNmber);
                                v.successMessage = true;
                                v.data = "分配成功!";
                            }
                        }
                        catch (Exception ex)
                        {
                            string delthreecardcopy = "delete three_cardcopy where AddTime='" + time + "'";
                            using (IDbConnection Conn = DapperService.MySqlConnection())
                            {
                                Conn.Execute(delthreecardcopy);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    v.successMessage = false;
                    v.data = "给用户分配卡出错:" + ex;
                }
            }
            else  //给用户分配移动电信联通漫游卡
            {
                list.Cards.RemoveAll(x => x.ICCID == "" || x.ICCID == null);
                foreach (ListItem item in list.Cards)
                {
                    if (item.ICCID != "")
                    {
                        item.ICCID = Regex.Replace(item.ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                        Card2 c = new Card2();
                        c.ActivateDate = item.ActivateDate;
                        c.EndDate = item.EndDate;
                        c.ICCID = item.ICCID;
                        c.Card_ICCID = item.ICCID;
                        List_Card.Add(c);
                    }
                }
                f.Card = List_Card;
                f.CompanyID = CompanyID;
                string CopyID = Unit.GetTimeStamp(DateTime.Now);
                List<Card2> ListMess = new List<Card2>();
                List<Card2> ListMessTotal = new List<Card2>();
                // 查重 去重
                string sqliccid = "";
                ListMess = f.Card.MyDistinct(s => s.ICCID).ToList();
                ListMessTotal = f.Card.MyDistinct(s => s.ICCID).ToList();
                int number = 0;
                #region  导入的数据大于3000条的情况
                if (ListMess.Count > 3000)
                {
                    number = ListMess.Count / 3000;
                    number = number + 1;//分的次数
                    for (int i = 0; i < number; i++)
                    {
                        ListMess = ListMessTotal.Skip(i * 3000).Take(3000).ToList(); ;
                        StringBuilder Card_ICCID1 = new StringBuilder("(");
                        List<string> List_ICCID = new List<string>();
                        foreach (Card2 item in ListMess)
                        {
                            item.ICCID = Regex.Replace(item.ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                            Card_ICCID1.Append("'"+item.ICCID+"',");
                            List_ICCID.Add(item.ICCID);
                        }
                        string ic = Card_ICCID1.ToString().Substring(0, Card_ICCID1.ToString().Length - 1);
                        #region  从card表中与excel匹配到的数据集合LIST    charing_Cards
                        List<Model_Charing_Card> charing_Cards = new List<Model_Charing_Card>();
                        #endregion
                        using (IDbConnection conns = DapperService.MySqlConnection())
                        {
                            #region 判断数据是否重复
                            if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                            {
                                StringBuilder sqliccids = new StringBuilder("");
                                sqliccids = new StringBuilder("select Card_ICCID from card_copy1 where Card_CompanyID='" + CompanyID + "' and Card_ICCID in (");
                                string s = "";
                                foreach (var item in ListMess)
                                {
                                    item.Card_ICCID = Regex.Replace(item.Card_ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    sqliccids.Append("'" + item.Card_ICCID + "',");
                                    s += item.Card_ICCID + ",";
                                }

                                sqliccid = sqliccids.ToString();
                                sqliccid = sqliccid.Substring(0, sqliccid.Length - 1) + ")";
                                sqliccid = sqliccid + "and status=1";
                                s = s.Substring(0, s.Length - 1);
                                var ss = conns.Query<Card>(sqliccid).ToList();
                                if (ss.Count > 0)
                                {
                                    v.data = "数据重复:" + s;
                                    v.successMessage = false;
                                    return v;
                                }
                            }
                            if (list.OperatorsFlg == "2")//电信
                            {
                                StringBuilder sqliccids = new StringBuilder("");
                                sqliccids = new StringBuilder("select Card_ICCID from ct_cardcopy where Card_CompanyID='" + CompanyID + "' and Card_ICCID in (");
                                string s = "";
                                foreach (var item in ListMess)
                                {
                                    item.Card_ICCID = Regex.Replace(item.Card_ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    sqliccids.Append("'" + item.Card_ICCID + "',");
                                    s += item.Card_ICCID + ",";
                                }

                                sqliccid = sqliccids.ToString();
                                sqliccid = sqliccid.Substring(0, sqliccid.Length - 1) + ")";
                                sqliccid = sqliccid + "and status=1";
                                s = s.Substring(0, s.Length - 1);
                                var ss = conns.Query<Card>(sqliccid).ToList();
                                if (ss.Count > 0)
                                {
                                    v.data = "数据重复:" + s;
                                    v.successMessage = false;
                                    return v;
                                }
                            }
                            if (list.OperatorsFlg == "3")//联通
                            {
                                StringBuilder sqliccids = new StringBuilder("");
                                sqliccids = new StringBuilder("select Card_ICCID from cucc_cardcopy where Card_CompanyID='" + CompanyID + "' and Card_ICCID in (");
                                string s = "";
                                foreach (var item in ListMess)
                                {
                                    item.Card_ICCID = Regex.Replace(item.Card_ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    sqliccids.Append("'" + item.Card_ICCID + "',");
                                    s += item.Card_ICCID + ",";
                                }

                                sqliccid = sqliccids.ToString();
                                sqliccid = sqliccid.Substring(0, sqliccid.Length - 1) + ")";
                                sqliccid = sqliccid + "and status=1";
                                s = s.Substring(0, s.Length - 1);
                                var ss = conns.Query<Card>(sqliccid).ToList();
                                if (ss.Count > 0)
                                {
                                    v.data = "数据重复:" + s;
                                    v.successMessage = false;
                                    return v;
                                }
                            }
                            if (list.OperatorsFlg == "5")//漫游
                            {
                                StringBuilder sqliccids = new StringBuilder("");
                                sqliccids = new StringBuilder("select Card_ICCID from roamcard_copy where Card_CompanyID='" + CompanyID + "' and Card_ICCID in (");
                                string s = "";
                                foreach (var item in ListMess)
                                {
                                    item.Card_ICCID = Regex.Replace(item.Card_ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    sqliccids.Append("'" + item.Card_ICCID + "',");
                                    s += item.Card_ICCID + ",";
                                }

                                sqliccid = sqliccids.ToString();
                                sqliccid = sqliccid.Substring(0, sqliccid.Length - 1) + ")";
                                sqliccid = sqliccid + "and status=1";
                                s = s.Substring(0, s.Length - 1);
                                var ss = conns.Query<Card>(sqliccid).ToList();
                                if (ss.Count > 0)
                                {
                                    v.data = "数据重复:" + s;
                                    v.successMessage = false;
                                    return v;
                                }
                            }
                            #endregion
                            #region  查询  SQL
                            string sql2 = string.Empty;

                            if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                            {
                                sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,                 
                                Card_Remarks,status,Card_CompanyID,Card_ICCID, Card_ICCID as ICCID  ,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate ,Platform,SetMealID,SetMealID2 from card where   Card_ICCID in " + ic + ")";
                            }
                            if (list.OperatorsFlg == "2")//电信
                            {
                                sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,                 
                                Card_Remarks,status,Card_CompanyID,Card_ICCID, Card_ICCID as ICCID  ,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate ,Platform,SetMealID,SetMealID2 from ct_card where   Card_ICCID in " + ic + ")";
                            }
                            if (list.OperatorsFlg == "3")//联通
                            {
                                sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,                 
                                Card_Remarks,status,Card_CompanyID,Card_ICCID, Card_ICCID as ICCID  ,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate ,Platform,SetMealID,SetMealID2 from cucc_card where   Card_ICCID in " + ic + ")";
                            }
                            if (list.OperatorsFlg == "5")//漫游
                            {
                                sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,                 
                                Card_Remarks,status,Card_CompanyID,Card_ICCID, Card_ICCID as ICCID  ,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate ,Platform,SetMealID,SetMealID2 from roamcard where  Card_ICCID in " + ic + ")";
                            }
                            #endregion
                            try
                            {
                                //从Card表中获取excel的匹配数据
                                charing_Cards = conns.Query<Model_Charing_Card>(sql2).ToList();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        #region   更新数据sql

                        StringBuilder Card_ID = new StringBuilder("");
                        if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                        {
                            if (string.IsNullOrWhiteSpace(ActivateDate.ToString()) || string.IsNullOrWhiteSpace(EndDate.ToString()))
                            {
                                Card_ID.Append(@"replace INTO card_copy1 (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform,SetMealID,SetMealID2,TestFlow,SilentTime,MaxTime,SetType,AddTime,Card_OpenDate,Card_ActivationDate,IsFlowSharing,Scene,ContractNo)values");
                            }
                            else
                            {
                                Card_ID.Append(@"replace INTO card_copy1 (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform  ,SetMealID,SetMealID2,IsFlowSharing,Scene,ContractNo)values");
                            }
                        }
                        if (list.OperatorsFlg == "2")//电信
                        {
                            Card_ID.Append(@"replace INTO ct_cardcopy (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform  ,SetMealID,SetMealID2,IsFlowSharing,Scene,ContractNo)values");
                        }
                        if (list.OperatorsFlg == "3")//联通
                        {
                            Card_ID.Append(@"replace INTO cucc_cardcopy (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform  ,SetMealID,SetMealID2,IsFlowSharing,Scene,ContractNo)values");
                        }
                        if (list.OperatorsFlg == "5")//漫游
                        {
                            Card_ID.Append(@"replace INTO roamcard_copy (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform  ,SetMealID,SetMealID2,IsFlowSharing,Scene,ContractNo)values");
                        }
                        #endregion
                        ///数据相同证明 没有遗漏或不相符的
                        //修改传入的数据  周期—正常  激活-
                        if (charing_Cards.Count == 0 || CompanyID is null)
                        {
                            v.data = "未找到任何匹配信息或全部已经上传";
                            v.successMessage = false; ;
                        }
                        else if (charing_Cards.Count != ListMess.Count)
                        {
                            #region 调试使用   关于查重
                            #endregion
                            v.data = "有数据不匹配,请检查EXCEL中是存在空格!";
                            v.successMessage = false; ;
                        }
                        else
                        {
                            StringBuilder Card_ICCID = new StringBuilder("");
                            if (string.IsNullOrWhiteSpace(ActivateDate.ToString()) || string.IsNullOrWhiteSpace(EndDate.ToString()))
                            {
                                foreach (Model_Charing_Card item in charing_Cards)
                                {
                                    foreach (Card2 items in ListMess)
                                    {
                                        if (items.ICCID == item.Card_ICCID)
                                        {
                                            item.Card_ActivationDate = ActivateDate.Value;
                                            item.Card_OpenDate = ActivateDate.Value;
                                            item.Card_CompanyID = CompanyID;
                                            item.Card_jifei_status = "00";
                                            item.Card_EndTime = EndDate.Value;
                                            item.CopyID = CopyID;
                                            item.Card_Monthlyusageflow = "0";
                                            Card_ID.Append("(" + "'" + item.Card_ID + "','" + item.Card_IMSI + "','" + item.Card_Type + "','" + item.Card_IMEI + "','" + item.Card_State + "','"
                                        + item.Card_WorkState + "','" + ActivateDate + "','" + ActivateDate + "','"
                                        + item.status + "','" + CompanyID + "','" + item.Card_ICCID + "','" + item.Card_totalflow + "','" + item.Card_userdflow + "','" + item.Card_Residualflow + "','"
                                        + item.Card_Monthlyusageflow + "','" + item.operatorsID + "','" + item.accountsID + "','" + item.pici + "','" + item.Card_testTime + "','" + item.Card_silentTime + "','"
                                        + EndDate + "','" + item.cardType + "','" + item.OpeningYearsTime + "','00" + "','" + item.BatchID + "','" + item.OutofstockID + "','" + item.RenewDate + "','"
                                        + CopyID + "','" + item.Platform + "','" + item.SetMealID + "','" + item.SetMealID2 + "'," + list.TestFlow + "," + list.SilentTime + "," + list.MaxTime + "," + list.SetType + ",'"+Addtime+"','"+Addtime+"','" + Addtime + "','"+list.IsFlowSharing+ "','" + list.Scene + "','"+list.ContractNo+"'" + "),"
                                        );
                                            Card_ICCID.Append("'" + item.Card_ICCID + "',");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (Model_Charing_Card item in charing_Cards)
                                {
                                    foreach (Card2 items in ListMess)
                                    {
                                        if (items.ICCID == item.Card_ICCID)
                                        {
                                            item.Card_ActivationDate = ActivateDate.Value;
                                            item.Card_OpenDate = ActivateDate.Value;
                                            item.Card_CompanyID = CompanyID;
                                            item.Card_jifei_status = "00";
                                            item.Card_EndTime = EndDate.Value;
                                            item.CopyID = CopyID;
                                            item.Card_Monthlyusageflow = "0";
                                            Card_ID.Append("(" + "'" + item.Card_ID + "','" + item.Card_IMSI + "','" + item.Card_Type + "','" + item.Card_IMEI + "','" + item.Card_State + "','"
                                        + item.Card_WorkState + "','" + ActivateDate + "','" + ActivateDate + "','"
                                        + item.status + "','" + CompanyID + "','" + item.Card_ICCID + "','" + item.Card_totalflow + "','" + item.Card_userdflow + "','" + item.Card_Residualflow + "','"
                                        + item.Card_Monthlyusageflow + "','" + item.operatorsID + "','" + item.accountsID + "','" + item.pici + "','" + item.Card_testTime + "','" + item.Card_silentTime + "','"
                                        + EndDate + "','" + item.cardType + "','" + item.OpeningYearsTime + "','00" + "','" + item.BatchID + "','" + item.OutofstockID + "','" + item.RenewDate + "','"
                                        + CopyID + "','" + item.Platform + "','" + item.SetMealID + "','" + item.SetMealID2 + "','"+list.IsFlowSharing+ "','" + list.Scene + "','"+list.ContractNo+"'" + "),"
                                        );
                                            Card_ICCID.Append("'" + item.Card_ICCID + "',");
                                        }
                                    }
                                }
                            }
                            #region sql
                            string sql = Card_ID.ToString().Substring(0, Card_ID.ToString().LastIndexOf(','));
                            #endregion
                            using (IDbConnection conns = DapperService.MySqlConnection())
                            {
                                try
                                {
                                    int results = conns.Execute(sql);
                                    StringBuilder sql3 = new StringBuilder("");
                                    if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                                    {
                                        sql3.Append("Update card set isout=1,copyid='" + CopyID + "',Scene='" + list.Scene + "' where Card_ICCID in(");
                                    }
                                    if (list.OperatorsFlg == "2")//电信
                                    {
                                        sql3.Append("Update ct_card set isout=1,copyid='" + CopyID + "',Scene='" + list.Scene + "' where Card_ICCID in(");
                                    }
                                    if (list.OperatorsFlg == "3")//联通
                                    {
                                        sql3.Append("Update cucc_card set isout=1,copyid='" + CopyID + "',Scene='" + list.Scene + "' where Card_ICCID in(");
                                    }
                                    if (list.OperatorsFlg == "5")//漫游
                                    {
                                        sql3.Append("Update roamcard set isout=1,copyid='" + CopyID + "',Scene='" + list.Scene + "' where Card_ICCID in(");
                                    }
                                    string ICCID = Card_ICCID.ToString();
                                    ICCID = ICCID.Substring(0, ICCID.LastIndexOf(',')) + ")";
                                    string sql4 = sql3.ToString() + ICCID;
                                    string sql5 = "Update card set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                    if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                                    {
                                        sql5 = "Update card set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                    }
                                    if (list.OperatorsFlg == "2")//电信
                                    {
                                        sql5 = "Update ct_card set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                    }
                                    if (list.OperatorsFlg == "3")//联通
                                    {
                                        sql5 = "Update cucc_card set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                    }
                                    if (list.OperatorsFlg == "5")//漫游
                                    {
                                        sql5 = "Update roamcard set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                    }
                                    using (IDbConnection connss = DapperService.MySqlConnection())
                                    {
                                        try
                                        {
                                            int resultss = connss.Execute(sql4);
                                        }
                                        catch (Exception ex)
                                        {
                                            int resultss = connss.Execute(sql5);
                                            v.data = "接口出现问题" + ex.ToString();
                                            v.successMessage = false; ;
                                        }
                                    }
                                    try
                                    {
                                        string s22 = "INSERT INTO log_cardcopy1 (CopyID,Number,status,CompanyID) VALUES(@CopyID,@Number,@status,@CompanyID)";
                                        using (IDbConnection connss = DapperService.MySqlConnection())
                                        {
                                            try
                                            {
                                                int resultss = connss.Execute(s22, new { CopyID = CopyID, Number = charing_Cards.Count, status = 1, CompanyID = CompanyID });
                                                v.data = "成功";
                                                v.successMessage = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                v.data = ex.ToString();
                                                v.successMessage = false;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                    if (v.successMessage == true)//数据大于3000条时
                    {
                        //修改用户卡数据
                        string updateCompanyCardNmber = "update company set CompanyTolCardNum=" + cardnumber + " where CompanyID='" + list.CompanyID + "'";
                        using (IDbConnection conns = DapperService.MySqlConnection())
                        {
                            conns.Execute(updateCompanyCardNmber);
                        }
                    }
                }
                #endregion
                else
                {
                    StringBuilder Card_ICCID1 = new StringBuilder("(");
                    List<string> List_ICCID = new List<string>();
                    foreach (Card2 item in ListMess)
                    {
                        Card_ICCID1.Append("'"+item.ICCID+"',");
                        List_ICCID.Add(item.ICCID);
                    }
                    string ic = Card_ICCID1.ToString().Substring(0, Card_ICCID1.ToString().Length - 1);
                    #region  从card表中与excel匹配到的数据集合LIST    charing_Cards
                    List<Model_Charing_Card> charing_Cards = new List<Model_Charing_Card>();
                    #endregion
                    using (IDbConnection conns = DapperService.MySqlConnection())
                    {
                        #region 判断数据是否重复
                        if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                        {
                            StringBuilder sqliccids = new StringBuilder("");
                            sqliccids = new StringBuilder("select Card_ICCID from card_copy1 where Card_CompanyID='" + CompanyID + "' and Card_ICCID in (");
                            string s = "";
                            foreach (var item in ListMess)
                            {
                                item.Card_ICCID = Regex.Replace(item.Card_ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                sqliccids.Append("'" + item.Card_ICCID + "',");
                                s += item.Card_ICCID + ",";
                            }

                            sqliccid = sqliccids.ToString();
                            sqliccid = sqliccid.Substring(0, sqliccid.Length - 1) + ")";
                            sqliccid = sqliccid + "and status=1";
                            s = s.Substring(0, s.Length - 1);
                            var ss = conns.Query<Card>(sqliccid).ToList();
                            if (ss.Count > 0)
                            {
                                v.data = "数据重复:" + s;
                                v.successMessage = false;
                                return v;
                            }
                        }
                        if (list.OperatorsFlg == "2")//电信
                        {
                            StringBuilder sqliccids = new StringBuilder("");
                            sqliccids = new StringBuilder("select Card_ICCID from ct_cardcopy where Card_CompanyID='" + CompanyID + "' and Card_ICCID in (");
                            string s = "";
                            foreach (var item in ListMess)
                            {
                                item.Card_ICCID = Regex.Replace(item.Card_ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                sqliccids.Append("'" + item.Card_ICCID + "',");
                                s += item.Card_ICCID + ",";
                            }

                            sqliccid = sqliccids.ToString();
                            sqliccid = sqliccid.Substring(0, sqliccid.Length - 1) + ")";
                            sqliccid = sqliccid + "and status=1";
                            s = s.Substring(0, s.Length - 1);
                            var ss = conns.Query<Card>(sqliccid).ToList();
                            if (ss.Count > 0)
                            {
                                v.data = "数据重复:" + s;
                                v.successMessage = false;
                                return v;
                            }
                        }
                        if (list.OperatorsFlg == "3")//联通
                        {
                            StringBuilder sqliccids = new StringBuilder("");
                            sqliccids = new StringBuilder("select Card_ICCID from cucc_cardcopy where Card_CompanyID='" + CompanyID + "' and Card_ICCID in (");
                            string s = "";
                            foreach (var item in ListMess)
                            {
                                item.Card_ICCID = Regex.Replace(item.Card_ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                sqliccids.Append("'" + item.Card_ICCID + "',");
                                s += item.Card_ICCID + ",";
                            }

                            sqliccid = sqliccids.ToString();
                            sqliccid = sqliccid.Substring(0, sqliccid.Length - 1) + ")";
                            sqliccid = sqliccid + "and status=1";
                            //s = s.Substring(0, s.Length - 1);
                            var ss = conns.Query<Card>(sqliccid).ToList();
                            if (ss.Count > 0)
                            {
                                foreach (var itemss in ss)
                                {
                                    s += itemss.Card_ICCID + ",";
                                }
                                v.data = "数据重复:" + s;
                                v.successMessage = false;
                                return v;
                            }
                        }
                        if (list.OperatorsFlg == "5")//漫游
                        {
                            StringBuilder sqliccids = new StringBuilder("");
                            sqliccids = new StringBuilder("select Card_ICCID from roamcard_copy where Card_CompanyID='" + CompanyID + "' and Card_ICCID in (");
                            string s = "";
                            foreach (var item in ListMess)
                            {
                                item.Card_ICCID = Regex.Replace(item.Card_ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                sqliccids.Append("'" + item.Card_ICCID + "',");
                                s += item.Card_ICCID + ",";
                            }

                            sqliccid = sqliccids.ToString();
                            sqliccid = sqliccid.Substring(0, sqliccid.Length - 1) + ")";
                            sqliccid = sqliccid + "and status=1";
                            s = s.Substring(0, s.Length - 1);
                            var ss = conns.Query<Card>(sqliccid).ToList();
                            if (ss.Count > 0)
                            {
                                v.data = "数据重复:" + s;
                                v.successMessage = false;
                                return v;
                            }
                        }
                        #endregion
                        #region  查询  SQL
                        string sql2 = string.Empty;

                        if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                        {
                            sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,                 
                            Card_Remarks,status,Card_CompanyID,Card_ICCID, Card_ICCID as ICCID  ,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                            DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                            cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate ,Platform,SetMealID,SetMealID2 from card where   Card_ICCID in " + ic + ")";
                        }   
                        if (list.OperatorsFlg == "2")//电信
                        {
                            sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,                 
                            Card_Remarks,status,Card_CompanyID,Card_ICCID, Card_ICCID as ICCID  ,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                            DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                            cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate ,Platform,SetMealID,SetMealID2 from ct_card where   Card_ICCID in " + ic + ")";
                        }
                        if (list.OperatorsFlg == "3")//联通
                        {
                            sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,                 
                            Card_Remarks,status,Card_CompanyID,Card_ICCID, Card_ICCID as ICCID  ,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                            DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                            cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate ,Platform,SetMealID,SetMealID2 from cucc_card where   Card_ICCID in " + ic + ")";
                        }
                        if (list.OperatorsFlg == "5")//漫游
                        {
                            sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate,                 
                            Card_Remarks,status,Card_CompanyID,Card_ICCID, Card_ICCID as ICCID  ,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                            DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                            cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate ,Platform,SetMealID,SetMealID2 from roamcard where   Card_ICCID in " + ic + ")";
                        }
                        #endregion
                        try
                        {
                            //从Card表中获取excel的匹配数据
                            charing_Cards = conns.Query<Model_Charing_Card>(sql2).ToList();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    #region   更新数据sql

                    StringBuilder Card_ID = new StringBuilder("");
                    if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                    {
                        if (string.IsNullOrWhiteSpace(ActivateDate.ToString()) || string.IsNullOrWhiteSpace(EndDate.ToString()) || list.EndDate==null && list.ActivateDate!=null)
                        {
                            Card_ID.Append(@"replace INTO card_copy1 (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,status,Card_CompanyID,
                                    Card_ICCID,operatorsID, accountsID, pici, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, CopyID, Platform  ,SetMealID,SetMealID2,TestFlow,SilentTime,MaxTime,SetType,AddTime,Card_OpenDate,Card_ActivationDate,IsFlowSharing,Scene,ContractNo)values");
                        }
                        else
                        {
                            Card_ID.Append(@"replace INTO card_copy1 (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform  ,SetMealID,SetMealID2,IsFlowSharing,Scene,ContractNo)values");
                        }
      
                    }
                    if (list.OperatorsFlg == "2")//电信
                    {
                        Card_ID.Append(@"replace INTO ct_cardcopy (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform  ,SetMealID,SetMealID2,IsFlowSharing,Scene,ContractNo)values");
                    }
                    if (list.OperatorsFlg == "3")//联通
                    {
                        Card_ID.Append(@"replace INTO cucc_cardcopy (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform  ,SetMealID,SetMealID2,IsFlowSharing,Scene,ContractNo)values");
                    }
                    if (list.OperatorsFlg == "5")//漫游
                    {
                        if (string.IsNullOrWhiteSpace(ActivateDate.ToString()) || string.IsNullOrWhiteSpace(EndDate.ToString()) || list.EndDate == null && list.ActivateDate != null)
                        {
                            Card_ID.Append(@"replace INTO roamcard_copy (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,status,Card_CompanyID,
                                    Card_ICCID,operatorsID, accountsID, pici, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, CopyID, Platform  ,SetMealID,SetMealID2,TestFlow,SilentTime,MaxTime,SetType,AddTime,Card_OpenDate,Card_ActivationDate,IsFlowSharing,Scene,ContractNo)values");
                        }
                        else
                        {
                            Card_ID.Append(@"replace INTO roamcard_copy (Card_ID,Card_IMSI, Card_Type,Card_IMEI,Card_State,Card_WorkState,Card_OpenDate,Card_ActivationDate,status,Card_CompanyID,
                                    Card_ICCID,Card_totalflow, Card_userdflow, Card_Residualflow,Card_Monthlyusageflow, operatorsID, accountsID, pici, Card_testTime, Card_silentTime, Card_EndTime, cardType,
                                    OpeningYearsTime, Card_jifei_status,BatchID, OutofstockID, RenewDate, CopyID, Platform  ,SetMealID,SetMealID2,IsFlowSharing,Scene,ContractNo)values");
                        }
                    }
                    #endregion
                    ///数据相同证明 没有遗漏或不相符的
                    //修改传入的数据  周期—正常  激活-
                    if (charing_Cards.Count == 0 || CompanyID is null)
                    {
                        v.data = "未找到任何匹配信息或全部已经上传";
                        v.successMessage = false; ;
                    }
                    else if (charing_Cards.Count != ListMess.Count)
                    {
                        #region 调试使用   关于查重
                        #endregion
                        v.data = "有数据不匹配,请检查EXCEL中是存在空格!";
                        v.successMessage = false; ;
                    }
                    else
                    {
                        StringBuilder Card_ICCID = new StringBuilder("");
                        if (string.IsNullOrWhiteSpace(ActivateDate.ToString()) || string.IsNullOrWhiteSpace(EndDate.ToString()) || list.EndDate == null && list.ActivateDate != null)
                        {
                            foreach (Model_Charing_Card item in charing_Cards)
                            {
                                foreach (Card2 items in ListMess)
                                {
                                    if (items.ICCID == item.Card_ICCID)
                                    {
                                        //item.Card_ActivationDate = ActivateDate.Value;
                                        //item.Card_OpenDate = ActivateDate.Value;
                                        item.Card_CompanyID = CompanyID;
                                        item.Card_jifei_status = "00";
                                        if (list.ActivateDate != null)
                                        {
                                            item.Card_OpenDate = ActivateDate.Value.AddHours(8);
                                            item.Card_ActivationDate = ActivateDate.Value.AddHours(8);
                                        }
                                        else
                                        {
                                            item.Card_OpenDate = Addtime;
                                            item.Card_ActivationDate = Addtime;
                                        }

                                        //item.Card_EndTime = EndDate.Value;
                                        item.CopyID = CopyID;
                                        Card_ID.Append("(" + "'" + item.Card_ID + "','" + item.Card_IMSI + "','" + item.Card_Type + "','" + item.Card_IMEI + "','" + card_status + "','"
                                    + item.Card_WorkState + "',"+ item.status + ",'" + CompanyID + "','" + item.Card_ICCID + "','" + item.operatorsID + "','" + item.accountsID + "'," +0+ "," +
                                    "'" + item.cardType + "'," + list.OpeningYearsTime + ",'00" + "','" + item.BatchID + "','" + item.OutofstockID + "','"
                                    + CopyID + "','" + item.Platform + "','" + item.SetMealID + "','" + item.SetMealID2 + "'," + list.TestFlow + "," + list.SilentTime + "," + list.MaxTime + "," + list.SetType + ",'"+ item.Card_ActivationDate + "','"+ item.Card_OpenDate + "','" + item.Card_ActivationDate + "','"+list.IsFlowSharing+"','"+list.Scene+"','"+list.ContractNo+"'" + "),"
                                    );
                                        Card_ICCID.Append("'" + item.Card_ICCID + "',");
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Model_Charing_Card item in charing_Cards)
                            {
                                foreach (Card2 items in ListMess)
                                {
                                    if (items.ICCID == item.Card_ICCID)
                                    {
                                        item.Card_ActivationDate = ActivateDate.Value;
                                        item.Card_OpenDate = ActivateDate.Value;
                                        item.Card_CompanyID = CompanyID;
                                        item.Card_jifei_status = "00";
                                        item.Card_EndTime = EndDate.Value;
                                        item.CopyID = CopyID;
                                        item.Card_Monthlyusageflow = "0";
                                        Card_ID.Append("(" + "'" + item.Card_ID + "','" + item.Card_IMSI + "','" + item.Card_Type + "','" + item.Card_IMEI + "','" + item.Card_State + "','"
                                    + item.Card_WorkState + "','" + ActivateDate + "','" + ActivateDate + "','"
                                    + item.status + "','" + CompanyID + "','" + item.Card_ICCID + "','" + item.Card_totalflow + "','" + item.Card_userdflow + "','" + item.Card_Residualflow + "','"
                                    + item.Card_Monthlyusageflow + "','" + item.operatorsID + "','" + item.accountsID + "','" + item.pici + "','" + item.Card_testTime + "','" + item.Card_silentTime + "','"
                                    + EndDate + "','" + item.cardType + "','" + item.OpeningYearsTime + "','00" + "','" + item.BatchID + "','" + item.OutofstockID + "','" + item.RenewDate + "','"
                                    + CopyID + "','" + item.Platform + "','" + item.SetMealID + "','" + item.SetMealID2 + "','"+list.IsFlowSharing+ "','" + list.Scene + "','"+list.ContractNo+"'" + "),"
                                    );
                                        Card_ICCID.Append("'" + item.Card_ICCID + "',");
                                    }
                                }
                            }
                        }
                        #region sql
                        string sql = Card_ID.ToString().Substring(0, Card_ID.ToString().LastIndexOf(','));
                        #endregion
                        using (IDbConnection conns = DapperService.MySqlConnection())
                        {
                            try
                            {
                                int results = conns.Execute(sql);
                                StringBuilder sql3 = new StringBuilder("");
                                if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                                {
                                    sql3.Append("Update card set isout=1,copyid='" + CopyID + "',Scene='" + list.Scene + "' where Card_ICCID in(");
                                }
                                if (list.OperatorsFlg == "2")//电信
                                {
                                    sql3.Append("Update ct_card set isout=1,copyid='" + CopyID + "',Scene='" + list.Scene + "' where Card_ICCID in(");
                                }
                                if (list.OperatorsFlg == "3")//联通
                                {
                                    sql3.Append("Update cucc_card set isout=1,copyid='" + CopyID + "',Scene='" + list.Scene + "' where Card_ICCID in(");
                                }
                                if (list.OperatorsFlg == "5")//漫游
                                {
                                    sql3.Append("Update roamcard set isout=1,copyid='" + CopyID + "',Scene='" + list.Scene + "' where Card_ICCID in(");
                                }
                                string ICCID = Card_ICCID.ToString();
                                ICCID = ICCID.Substring(0, ICCID.LastIndexOf(',')) + ")";
                                string sql4 = sql3.ToString() + ICCID;
                                string sql5 = "Update card set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                                {
                                    sql5 = "Update card set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                }
                                if (list.OperatorsFlg == "2")//电信
                                {
                                    sql5 = "Update ct_card set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                }
                                if (list.OperatorsFlg == "3")//联通
                                {
                                    sql5 = "Update cucc_card set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                }
                                if (list.OperatorsFlg == "5")//漫游
                                {
                                    sql5 = "Update roamcard set isout=0, copyid = '' where Card_ICCID in(" + ICCID;
                                }
                                using (IDbConnection connss = DapperService.MySqlConnection())
                                {
                                    try
                                    {
                                        int resultss = connss.Execute(sql4);
                                    }
                                    catch (Exception ex)
                                    {
                                        int resultss = connss.Execute(sql5);
                                        v.data = "接口出现问题1" + ex.ToString();
                                        v.successMessage = false; ;
                                    }
                                }
                                try
                                {
                                    string s22 = "INSERT INTO log_cardcopy1 (CopyID,Number,status,CompanyID) VALUES(@CopyID,@Number,@status,@CompanyID)";
                                    //修改用户卡数据
                                    string updateCompanyCardNmber = "update company set CompanyTolCardNum=" + cardnumber + " where CompanyID='" + list.CompanyID + "'";
                                    using (IDbConnection connss = DapperService.MySqlConnection())
                                    {
                                        try
                                        {
                                            conns.Execute(updateCompanyCardNmber);
                                            int resultss = connss.Execute(s22, new { CopyID = CopyID, Number = charing_Cards.Count, status = 1, CompanyID = CompanyID });
                                            v.data = "成功";
                                            v.successMessage = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            v.data = ex.ToString();
                                            v.successMessage = false;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return v;
        }

        ///<summary>
        ///修改客户卡开户日期和续费日期
        /// </summary>
        public static Information UpdateCustomerTime(Root root)
        {
            Information info = new Information();
            try
            {
                StringBuilder sqlupdate = new StringBuilder("");
                string strsqliccid = string.Empty;
                if (root.ActivateDate != null && root.EndDate == null)
                {
                    root.ActivateDate= root.ActivateDate.Value.AddHours(8);
                    sqlupdate.Append("update card_copy1  set Card_ActivationDate='" + root.ActivateDate + "' where Card_CompanyID='" + root.CompanyID + "' and Card_ICCID in(");
                }
                if (root.EndDate != null && root.ActivateDate == null)
                {
                    root.EndDate = root.EndDate.Value.AddHours(8);
                    sqlupdate.Append("update card_copy1  set Card_EndTime='" + root.EndDate + "' where Card_CompanyID='" + root.CompanyID + "' and Card_ICCID in(");
                }
                if (root.EndDate != null && root.ActivateDate != null)
                {
                    root.ActivateDate = root.ActivateDate.Value.AddHours(8);
                    root.EndDate = root.EndDate.Value.AddHours(8);
                    sqlupdate.Append("update card_copy1  set Card_ActivationDate='" + root.ActivateDate + "',Card_EndTime='" + root.EndDate + "' where Card_CompanyID='" + root.CompanyID + "' and Card_ICCID in(");
                }
                if (root.Cards.Count > 0)
                {
                    foreach (var item in root.Cards)
                    {
                        strsqliccid +="'" +item.ICCID + "',";
                    }
                    strsqliccid = strsqliccid.Substring(0, strsqliccid.Length - 1) + ")";
                    sqlupdate.Append(strsqliccid);
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sqlupdate.ToString());
                    info.flg = "1";
                    info.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///客户数据导入 针对开始时间结束时间多乱杂情况  目前支持移动卡
        /// </summary>
        public static Information UploadUserCardData(Model_Charing_GetExcel para)
        {
            Information info = new Information();
            string copy = Unit.GetTimeStamp(DateTime.Now);
            string nocardstr = string.Empty;
            try
            {
                if (para.Card.Count <= 3000)
                {
                    StringBuilder Card_ICCID = new StringBuilder("(");
                    foreach (var item in para.Card)
                    {
                        Card_ICCID.Append("'"+item.ICCID+"',");
                    }
                    string striccids = Card_ICCID.ToString().Substring(0, Card_ICCID.Length - 1);
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sqlcard = "select Card_ICCID from card_copy1 where Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in "+striccids+"";//判断数据是否重复
                        var cardlist = conn.Query<Card>(sqlcard).ToList();
                        StringBuilder nocard = new StringBuilder("(");
                        if (cardlist.Count > 0) //有重复数据
                        {
                            foreach (var item in cardlist)
                            {
                                nocard.Append("'" + item.Card_ICCID + "',");
                            }
                            //重复的卡数据
                            nocardstr = nocard.ToString().Substring(0, nocard.Length - 1);
                            info.flg = "-1";
                            info.Msg = "数据重复:" + nocardstr;
                            return info;
                        }
                        else
                        {
                            //判断是否在公海数据
                            string sqlcardlist = "select * from card where Card_ICCID in " + striccids + "";
                            var cardlists = conn.Query<Model_Charing_Card>(sqlcardlist).ToList();
                            if (cardlists.Count != para.Card.Count)
                            {
                                //数据不符
                                info.flg = "-1";
                                info.Msg = "数据不符";
                                return info;
                            }
                            else
                            {
                                //更新公海数据数据
                                string updatecard = "update card set CopyID='"+copy+"' where Card_ICCID in "+ striccids + "";
                                StringBuilder addcardcopy = new StringBuilder("insert into card_copy1 (Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_ActivationDate,Card_OpenDate,status," +
                                    "Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_EndTime,cardType,CopyID,Platform,SetMealID,SetMealID2) values ");
                                foreach (var item in para.Card)
                                {
                                    foreach (var items in cardlists)
                                    {
                                        if (item.ICCID == items.Card_ICCID)
                                        {
                                            addcardcopy.Append("(" + "'" + items.Card_ID + "','" + items.Card_IMSI + "','" + items.Card_Type + "','" + items.Card_IMEI + "','" + 2 + "','"
                                            + item.ActivateDate + "','" + item.ActivateDate + "','"+ items.status + "','" + para.CompanyID + "','" + item.ICCID + "','"
                                            + items.operatorsID + "','" + items.accountsID + "','" + item.EndDate + "','" + items.cardType + "','"
                                            + copy + "','" + items.Platform + "','" + items.SetMealID + "','" + items.SetMealID2 + "'" + "),"
                                            );
                                        }
                                    }
                                }
                                string sqladdcopystr = addcardcopy.ToString(0,addcardcopy.Length - 1);
                                conn.Execute(updatecard);
                                conn.Execute(sqladdcopystr);
                                info.flg = "1";
                                info.Msg = "成功!";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:"+ex;
            }
            return info;
        }
    }
}