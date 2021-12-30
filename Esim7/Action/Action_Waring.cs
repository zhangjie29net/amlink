using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Dapper;
using Esim7.Models;
using Esim7.parameter.PayManage;
using Esim7.UNity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using static Esim7.Action.Action_haringGetExcel;

namespace Esim7.Action
{
    public class Action_Waring
    {
        /// <summary>
        /// 获取全部的信息 公海和客户的
        /// </summary>
        /// <param name="where"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public static List<Model_waring> model_Warings()
        {
            List<Model_waring> m = new List<Model_waring>();
            List<Model_waring> m2 = new List<Model_waring>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = @"select DISTINCT t1.Card_ICCID,t1.Card_ID,t1.Card_CompanyID ,t3.CompanyName ,t5.SetMealName,t6.operatorsName,t8.cityName,t2.Card_EndTime as CostomEdndate,t1.Card_EndTime as RealEndDatetime,
                                t1.Card_ActivationDate as RealActivationDate,t2.Card_ActivationDate as CostomActivationdatefrom card t1 left join card_copy1 t2 on t1.Card_ICCID = t2.Card_ICCID left
                                join company t3 on t3.CompanyID = t2.Card_CompanyID left join outofstock t4 on t1.OutofstockID = t4.OutofstockID left join taocan t5 on t5.SetmealID = t4.SetMealID
                                left join operators t6 on t5.OperatorsID = t6.operatorsID left join accounts t7 on t7.accountID = t4.AccountID left join city t8 on t8.cityID = t7.cityID
                                left join product t9 on t4.BatchID = t9.BatchID where  ( (TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW()))<=60
                                or  (TO_DAYS(t2.Card_EndTime) -TO_DAYS(t2.Card_ActivationDate))<=60 ) GROUP BY t1.Card_ICCID ";
                m = conn.Query<Model_waring>(sql2).ToList();
            }
            //库存
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = @"select DISTINCT t1.ICCID as Card_ICCID,t2.opendate as RealActivationDate,from_unixtime(t2.EndDate/1000)as RealEndDatetime,t5.cityName as cityName
                                ,t6.operatorsName as operatorsName,t3.SetmealName as SetMealName from product_excel  t1 left join  product t2 on t1.BatchID=t2.batchID
                                left join  taocan t3 on t3.SetmealID=t2.SetMealID left join  accounts  t4 on t4.accountID=t2.AccountID left join  city t5 on t5.cityID=t4.cityID
                                left join operators t6 on t6.OperatorsID=t3.OperatorsID where t1.isout<>1  and (TO_DAYS(from_unixtime(t2.EndDate/1000)) -TO_DAYS(NOW()))<=60 ";
                m2 = conn.Query<Model_waring>(sql2).ToList();
            }
            List<Model_waring> m3 = m.Union(m2).ToList<Model_waring>();
            return m3;
        }
        

        /// <summary>
        /// 续费操作
        /// </summary>
        /// <param name="year"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static V_ResultModel ImportExcel(int year,int type)
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
                        string EndTime = DateTime.Now.AddYears(year).ToString("yyyy-MM-dd HH:mm:ss");
                        string ssssss = DateTime.Now.AddYears(year).ToString("yyyy-MM-dd");
                        string  DT =   Unit.GetTimeStamp( DateTime.Now.AddYears(year));
                        List<string> List_ICCID = new List<string>();
                        foreach (DataRow item in table.Rows)
                        {
                            if (item["ICCID"].ToString() != "")
                            {
                                Card2 c = new Card2();
                                c.EndDate = EndTime;
                                c.ICCID = item["ICCID"].ToString();
                                List_Card.Add(c);
                                List_ICCID.Add(item["ICCID"].ToString());
                            }
                        }
                        f.Card = List_Card;
                       // f.CompanyID = CompanyID;
                        string CopyID = Unit.GetTimeStamp(DateTime.Now);
                        List<Model_Charing_Card> charing_Cards = new List<Model_Charing_Card>();
                        //检查数据是否符合
                        using (IDbConnection conns = DapperService.MySqlConnection())
                        {
                            //  DATE_FORMAT(Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate,
                            // IF(Card_ActivationDate = '0000-00-00 00:00:00', 'null', Card_ActivationDate) as Card_ActivationDate ,
                            string sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                            Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                            DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                            DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                            cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card where status=1 and (ISNULL( isout ) or isout<>1) and  Card_ICCID in @Card_ICCID";
                            charing_Cards = conns.Query<Model_Charing_Card>(sql2, new { Card_ICCID = List_ICCID }).ToList();
                        }
                        if (f.Card.Count != charing_Cards.Count)
                        {
                            result.data = "有数据不符";
                            result.successMessage = false;
                        }
                        else
                        {
                            //数据相同证明 没有遗漏或不相符的
                            //修改传入的数据  周期—正常  激活-
                            foreach (Model_Charing_Card item in charing_Cards)
                            {
                                foreach (Card2 items in f.Card)
                                {
                                    if (items.ICCID == item.Card_ICCID)
                                    {
                                      //  item.Card_ActivationDate = Convert.ToDateTime(items.ActivateDate);
                                       // item.Card_OpenDate = Convert.ToDateTime(items.ActivateDate);
                                      //  item.Card_CompanyID = CompanyID;
                                      //  item.Card_jifei_status = "00";
                                        item.Card_EndTime = Convert.ToDateTime(EndTime);
                                        //item.CopyID = CopyID;
                                        item.RenewDate = DT;
                                    }
                                }
                            }
                            string sqlCommandText = "";
                            if (type==1)
                            {
                                sqlCommandText = "update card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID in @Card_ICCID;";
                            }
                            else if (type==2)
                            {
                                sqlCommandText = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID in @Card_ICCID;";
                            }
                            #region sql 更新card的截止日期

                            #endregion
                            using (IDbConnection conns = DapperService.MySqlConnection())
                            {
                                try
                                {
                                    int results = conns.Execute(sqlCommandText, new { Card_ICCID = List_ICCID.ToArray() });
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
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        result.successMessage = false;
                        result.data = "导入失败！"+ex.ToString();
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




        public class CardsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string ICCID { get; set; }
        }
        public class Root_Renew
        {
            /// <summary>
            /// 
            /// </summary>
            public string Year { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CompanyID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<CardsItem> Cards { get; set; }
        }
        public static V_ResultModel ImportExcel2()
        {
            Root_Renew list = new Root_Renew();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            list = JsonConvert.DeserializeObject<Root_Renew>(postContent);
            string EndTime = DateTime.Now.AddYears(int.Parse(list.Year)).ToString("yyyy-MM-dd HH:mm:ss");
            string ssssss = DateTime.Now.AddYears(int.Parse(list.Year)).ToString("yyyy-MM-dd");
            string DT = Unit.GetTimeStamp(DateTime.Now.AddYears(int.Parse(list.Year)));
            string Type = list.Type;
            var result = new V_ResultModel();
            Molde_ForCustom f = new Molde_ForCustom();
            List<Card2> List_Card = new List<Card2>();
            List<string> List_ICCID = new List<string>();
            string sqlstr = string.Empty;
            foreach (CardsItem item in list.Cards)
            {
                Card2 c = new Card2();
                c.EndDate = EndTime;
                c.ICCID = item.ICCID;
                List_Card.Add(c);
                List_ICCID.Add(item.ICCID);
                sqlstr += "'" + item.ICCID + "',";
            }
            f.Card = List_Card;
            // f.CompanyID = CompanyID;
            string CopyID = Unit.GetTimeStamp(DateTime.Now);
            List<Model_Charing_Card> charing_Cards = new List<Model_Charing_Card>();
            if (Type=="1")
            {
                ///检查数据是否符合
                using (IDbConnection conns = DapperService.MySqlConnection())
                {
                    //  DATE_FORMAT(Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate,
                    // IF(Card_ActivationDate = '0000-00-00 00:00:00', 'null', Card_ActivationDate) as Card_ActivationDate ,
                    string sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                    Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                    DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                    DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                    DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                    cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card where status=1 and Card_ICCID in @Card_ICCID";
                    charing_Cards = conns.Query<Model_Charing_Card>(sql2, new { Card_ICCID = List_ICCID }).ToList();
                }
                if (f.Card.Count != charing_Cards.Count)
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
                        foreach (Card2 items in f.Card)
                        {
                            if (items.ICCID == item.Card_ICCID)
                            {
                                item.Card_EndTime = item.Card_EndTime.AddYears(int.Parse(list.Year));
                                DT = Unit.GetTimeStamp(item.Card_EndTime); ;
                                string sqlCommandText = "";
                                //sqlCommandText = "update card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID in @Card_ICCID;";
                                sqlCommandText = "update card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='"+items.ICCID+"'";
                                #region sql 更新card的截止日期
                                #endregion
                                using (IDbConnection conns = DapperService.MySqlConnection())
                                {
                                    try
                                    {
                                        //int results = conns.Execute(sqlCommandText, new { Card_ICCID = List_ICCID.ToArray() });
                                        conns.Execute(sqlCommandText);
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
                        }
                    }                   
                }
            }
            else if(Type=="2")
            {
                ///检查数据是否符合
                using (IDbConnection conns = DapperService.MySqlConnection())
                {
                    string sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                    Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                    DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                    DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                    DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                    cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card_copy1 where status=1  and Card_ICCID   in @Card_ICCID";
                    charing_Cards = conns.Query<Model_Charing_Card>(sql2).ToList();
                }
                if (f.Card.Count != charing_Cards.Count)
                {
                    result.data = "有数据不符";
                    result.successMessage = false;
                }
                else
                {
                    //数据相同证明 没有遗漏或不相符的
                    //修改传入的数据  周期—正常  激活-
                    foreach (Model_Charing_Card item in charing_Cards)
                    {
                        foreach (Card2 items in f.Card)
                        {
                            if (items.ICCID == item.Card_ICCID)
                            {
                                item.Card_EndTime = item.Card_EndTime.AddYears(int.Parse(list.Year));
                                DT = Unit.GetTimeStamp(item.Card_EndTime); ;
                                string sqlCommandText = "";
                                sqlCommandText = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='"+items.ICCID+"'";
                                using (IDbConnection conns = DapperService.MySqlConnection())
                                {
                                    try
                                    {
                                        conns.Execute(sqlCommandText);
                                        //int results = conns.Execute(sqlCommandText, new { Card_ICCID = List_ICCID.ToArray() });
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
                        }
                    }                   
                    #region sql 更新card的截止日期

                    #endregion            
                }
            }            
            return result;
        }

        ///<summary>
        ///续费操作改版
        /// </summary>
        public static OnlineRenewDto Renew_realDate(OnlineRenewPara para)
        {
            OnlineRenewDto dto = new OnlineRenewDto();
            List<Model_Charing_Card> charing_Cards = new List<Model_Charing_Card>();
            
            string strsn = string.Empty;
            string DT = string.Empty;
            string sqlupdateendtime = string.Empty;
            string sql2 = string.Empty;
            string sqlcardpayprice = string.Empty;
            decimal TotalPrice = 0;//总价
            string RenewrealOrder = string.Empty;
            string OperatorName = string.Empty;
            string sss = string.Empty;
            #region 续费条数大于3000
            if (para.Cards.Count > 20000)
            {
                dto.flg = "-1";
                dto.Msg = "失败:上传的excel卡数据不能大于2万条!";
                return dto;
            }
            if (para.Cards.Count > 3000)
            {
                int num = 0;
                num = para.Cards.Count / 3000;
                if (para.Cards.Count % 3000 != 0)
                {
                    num = num + 1;
                } 
                for (int i = 0; i < num; i++)
                {
                    string striccid = string.Empty;
                    if (para.OperatorsFlg == "1")
                    {
                        OperatorName = "中国移动";
                    }
                    if (para.OperatorsFlg == "2")
                    {
                        OperatorName = "中国电信";
                    }
                    if (para.OperatorsFlg == "3")
                    {
                        OperatorName = "中国联通";
                    }
                    if (para.OperatorsFlg == "5")
                    {
                        OperatorName = "漫游";
                    }
                    if (para.OperatorsFlg != "4")//不是全网通卡
                    {
                        int s = i * 3000;
                        foreach (var item in para.Cards.Skip(s).Take(3000))
                        {
                            striccid += "'" + item.ICCID.Trim() + "',";
                        }
                        striccid = striccid.Substring(0, striccid.Length - 1);
                    }
                    else //是全网通卡
                    {
                        OperatorName = "全网通";
                        int s = i * 3000;
                        foreach (var item in para.Cards.Skip(s).Take(3000))
                        {
                            strsn += "'" + item.SN.Trim() + "',";
                        }
                        strsn = strsn.Substring(0, strsn.Length - 1);
                    }
                    try
                    {
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            if (para.Company_ID == "1556265186243")//奇迹物联
                            {
                                if (string.IsNullOrWhiteSpace(para.CustomCompany_ID))//没有选择任何用户说明是给奇迹自己增加续费日期
                                {
                                    if (para.OperatorsFlg == "1")//移动
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                        Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                        DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                        DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                        DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                        cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card where status=1 and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (para.OperatorsFlg == "2")//电信
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                        Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                        DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                        DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                        DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                        cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from ct_card where status=1 and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (para.OperatorsFlg == "3")//联通
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                        Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                        DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                        DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                        DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                        cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from cucc_card where status=1 and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (para.OperatorsFlg == "5")//漫游
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                        Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                        DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                        DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                        DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                        cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from roamcard where status=1 and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (para.OperatorsFlg == "4")//全网通
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select SN,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from three_card where status=1 and SN in (" + strsn + ")";
                                    }
                                    charing_Cards = conn.Query<Model_Charing_Card>(sql2).ToList();
                                    if (para.Cards.Count != charing_Cards.Count)
                                    {
                                        var listcardiccid = para.Cards.Where(t => !charing_Cards.Any(x => t.ICCID == x.Card_ICCID)).ToList();
                                        foreach (var item in listcardiccid)
                                        {
                                            sss += item.ICCID + ",";
                                        }
                                        dto.Msg = "有数据不符" + sss;
                                        dto.flg = "-1";
                                        string txtcontent = "奇迹:" + para.Company_ID + "  " + "用户companyid:" + para.CustomCompany_ID + "不符合的卡数据:" + sss + "时间:" + DateTime.Now + "\br";
                                        string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + "xufeilog.txt";
                                        string pathfilesname = AppDomain.CurrentDomain.BaseDirectory + @"Files\" + FileName;
                                        System.IO.File.WriteAllText(pathfilesname, txtcontent);
                                        return dto;
                                    }
                                    else
                                    {
                                        if (para.RenewType == "1")//年
                                        {
                                            foreach (var item in charing_Cards)
                                            {
                                                item.Card_EndTime = item.Card_EndTime.AddYears(para.RenewNum);
                                                DT = Unit.GetTimeStamp(item.Card_EndTime);
                                                if (para.OperatorsFlg == "1")//移动
                                                {
                                                    sqlupdateendtime = "update card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "2")//电信
                                                {
                                                    sqlupdateendtime = "update ct_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "3")//联通
                                                {
                                                    sqlupdateendtime = "update cucc_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "5")//漫游
                                                {
                                                    sqlupdateendtime = "update roamcard set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "4")//全网通
                                                {
                                                    sqlupdateendtime = "update three_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where SN='" + item.SN + "'";
                                                }
                                                conn.Execute(sqlupdateendtime);
                                            }
                                        }
                                        if (para.RenewType == "2")//月
                                        {
                                            foreach (var item in charing_Cards)
                                            {
                                                item.Card_EndTime = item.Card_EndTime.AddMonths(para.RenewNum);
                                                DT = Unit.GetTimeStamp(item.Card_EndTime);
                                                if (para.OperatorsFlg == "1")//移动
                                                {
                                                    sqlupdateendtime = "update card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "2")//电信
                                                {
                                                    sqlupdateendtime = "update ct_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "3")//联通
                                                {
                                                    sqlupdateendtime = "update cucc_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "5")//漫游
                                                {
                                                    sqlupdateendtime = "update roamcard set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "4")//全网通
                                                {
                                                    sqlupdateendtime = "update three_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where SN='" + item.SN + "'";
                                                }
                                                conn.Execute(sqlupdateendtime);
                                            }
                                        }
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(para.CustomCompany_ID))//奇迹给用户续费
                                {
                                    if (para.OperatorsFlg == "1")//移动
                                    {
                                        //检查数据是否匹配
                                        //sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                        //Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                        //DATE_FORMAT(Card_testTime, '%Y-%m-%d %H:%i:%s') as Card_testTime,
                                        //DATE_FORMAT(Card_silentTime, '%Y-%m-%d %H:%i:%s') as Card_silentTime
                                        //DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                        //cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card_copy1 where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from card_copy1 where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (para.OperatorsFlg == "2")//电信
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from ct_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (para.OperatorsFlg == "3")//联通
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from cucc_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (para.OperatorsFlg == "5")//漫游
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from roamcard_copy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (para.OperatorsFlg == "4")//全网通
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select SN,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from three_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and SN in (" + strsn + ")";
                                    }
                                    charing_Cards = conn.Query<Model_Charing_Card>(sql2).ToList();
                                    int ss = i * 3000;
                                    var listcards = para.Cards.Skip(ss).Take(3000).ToList();
                                    int listcardcount = listcards.Count;
                                    if (listcardcount != charing_Cards.Count)
                                    {
                                        var listcardiccid = para.Cards.Where(t => !charing_Cards.Any(x => t.ICCID == x.Card_ICCID)).ToList();
                                        foreach (var item in listcardiccid)
                                        {
                                            sss += item.ICCID + ",";
                                        }
                                        dto.Msg = "有数据不符" + sss;
                                        dto.flg = "-1";
                                        string txtcontent = "奇迹:" + para.Company_ID + "  " + "用户companyid:" + para.CustomCompany_ID + "不符合的卡数据:" + sss + "时间:" + DateTime.Now + "\br";
                                        string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + "xufeilog.txt";
                                        string pathfilesname = AppDomain.CurrentDomain.BaseDirectory + @"Files\" + FileName;
                                        System.IO.File.WriteAllText(pathfilesname, txtcontent);
                                        return dto;
                                    }
                                    else
                                    {
                                        if (para.RenewType == "1")//年
                                        {
                                            foreach (var item in charing_Cards)
                                            {
                                                item.Card_EndTime = item.Card_EndTime.AddYears(para.RenewNum);
                                                DT = Unit.GetTimeStamp(item.Card_EndTime);
                                                if (para.OperatorsFlg == "1")//移动
                                                {
                                                    sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "2")//电信
                                                {
                                                    sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "3")//联通
                                                {
                                                    sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "5")//漫游
                                                {
                                                    sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "4")//全网通
                                                {
                                                    sqlupdateendtime = "update three_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where SN='" + item.SN + "'";
                                                }
                                               
                                                conn.Execute(sqlupdateendtime);
                                            }
                                        }
                                        if (para.RenewType == "2")//月
                                        {
                                            foreach (var item in charing_Cards)
                                            {
                                                item.Card_EndTime = item.Card_EndTime.AddMonths(para.RenewNum);
                                                DT = Unit.GetTimeStamp(item.Card_EndTime);
                                                if (para.OperatorsFlg == "1")//移动
                                                {
                                                    sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "2")//电信
                                                {
                                                    sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "3")//联通
                                                {
                                                    sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "5")//漫游
                                                {
                                                    sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                                }
                                                if (para.OperatorsFlg == "4")//全网通
                                                {
                                                    sqlupdateendtime = "update three_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where SN='" + item.SN + "'";
                                                }
                                                conn.Execute(sqlupdateendtime);
                                            }
                                        }
                                    }
                                }
                                dto.Msg = "成功!";
                                dto.flg = "1";
                            }
                            else //不是奇迹操作续费
                            {
                                if (para.OperatorsFlg == "1")//移动
                                {
                                    //检查数据是否匹配
                                    //sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                    //    Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                    //    DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                    //    DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,
                                    //    DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                    //    cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card_copy1 where status=1 and Card_CompanyID='" + para.Company_ID + "' and Card_ICCID in (" + striccid + ")";
                                    sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from card_copy1 where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "2")//电信
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from ct_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "3")//联通
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from cucc_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";

                                }
                                if (para.OperatorsFlg == "5")//漫游
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from roamcard_copy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                }
                                charing_Cards = conn.Query<Model_Charing_Card>(sql2).ToList();
                                if (para.Cards.Count != charing_Cards.Count)
                                {
                                    dto.Msg = "有数据不符";
                                    dto.flg = "-1";
                                    return dto;
                                }
                                else
                                {
                                    if (para.RenewType == "1")//年
                                    {
                                        //统计价格
                                        sqlcardpayprice = "select count(Card_ICCID) as num, SetMealID2,Card_ICCID,Card_CompanyID from card_copy1 where status=1 and Card_CompanyID='" + para.Company_ID + "' and Card_ICCID in(" + striccid + ") group by SetMealID2";
                                        var listsetmeal = conn.Query<Model_Charing_Card>(sqlcardpayprice).ToList();
                                        foreach (var itemsetmeal in listsetmeal)
                                        {
                                            string sqlsetmealprice = "select Price,SetmealID from setupapackage where CustomerCompanyID='" + para.Company_ID + "' and SetmealID='" + itemsetmeal.SetMealID2 + "'";
                                            decimal price = conn.Query<SetUpapackageDto>(sqlsetmealprice).Select(t => t.Price).FirstOrDefault();
                                            decimal yearsprice = para.RenewNum * 12 * price;//按月算购买的时间X12在X月单价为一张卡的价格
                                            TotalPrice += itemsetmeal.num * yearsprice;//续费的卡的总价格
                                        }
                                        //订单编号
                                        RenewrealOrder = "Renewreal" + Unit.GetTimeStamp(DateTime.Now);
                                        //添加订单
                                        string addordersql = "insert into saleorder(CustomerCompany,OrderType,OperatorName,OrderNum,TotalPrice,AddTime) values('" + para.Company_ID + "','2','" + OperatorName + "','" + RenewrealOrder + "'," + TotalPrice + ",'" + DateTime.Now + "')";
                                        //将卡号添加至续费记录表里
                                        StringBuilder addrenewalrecord = new StringBuilder("insert into renewalrecord (OrderNum,Card_ICCID,SN,OperatorsFlg,RenewType,RenewNum,CustomerCompany,AddTime) values");
                                        DateTime time = DateTime.Now;
                                        foreach (var item in para.Cards)
                                        {
                                            addrenewalrecord.Append("('" + RenewrealOrder + "','" + item.ICCID + "','" + item.SN + "','" + para.OperatorsFlg + "','" + para.RenewType + "'," + para.RenewNum + ",'" + para.Company_ID + "','" + time + "'),");
                                        }
                                        string sqladdrenewalre = addrenewalrecord.ToString();
                                        sqladdrenewalre = sqladdrenewalre.Substring(0, sqladdrenewalre.Length - 1);
                                        //生成支付信息支付宝扫码支付
                                        string APP_PRIVATE_KEY = "MIIEogIBAAKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQABAoIBAG0Cc/Z9IgU5cYYoEhid+wd6zxGMlmxiJGr0M+U9l7P7y00BsfdFQ5BJPzx1m14xLKWYeaZPVXb0AnnCd4LUvHe7f6rLQ5Wbjg/xOioHTTBYhvRGpIIokY7rlUhNwNANR9J+gxoE7OPeZaWDdEbCWXMQlxi2yH8zH3QA8PBrfcLtF0i7mfLA+/oo4hw4HpsqebUjNYGh5KR6O63yf7vJS99wFZP5Dmk5zoO8AAxeuRkEAoiGFtsa8S4xQw4GRp/3nAhUK2Ib7y0qZSlB1oUd4j6QR7IwbNDA7/8uJv4lU6OQ7QZMm02JX4zieQmkv3XtTDQ2V6TJ8EQ3NvtpyzYa9AECgYEA70SrOl0J7TygrJWVMm1hiW/uO58cZ+d+Ab/2DGOQs/pTL4I2aT0fW95Hc/IoTTRoi09VeQUAvfmhpmOwwkfZSUToB6ojV6Mg8BCntmyY7ASXWSUK95Lgfcj1Vr964PlAMBsSoM8jjgk0zvgeb8P1a6qyU/IPV1i4IAh9PbVoyEECgYEA4mTkKnkq/+iHiQsLXsyJNbNX90PbktbwCW3sa9GNLwZEhZFkq/N/tvB9+qVRYE1S/TXBHIhKaj34T0ZlYOu78ZQQM6LgM7nRStoLrlVSG/srgQlHm9LgOgynzZkXBUD+uU347cJ5aM4Ew8FAMEd9OWEeIPHrZDK4VvFXqOBYq/ECgYBn0CCG4yVSdJK2LvSb+49tRU5VOhTmFC+87KACAhUfscX0AAhBow5/GrNf4DqSPOH7R8GrD3uh8bSsb+aadPgW7TnLUYuiE5pP7roF0ZqMFPXh7MuUXXrfuJiSOeRDxoGOHcD4Wsdvchkij88M6TYLr/VNrOHxIQJKi8RjSNmcwQKBgANKxkqb0nVAM2BZycOKI+ClB/1vfizndTwd3hc/R9dMNwjeMWGSu+O0IZDYgJNu7GsEMhexH6vl1MuKUYUUSHpd1dJ6Zto5tIJrI0pYsUX45AwPT3xDl8EgV/xUYpJP/KRDLwB+GHferxENqVpKX9bKw75k5jBh0G5rOgQZpxBxAoGATTXDJ02LLE+cjJEF814V7AnMaH6/xMA9DJ/fNKaajyIviDTbB2+0ZM7bWoLdCuQ/k4K+tlMpT7foY6GE+sZHHKcdUe3llWVKR4kpGKrreP8/BZPTL3NjBMngvn68T5cq453/f7IaVDcXJyAJV/sM5wDqWfaY5KuUL1YTTallxzk=";
                                        string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQAB";
                                        IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021002118612482", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "UTF-8", false);
                                        AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
                                        request.BizContent = "{" +
                                        "\"out_trade_no\":\"" + RenewrealOrder + "\"," +
                                        "\"total_amount\":" + TotalPrice + "," +
                                        "\"product_code\":\"FAST_INSTANT_TRADE_PAY\"," +
                                        "\"subject\":\"RenewrealSimcard\"" +
                                        "  }";
                                        request.SetReturnUrl("http://www.iot-simlink.com/#/payment");//支付成功后返回的地址
                                                                                                     //request.SetReturnUrl("http://localhost:8080/#/payment");//支付成功后返回的地址
                                        AlipayTradePagePayResponse response = client.pageExecute(request, null, "post");
                                        conn.Execute(addordersql);
                                        conn.Execute(sqladdrenewalre);
                                        dto.Body = response.Body;
                                        dto.flg = "1";
                                        dto.Msg = "成功!";
                                        //foreach (var item in charing_Cards)
                                        //{
                                        //    item.Card_EndTime = item.Card_EndTime.AddYears(para.RenewNum);
                                        //    DT = Unit.GetTimeStamp(item.Card_EndTime);
                                        //    if (para.OperatorsFlg == "1")//移动
                                        //    {
                                        //        sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                        //    }
                                        //    if (para.OperatorsFlg == "2")//电信
                                        //    {
                                        //        sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                        //    }
                                        //    if (para.OperatorsFlg == "3")//联通
                                        //    {
                                        //        sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                        //    }
                                        //    if (para.OperatorsFlg == "5")//漫游
                                        //    {
                                        //        sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                        //    }
                                        //    conn.Execute(sqlupdateendtime);
                                        //}
                                    }
                                    if (para.RenewType == "2")//月
                                    {
                                        //统计价格
                                        sqlcardpayprice = "select count(Card_ICCID) as num, SetMealID2,Card_ICCID,Card_CompanyID from card_copy1 where status=1 and Card_CompanyID='" + para.Company_ID + "' and Card_ICCID in(" + striccid + ") group by SetMealID2";
                                        var listsetmeal = conn.Query<Model_Charing_Card>(sqlcardpayprice).ToList();
                                        foreach (var itemsetmeal in listsetmeal)
                                        {
                                            string sqlsetmealprice = "select Price,SetmealID from setupapackage where CustomerCompanyID='" + para.Company_ID + "' and SetmealID='" + itemsetmeal.SetMealID2 + "'";
                                            decimal price = conn.Query<SetUpapackageDto>(sqlsetmealprice).Select(t => t.Price).FirstOrDefault();
                                            decimal yearsprice = para.RenewNum * price;//按月算购买的时间X12在X月单价为一张卡的价格
                                            TotalPrice += itemsetmeal.num * yearsprice;//续费的卡的总价格
                                        }
                                        //订单编号
                                        RenewrealOrder = "Renewreal" + Unit.GetTimeStamp(DateTime.Now);
                                        //添加订单

                                        string addordersql = "insert into saleorder(CustomerCompany,OrderType,OperatorName,OrderNum,TotalPrice,AddTime) values('" + para.Company_ID + "','2','" + OperatorName + "','" + RenewrealOrder + "'," + TotalPrice + ",'" + DateTime.Now + "')";
                                        //将卡号添加至续费记录表里
                                        StringBuilder addrenewalrecord = new StringBuilder("insert into renewalrecord (OrderNum,Card_ICCID,SN,OperatorsFlg,RenewType,RenewNum,CustomerCompany,AddTime) values");
                                        DateTime time = DateTime.Now;
                                        foreach (var item in para.Cards)
                                        {
                                            addrenewalrecord.Append("('" + RenewrealOrder + "','" + item.ICCID + "','" + item.SN + "','" + para.OperatorsFlg + "','" + para.RenewType + "'," + para.RenewNum + ",'" + para.Company_ID + "','" + time + "'),");
                                        }
                                        string sqladdrenewalre = addrenewalrecord.ToString();
                                        sqladdrenewalre = sqladdrenewalre.Substring(0, sqladdrenewalre.Length - 1);
                                        //生成支付信息支付宝扫码支付
                                        string APP_PRIVATE_KEY = "MIIEogIBAAKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQABAoIBAG0Cc/Z9IgU5cYYoEhid+wd6zxGMlmxiJGr0M+U9l7P7y00BsfdFQ5BJPzx1m14xLKWYeaZPVXb0AnnCd4LUvHe7f6rLQ5Wbjg/xOioHTTBYhvRGpIIokY7rlUhNwNANR9J+gxoE7OPeZaWDdEbCWXMQlxi2yH8zH3QA8PBrfcLtF0i7mfLA+/oo4hw4HpsqebUjNYGh5KR6O63yf7vJS99wFZP5Dmk5zoO8AAxeuRkEAoiGFtsa8S4xQw4GRp/3nAhUK2Ib7y0qZSlB1oUd4j6QR7IwbNDA7/8uJv4lU6OQ7QZMm02JX4zieQmkv3XtTDQ2V6TJ8EQ3NvtpyzYa9AECgYEA70SrOl0J7TygrJWVMm1hiW/uO58cZ+d+Ab/2DGOQs/pTL4I2aT0fW95Hc/IoTTRoi09VeQUAvfmhpmOwwkfZSUToB6ojV6Mg8BCntmyY7ASXWSUK95Lgfcj1Vr964PlAMBsSoM8jjgk0zvgeb8P1a6qyU/IPV1i4IAh9PbVoyEECgYEA4mTkKnkq/+iHiQsLXsyJNbNX90PbktbwCW3sa9GNLwZEhZFkq/N/tvB9+qVRYE1S/TXBHIhKaj34T0ZlYOu78ZQQM6LgM7nRStoLrlVSG/srgQlHm9LgOgynzZkXBUD+uU347cJ5aM4Ew8FAMEd9OWEeIPHrZDK4VvFXqOBYq/ECgYBn0CCG4yVSdJK2LvSb+49tRU5VOhTmFC+87KACAhUfscX0AAhBow5/GrNf4DqSPOH7R8GrD3uh8bSsb+aadPgW7TnLUYuiE5pP7roF0ZqMFPXh7MuUXXrfuJiSOeRDxoGOHcD4Wsdvchkij88M6TYLr/VNrOHxIQJKi8RjSNmcwQKBgANKxkqb0nVAM2BZycOKI+ClB/1vfizndTwd3hc/R9dMNwjeMWGSu+O0IZDYgJNu7GsEMhexH6vl1MuKUYUUSHpd1dJ6Zto5tIJrI0pYsUX45AwPT3xDl8EgV/xUYpJP/KRDLwB+GHferxENqVpKX9bKw75k5jBh0G5rOgQZpxBxAoGATTXDJ02LLE+cjJEF814V7AnMaH6/xMA9DJ/fNKaajyIviDTbB2+0ZM7bWoLdCuQ/k4K+tlMpT7foY6GE+sZHHKcdUe3llWVKR4kpGKrreP8/BZPTL3NjBMngvn68T5cq453/f7IaVDcXJyAJV/sM5wDqWfaY5KuUL1YTTallxzk=";
                                        string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQAB";
                                        IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021002118612482", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "UTF-8", false);
                                        AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
                                        request.BizContent = "{" +
                                        "\"out_trade_no\":\"" + RenewrealOrder + "\"," +
                                        "\"total_amount\":" + TotalPrice + "," +
                                        "\"product_code\":\"FAST_INSTANT_TRADE_PAY\"," +
                                        "\"subject\":\"RenewrealSimcard\"" +
                                        "  }";
                                        request.SetReturnUrl("http://www.iot-simlink.com/#/payment");//支付成功后返回的地址
                                                                                                     //request.SetReturnUrl("http://localhost:8080/#/payment");//支付成功后返回的地址
                                        AlipayTradePagePayResponse response = client.pageExecute(request, null, "post");
                                        conn.Execute(addordersql);
                                        conn.Execute(sqladdrenewalre);
                                        dto.Body = response.Body;
                                        dto.flg = "1";
                                        dto.Msg = "成功!";
                                        //foreach (var item in charing_Cards)
                                        //{
                                        //    item.Card_EndTime = item.Card_EndTime.AddMonths(para.RenewNum);
                                        //    DT = Unit.GetTimeStamp(item.Card_EndTime);
                                        //    if (para.OperatorsFlg == "1")//移动
                                        //    {
                                        //        sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                        //    }
                                        //    if (para.OperatorsFlg == "2")//电信
                                        //    {
                                        //        sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                        //    }
                                        //    if (para.OperatorsFlg == "3")//联通
                                        //    {
                                        //        sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                        //    }
                                        //    if (para.OperatorsFlg == "5")//漫游
                                        //    {
                                        //        sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                        //    }
                                        //    conn.Execute(sqlupdateendtime);
                                        //}
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dto.Msg = "失败:" + ex;
                        dto.flg = "-1";
                    }
                }
            }
#endregion
            #region 续费的卡数据小于等于3000
            if (para.Cards.Count <= 3000)
            {
                string striccid = string.Empty;
                if (para.OperatorsFlg == "1")
                {
                    OperatorName = "中国移动";
                }
                if (para.OperatorsFlg == "2")
                {
                    OperatorName = "中国电信";
                }
                if (para.OperatorsFlg == "3")
                {
                    OperatorName = "中国联通";
                }
                if (para.OperatorsFlg == "5")
                {
                    OperatorName = "漫游";
                }
                if (para.OperatorsFlg != "4")//不是全网通卡
                {
                    foreach (var item in para.Cards)
                    {
                        striccid += "'" + item.ICCID.Trim() + "',";
                    }
                    striccid = striccid.Substring(0, striccid.Length - 1);
                }
                else //是全网通卡
                {
                    OperatorName = "全网通";
                    foreach (var item in para.Cards)
                    {
                        strsn += "'" + item.SN.Trim() + "',";
                    }
                    strsn = strsn.Substring(0, strsn.Length - 1);
                }
                try
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        if (para.Company_ID == "1556265186243")//奇迹物联
                        {
                            if (string.IsNullOrWhiteSpace(para.CustomCompany_ID))//没有选择任何用户说明是给奇迹自己增加续费日期
                            {
                                if (para.OperatorsFlg == "1")//移动
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                    Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                    DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                    DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                    DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                    cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card where status=1 and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "2")//电信
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                    Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                    DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                    DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                    DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                    cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from ct_card where status=1 and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "3")//联通
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                    Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                    DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                    DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                    DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                    cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from cucc_card where status=1 and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "5")//漫游
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                    Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                    DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                    DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as CCard_silentTime,
                                    DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                    cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from roamcard where status=1 and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "4")//全网通
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select SN,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from three_card where status=1 and SN in (" + strsn + ")";
                                }
                                charing_Cards = conn.Query<Model_Charing_Card>(sql2).ToList();
                                if (para.Cards.Count != charing_Cards.Count)
                                {

                                    var listcardiccid = para.Cards.Where(t => !charing_Cards.Any(x => t.ICCID == x.Card_ICCID)).ToList();
                                    foreach (var item in listcardiccid)
                                    {
                                        sss += item.ICCID + ",";
                                    }
                                    dto.Msg = "有数据不符" + sss;
                                    dto.flg = "-1";
                                    string txtcontent = "奇迹:" + para.Company_ID + "  " + "用户companyid:" + para.CustomCompany_ID + "不符合的卡数据:" + sss + "时间:" + DateTime.Now + "\br";
                                    string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + "xufeilog.txt";
                                    string pathfilesname = AppDomain.CurrentDomain.BaseDirectory + @"Files\" + FileName;
                                    System.IO.File.WriteAllText(pathfilesname, txtcontent);
                                    return dto;
                                }
                                else
                                {
                                    if (para.RenewType == "1")//年
                                    {
                                        foreach (var item in charing_Cards)
                                        {
                                            item.Card_EndTime = item.Card_EndTime.AddYears(para.RenewNum);
                                            DT = Unit.GetTimeStamp(item.Card_EndTime);
                                            if (para.OperatorsFlg == "1")//移动
                                            {
                                                sqlupdateendtime = "update card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "2")//电信
                                            {
                                                sqlupdateendtime = "update ct_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "3")//联通
                                            {
                                                sqlupdateendtime = "update cucc_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "5")//漫游
                                            {
                                                sqlupdateendtime = "update roamcard set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "4")//全网通
                                            {
                                                sqlupdateendtime = "update three_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where SN='" + item.SN + "'";
                                            }
                                            conn.Execute(sqlupdateendtime);
                                        }
                                    }
                                    if (para.RenewType == "2")//月
                                    {
                                        foreach (var item in charing_Cards)
                                        {
                                            item.Card_EndTime = item.Card_EndTime.AddMonths(para.RenewNum);
                                            DT = Unit.GetTimeStamp(item.Card_EndTime);
                                            if (para.OperatorsFlg == "1")//移动
                                            {
                                                sqlupdateendtime = "update card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "2")//电信
                                            {
                                                sqlupdateendtime = "update ct_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "3")//联通
                                            {
                                                sqlupdateendtime = "update cucc_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "5")//漫游
                                            {
                                                sqlupdateendtime = "update roamcard set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "4")//全网通
                                            {
                                                sqlupdateendtime = "update three_card set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where SN='" + item.SN + "'";
                                            }
                                            conn.Execute(sqlupdateendtime);
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(para.CustomCompany_ID))//奇迹给用户续费
                            {
                                if (para.OperatorsFlg == "1")//移动
                                {
                                    //检查数据是否匹配
                                    //sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                    //Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                    //DATE_FORMAT(Card_testTime, '%Y-%m-%d %H:%i:%s') as Card_testTime,
                                    //DATE_FORMAT(Card_silentTime, '%Y-%m-%d %H:%i:%s') as Card_silentTime
                                    //DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                    //cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card_copy1 where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from card_copy1 where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "2")//电信
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from ct_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "3")//联通
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from cucc_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "5")//漫游
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from roamcard_copy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                                }
                                if (para.OperatorsFlg == "4")//全网通
                                {
                                    //检查数据是否匹配
                                    sql2 = @"select SN,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from three_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and SN in (" + strsn + ")";
                                }
                                charing_Cards = conn.Query<Model_Charing_Card>(sql2).ToList();
                                if (para.Cards.Count != charing_Cards.Count)
                                {
                                    var listcardiccid = para.Cards.Where(t => !charing_Cards.Any(x => t.ICCID == x.Card_ICCID)).ToList();
                                    foreach (var item in listcardiccid)
                                    {
                                        sss += item.ICCID + ",";
                                    }
                                    dto.Msg = "有数据不符" + sss;
                                    dto.flg = "-1";
                                    string txtcontent = "奇迹:" + para.Company_ID + "  " + "用户companyid:" + para.CustomCompany_ID + "不符合的卡数据:" + sss + "时间:" + DateTime.Now + "\br";
                                    string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + "xufeilog.txt";
                                    string pathfilesname = AppDomain.CurrentDomain.BaseDirectory + @"Files\" + FileName;
                                    System.IO.File.WriteAllText(pathfilesname, txtcontent);
                                    return dto;
                                }
                                else
                                {
                                    if (para.RenewType == "1")//年
                                    {
                                        foreach (var item in charing_Cards)
                                        {
                                            item.Card_EndTime = item.Card_EndTime.AddYears(para.RenewNum);
                                            DT = Unit.GetTimeStamp(item.Card_EndTime);
                                            if (para.OperatorsFlg == "1")//移动
                                            {
                                                sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "2")//电信
                                            {
                                                sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "3")//联通
                                            {
                                                sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "5")//漫游
                                            {
                                                sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "4")//全网通
                                            {
                                                sqlupdateendtime = "update three_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where SN='" + item.SN + "'";
                                            }
                                            conn.Execute(sqlupdateendtime);
                                        }
                                    }
                                    if (para.RenewType == "2")//月
                                    {
                                        foreach (var item in charing_Cards)
                                        {
                                            item.Card_EndTime = item.Card_EndTime.AddMonths(para.RenewNum);
                                            DT = Unit.GetTimeStamp(item.Card_EndTime);
                                            if (para.OperatorsFlg == "1")//移动
                                            {
                                                sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "2")//电信
                                            {
                                                sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "3")//联通
                                            {
                                                sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "5")//漫游
                                            {
                                                sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                            }
                                            if (para.OperatorsFlg == "4")//全网通
                                            {
                                                sqlupdateendtime = "update three_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where SN='" + item.SN + "'";
                                            }
                                            conn.Execute(sqlupdateendtime);
                                        }
                                    }
                                }
                            }
                            dto.Msg = "成功!";
                            dto.flg = "1";
                        }
                        else //不是奇迹操作续费
                        {
                            if (para.OperatorsFlg == "1")//移动
                            {
                                //检查数据是否匹配
                                //sql2 = @"select Card_ID,Card_IMSI,Card_IMEI,Card_Type,Card_State,Card_WorkState,DATE_FORMAT(Card_OpenDate,'%Y-%m-%d %H:%i:%s') as Card_OpenDate, 
                                //    Card_Remarks,status,Card_CompanyID,Card_ICCID,Card_totalflow,Card_userdflow,Card_Residualflow,Card_Monthlyusageflow,operatorsID,accountsID,pici,
                                //    DATE_FORMAT(Card_testTime,'%Y-%m-%d %H:%i:%s') as Card_testTime,
                                //    DATE_FORMAT(Card_silentTime,'%Y-%m-%d %H:%i:%s') as Card_silentTime,
                                //    DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,
                                //    cardType,OpeningYearsTime,Card_jifei_status,BatchID,OutofstockID,renewdate from card_copy1 where status=1 and Card_CompanyID='" + para.Company_ID + "' and Card_ICCID in (" + striccid + ")";
                                sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from card_copy1 where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                            }
                            if (para.OperatorsFlg == "2")//电信
                            {
                                //检查数据是否匹配
                                sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from ct_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                            }
                            if (para.OperatorsFlg == "3")//联通
                            {
                                //检查数据是否匹配
                                sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from cucc_cardcopy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";

                            }
                            if (para.OperatorsFlg == "5")//漫游
                            {
                                //检查数据是否匹配
                                sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from roamcard_copy where status=1 and Card_CompanyID='" + para.CustomCompany_ID + "' and Card_ICCID in (" + striccid + ")";
                            }
                            charing_Cards = conn.Query<Model_Charing_Card>(sql2).ToList();
                            if (para.Cards.Count != charing_Cards.Count)
                            {
                                dto.Msg = "有数据不符";
                                dto.flg = "-1";
                                return dto;
                            }
                            else
                            {
                                if (para.RenewType == "1")//年
                                {
                                    //统计价格
                                    sqlcardpayprice = "select count(Card_ICCID) as num, SetMealID2,Card_ICCID,Card_CompanyID from card_copy1 where status=1 and Card_CompanyID='" + para.Company_ID + "' and Card_ICCID in(" + striccid + ") group by SetMealID2";
                                    var listsetmeal = conn.Query<Model_Charing_Card>(sqlcardpayprice).ToList();
                                    foreach (var itemsetmeal in listsetmeal)
                                    {
                                        string sqlsetmealprice = "select Price,SetmealID from setupapackage where CustomerCompanyID='" + para.Company_ID + "' and SetmealID='" + itemsetmeal.SetMealID2 + "'";
                                        decimal price = conn.Query<SetUpapackageDto>(sqlsetmealprice).Select(t => t.Price).FirstOrDefault();
                                        decimal yearsprice = para.RenewNum * 12 * price;//按月算购买的时间X12在X月单价为一张卡的价格
                                        TotalPrice += itemsetmeal.num * yearsprice;//续费的卡的总价格
                                    }
                                    //订单编号
                                    RenewrealOrder = "Renewreal" + Unit.GetTimeStamp(DateTime.Now);
                                    //添加订单
                                    string addordersql = "insert into saleorder(CustomerCompany,OrderType,OperatorName,OrderNum,TotalPrice,AddTime) values('" + para.Company_ID + "','2','" + OperatorName + "','" + RenewrealOrder + "'," + TotalPrice + ",'" + DateTime.Now + "')";
                                    //将卡号添加至续费记录表里
                                    StringBuilder addrenewalrecord = new StringBuilder("insert into renewalrecord (OrderNum,Card_ICCID,SN,OperatorsFlg,RenewType,RenewNum,CustomerCompany,AddTime) values");
                                    DateTime time = DateTime.Now;
                                    foreach (var item in para.Cards)
                                    {
                                        addrenewalrecord.Append("('" + RenewrealOrder + "','" + item.ICCID + "','" + item.SN + "','" + para.OperatorsFlg + "','" + para.RenewType + "'," + para.RenewNum + ",'" + para.Company_ID + "','" + time + "'),");
                                    }
                                    string sqladdrenewalre = addrenewalrecord.ToString();
                                    sqladdrenewalre = sqladdrenewalre.Substring(0, sqladdrenewalre.Length - 1);
                                    //生成支付信息支付宝扫码支付
                                    string APP_PRIVATE_KEY = "MIIEogIBAAKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQABAoIBAG0Cc/Z9IgU5cYYoEhid+wd6zxGMlmxiJGr0M+U9l7P7y00BsfdFQ5BJPzx1m14xLKWYeaZPVXb0AnnCd4LUvHe7f6rLQ5Wbjg/xOioHTTBYhvRGpIIokY7rlUhNwNANR9J+gxoE7OPeZaWDdEbCWXMQlxi2yH8zH3QA8PBrfcLtF0i7mfLA+/oo4hw4HpsqebUjNYGh5KR6O63yf7vJS99wFZP5Dmk5zoO8AAxeuRkEAoiGFtsa8S4xQw4GRp/3nAhUK2Ib7y0qZSlB1oUd4j6QR7IwbNDA7/8uJv4lU6OQ7QZMm02JX4zieQmkv3XtTDQ2V6TJ8EQ3NvtpyzYa9AECgYEA70SrOl0J7TygrJWVMm1hiW/uO58cZ+d+Ab/2DGOQs/pTL4I2aT0fW95Hc/IoTTRoi09VeQUAvfmhpmOwwkfZSUToB6ojV6Mg8BCntmyY7ASXWSUK95Lgfcj1Vr964PlAMBsSoM8jjgk0zvgeb8P1a6qyU/IPV1i4IAh9PbVoyEECgYEA4mTkKnkq/+iHiQsLXsyJNbNX90PbktbwCW3sa9GNLwZEhZFkq/N/tvB9+qVRYE1S/TXBHIhKaj34T0ZlYOu78ZQQM6LgM7nRStoLrlVSG/srgQlHm9LgOgynzZkXBUD+uU347cJ5aM4Ew8FAMEd9OWEeIPHrZDK4VvFXqOBYq/ECgYBn0CCG4yVSdJK2LvSb+49tRU5VOhTmFC+87KACAhUfscX0AAhBow5/GrNf4DqSPOH7R8GrD3uh8bSsb+aadPgW7TnLUYuiE5pP7roF0ZqMFPXh7MuUXXrfuJiSOeRDxoGOHcD4Wsdvchkij88M6TYLr/VNrOHxIQJKi8RjSNmcwQKBgANKxkqb0nVAM2BZycOKI+ClB/1vfizndTwd3hc/R9dMNwjeMWGSu+O0IZDYgJNu7GsEMhexH6vl1MuKUYUUSHpd1dJ6Zto5tIJrI0pYsUX45AwPT3xDl8EgV/xUYpJP/KRDLwB+GHferxENqVpKX9bKw75k5jBh0G5rOgQZpxBxAoGATTXDJ02LLE+cjJEF814V7AnMaH6/xMA9DJ/fNKaajyIviDTbB2+0ZM7bWoLdCuQ/k4K+tlMpT7foY6GE+sZHHKcdUe3llWVKR4kpGKrreP8/BZPTL3NjBMngvn68T5cq453/f7IaVDcXJyAJV/sM5wDqWfaY5KuUL1YTTallxzk=";
                                    string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQAB";
                                    IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021002118612482", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "UTF-8", false);
                                    AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
                                    request.BizContent = "{" +
                                    "\"out_trade_no\":\"" + RenewrealOrder + "\"," +
                                    "\"total_amount\":" + TotalPrice + "," +
                                    "\"product_code\":\"FAST_INSTANT_TRADE_PAY\"," +
                                    "\"subject\":\"RenewrealSimcard\"" +
                                    "  }";
                                    request.SetReturnUrl("http://www.iot-simlink.com/#/payment");//支付成功后返回的地址
                                                                                                 //request.SetReturnUrl("http://localhost:8080/#/payment");//支付成功后返回的地址
                                    AlipayTradePagePayResponse response = client.pageExecute(request, null, "post");
                                    conn.Execute(addordersql);
                                    conn.Execute(sqladdrenewalre);
                                    dto.Body = response.Body;
                                    dto.flg = "1";
                                    dto.Msg = "成功!";
                                    //foreach (var item in charing_Cards)
                                    //{
                                    //    item.Card_EndTime = item.Card_EndTime.AddYears(para.RenewNum);
                                    //    DT = Unit.GetTimeStamp(item.Card_EndTime);
                                    //    if (para.OperatorsFlg == "1")//移动
                                    //    {
                                    //        sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                    //    }
                                    //    if (para.OperatorsFlg == "2")//电信
                                    //    {
                                    //        sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                    //    }
                                    //    if (para.OperatorsFlg == "3")//联通
                                    //    {
                                    //        sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                    //    }
                                    //    if (para.OperatorsFlg == "5")//漫游
                                    //    {
                                    //        sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                    //    }
                                    //    conn.Execute(sqlupdateendtime);
                                    //}
                                }
                                if (para.RenewType == "2")//月
                                {
                                    //统计价格
                                    sqlcardpayprice = "select count(Card_ICCID) as num, SetMealID2,Card_ICCID,Card_CompanyID from card_copy1 where status=1 and Card_CompanyID='" + para.Company_ID + "' and Card_ICCID in(" + striccid + ") group by SetMealID2";
                                    var listsetmeal = conn.Query<Model_Charing_Card>(sqlcardpayprice).ToList();
                                    foreach (var itemsetmeal in listsetmeal)
                                    {
                                        string sqlsetmealprice = "select Price,SetmealID from setupapackage where CustomerCompanyID='" + para.Company_ID + "' and SetmealID='" + itemsetmeal.SetMealID2 + "'";
                                        decimal price = conn.Query<SetUpapackageDto>(sqlsetmealprice).Select(t => t.Price).FirstOrDefault();
                                        decimal yearsprice = para.RenewNum * price;//按月算购买的时间X12在X月单价为一张卡的价格
                                        TotalPrice += itemsetmeal.num * yearsprice;//续费的卡的总价格
                                    }
                                    //订单编号
                                    RenewrealOrder = "Renewreal" + Unit.GetTimeStamp(DateTime.Now);
                                    //添加订单

                                    string addordersql = "insert into saleorder(CustomerCompany,OrderType,OperatorName,OrderNum,TotalPrice,AddTime) values('" + para.Company_ID + "','2','" + OperatorName + "','" + RenewrealOrder + "'," + TotalPrice + ",'" + DateTime.Now + "')";
                                    //将卡号添加至续费记录表里
                                    StringBuilder addrenewalrecord = new StringBuilder("insert into renewalrecord (OrderNum,Card_ICCID,SN,OperatorsFlg,RenewType,RenewNum,CustomerCompany,AddTime) values");
                                    DateTime time = DateTime.Now;
                                    foreach (var item in para.Cards)
                                    {
                                        addrenewalrecord.Append("('" + RenewrealOrder + "','" + item.ICCID + "','" + item.SN + "','" + para.OperatorsFlg + "','" + para.RenewType + "'," + para.RenewNum + ",'" + para.Company_ID + "','" + time + "'),");
                                    }
                                    string sqladdrenewalre = addrenewalrecord.ToString();
                                    sqladdrenewalre = sqladdrenewalre.Substring(0, sqladdrenewalre.Length - 1);
                                    //生成支付信息支付宝扫码支付
                                    string APP_PRIVATE_KEY = "MIIEogIBAAKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQABAoIBAG0Cc/Z9IgU5cYYoEhid+wd6zxGMlmxiJGr0M+U9l7P7y00BsfdFQ5BJPzx1m14xLKWYeaZPVXb0AnnCd4LUvHe7f6rLQ5Wbjg/xOioHTTBYhvRGpIIokY7rlUhNwNANR9J+gxoE7OPeZaWDdEbCWXMQlxi2yH8zH3QA8PBrfcLtF0i7mfLA+/oo4hw4HpsqebUjNYGh5KR6O63yf7vJS99wFZP5Dmk5zoO8AAxeuRkEAoiGFtsa8S4xQw4GRp/3nAhUK2Ib7y0qZSlB1oUd4j6QR7IwbNDA7/8uJv4lU6OQ7QZMm02JX4zieQmkv3XtTDQ2V6TJ8EQ3NvtpyzYa9AECgYEA70SrOl0J7TygrJWVMm1hiW/uO58cZ+d+Ab/2DGOQs/pTL4I2aT0fW95Hc/IoTTRoi09VeQUAvfmhpmOwwkfZSUToB6ojV6Mg8BCntmyY7ASXWSUK95Lgfcj1Vr964PlAMBsSoM8jjgk0zvgeb8P1a6qyU/IPV1i4IAh9PbVoyEECgYEA4mTkKnkq/+iHiQsLXsyJNbNX90PbktbwCW3sa9GNLwZEhZFkq/N/tvB9+qVRYE1S/TXBHIhKaj34T0ZlYOu78ZQQM6LgM7nRStoLrlVSG/srgQlHm9LgOgynzZkXBUD+uU347cJ5aM4Ew8FAMEd9OWEeIPHrZDK4VvFXqOBYq/ECgYBn0CCG4yVSdJK2LvSb+49tRU5VOhTmFC+87KACAhUfscX0AAhBow5/GrNf4DqSPOH7R8GrD3uh8bSsb+aadPgW7TnLUYuiE5pP7roF0ZqMFPXh7MuUXXrfuJiSOeRDxoGOHcD4Wsdvchkij88M6TYLr/VNrOHxIQJKi8RjSNmcwQKBgANKxkqb0nVAM2BZycOKI+ClB/1vfizndTwd3hc/R9dMNwjeMWGSu+O0IZDYgJNu7GsEMhexH6vl1MuKUYUUSHpd1dJ6Zto5tIJrI0pYsUX45AwPT3xDl8EgV/xUYpJP/KRDLwB+GHferxENqVpKX9bKw75k5jBh0G5rOgQZpxBxAoGATTXDJ02LLE+cjJEF814V7AnMaH6/xMA9DJ/fNKaajyIviDTbB2+0ZM7bWoLdCuQ/k4K+tlMpT7foY6GE+sZHHKcdUe3llWVKR4kpGKrreP8/BZPTL3NjBMngvn68T5cq453/f7IaVDcXJyAJV/sM5wDqWfaY5KuUL1YTTallxzk=";
                                    string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQAB";
                                    IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021002118612482", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "UTF-8", false);
                                    AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
                                    request.BizContent = "{" +
                                    "\"out_trade_no\":\"" + RenewrealOrder + "\"," +
                                    "\"total_amount\":" + TotalPrice + "," +
                                    "\"product_code\":\"FAST_INSTANT_TRADE_PAY\"," +
                                    "\"subject\":\"RenewrealSimcard\"" +
                                    "  }";
                                    request.SetReturnUrl("http://www.iot-simlink.com/#/payment");//支付成功后返回的地址
                                                                                                 //request.SetReturnUrl("http://localhost:8080/#/payment");//支付成功后返回的地址
                                    AlipayTradePagePayResponse response = client.pageExecute(request, null, "post");
                                    conn.Execute(addordersql);
                                    conn.Execute(sqladdrenewalre);
                                    dto.Body = response.Body;
                                    dto.flg = "1";
                                    dto.Msg = "成功!";
                                    //foreach (var item in charing_Cards)
                                    //{
                                    //    item.Card_EndTime = item.Card_EndTime.AddMonths(para.RenewNum);
                                    //    DT = Unit.GetTimeStamp(item.Card_EndTime);
                                    //    if (para.OperatorsFlg == "1")//移动
                                    //    {
                                    //        sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                    //    }
                                    //    if (para.OperatorsFlg == "2")//电信
                                    //    {
                                    //        sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                    //    }
                                    //    if (para.OperatorsFlg == "3")//联通
                                    //    {
                                    //        sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                    //    }
                                    //    if (para.OperatorsFlg == "5")//漫游
                                    //    {
                                    //        sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + item.Card_ICCID + "'";
                                    //    }
                                    //    conn.Execute(sqlupdateendtime);
                                    //}
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    dto.Msg = "失败:" + ex;
                    dto.flg = "-1";
                }
            }
            #endregion
            return dto;
        }
    }
}