using Dapper;
using Esim7.Models;
using Esim7.Models.UserStockModel;
using Esim7.Models.UserUploadCardModel;
using Esim7.ReturnMessage;
using Esim7.UNity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using static Esim7.Model_Stock.Model_Stock_Config;

namespace Esim7.Action
{
    public class UserUploadCardAction
    {
        ///<summary>
        ///用户查看API信息
        /// </summary>
        public UserApiDto GetUserAPI(string Company_ID)
        {
            UserApiDto dto = new UserApiDto();
            try
            {
                string sqluserapi = "select * from accounts where Company_ID='" + Company_ID + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    dto.accounts = conn.Query<Account>(sqluserapi).ToList();
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }

            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败:" + ex;
            }
            return dto;
        }
        ///<summary>
        ///用户添加运营商平台API
        /// </summary>
        public Information UserAddAPI(Account para)
        {
            Information info = new Information();
            string accountID= Unit.GetTimeStamp(DateTime.Now);
            try
            {
                string addsql = "INSERT INTO accounts (APPID,ECID,PASSWORD,cityID,accountName,accountID,TOKEN,TRANSID,Remark,URL,Platform,UserId,UserName,APIkey,AddTime,Isqiji,Company_ID)values" +
                    "('" + para.APPID + "','" + para.ECID + "','" + para.PASSWORD + "','" + para.cityID + "','" + para.accountName + "','" +accountID + "','" + para.TOKEN + "'," +
                    "'" + para.TRANSID + "','" + para.Remark + "','" + para.URL + "','" + para.Platform + "','" + para.UserId + "','" + para.UserName + "','" + para.APIkey + "','" + DateTime.Now + "'," + false + ",'" + para.Company_ID + "')";
                using (IDbConnection Conn = DapperService.MySqlConnection())
                {
                    Conn.Execute(addsql);
                    info.flg = "1";
                    info.Msg = "添加成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "添加失败:" + ex;
            }
            return info;
        }

        ///<summary>
        ///用户编辑API
        /// </summary>
        public Information UserEditAPI(Account para)
        {
            Information info = new Information();
            try
            {
                string editsql = "update accounts set APPID='" + para.APPID + "',ECID='" + para.ECID + "',PASSWORD='" + para.PASSWORD + "',cityID='" + para.cityID + "'," +
                    "accountName='" + para.accountName + "',TOKEN='" + para.TOKEN + "',TRANSID='" + para.TRANSID + "'," +
                    "Remark='" + para.Remark + "',URL='" + para.URL + "',Platform='" + para.Platform + "',UserId='" + para.UserId + "',UserName='" + para.UserName + "',APIkey='" + para.APIkey + "' where accountid='" + para.accountID + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(editsql);
                    info.flg = "1";
                    info.Msg = "修改成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "修改失败:" + ex;
            }
            return info;
        }

        ///<summary>
        ///用户添加套餐
        /// </summary>
        public Information UserAddsetmeal(Package para)
        {
            Information info = new Information();
            string SetmealID = Unit.GetTimeStamp(DateTime.Now);
            int CardSetmealType = 0;
            if (para.Operator == "1573631210918")//移动
            {
                CardSetmealType = 1;
            }
            if (para.Operator == "1573631225967")//电信
            {
                CardSetmealType = 2;
            }
            if (para.Operator == "1573631234734")//联通
            {
                CardSetmealType = 3;
            }
            string sql = "INSERT INTO setmeal (OperatorID,Code,PartNumber,Remark,PackageDescribe,SetmealID,Flow,CardXTID,CardTypeID,CardSetmealType,Company_ID)values('" + para.Operator + "','" +
            para.Code + "','" + para.PartNumber + "','" + para.Remark + "','" + para.PackageDescribe + "','" + SetmealID + "','" + para.Flow + "','" + para.CardXTID + "','" + para.CardTypeID + "'," + CardSetmealType + ",'" + para.Company_ID + "')";
            using (IDbConnection connss = DapperService.MySqlConnection())
            {
                try
                {
                    connss.Execute(sql);
                    info.flg = "1";
                    info.Msg = "添加成功!";
                }
                catch (Exception ex)
                {
                    info.flg = "-1";
                    info.Msg = "添加失败:" + ex;
                }
            }
            return info;
        }

        ///<summary>
        ///用户查看套餐
        /// </summary>
        public GetUserSetmealDto GetUserSetmeal(string Company_ID,string CardSetmealType)
        {
            GetUserSetmealDto v = new GetUserSetmealDto();
            
            string sql = @"select  t1.CardTypeID,t2.CardTypeName,t1.OperatorID,t3.OperatorName ,t1.CardXTID,t4.CardXTName,t1.`Code`,t1.Flow,t1.PackageDescribe,t1.PartNumber,t1.Remark, t1.SetMealID from setmeal t1
                            left  join cardtype t2 on t2.CardTypeID = t1.CardTypeID left  join  operator t3 on t3.OperatorID = t1.OperatorID left  join card_xingtai t4 on t4.CardXTID = t1.CardXTID where t1.Company_ID='"+Company_ID+ "' and t1.CardSetmealType="+CardSetmealType+"";
            try
            {
                using (IDbConnection connss = DapperService.MySqlConnection())
                {
                    v.userSetmeals = connss.Query<UserSetmeal>(sql).ToList();
                }

                v.flg = "1";
                v.Msg = "成功!";
            }
            catch (Exception ex)
            {
                v.flg = "-1";
                v.Msg = "失败:"+ex;
            }
            
            return v;
        }
            

        ///<summary>
        ///用户上传卡数据到copy1或库存中
        /// </summary>
        public Information UploadCard(UserUploadCardDto para)
        {
            Information info = new Information();
            int num = para.ICCIDS.Count;//入库数量、库存总数
            int i = 0;
            string pici = "RK" + DateTime.Now.ToString("yyyyMMdd");
            string EnterCode = Unit.GetTimeStamp(DateTime.Now);//入库单号
            string OperatorName = string.Empty;//运营商名称
            string SetmealName = string.Empty;//套餐名称
            string CardXTName = string.Empty;//卡形态名称
            string CardTypeName = string.Empty;//卡类型名称
            string Flow = string.Empty;//流量
            string Remark = string.Empty;
            string MaterielCode = string.Empty;
            string addsopy1str = string.Empty;
            string updatecardscene = string.Empty; //Scene = '"+list.Scene+"'
            string addwarehousingstr = string.Empty;
            string adduserstock = string.Empty;
            int OperatorsFlg = 0;
            string CopyID = Unit.GetTimeStamp(DateTime.Now);
            StringBuilder sql_copy1 = new StringBuilder("");
            string iccids = string.Empty;
            string card_status = string.Empty;
            string TestFlow = string.Empty;
            int SetType = 0;
            DateTime? ActivateDate = para.ActivateDate;
            DateTime? EndDate = para.EndDate;
            string PurchaseNo = string.Empty;
            //采购单号判断
            if (!string.IsNullOrWhiteSpace(para.PurchaseNo))
            {
                //查询是否能找到采购单号
                string sqlpurchase= "select PurchaseNo from purchase where status='1' and PurchaseNo='"+para.PurchaseNo+"'";
                using (IDbConnection conns = DapperService.MySqlConnection())
                {
                    PurchaseNo = conns.Query<UserUploadCardDto>(sqlpurchase).Select(t => t.PurchaseNo).FirstOrDefault();
                    if (para.PurchaseNo != PurchaseNo)
                    {
                        info.flg = "-1";
                        info.Msg = "采购单号不存在或输入错误";
                        return info;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(para.silentDate) && !string.IsNullOrWhiteSpace(para.testDate))//有测试流量没有沉默期的情况
            {
                card_status = "6";//可测试状态
                para.silentDate = "0";
                SetType = 1;
            }
            if (!string.IsNullOrWhiteSpace(para.silentDate) && !string.IsNullOrWhiteSpace(para.testDate))//有测试流量和沉默期的情况
            {
                card_status = "6";//可测试状态
                SetType = 2;
            }
            if (!string.IsNullOrWhiteSpace(para.silentDate) && string.IsNullOrWhiteSpace(para.testDate))//有沉默期没有测试流量的情况
            {
                card_status = "1";//待激活
                TestFlow = "0";
                SetType = 3;
            }
            if (!string.IsNullOrWhiteSpace(para.ActivateDate.ToString()) && !string.IsNullOrWhiteSpace(para.EndDate.ToString()))
            {
                DateTime ActivateDates = para.ActivateDate.Value.AddHours(8);
                DateTime EndDates = para.EndDate.Value.AddHours(8);
                ActivateDate = ActivateDates;
                EndDate = EndDates;
                SetType = 0;
                //ActivateDate = ActivateDate.value.AddHours(8);
                //EndDate = EndDate.AddHours(8);
            }
            if (!string.IsNullOrWhiteSpace(para.ActivateDate.ToString()) && string.IsNullOrWhiteSpace(para.EndDate.ToString()))
            {
                DateTime ActivateDates = para.ActivateDate.Value.AddHours(8);
                DateTime EndDates = para.EndDate.Value.AddHours(8);
                ActivateDate = ActivateDates;
                EndDate = EndDates;
                //ActivateDate = ActivateDate.value.AddHours(8);
                //EndDate = EndDate.AddHours(8);
            }
            //if (string.IsNullOrWhiteSpace(para.IsUploadStock))//默认直接导入库存中
            //{
            //    para.IsUploadStock = "2";
            //}
            if (para.Platform == "10" || para.Platform == "11")//移动
            {
                OperatorsFlg = 1;
            }
            if (para.Platform == "20" || para.Platform == "21")//电信
            {
                OperatorsFlg = 2;
            }
            if (para.Platform == "30" || para.Platform == "31")//联通
            {
                OperatorsFlg = 3;
            }
            if (para.Platform == "41" )//全网通
            {
                OperatorsFlg = 4;
            }
            if (para.Platform == "61" )//漫游
            {
                OperatorsFlg = 5;
            }
            #region 判断数据是否重复
            if (OperatorsFlg == 1)
            {
                if (para.Company_ID == "1556265186243")//奇迹上传卡数据
                {
                    sql_copy1.Append("select  Card_ICCID from card where Card_ICCID in( ");//公海卡表
                }
                else
                {
                    sql_copy1.Append("select  Card_ICCID from card_copy1 where Card_ICCID in( ");//用户卡表
                }
            }
            if (OperatorsFlg == 2)
            {
                if (para.Company_ID == "1556265186243")//奇迹上传卡数据
                {
                    sql_copy1.Append("select  Card_ICCID from ct_card where Card_ICCID in( ");//公海卡表
                }
                else
                {
                    sql_copy1.Append("select  Card_ICCID from ct_cardcopy where Card_ICCID in( ");//用户卡表
                }
            }
            if (OperatorsFlg == 3)
            {
                if (para.Company_ID == "1556265186243")//奇迹上传卡数据
                {
                    sql_copy1.Append("select  Card_ICCID from cucc_card where Card_ICCID in( ");//公海卡表
                }
                else
                {
                    sql_copy1.Append("select  Card_ICCID from cucc_cardcopy where Card_ICCID in( ");//用户卡表
                }
            }
            //if (OperatorsFlg == 4)
            //{
            //    if (para.Company_ID == "1556265186243")//奇迹上传卡数据
            //    {
            //        sql_copy1.Append("select  SN from three_card where SN in( ");//公海卡表
            //    }
            //    else
            //    {
            //        sql_copy1.Append("select  SN from three_cardcopy where SN in( ");//用户卡表
            //    }
            //}
            if (OperatorsFlg == 5)
            {
                if (para.Company_ID == "1556265186243")//奇迹上传卡数据
                {
                    sql_copy1.Append("select  Card_ICCID from roamcard where Card_ICCID in ( ");//公海卡表
                }
                else
                {
                    sql_copy1.Append("select  Card_ICCID from roamcard_copy where Card_ICCID in( ");//用户卡表
                }
            }
            
            StringBuilder Sql_JudgeRepet = new StringBuilder("select  Card_ICCID from userwarehousing where Card_ICCID in( ");//用户库存表
            //StringBuilder sql_copy1 = new StringBuilder("select  Card_ICCID from card_copy1 where Card_ICCID in( ");//用户卡表 
            //获取入库次数
            string userstocklist = "select *  from userstock where  StockType=1";
            string Sql_Setmeal = "select * from setmeal where SetmealID='" + para.SetmealID2+"'";//根据套餐编号查看套餐信息
            List<setmeal> setmealinfo = new List<setmeal>();
            //if (OperatorsFlg != 4)
            //{
            foreach (Excel_Card item in para.ICCIDS)
            {
                Sql_JudgeRepet.Append("'" + item.ICCID + "',");
                sql_copy1.Append("'" + item.ICCID + "',");
            }
            //}
            //else
            //{
            //    foreach (Excel_Card item in para.ICCIDS)
            //    {
            //        Sql_JudgeRepet.Append("'" + item.SN + "',");
            //        sql_copy1.Append("'" + item.SN + "',");
            //    }
            //}
            
            string sql_JD = Sql_JudgeRepet.ToString();
            string sql_strcopy1 = sql_copy1.ToString();
            sql_JD = sql_JD.Substring(0, sql_JD.Length - 1) + ")";
            sql_strcopy1 = sql_strcopy1.Substring(0, sql_strcopy1.Length - 1) + ")";
            List<Excel_Card> li = new List<Excel_Card>();
            List<Excel_Card> lis = new List<Excel_Card>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                li = conn.Query<Excel_Card>(sql_JD).ToList();
                lis = conn.Query<Excel_Card>(sql_strcopy1).ToList();
                i = conn.Query<UserStock>(userstocklist).ToList().Where(t => t.EnterTime.Year == DateTime.Now.Year && t.EnterTime.Month == DateTime.Now.Month && t.EnterTime.Day == DateTime.Now.Day).Count();
                i = i + 1;
                pici = pici + i.ToString("d3");
                setmealinfo = conn.Query<setmeal>(Sql_Setmeal).ToList();
                if (setmealinfo != null)
                {
                    foreach (var item in setmealinfo)
                    {
                        SetmealName = item.PackageDescribe;
                        MaterielCode = item.Code;
                        string xtname = "select CardXTName from card_xingtai where CardXTID='" + item.CardXTID + "'";
                        string typename = "select CardTypeName from cardtype where CardTypeID='" + item.CardTypeID + "'";
                        string operatorname = "select OperatorName from operator where OperatorID='"+item.OperatorID+"'";
                        CardXTName = conn.Query<CardXingTai>(xtname).Select(t=>t.CardXTName).FirstOrDefault();
                        CardTypeName = conn.Query<cardtype>(typename).Select(t => t.CardTypeName).FirstOrDefault();
                        OperatorName = conn.Query<Operator>(operatorname).Select(t=>t.OperatorName).FirstOrDefault();
                        Flow = item.Flow.ToString();
                        Remark = item.Remark;
                    }
                }
               
            }
            #endregion
            if (li.Count != 0)
            {
                string s = "";
                foreach (Excel_Card item in li)
                {
                    s += item.ICCID + ",";
                }
                s = s.Substring(0, s.Length - 1);
                info.Msg = "数据重复:" + s;
                info.flg = "-1";
                return info;
            }
            if (lis.Count != 0)
            {
                string s = "";
                foreach (Excel_Card item in lis)
                {
                    s += item.ICCID + ",";
                }
                s = s.Substring(0, s.Length - 1);
                info.Msg = "数据重复:"+s;
                info.flg = "-1";
                return info;
            }
            else
            {
                //if (para.IsUploadStock == "1")//不直接导入到库存 
                //{
                //    StringBuilder addcopy1 = new StringBuilder("insert into card_copy1(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg)values");
                //    foreach (var item in para.ICCIDS)
                //    {
                //        addcopy1.Append("('" + item.Card_ID + "','" + para.Company_ID + "','" + item.ICCID + "','"+para.OperatorsID+"','"+para.AccountID+"','"+para.Remarks+"'," +
                //            ""+2+",'"+para.SetmealID2+"',"+ OperatorsFlg + "),");
                //    }
                //     addsopy1str = addcopy1.ToString().Substring(0, addcopy1.ToString().Length - 1);
                //}
                //if (para.IsUploadStock == "2")//直接导入到库存
                //{
                StringBuilder addcopy1 = new StringBuilder("");
                if (para.Company_ID == "1556265186243")//奇迹上传卡数据
                {
                    if (OperatorsFlg == 1)
                    {
                        addcopy1.Append("insert into card(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg,RegionLabel,Scene,SetType,Card_ActivationDate,Card_EndTime,PurchaseNo,status,AddTime)values");
                    }
                    if (OperatorsFlg == 2)
                    {
                        addcopy1.Append("insert into ct_card(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg,RegionLabel,Scene,SetType,Card_ActivationDate,Card_EndTime,PurchaseNo,status,AddTime)values");
                    }
                    if (OperatorsFlg == 3)
                    {
                        addcopy1.Append("insert into cucc_card(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg,RegionLabel,Scene,SetType,Card_ActivationDate,Card_EndTime,PurchaseNo,status,AddTime)values");
                    }
                    if (OperatorsFlg == 5)
                    {
                        addcopy1.Append("insert into roamcard(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg,RegionLabel,Scene,SetType,Card_ActivationDate,Card_EndTime,PurchaseNo,status,AddTime)values");
                    }
                }
                else//用户上传卡数据
                {
                    if (OperatorsFlg == 1)
                    {
                        addcopy1.Append("insert into card_copy1(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg,RegionLabel,Scene,SetType,Card_ActivationDate,Card_EndTime,PurchaseNo,status,AddTime)values");
                    }
                    if (OperatorsFlg == 2)
                    {
                        addcopy1.Append("insert into ct_cardcopy(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg,RegionLabel,Scene,SetType,Card_ActivationDate,Card_EndTime,PurchaseNo,status,AddTime)values");
                    }
                    if (OperatorsFlg == 3)
                    {
                        addcopy1.Append("insert into cucc_cardcopy(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg,RegionLabel,Scene,SetType,Card_ActivationDate,Card_EndTime,PurchaseNo,status,AddTime)values");
                    }
                    if (OperatorsFlg == 4)
                    {
                        addcopy1.Append("insert into roamcard_copy(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg,RegionLabel,Scene,SetType,Card_ActivationDate,Card_EndTime,PurchaseNo,status,AddTime)values");
                    }
                }
                    //StringBuilder addcopy1 = new StringBuilder("insert into card_copy1(Card_ID,Card_CompanyID,Card_ICCID,operatorsID,accountsID,Card_Remarks,IsUserAdd,SetMealID2,OperatorsFlg)values");
                StringBuilder addwarehousing = new StringBuilder("insert into userwarehousing(Card_ICCID,Card_ID,Isout,EnterCode,AddTime)values");
                foreach (var item in para.ICCIDS)
                {
                    addcopy1.Append("('" + item.Card_ID + "','" + para.Company_ID + "','" + item.ICCID + "','" + para.OperatorsID + "','" + para.AccountID + "','" + para.Remarks + "'," +
                        "" + 2 + ",'"+para.SetmealID2+"',"+ OperatorsFlg + ",'"+para.RegionLabel+"','"+para.Scene+ "'," + SetType + ",'" + ActivateDate + "','" + EndDate + "','"+para.PurchaseNo+"','1','"+DateTime.Now+"'),");//导入到copy1
                    addwarehousing.Append("('" + item.ICCID + "','" + item.Card_ID + "'," +0+ ",'" + EnterCode + "','" +DateTime.Now + "'),");
                    iccids += "'" + item.ICCID + "',";
                }
                iccids = iccids.Substring(0, iccids.Length - 1);
                
                //添加信息到copy1
                    addsopy1str = addcopy1.ToString().Substring(0, addcopy1.ToString().Length - 1);
                //更新公海表的卡使用场景
                if (OperatorsFlg == 1)
                {
                    updatecardscene = "update card set CopyID='" + CopyID + "', Scene='" + para.Scene + "' where Card_ICCID in (" + iccids + ")";
                }
                if (OperatorsFlg == 2)
                {
                    updatecardscene = "update ct_card set CopyID='" + CopyID + "', Scene='" + para.Scene + "' where Card_ICCID in (" + iccids + ")";
                }
                if (OperatorsFlg == 3)
                {
                    updatecardscene = "update cucc_card set CopyID='" + CopyID + "', Scene='" + para.Scene + "' where Card_ICCID in (" + iccids + ")";
                }
                if (OperatorsFlg == 5)
                {
                    updatecardscene = "update three_card set  CopyID='" + CopyID + "', Scene='" + para.Scene + "' where Card_ICCID in (" + iccids + ")";
                }
                //添加信息到用户入库表中
                addwarehousingstr = addwarehousing.ToString().Substring(0, addwarehousing.ToString().Length - 1);
                //添加信息到入库信息
                string StartICCID = para.ICCIDS.OrderBy(t => t.ICCID).Select(t=>t.ICCID).FirstOrDefault();
                string EndICCID = para.ICCIDS.OrderBy(t => t.ICCID).Select(t => t.ICCID).LastOrDefault();
                string StartCardID = para.ICCIDS.OrderBy(t => t.Card_ID).Select(t => t.Card_ID).FirstOrDefault();
                string EndCardID = para.ICCIDS.OrderBy(t => t.Card_ID).Select(t => t.Card_ID).LastOrDefault();
                adduserstock = "insert into userstock(Company_ID,personnel,StockType,CardNumber,CardEnterNumber,MaterielCode,StockAdderss,CardPrice,Remark,OperatorName,StartICCID,EndICCID,StartCardID," +
                    "EndCardID,CardXTName,CardTypeName,silentDate,TestDate,Flow,EnterCode,pici,EnterStockType,EnterTime,AddTime,SetmealName,SetmealID)" +
                    "values('"+para.Company_ID+"','"+para.operatorID+"',"+1+","+num+","+num+",'"+MaterielCode+"','"+para.StockAdderss+"',"+para.CardPrice+",'"+para.Remarks+"','"+OperatorName+"'," +
                    "'"+StartICCID+"','"+EndICCID+"','"+StartCardID+"','"+EndCardID+"','"+CardXTName+"','"+CardTypeName+"','"+para.silentDate+"','"+para.testDate+"','"+Flow+"'," +
                    "'"+EnterCode+"','"+pici+"',"+2+",'"+DateTime.Now+"','"+DateTime.Now+"','"+ SetmealName + "','"+para.SetmealID2+"')";
                //}
                using (IDbConnection connss = DapperService.MySqlConnection())
                {
                    try
                    {
                        //if (para.IsUploadStock == "1")//只把数据导入到copy1
                        //{
                        //    IDbTransaction transaction = connss.BeginTransaction();
                        //    connss.Execute(addsopy1str);
                        //    transaction.Commit();
                        //    info.flg = "1";
                        //    info.Msg = "成功!";
                        //}
                        //else
                        //{
                        IDbTransaction transaction = connss.BeginTransaction();
                        connss.Execute(addsopy1str);
                        connss.Execute(updatecardscene);
                        connss.Execute(addwarehousingstr);
                        connss.Execute(adduserstock);
                        transaction.Commit();
                        info.flg = "1";
                        info.Msg = "上传成功!";
                        //} 
                    }
                    catch (Exception ex)
                    {
                        info.flg = "-1";
                        info.Msg = "失败:"+ex;
                    }
                }
                return info;
            }
            
        }

        ///<summary>
        ///用户添加反馈信息
        /// </summary>
        public Information Addfeedback(feedbackInfo para)
        {
            Information info = new Information();
            DateTime time = DateTime.Now;
            string CompanyName = string.Empty;
            try
            {
                string sqladd = "insert into feedback (Company_ID,CompanyName,Content,Addtime) values('" + para.Company_ID + "','" + CompanyName + "','" + para.Content + "','" + time + "')";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sqladd);
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
    }
}