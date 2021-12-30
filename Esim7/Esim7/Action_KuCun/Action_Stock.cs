using Dapper;
using Esim7.Models;
using Esim7.UNity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static Esim7.Action.Action_haringGetExcel;
using static Esim7.Model_Stock.Model_Stock_Config;

namespace Esim7.Action_KuCun
{       /// <summary>
/// 库存模块    
/// </summary>
    public class Action_Stock
    {
        #region  配置信息模块  添加
        /// <summary>
        /// 添加套餐     {"Operator":"12","Code":"123","PartNumber":"wef","PackageDescribe":"sgfer","Remark":"cewcw","Flow":"","CardXTID":"","CardTypeID":""}
        /// </summary>
        public static V_ResultModel Add_SetMeal()
        {
            V_ResultModel v = new V_ResultModel();
            Model_Stock.Model_Stock_Config.Package package = new Model_Stock.Model_Stock_Config.Package();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            package = JsonConvert.DeserializeObject<Model_Stock.Model_Stock_Config.Package>(postContent);
            string SetmealID = Unit.GetTimeStamp(DateTime.Now);
            StringBuilder sql = new StringBuilder("INSERT INTO setmeal (OperatorID,Code,PartNumber,Remark,PackageDescribe,SetmealID,Flow,CardXTID,CardTypeID)values('" + package.Operator + "','" +
            package.Code + "','" + package.PartNumber + "','" + package.Remark + "','" + package.PackageDescribe + "','" + SetmealID + "','" + package.Flow + "','" + package.CardXTID + "','" + package.CardTypeID + "')");
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                try
                {
                    int resultss = connss.Execute(sql.ToString());
                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                {
                    v.data = ex.ToString();
                    v.successMessage = false;
                }
            }
            return v;
        }
        /// <summary>
        /// 添加运营商{"OperatorName":"中国联通"}
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel Add_Operator()
        {
            V_ResultModel v = new V_ResultModel();
            Model_Stock.Model_Stock_Config.Operator package = new Model_Stock.Model_Stock_Config.Operator();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            package = JsonConvert.DeserializeObject<Model_Stock.Model_Stock_Config.Operator>(postContent);
            string SetmealID = Unit.GetTimeStamp(DateTime.Now);
            StringBuilder sql = new StringBuilder("INSERT INTO operator (OperatorID,OperatorName)values('" + SetmealID + "','" + package.OperatorName + "')");
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                try
                {
                    int resultss = connss.Execute(sql.ToString());
                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                {
                    v.data = ex.ToString();
                    v.successMessage = false;
                }
            }
            return v;
        }
        /// <summary>
        /// 添加库房
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel Add_storageroom()
        {
            V_ResultModel v = new V_ResultModel();
            Model_Stock.Model_Stock_Config.storageroom package = new Model_Stock.Model_Stock_Config.storageroom();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            package = JsonConvert.DeserializeObject<Model_Stock.Model_Stock_Config.storageroom>(postContent);
            string SetmealID = Unit.GetTimeStamp(DateTime.Now);
            StringBuilder sql = new StringBuilder("INSERT INTO storageroom (StorageRoomID,StorageRoomName)values('" + SetmealID + "','" + package.StorageRoomName + "')");
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                try
                {
                    int resultss = connss.Execute(sql.ToString());
                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                {
                    v.data = ex.ToString();
                    v.successMessage = false;
                }
            }
            return v;
        }
        /// <summary>
        /// 添加卡类型
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel Add_CardType()
        {
            V_ResultModel v = new V_ResultModel();
            Model_Stock.Model_Stock_Config.CardType package = new Model_Stock.Model_Stock_Config.CardType();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            package = JsonConvert.DeserializeObject<Model_Stock.Model_Stock_Config.CardType>(postContent);
            string SetmealID = Unit.GetTimeStamp(DateTime.Now);
            StringBuilder sql = new StringBuilder("INSERT INTO cardtype (CardTypeID,CardTypeName)values('" + SetmealID + "','" + package.CardTypeName + "')");
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                try
                {
                    int resultss = connss.Execute(sql.ToString());
                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                {
                    v.data = ex.ToString();
                    v.successMessage = false;
                }
            }
            return v;
        }
        /// <summary>
        ///  卡形态 添加
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel Add_CardXingTai()
        {
            V_ResultModel v = new V_ResultModel();
            Model_Stock.Model_Stock_Config.CardXingTai package = new Model_Stock.Model_Stock_Config.CardXingTai();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            package = JsonConvert.DeserializeObject<Model_Stock.Model_Stock_Config.CardXingTai>(postContent);
            string SetmealID = Unit.GetTimeStamp(DateTime.Now);
            StringBuilder sql = new StringBuilder("INSERT INTO card_xingtai (CardXTID,CardXTName)values('" + SetmealID + "','" + package.CardXTName + "')");
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                try
                {
                    int resultss = connss.Execute(sql.ToString());
                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                {
                    v.data = ex.ToString();
                    v.successMessage = false;
                }
            }
            return v;
        }
        #endregion
        #region     查看配置信息
        /// <summary>
        ///  查看配置信息  operator 运营商信息    storageroom 库房信息   cardtype 卡类型信息   cardxingtai 卡形态信息    account   API设置如惠州移动  西安移动 等
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static V_ResultModel2 GetStockMessage(string Type)
        {
            V_ResultModel2 v = new V_ResultModel2();
            string sql = "select *  from ";
            switch (Type)
            {
                case "operator":
                    sql += " operator";
                    break;
                case "storageroom":
                    sql += "storageroom";
                    break;
                case "cardtype":
                    sql += "cardtype";
                    break;
                case "cardxingtai":
                    sql += "card_xingtai";
                    break;
                case "account":
                    sql += " accounts where Company_ID=1556265186243";
                    break;
                default:
                    break;
            }
            try
            {
                using (IDbConnection connss = DapperService.MySqlConnection())
                {
                    v.AdditionalInformation = connss.Query(sql).ToList();
                }
                v.data = "Success";
                v.successMessage = true;
            }
            catch (Exception ex)
            {
                v.data = "Fail";
                v.successMessage = false;
                v.AdditionalInformation = ex.ToString();
            }
            return v;
        }
        /// <summary>
        ///获取套餐信息
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel2 GetSelmeal(string Company_ID)
        {
            string sql = @"select  t1.CardTypeID,t2.CardTypeName,t1.OperatorID,t3.OperatorName ,t1.CardXTID,t4.CardXTName,t1.`Code`,t1.Flow,t1.PackageDescribe,t1.PartNumber,t1.Remark, t1.SetMealID from setmeal t1
                            left  join cardtype t2 on t2.CardTypeID = t1.CardTypeID left  join  operator t3 on t3.OperatorID = t1.OperatorID left  join card_xingtai t4 on t4.CardXTID = t1.CardXTID where t1.Company_ID='" + Company_ID + "' and t1.status='0'";
            V_ResultModel2 v = new V_ResultModel2();
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                v.AdditionalInformation = connss.Query(sql).OrderBy(t => t.id).ToList();
            }
            v.data = "Success";
            v.successMessage = true;
            return v;
        }

        ///<summary>
        ///修改套餐
        /// </summary>
        public static Information UpateSetmeal(UpdateSetmealPara para)
        {
            Information info = new Information();
            try
            {
                string sqlupdate = "update setmeal set OperatorID='"+para.OperatorID+ "',PartNumber='"+para.PartNumber+ "',Remark='"+para.Remark+"'," +
                    "PackageDescribe='"+para.PackageDescribe+"',Flow="+para.Flow+",CardTypeID='"+para.CardTypeID+"',CardXTID='"+para.CardXTID+"' where SetmealID='"+para.SetMealID+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sqlupdate);
                    info.flg = "1";
                    info.Msg = "修改成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "修改失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///删除套餐
        /// </summary>
        public static Information DeleSetmeal(string SetMealID)
        {
            Information info = new Information();
            try
            {
                string sqldel = "update setmeal set status='-1' where SetmealID='" + SetMealID+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sqldel);
                    info.flg = "1";
                    info.Msg = "删除成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "删除失败!";
            }
            return info;
        }

        ///<summary>
        ///获取各种卡的套餐
        /// </summary>
        public static SelmealTypeDto GetSelmealType(string Company_ID)
        {
            SelmealTypeDto dto = new SelmealTypeDto();
            List<SelmealType> types = new List<SelmealType>();
            try
            {
                string sql = @"select OperatorID from setmeal where Company_ID='"+Company_ID+"'";
                using (IDbConnection connss = DapperService.MySqlConnection())
                {
                    var typesinfo = connss.Query<SelmealType>(sql).ToList();
                    foreach (var item in typesinfo.GroupBy(t=>t.OperatorID))
                    {
                        List<SelmealInfo> infos = new List<SelmealInfo>();
                        SelmealType type = new SelmealType();
                        string OperatorNamesql = "select * from operator where OperatorID='" + item.Key+"'";
                        //type.value = item.Key;
                        type.value = Convert.ToInt64(item.Key);
                        type.label = connss.Query<SelmealType>(OperatorNamesql).Select(t => t.OperatorName).FirstOrDefault();
                        types.Add(type);
                        string sqlsetmealinfo = @"select  t1.CardTypeID,t2.CardTypeName,t1.OperatorID,t3.OperatorName ,t1.CardXTID,t4.CardXTName,t1.`Code`,t1.Flow,t1.PackageDescribe as label,t1.PartNumber,t1.Remark, t1.SetMealID as value from setmeal t1
                            left  join cardtype t2 on t2.CardTypeID = t1.CardTypeID left  join  operator t3 on t3.OperatorID = t1.OperatorID left  join card_xingtai t4 on t4.CardXTID = t1.CardXTID where t1.Company_ID=1556265186243 and t1.OperatorID='"+item.Key+"'";
                        var typesinfoitem = connss.Query<SelmealInfo>(sqlsetmealinfo).ToList();
                        foreach (var items in typesinfoitem)
                        {
                            SelmealInfo info = new SelmealInfo();
                            info.label = items.label;
                            info.value = items.value;
                            info.OperatorName = items.label;
                            info.CardTypeID = items.CardTypeID;
                            info.CardTypeName = items.CardTypeName;
                            info.CardXTID = items.CardXTID;
                            info.CardXTName = items.CardXTName;
                            info.Code = items.Code;
                            info.Flow = items.Flow;
                            info.PartNumber = items.PartNumber;
                            info.Remark = items.Remark;
                            infos.Add(info);
                            type.children = infos;
                        }
                        dto.types = types;
                    }
                    dto.flg = "1";
                    dto.Msg = "成功";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }
        #endregion        
        /// <summary>
        /// 入库操作     product记录库存  croduct_excel 库存库房    taocan 对应套餐拓展信息 
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel WareHousing()
        {
            WareHousing_Message wareHousing = new WareHousing_Message();
            V_ResultModel v = new V_ResultModel();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            DateTime time = DateTime.Now;
            sRead.Close();
            wareHousing = JsonConvert.DeserializeObject<WareHousing_Message>(postContent);
            DateTime ActivateDate = wareHousing.ActivateDate.AddHours(8);
            DateTime EndDate = wareHousing.EndDate.AddHours(8);
            string companycardnum = string.Empty;
            int cardnumber = 0;
            cardnumber = wareHousing.ICCIDS.Count;
            string iccids = string.Empty;
            #region   判重           
            StringBuilder Sql_JudgeRepet = new StringBuilder("select  ICCID from product_excel where ICCID in( ");
            foreach (Excel_ICCID item in wareHousing.ICCIDS)
            {
                Regex regExp = new Regex(@"^[a-zA-z0-9]+$");
                if (!regExp.IsMatch(item.ICCID))
                {
                    iccids += item.ICCID + ",";
                    v.data = "ICCID格式错误:" + iccids;
                    v.successMessage = false;
                    return v;
                }
                else
                {
                    item.ICCID = Regex.Replace(item.ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                    Sql_JudgeRepet.Append("'" + item.ICCID + "',");
                }
            }
            string sql_JD = Sql_JudgeRepet.ToString();
            sql_JD = sql_JD.Substring(0, sql_JD.Length - 1) + ")";
            List<Excel_ICCID> li = new List<Excel_ICCID>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {               
                li = conn.Query<Excel_ICCID>(sql_JD).ToList();
            }
            #endregion
            if (li.Count != 0)
            {
                string s = "";
                foreach (Excel_ICCID item in li)
                {
                    s += item.ICCID + ",";
                }
                s = s.Substring(0, s.Length - 1);
                v.data = "数据重复:" + s;
                v.successMessage = false;
                return v;
            }
            else
            {
                if (wareHousing.ICCIDS.Count > 3000)
                {
                    int num = wareHousing.ICCIDS.Count / 3000;
                    if (wareHousing.ICCIDS.Count % 3000 == 0)
                    {
                        num =num;
                    }
                    if (wareHousing.ICCIDS.Count % 3000 != 0)
                    {
                        num = num + 1;
                    }
                    for (int j = 0; j < num; j++)
                    {
                        //var ICCID = wareHousing.ICCIDS.GroupBy(c => c.ICCID).Select(c => c.First());
                        int knum = j * 3000;
                        var ICCID = wareHousing.ICCIDS.GroupBy(c => c.ICCID).Select(c => c.First()).Skip(knum).Take(3000);
                        int cardnum = ICCID.Count();
                        //表taocan 中的SetmaelID
                        string SetmealID = Unit.GetTimeStamp(DateTime.Now);
                        //添加到套餐  sql
                        StringBuilder sql_Taocan = new StringBuilder("INSERT INTO taocan (SetmealID,testDate,silentDate,OpeningDate,OperatorsID,Remarks,SetmealID2)" +
                                       "values('" + SetmealID + "','" + wareHousing.testDate + "','" + wareHousing.silentDate + "','" + wareHousing.OpeningDate + "','" + wareHousing.OperatorsID + "','" + wareHousing.Remarks + "','" + wareHousing.SetmealID2 + "')");
                        var Card_ICCIDStart = wareHousing.ICCIDS.First().ICCID;
                        var Card_ICCIDEND = wareHousing.ICCIDS.Last().ICCID;
                        StringBuilder sql_Product_Excel = new StringBuilder("INSERT INTO product_excel  (ICCID, BatchID, AccountID, isout)VALUES");
                        StringBuilder Sql_Card = new StringBuilder();
                        if (wareHousing.OperatorsFlg == "1573631210918" || string.IsNullOrWhiteSpace(wareHousing.OperatorsFlg))//移动卡
                        {
                            Sql_Card.Append("Insert into card (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,Card_ActivationDate,Card_EndTime,RenewDate,RegionLabel)VALUES");
                        }
                        if (wareHousing.OperatorsFlg == "1573631225967")//电信卡
                        {
                            Sql_Card.Append("Insert into ct_card (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,Card_ActivationDate,Card_EndTime,RenewDate,RegionLabel)VALUES");
                        }
                        if (wareHousing.OperatorsFlg == "1573631234734")//联通卡
                        {
                            Sql_Card.Append("Insert into cucc_card (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,Card_ActivationDate,Card_EndTime,RenewDate,RegionLabel)VALUES");
                        }
                        if (wareHousing.OperatorsFlg == "1594176308883")//漫游卡
                        {
                            Sql_Card.Append("Insert into roamcard (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,Card_ActivationDate,Card_EndTime,RenewDate,RegionLabel)VALUES");
                        }
                        //StringBuilder Sql_Card = new StringBuilder("Insert into card (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2)VALUES");
                        int i = 0;
                        foreach (Excel_ICCID item in ICCID)
                        {
                            item.ICCID = Regex.Replace(item.ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                            sql_Product_Excel.Append("('" + item.ICCID + "','" + SetmealID + "','" + wareHousing.AccountID + "','0'),");
                            Sql_Card.Append("('" + item.ICCID + "','" + wareHousing.AccountID + "','1556265186243','1','" + wareHousing.Platform + "','" + SetmealID + "','" + wareHousing.SetmealID2 + "','" + ActivateDate + "','" + EndDate + "','" + EndDate + "','" + wareHousing.RegionLabel + "'),");
                            i++;
                        }
                        StringBuilder sql_Product = new StringBuilder(@"INSERT INTO product  (SetMealID, StorageRoomID, ProductNumber, ICCIDStart, ICCIDEnd,  operatorID, batchID, Remark, AccountID, isExcel, ProductNumbers,AddTime)
                             VALUES(" + "'" + SetmealID + "','" + wareHousing.StorageRoomID + "'," + i + ",'" + Card_ICCIDStart + "','" + Card_ICCIDEND + "','" + wareHousing.operatorID + "','" + SetmealID + "','" + wareHousing.Remarks + "','" + wareHousing.AccountID + "',1 ," + ICCID.ToList().Count + ",'" + time + "') ");
                        string sql_PE = sql_Product_Excel.ToString();
                        sql_PE = sql_PE.Substring(0, sql_PE.Length - 1);
                        string Sql_CD = Sql_Card.ToString();
                        Sql_CD = Sql_CD.Substring(0, Sql_CD.Length - 1);

                        using (IDbConnection connss = DapperService.MySqlConnection())
                        {
                            try
                            {
                                string sql_pr = sql_Product.ToString();
                                string sql_tao = sql_Taocan.ToString();
                                var InnerTaocan = connss.Execute(sql_Taocan.ToString()); //向套餐详细信息log添加
                                var INnerProduct = connss.Execute(sql_Product.ToString());  //向库存log中添加
                                var InnerProdoct_excel = connss.Execute(sql_PE);  //向库存添加
                                var InnerCard = connss.Execute(Sql_CD);    //向公海中添加                                         
                                string sqlcompanynum = "select CompanyTolCardNum as Number from company where CompanyID='1556265186243'";//获取用户卡数量
                                companycardnum = connss.Query<Company>(sqlcompanynum).Select(t => t.Number).FirstOrDefault();//获取数量
                                cardnumber = Convert.ToInt32(companycardnum) + cardnum;
                                //修改用户卡数据
                                string updateCompanyCardNmber = "update company set CompanyTolCardNum=" + cardnumber + " where CompanyID='1556265186243'";
                                connss.Execute(updateCompanyCardNmber);//修改奇迹物联卡数量
                                v.data = "成功";
                                v.successMessage = true;
                            }
                            catch (Exception ex)
                            {
                                StringBuilder RollBACK_sql_Taocan = new StringBuilder("delete from taocan  where SetmealID='" + SetmealID + "' ");
                                StringBuilder RollBACK_sql_Product = new StringBuilder("delete from product where batchID='" + SetmealID + "' ");
                                StringBuilder RollBACK_sql_Product_Excel = new StringBuilder("delete from product_excel where BatchID='" + SetmealID + "' ");
                                StringBuilder RollBACK_Sql_Card = new StringBuilder("delete from card where SetmealID='" + SetmealID + "' ");
                                connss.Execute(RollBACK_sql_Taocan.ToString());
                                connss.Execute(RollBACK_sql_Product.ToString());
                                connss.Execute(RollBACK_sql_Product_Excel.ToString());
                                connss.Execute(RollBACK_Sql_Card.ToString());
                                v.data = "出现错误请联系管理人员" + "";
                                Console.WriteLine("ERROR：" + ex.ToString());
                                v.successMessage = false;
                                throw ex;
                            }
                        }
                    }
                }
                if (wareHousing.ICCIDS.Count <= 3000)
                {
                    var ICCID = wareHousing.ICCIDS.GroupBy(c => c.ICCID).Select(c => c.First());
                    //表taocan 中的SetmaelID
                    string SetmealID = Unit.GetTimeStamp(DateTime.Now);
                    //添加到套餐  sql
                    StringBuilder sql_Taocan = new StringBuilder("INSERT INTO taocan (SetmealID,testDate,silentDate,OpeningDate,OperatorsID,Remarks,SetmealID2)" +
                                   "values('" + SetmealID + "','" + wareHousing.testDate + "','" + wareHousing.silentDate + "','" + wareHousing.OpeningDate + "','" + wareHousing.OperatorsID + "','" + wareHousing.Remarks + "','" + wareHousing.SetmealID2 + "')");
                    var Card_ICCIDStart = wareHousing.ICCIDS.First().ICCID;
                    var Card_ICCIDEND = wareHousing.ICCIDS.Last().ICCID;
                    StringBuilder sql_Product_Excel = new StringBuilder("INSERT INTO product_excel  (ICCID, BatchID, AccountID, isout)VALUES");
                    StringBuilder Sql_Card = new StringBuilder();
                    if (wareHousing.OperatorsFlg == "1573631210918" || string.IsNullOrWhiteSpace(wareHousing.OperatorsFlg))//移动卡
                    {
                        Sql_Card.Append("Insert into card (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,Card_ActivationDate,Card_EndTime,RenewDate,RegionLabel)VALUES");
                    }
                    if (wareHousing.OperatorsFlg == "1573631225967")//电信卡
                    {
                        Sql_Card.Append("Insert into ct_card (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,Card_ActivationDate,Card_EndTime,RenewDate,RegionLabel)VALUES");
                    }
                    if (wareHousing.OperatorsFlg == "1573631234734")//联通卡
                    {
                        Sql_Card.Append("Insert into cucc_card (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,Card_ActivationDate,Card_EndTime,RenewDate,RegionLabel)VALUES");
                    }
                    if (wareHousing.OperatorsFlg == "1594176308883")//漫游卡
                    {
                        Sql_Card.Append("Insert into roamcard (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,Card_ActivationDate,Card_EndTime,RenewDate,RegionLabel)VALUES");
                    }
                    //StringBuilder Sql_Card = new StringBuilder("Insert into card (Card_ICCID,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2)VALUES");
                    int i = 0;
                    foreach (Excel_ICCID item in ICCID)
                    {
                        item.ICCID = Regex.Replace(item.ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                        sql_Product_Excel.Append("('" + item.ICCID + "','" + SetmealID + "','" + wareHousing.AccountID + "','0'),");
                        Sql_Card.Append("('" + item.ICCID + "','" + wareHousing.AccountID + "','1556265186243','1','" + wareHousing.Platform + "','" + SetmealID + "','" + wareHousing.SetmealID2 + "','" + ActivateDate + "','" + EndDate + "','" + EndDate + "','" + wareHousing.RegionLabel + "'),");
                        i++;
                    }
                    StringBuilder sql_Product = new StringBuilder(@"INSERT INTO product  (SetMealID, StorageRoomID, ProductNumber, ICCIDStart, ICCIDEnd,  operatorID, batchID, Remark, AccountID, isExcel, ProductNumbers,AddTime)
                             VALUES(" + "'" + SetmealID + "','" + wareHousing.StorageRoomID + "'," + i + ",'" + Card_ICCIDStart + "','" + Card_ICCIDEND + "','" + wareHousing.operatorID + "','" + SetmealID + "','" + wareHousing.Remarks + "','" + wareHousing.AccountID + "',1 ," + ICCID.ToList().Count + ",'" + time + "') ");
                    string sql_PE = sql_Product_Excel.ToString();
                    sql_PE = sql_PE.Substring(0, sql_PE.Length - 1);
                    string Sql_CD = Sql_Card.ToString();
                    Sql_CD = Sql_CD.Substring(0, Sql_CD.Length - 1);
                    using (IDbConnection connss = DapperService.MySqlConnection())
                    {
                        try
                        {
                            string sql_pr = sql_Product.ToString();
                            string sql_tao = sql_Taocan.ToString();
                            var InnerTaocan = connss.Execute(sql_Taocan.ToString()); //向套餐详细信息log添加
                            var INnerProduct = connss.Execute(sql_Product.ToString());  //向库存log中添加
                            var InnerProdoct_excel = connss.Execute(sql_PE);  //向库存添加
                            var InnerCard = connss.Execute(Sql_CD);    //向公海中添加                                         
                            string sqlcompanynum = "select CompanyTolCardNum as Number from company where CompanyID='1556265186243'";//获取用户卡数量
                            companycardnum = connss.Query<Company>(sqlcompanynum).Select(t => t.Number).FirstOrDefault();//获取数量
                            cardnumber = Convert.ToInt32(companycardnum) + cardnumber;
                            //修改用户卡数据
                            string updateCompanyCardNmber = "update company set CompanyTolCardNum=" + cardnumber + " where CompanyID='1556265186243'";
                            connss.Execute(updateCompanyCardNmber);//修改奇迹物联卡数量
                            v.data = "成功";
                            v.successMessage = true;
                        }
                        catch (Exception ex)
                        {
                            StringBuilder RollBACK_sql_Taocan = new StringBuilder("delete from taocan  where SetmealID='" + SetmealID + "' ");
                            StringBuilder RollBACK_sql_Product = new StringBuilder("delete from product where batchID='" + SetmealID + "' ");
                            StringBuilder RollBACK_sql_Product_Excel = new StringBuilder("delete from product_excel where BatchID='" + SetmealID + "' ");
                            StringBuilder RollBACK_Sql_Card = new StringBuilder("delete from card where SetmealID='" + SetmealID + "' ");
                            connss.Execute(RollBACK_sql_Taocan.ToString());
                            connss.Execute(RollBACK_sql_Product.ToString());
                            connss.Execute(RollBACK_sql_Product_Excel.ToString());
                            connss.Execute(RollBACK_Sql_Card.ToString());
                            v.data = "出现错误请联系管理人员" + "";
                            Console.WriteLine("ERROR：" + ex.ToString());
                            v.successMessage = false;
                            throw ex;
                        }
                    }
                }
                return v;
            }
        }
        /// <summary>
        /// 获取库存信息 
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel2 GetProuud()
        {
            V_ResultModel2 v = new V_ResultModel2();
            try
            {
                string sql = @"select t1.ICCIDStart,t1.ICCIDEnd,date_format(t1.Addtime, '%Y-%m-%d %H:%i:%s') as Addtime,t1.ProductNumber,t1.ProductNumbers,t1.batchID,t2.silentDate,t2.testDate,t3.PackageDescribe,t3.`Code`,t3.Flow,t1.Remark as Remark ,t4.CardTypeName,t5.CardXTName,t6.OperatorName,t7.StorageRoomName,
                                t10.operatorsName  ,t9.accountID ,t1.SetmealID from   product t1 left join taocan t2 on t2.SetmealID=t1.SetMealID
                                left join setmeal t3 on t3.SetmealID=t2.SetmealID2 left  join cardtype t4 on t4.CardTypeID=t3.CardTypeID left join  card_xingtai t5 on t5.CardXTID=t3.CardXTID
                                left join  operator t6 on t6.OperatorID=t3.OperatorID left join  storageroom t7 on t7.StorageRoomID=t1.StorageRoomID
                                left join  accounts t9  on  t9.AccountID=t1.AccountID left join operators t10 on t10.operatorsid=t9.operatorsid ORDER BY t1.Addtime desc";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    v.AdditionalInformation = conn.Query<object>(sql).ToList();
                    v.data = "Success";
                    v.successMessage = true;
                }
            }
            catch (Exception)
            {
                v.AdditionalInformation = "出现错误";
                v.data = "fail";
                v.successMessage = false;
            }
            return v;
        }
        /// <summary>falilefafismcvsdj
        /// 出库操作     excel
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel OutOfStock()
        {
            OutOfStock outOfStock = new OutOfStock();
            V_ResultModel v = new V_ResultModel();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            outOfStock = JsonConvert.DeserializeObject<OutOfStock>(postContent);
            #region   判重 和数量  
            //判重
            StringBuilder Sql_JudgeRepet = new StringBuilder("select  ICCID from product_excel where ICCID in( ");
            //数量
            StringBuilder Sql_Num = new StringBuilder("select  ICCID  from product_excel where ICCID in( ");
            var ICCIDS = outOfStock.ICCIDS.GroupBy(c => c.ICCID).Select(c => c.First());
            foreach (Excel_ICCID item in ICCIDS)
            {
                Sql_JudgeRepet.Append("'" + item.ICCID + "',");
                Sql_Num.Append("'" + item.ICCID + "',");
            }
            string sql_JD = Sql_JudgeRepet.ToString();
            sql_JD = sql_JD.Substring(0, sql_JD.Length - 1) + ")where isout='1'";
            string sql_Num = Sql_Num.ToString();
            sql_Num = sql_Num.Substring(0, sql_Num.Length - 1) + ")where isout='0'";
            List<Excel_ICCID> li = new List<Excel_ICCID>();
            List<Excel_ICCID> li_Num = new List<Excel_ICCID>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                li = conn.Query<Excel_ICCID>(sql_JD).ToList();
                li_Num = conn.Query<Excel_ICCID>(sql_Num).ToList();
            }
            string ICCIDstart = li.First().ICCID;
            string ICCIDend = li.Last().ICCID;
            #endregion

            if (li.Count != 0)
            {
                string s = null;
                foreach (Excel_ICCID item in li)
                {
                    s += item.ICCID + ",";
                }
                s = s.Substring(0, s.Length - 1);
                v.data = "有数据已经出库:" + s;
                v.successMessage = true;
                return v;
            }
            else if (li_Num.Count != ICCIDS.ToList().Count)
            {
                var L = li.Except(ICCIDS);
                string s = null;
                foreach (Excel_ICCID item in L)
                {
                    s += item.ICCID + ",";
                }
                s = s.Substring(0, s.Length - 1);
                v.data = "有数据不符:" + s;
                v.successMessage = true;
                return v;
            }
            else
            {
                #region  通过excel 筛选  套餐 和出库单号

                  //获取分组出库库单
                StringBuilder Get_OutodStock_Group = new StringBuilder("");
                #endregion
                //出库单内码
                string OutofstockID = Unit.GetTimeStamp(DateTime.Now);
                //添加出库单 log  字符串拼接 此处可以使用dapper               
                StringBuilder AddOutOfStock = new StringBuilder(@"INSERT INTO outofstock (OutofstockID,remark,BatchID,ICCIDstart,ICCIDend,OUTNumber,Operator,purpose,AccountID,SetmealID,status) 
                                      VALUES('" + OutofstockID + "','" + outOfStock.remark + "','" + outOfStock.BatchID + "','" + ICCIDstart + "','" + ICCIDend + "'," + ICCIDS.ToList().Count + ",'" + outOfStock.Operator + "','" + outOfStock.purpose + "','" + outOfStock.AccountID + "','" + outOfStock.SetmealID + "','1')");
                StringBuilder UpDate_Product_Excel = new StringBuilder("update product_excel set isout=1 ,OutofstockID='" + OutofstockID + "' where ICCID in (");
                foreach (Excel_ICCID item in li)
                {
                    UpDate_Product_Excel.Append("'" + item.ICCID + "',");
                }
                string UpdatePE = UpDate_Product_Excel.ToString();
                // 更新库存
                UpdatePE = UpdatePE.Substring(0, UpdatePE.Length - 1) + ")";
                int ProductNumber = int.Parse(outOfStock.ProductNumber) - li.Count;
                StringBuilder UpdateProduct = new StringBuilder("update product set ICCIDMiddle='" + ICCIDend + "', ProductNumber=" + ProductNumber + " where batchID='" + outOfStock.BatchID + "'");
                using (IDbConnection connss = DapperService.MySqlConnection())
                {
                    IDbTransaction transaction = connss.BeginTransaction();
                    try
                    {
                        //发起事务 
                        var InnerTaocan = connss.Execute(AddOutOfStock.ToString(), transaction); //添加出库单 log
                        var INnerProduct = connss.Execute(UpdatePE, transaction);  //更新Product_Excel
                        var InnerProdoct_excel = connss.Execute(UpdateProduct.ToString(), transaction);  //更新库存数据
                        transaction.Commit();
                        v.data = "成功";
                        v.successMessage = true;
                    }
                    catch (Exception ex)
                    {   //事务回滚
                        transaction.Rollback();
                        v.data = "出现错误请联系管理人员" + "";
                        Console.WriteLine("ERROR：" + ex.ToString());
                        v.successMessage = false;
                    }
                }
                return v;
            }
        }
        /// <summary>
        ///  添加Account 表
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel AddAccount()
        {
            V_ResultModel v = new V_ResultModel();
            Model_Stock.Model_Stock_Config.Account package = new Model_Stock.Model_Stock_Config.Account();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            package = JsonConvert.DeserializeObject<Model_Stock.Model_Stock_Config.Account>(postContent);
            string AccountID = Unit.GetTimeStamp(DateTime.Now);
            StringBuilder sql = new StringBuilder("INSERT INTO accounts (APPID,ECID,PASSWORD,cityID,accountName,accountID,TOKEN,TRANSID,Remark,URL,Platform,UserId,UserName,APIkey,AddTime)values('"
                                                  + package.APPID + "','" + package.ECID + "','" + package.PASSWORD + "','" + package.cityID + "','" + package.accountName + "','" + AccountID + "','"
                                                  + package.TOKEN + "','" + package.TRANSID + "','" + package.Remark + "','" + package.URL + "','" + package.Platform + "','"+package.UserId+"'," 
                                                  +"'"+package.UserName+"','"+package.APIkey+"','"+DateTime.Now+"')");
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                try
                {
                    string ss = sql.ToString();
                    int resultss = connss.Execute(sql.ToString());

                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                {
                    v.data = ex.ToString();
                    v.successMessage = false;
                }
            }
            return v;
        }
        /// <summary>
        /// 修改 Account 信息
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel UpdateAccount()
        {
            V_ResultModel v = new V_ResultModel();
            Model_Stock.Model_Stock_Config.Account package = new Model_Stock.Model_Stock_Config.Account();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            package = JsonConvert.DeserializeObject<Model_Stock.Model_Stock_Config.Account>(postContent);
            string SetmealID = Unit.GetTimeStamp(DateTime.Now);
            StringBuilder sql = new StringBuilder("update accounts  set   APPID='" + package.APPID + "',ECID='" + package.ECID + "',PASSWORD='" + package.PASSWORD + "',accountName='" + package.accountName + "',TOKEN='" + package.TOKEN + "',TRANSID='" + package.TRANSID + "',Remark='" + package.Remark + "',URL='" + package.URL + "',Platform='" + package.Platform + "',UserId='"+package.UserId+ "',UserName='"+package.UserName+ "',APIkey='"+package.APIkey+"' where accountid='" + package.accountID + "'");
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                try
                {
                    int resultss = connss.Execute(sql.ToString());
                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                {
                    v.data = ex.ToString();
                    v.successMessage = false;
                }
            }
            return v;
        }
        /// <summary>
        ///获取出库log日志
        /// </summary>
        /// <returns></returns>
        public V_ResultModel2 GetOutOfStock_Log()
        {
            V_ResultModel2 v = new V_ResultModel2();
            string sql = @"select t1.ICCIDStart,t1.ICCIDEnd,t1.ProductNumber,t1.ProductNumbers,t1.batchID,t2.silentDate,t2.testDate,
                            t3.PackageDescribe,t3.`Code`,t3.Flow,t3.Remark,t4.CardTypeName,t5.CardXTName,t6.OperatorName,t7.StorageRoomName,t10.operatorsName
                            from   product t1 left join taocan t2 on t2.SetmealID=t1.SetMealID left join setmeal t3 on t3.SetmealID=t2.SetmealID2
                            left  join cardtype t4 on t4.CardTypeID=t3.CardTypeID left join  card_xingtai t5 on t5.CardXTID=t3.CardXTID
                            left join  operator t6 on t6.OperatorID=t3.OperatorID left join  storageroom t7 on t7.StorageRoomID=t1.StorageRoomID
                            left join  accounts t9  on  t9.AccountID=t1.AccountID left join operators t10 on t10.operatorsid=t9.operatorsid";
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                v.AdditionalInformation = conn.Query<object>(sql).ToList();
            }
            return v;
        }
        /// <summary>
        ///  object接受 不赞成但是可以用
        /// </summary>
        /// <returns></returns>
        /// 
        public static object GetHTTP()
        {
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            return JsonConvert.DeserializeObject<object>(postContent);
        }
        /// <summary>
        /// 出库操作 直接输入数量
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel OutOfStock2()
        {
            OutOfStock2 outOfStock = new OutOfStock2();
            V_ResultModel v = new V_ResultModel();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            outOfStock = JsonConvert.DeserializeObject<OutOfStock2>(postContent);
            string UpdatePE = "";
            StringBuilder sql_GetDate = new StringBuilder("select ICCID   from product_excel where isout=0  and BatchID='" + outOfStock.BatchID + "' limit 0," + outOfStock.OutNumber);
            List<Excel_ICCID> Li_getdate = new List<Excel_ICCID>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                Li_getdate = conn.Query<Excel_ICCID>(sql_GetDate.ToString()).ToList();
            }
            string ICCIDstart = Li_getdate.First().ICCID;
            string ICCIDend = Li_getdate.Last().ICCID;
            //出库单内码
            string OutofstockID = Unit.GetTimeStamp(DateTime.Now);
            //添加出库单 log  字符串拼接 此处可以使用dapper
            StringBuilder AddOutOfStock = new StringBuilder(@"INSERT INTO outofstock (OutofstockID,remark,BatchID,ICCIDstart,ICCIDend,OUTNumber,Operator,purpose,AccountID,SetmealID,status) 
                                      VALUES('" + OutofstockID + "','" + outOfStock.remark + "','" + outOfStock.BatchID + "','" + ICCIDstart + "','" + ICCIDend + "'," + outOfStock.OutNumber + ",'" + outOfStock.Operator + "','" + outOfStock.purpose + "','" + outOfStock.AccountID + "','" + outOfStock.SetmealID + "','1')");
            StringBuilder UpDate_Product_Excel = new StringBuilder("update product_excel set isout=1 ,OutofstockID='" + OutofstockID + "' where ICCID in (");
            foreach (Excel_ICCID item in Li_getdate)
            {
                UpDate_Product_Excel.Append("'" + item.ICCID + "',");
            }
            UpdatePE = UpDate_Product_Excel.ToString();
            // 更新库存
            UpdatePE = UpdatePE.Substring(0, UpdatePE.Length - 1) + ")";
            int ProductNumber = int.Parse(outOfStock.ProductNumber) - Li_getdate.Count;
            StringBuilder UpdateProduct = new StringBuilder("update product set ICCIDMiddle='" + ICCIDend + "', ProductNumber=" + ProductNumber + " where batchID='" + outOfStock.BatchID + "'");
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                IDbTransaction transaction = connss.BeginTransaction();
                try
                {
                    //发起事务 
                    var InnerTaocan = connss.Execute(AddOutOfStock.ToString(), transaction); //添加出库单 log
                    var INnerProduct = connss.Execute(UpdatePE, transaction);  //更新Product_Excel
                    var InnerProdoct_excel = connss.Execute(UpdateProduct.ToString(), transaction);  //更新库存数据
                    transaction.Commit();
                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                {   //事务回滚
                    transaction.Rollback();
                    v.data = "出现错误请联系管理人员" + "";
                    Console.WriteLine("ERROR：" + ex.ToString());
                    v.successMessage = false;
                }
            }
            return v;
        }
        /// <summary>
        ///出库单日志获取
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel2 Get_OutofStock_log() {
            
            V_ResultModel2 v = new V_ResultModel2();
            try
            {
                string sql = @"select  t1.OutofstockID, t1.BatchID , t1.OutNumber,t1.Operator,t1.remark as outofstockRemark ,t3.silentDate,t3.testDate,t4.Flow ,t4.PackageDescribe,t4.PartNumber,t4.`Code`,t5.CardTypeName,t6.CardXTName,t7.OperatorName,t8.accountName,t1.remark from outofstock t1
                                inner join  product   t2  on t1.BatchID=t2.batchID inner join  taocan    t3  on t3.SetmealID=t2.SetMealID inner join  setmeal   t4  on t4.SetmealID=t3.SetmealID2
                                inner join  cardtype  t5  on  t4.CardTypeID=t5.CardTypeID inner join  card_xingtai t6 on t6.CardXTID=t4.CardXTID
                                inner join  operator t7 on t7.OperatorID=t4.OperatorID inner join  accounts t8  on t8.accountID=t2.AccountID ORDER BY t1.OutofstockID desc";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    v.AdditionalInformation = conn.Query<object>(sql).ToList();
                    v.data = "Success";
                    v.successMessage = true;
                }
            }
            catch (Exception)
            {
                v.AdditionalInformation = "出现错误";
                v.data = "fail";
                v.successMessage = false;
            }
            return v;
        }
        /// <summary>
        ///获取 每个批次对应的ICCID
        /// </summary>
        /// <returns></returns>
        public static V_ResultModel2 Get_OutOfStock_ICCID(string OutofstockID)
        {
            V_ResultModel2 v = new V_ResultModel2();
            try
            {
                string sql = @"select DISTINCT(t10.ICCID),t1.BatchID , t1.OutNumber,t1.Operator,t1.remark as outofstockRemark ,t3.silentDate,t3.testDate,t4.Flow ,t4.PackageDescribe,t4.PartNumber,t4.`Code`,t5.CardTypeName,t6.CardXTName,t7.OperatorName,t8.accountName,t1.remark,t1.OutofstockID as OutofstockID from 
                                product_excel   t10 left join outofstock t1 on  t10.BatchID=t1.BatchID inner join  product   t2  on t1.BatchID=t2.batchID inner join  taocan    t3  on t3.SetmealID=t2.SetMealID
                                inner join  setmeal   t4  on t4.SetmealID=t3.SetmealID2 inner join  cardtype  t5  on  t4.CardTypeID=t5.CardTypeID
                                inner join  card_xingtai t6 on t6.CardXTID=t4.CardXTID inner join  operator t7 on t7.OperatorID=t4.OperatorID
                                inner join  accounts t8  on t8.accountID=t2.AccountID where t10.OutofstockID='" + OutofstockID+"' GROUP BY t10.ICCID ";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    v.AdditionalInformation = conn.Query<object>(sql).ToList();
                    v.data = "Success";
                    v.successMessage = true;
                }
            }
            catch (Exception ex)
            {
                v.AdditionalInformation = "出现错误"+ex;
                v.data = "fail";
                v.successMessage = false;
            }
            return v;
        }
         /// <summary>
         /// 修改套餐
         /// </summary>
         /// <returns></returns>
        public static V_ResultModel Update_package()
        {   
            V_ResultModel v = new V_ResultModel();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            var ICCIDs = JsonConvert.DeserializeObject<UpdatePackage>(postContent);
            StringBuilder UpdatePackAge = new StringBuilder();
            StringBuilder UpdatePackAge2 = new StringBuilder();
            if (ICCIDs.OperatorsFlg == "1573631210918" || string.IsNullOrWhiteSpace(ICCIDs.OperatorsFlg))//移动
            {
                UpdatePackAge.Append("update  card set SetMealID2='" + ICCIDs.SetmealID + "' where Card_ICCID in(");
                UpdatePackAge2.Append("update card_copy1 set  SetMealID2='" + ICCIDs.SetmealID + "' where Card_ICCID in(");
            }
            if (ICCIDs.OperatorsFlg == "1573631225967")//电信
            {
                UpdatePackAge.Append("update  ct_card set SetMealID2='" + ICCIDs.SetmealID + "' where Card_ICCID in(");
                UpdatePackAge2.Append("update ct_cardcopy set  SetMealID2='" + ICCIDs.SetmealID + "' where Card_ICCID in(");
            }
            if (ICCIDs.OperatorsFlg == "1573631234734")//联通
            {
                UpdatePackAge.Append("update  cucc_card set SetMealID2='" + ICCIDs.SetmealID + "' where Card_ICCID in(");
                UpdatePackAge2.Append("update cucc_cardcopy set  SetMealID2='" + ICCIDs.SetmealID + "' where Card_ICCID in(");
            }
            if (ICCIDs.OperatorsFlg == "1594176308883")//漫游
            {
                UpdatePackAge.Append("update  roamcard set SetMealID2='" + ICCIDs.SetmealID + "' where Card_ICCID in(");
                UpdatePackAge2.Append("update roamcard_copy set  SetMealID2='" + ICCIDs.SetmealID + "' where Card_ICCID in(");
            }
            foreach (Excel_ICCID item in ICCIDs.Cards)
            {
                UpdatePackAge.Append("'"+item.ICCID+"',");
                UpdatePackAge2.Append("'" + item.ICCID + "',");
            }
            string Sql = UpdatePackAge.ToString();
            Sql = Sql.Substring(0,Sql.Length-1)+")";
            string Sql2 = UpdatePackAge2.ToString();
            Sql2 = Sql2.Substring(0, Sql2.Length - 1) + ")";
            using (IDbConnection connss = DapperService.MySqlConnection())
            {  
                try
                {
                    connss.Execute(Sql);
                    connss.Execute(Sql2);
                    v.data = "成功";
                    v.successMessage = true;
                }
                catch (Exception ex)
                { 
                    v.data = "失败";
                    v.successMessage =  false;
                }
            }
            return v;
           
        }

        ///<summary>
        ///奇迹修改卡的续费起止时间
        /// </summary>
        public static Information UpdateCardRenewTime(Root para)
        {
            Information info = new Information();
            string updatestr = string.Empty;
            string sqliccidstr = string.Empty;
            string sqlsnstr = string.Empty;
            if (para.ActivateDate != null)
            {
                para.ActivateDate = para.ActivateDate.Value.AddHours(8);
            }
            if (para.EndDate != null)
            {
                para.EndDate = para.EndDate.Value.AddHours(8);
            }
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    if (para.OperatorsFlg == "1")//移动
                    {
                        if (para.ActivateDate != null && para.EndDate != null)//起始和终止时间都不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqliccidstr += "'" + item.ICCID + "',";
                            }
                            sqliccidstr = sqliccidstr.Substring(0, sqliccidstr.Length - 1);
                            updatestr = "update card set Card_ActivationDate='" + para.ActivateDate + "',Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqliccidstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate != null)//起始时间为空 终止时间不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqliccidstr += "'" + item.ICCID + "',";
                            }
                            sqliccidstr = sqliccidstr.Substring(0, sqliccidstr.Length - 1);
                            updatestr = "update card set Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqliccidstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate == null)//起始和终止时间都为空
                        {
                            info.Msg = "100";
                            info.Msg = "起止时间和终止时间不能同时为空！";
                            return info;
                        }
                    }
                    if (para.OperatorsFlg == "2")//电信
                    {
                        if (para.ActivateDate != null && para.EndDate != null)//起始和终止时间都不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqliccidstr += "'" + item.ICCID + "',";
                            }
                            sqliccidstr = sqliccidstr.Substring(0, sqliccidstr.Length - 1);
                            updatestr = "update ct_card set Card_ActivationDate='" + para.ActivateDate + "',Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqliccidstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate != null)//起始时间为空 终止时间不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqliccidstr += "'" + item.ICCID + "',";
                            }
                            sqliccidstr = sqliccidstr.Substring(0, sqliccidstr.Length - 1);
                            updatestr = "update ct_card set Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqliccidstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate == null)//起始和终止时间都为空
                        {
                            info.Msg = "100";
                            info.Msg = "起止时间和终止时间不能同时为空！";
                            return info;
                        }
                    }
                    if (para.OperatorsFlg == "3")//联通
                    {
                        if (para.ActivateDate != null && para.EndDate != null)//起始和终止时间都不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqliccidstr += "'" + item.ICCID + "',";
                            }
                            sqliccidstr = sqliccidstr.Substring(0, sqliccidstr.Length - 1);
                            updatestr = "update cucc_card set Card_ActivationDate='" + para.ActivateDate + "',Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqliccidstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate != null)//起始时间为空 终止时间不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqliccidstr += "'" + item.ICCID + "',";
                            }
                            sqliccidstr = sqliccidstr.Substring(0, sqliccidstr.Length - 1);
                            updatestr = "update cucc_card set Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqliccidstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate == null)//起始和终止时间都为空
                        {
                            info.Msg = "100";
                            info.Msg = "起止时间和终止时间不能同时为空！";
                            return info;
                        }
                    }
                    if (para.OperatorsFlg == "4")//全网通
                    {
                        if (para.ActivateDate != null && para.EndDate != null)//起始和终止时间都不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqlsnstr += "'" + item.SN + "',";
                            }
                            sqlsnstr = sqlsnstr.Substring(0, sqlsnstr.Length - 1);
                            updatestr = "update three_card set Card_ActivationDate='" + para.ActivateDate + "',Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqlsnstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate != null)//起始时间为空 终止时间不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqlsnstr += "'" + item.SN + "',";
                            }
                            sqlsnstr = sqlsnstr.Substring(0, sqlsnstr.Length - 1);
                            updatestr = "update card set Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqlsnstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate == null)//起始和终止时间都为空
                        {
                            info.Msg = "100";
                            info.Msg = "起止时间和终止时间不能同时为空！";
                            return info;
                        }
                    }
                    if (para.OperatorsFlg == "5")//漫游
                    {
                        if (para.ActivateDate != null && para.EndDate != null)//起始和终止时间都不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqliccidstr += "'" + item.ICCID + "',";
                            }
                            sqliccidstr = sqliccidstr.Substring(0, sqliccidstr.Length - 1);
                            updatestr = "update roamcard set Card_ActivationDate='" + para.ActivateDate + "',Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqliccidstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate != null)//起始时间为空 终止时间不为空
                        {
                            foreach (var item in para.Cards)
                            {
                                sqliccidstr += "'" + item.ICCID + "',";
                            }
                            sqliccidstr = sqliccidstr.Substring(0, sqliccidstr.Length - 1);
                            updatestr = "update roamcard set Card_EndTime='" + para.EndDate + "' where Card_ICCID in (" + sqliccidstr + ")";
                            conn.Execute(updatestr);
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        if (para.ActivateDate == null && para.EndDate == null)//起始和终止时间都为空
                        {
                            info.Msg = "100";
                            info.Msg = "起止时间和终止时间不能同时为空！";
                            return info;
                        }
                    }
                }
                catch (Exception ex)
                {
                    info.Msg = "-1";
                    info.Msg = "出现错误:"+ex;
                }
            }  
            return info;
        } 
    }

}