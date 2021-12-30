using Dapper;
using Esim7.Models;
using Esim7.Models.UserStockModel;
using Esim7.parameter.UserStockManage;
using Esim7.UNity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Esim7.Action
{
    /// <summary>
    /// 用户库存
    /// </summary>
    public class UserStockAction
    {
        ///<summary>
        ///查看用户可以导入的库存信息
        /// </summary>
        public Infos ImportInfoList(string Company_ID)
        {
            Infos info = new Infos();
            int i = 0;
            int Year =0;
            int Month =0;
            int Day =0;
            string StartICCID = string.Empty;
            string EndICCID = string.Empty;
            string SetmealID = string.Empty;//关联套餐编号
            List<ImportInfoDto> importInfos = new List<ImportInfoDto>();          
            try
            {
                //获取不同批次的卡
                string cardgroup = "select DATE_FORMAT(Card_OpenDate,'%Y%m%d') days from card_copy1 where Card_CompanyID='" + Company_ID + "' and IsEnterStock=0 GROUP BY days";
                string carinfo = "select *,Card_OpenDate as OpenCardTime  from card_copy1 where  Card_CompanyID='" + Company_ID + "' and IsEnterStock=0";
                string cardxtinfo = "select * from card_xingtai";
                string cardtypeinfo = "select * from cardtype";
                string operatorinfo = "select * from operator";
                string taocaninfo = "select id,testDate,silentDate,SetmealID2 from taocan";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var cardinfolist = conn.Query<Card>(carinfo).ToList();
                    var cardgrouplist = conn.Query<CardGroup>(cardgroup).ToList();
                    foreach (var item in cardgrouplist)
                    {
                        List<ICCIDS> importICCIDs = new List<ICCIDS>();
                        ImportInfoDto infoDto = new ImportInfoDto();
                        if (item.days != null)
                        {
                            Year = Convert.ToInt32(cardgrouplist[i].days.Substring(0, 4));
                            Month = Convert.ToInt32(cardgrouplist[i].days.Substring(4, 2));
                            Day = Convert.ToInt32(cardgrouplist[i].days.Substring(6, 2));
                            var cardgroupinfo = cardinfolist.Where(t => t.OpenCardTime.Year == Year && t.OpenCardTime.Month == Month && t.OpenCardTime.Day == Day).OrderBy(t => t.Card_ICCID).ToList();
                            StartICCID = cardgroupinfo.Select(t => t.Card_ICCID).FirstOrDefault();
                            EndICCID = cardgroupinfo.Select(t => t.Card_ICCID).LastOrDefault();
                            SetmealID = cardgroupinfo.Select(t => t.SetmealID2).FirstOrDefault();
                            infoDto.CardNumber = cardgroupinfo.Count;
                            if (!string.IsNullOrWhiteSpace(SetmealID))
                            {
                                string SetmealSql = "select * from Setmeal where SetmealID='" + SetmealID + "'";
                                var setmeallist = conn.Query<setmeal>(SetmealSql).ToList();
                                var taocanlist = conn.Query<taocan>(taocaninfo).ToList();
                                infoDto.TestDate = taocanlist.Where(t => t.SetmealID2 == SetmealID).Select(t => Convert.ToInt32(t.testDate)).FirstOrDefault();
                                infoDto.silentDate = taocanlist.Where(t => t.SetmealID2 == SetmealID).Select(t => Convert.ToInt32(t.silentDate)).FirstOrDefault();
                                infoDto.SetmealID = SetmealID;
                                foreach (var itemsetmeal in setmeallist)
                                {
                                    infoDto.MaterielCode = itemsetmeal.PartNumber;
                                    infoDto.Flow = itemsetmeal.Flow;
                                    infoDto.SetmealName = itemsetmeal.PackageDescribe;
                                    if (!string.IsNullOrWhiteSpace(itemsetmeal.CardTypeID))
                                    {
                                        infoDto.CardTypeName = conn.Query<cardtype>(cardtypeinfo).Where(t => t.CardTypeID == itemsetmeal.CardTypeID).ToList().Select(t => t.CardTypeName).FirstOrDefault();
                                    }
                                    if (!string.IsNullOrWhiteSpace(itemsetmeal.CardXTID))
                                    {
                                        infoDto.CardXTName = conn.Query<card_xingtai>(cardxtinfo).Where(t => t.CardXTID == itemsetmeal.CardXTID).ToList().Select(t => t.CardXTName).FirstOrDefault();
                                    }
                                    if (!string.IsNullOrWhiteSpace(itemsetmeal.OperatorID))
                                    {
                                        infoDto.OperatorName = conn.Query<operators>(operatorinfo).Where(t => t.OperatorID == itemsetmeal.OperatorID).ToList().Select(t => t.OperatorName).FirstOrDefault();
                                    }
                                }
                            }
                            infoDto.StartICCID = StartICCID;
                            infoDto.EndICCID = EndICCID;
                            infoDto.Company_ID = Company_ID;
                            importInfos.Add(infoDto);
                            foreach (var items in cardgroupinfo)
                            {
                                ICCIDS iCCID = new ICCIDS();
                                iCCID.ICCID = items.Card_ICCID;
                                iCCID.Card_ID = items.Card_ID;
                                iCCID.Card_IMEI = items.Card_IMEI;
                                importICCIDs.Add(iCCID);
                            }
                            infoDto.EnterICCID = importICCIDs;
                        }
                        i++;
                    }
                }
                info.importInfos = importInfos;
                info.flg = "1";
                info.Msg = "查询成功!";
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "发生错误:"+ex;
            }
            return info;
        }

        ///<summary>
        ///用户通过EXCEL导入SIM卡到库存或者一键导入到数据库
        /// </summary>
        public Information ImportCardInfo(ExeclEnterStockPara para)
        {
            Information info = new Information();
            #region 判断用户上传的卡数量
            string sqluser = "select UserType from cf_user where Company_ID='"+para.Company_ID+"'";//获取用户的类型是否是自己注册用户
            string sqlstockcardnum = "select sum(CardEnterNumber) as CardEnterNumber from userstock where Company_ID='"+para.Company_ID+"'";
            //限制自己注册用户上传卡数量为10000张
            int updatecardnum = para.EnterICCID.Count();//用户上传的卡数量
            string UserType = string.Empty;
            int SumCardEnterNumber = 0;
            using (IDbConnection db = DapperService.MySqlConnection())
            {
                UserType = db.Query<User>(sqluser).Select(t => t.UserType).FirstOrDefault();
                SumCardEnterNumber = db.Query<UserStock>(sqlstockcardnum).Select(t => t.CardEnterNumber).FirstOrDefault();
                if (UserType == "2")//判断用户上传的卡数据是否大于10000
                {
                    if (updatecardnum > 10000)
                    {
                        info.flg = "-1";
                        info.Msg = "最多只能上传1万张卡";
                        return info;
                    }
                    if (SumCardEnterNumber + updatecardnum > 10000)
                    {
                        int num = 10000 - SumCardEnterNumber;
                        info.flg = "-1";
                        info.Msg = "试用功能只能上传1万张卡,还能上传"+num.ToString()+"张卡";
                        return info;
                    }
                }
            }
            #endregion
            string pici = string.Empty;//批次  RK20200417001 
            DateTime time = DateTime.Now;
            int i = 0;
            int addnum = 0;
            pici = "RK" + DateTime.Now.ToString("yyyyMMdd");
            string EnterCode = Unit.GetTimeStamp(DateTime.Now);//入库单号
            int CardNumber = 0;
            string StartICCID = string.Empty;
            string EndICCID = string.Empty;
            string StartCardID = string.Empty;
            string EndCardID = string.Empty;
            string adduserstock = string.Empty;
            int EnterStockType = 0;
            try
            {
                if (para.EnterICCID.Count == 0)
                {
                    EnterStockType = 1;
                }
                if (para.EnterICCID.Count < 0)
                {
                    EnterStockType = 2;
                }
                //获取入库次数
                string userstocklist = "select *  from userstock where  StockType=1";
                #region   判重           
                StringBuilder Sql_JudgeRepet = new StringBuilder("select  Card_ICCID from userwarehousing where Card_ICCID in( ");
                StringBuilder updatecopy1 = new StringBuilder(@"update card_copy1 set IsEnterStock=1 where Card_CompanyID='"+para.Company_ID+"' and Card_ICCID in (");
                foreach (var item in para.EnterICCID)
                {
                    Sql_JudgeRepet.Append("'" + item.ICCID + "',");
                    updatecopy1.Append("'" + item.ICCID + "',");
                }
                string sql_JD = Sql_JudgeRepet.ToString();
                sql_JD = sql_JD.Substring(0, sql_JD.Length - 1) + ")";
                string sqlcopy1update = updatecopy1.ToString();
                sqlcopy1update = sqlcopy1update.Substring(0, sqlcopy1update.Length - 1) + ")";
                List<ICCIDS> li = new List<ICCIDS>();
                //para.EnterICCID
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    li = conn.Query<ICCIDS>(sql_JD).ToList();
                    i = conn.Query<UserStock>(userstocklist).ToList().Where(t => t.EnterTime.Year == DateTime.Now.Year && t.EnterTime.Month == DateTime.Now.Month && t.EnterTime.Day == DateTime.Now.Day).Count() ;
                    i = i + 1;
                    pici=pici+ i.ToString("d3");
                    CardNumber = para.EnterICCID.Count();
                }
                #endregion
                if (li.Count != 0)
                {
                    string s = "";
                    foreach (ICCIDS item in li)
                    {
                        s += item.ICCID + ",";
                    }
                    s = s.Substring(0, s.Length - 1);
                    info.Msg = "数据重复:" + s;
                    info.flg = "1";
                }
                else
                {
                    StringBuilder sql_userwarehousing = new StringBuilder("INSERT INTO userwarehousing  (Card_ICCID,Card_ID,Isout,Card_IMEI, EnterCode, AddTime)VALUES");
                    StringBuilder sql_copy1 = new StringBuilder("INSERT INTO userwarehousing  (Card_ICCID,Isout, EnterCode, AddTime)VALUES");
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {                       
                        List<ICCIDS> list = new List<ICCIDS>();
                        list = para.EnterICCID.ToList();
                        if (para.EnterICCID.Count > 2000)
                        {
                            if (para.EnterICCID.Count % 2000 == 0)
                            {
                                addnum = para.EnterICCID.Count / 2000;
                                for (int j = 0; j < addnum; j++)
                                {
                                    StringBuilder sql_userwarehousing1 = new StringBuilder("INSERT INTO userwarehousing  (Card_ICCID,Card_ID,Isout,Card_IMEI, EnterCode, AddTime)VALUES");
                                    if (j == 0)
                                    {
                                        para.EnterICCID = para.EnterICCID.Skip(0).Take(2000).ToList();
                                    }
                                    else
                                    {
                                        para.EnterICCID =list.Skip(j * 10).Take(2000).ToList();
                                    }
                                    foreach (var item in para.EnterICCID)
                                    {
                                        sql_userwarehousing1.Append("('" + item.ICCID + "','" + item.Card_ID + "'," + 0 + ",'"+item.Card_IMEI+"','" + EnterCode + "','" + time + "'),");
                                    }
                                    conn.Execute(sql_userwarehousing1.ToString().Substring(0, sql_userwarehousing1.Length - 1)); //向用户入库详细表中添加
                                }
                            }
                            if (para.EnterICCID.Count % 2000 != 0)
                            {
                                addnum = para.EnterICCID.Count / 2000;
                                addnum = addnum + 1;
                                for (int j = 0; j < addnum; j++)
                                {
                                    StringBuilder sql_userwarehousing1 = new StringBuilder("INSERT INTO userwarehousing  (Card_ICCID,Card_ID,Isout,Card_IMEI, EnterCode, AddTime)VALUES");
                                    if (j == 0)
                                    {                                       
                                        para.EnterICCID = para.EnterICCID.Skip(0).Take(10).ToList();
                                    }
                                    else
                                    {
                                        para.EnterICCID =list.Skip(j * 10).Take(2000).ToList();
                                    }
                                    foreach (var item in para.EnterICCID)
                                    {
                                        sql_userwarehousing1.Append("('" + item.ICCID + "','" + item.Card_ID + "'," + 0 + ",'"+item.Card_IMEI+"','" + EnterCode + "','" + time + "'),"); 
                                    }
                                    conn.Execute(sql_userwarehousing1.ToString().Substring(0, sql_userwarehousing1.Length - 1)); //向用户入库详细表中添加
                                }
                            }
                        }
                        if(list.Count<= 2000)
                        {
                            foreach (var item in list)
                            {
                                sql_userwarehousing.Append("('" + item.ICCID + "','" + item.Card_ID + "'," + 0 + ",'"+item.Card_IMEI+"','" + EnterCode + "','" + time + "'),");
                            }
                            conn.Execute(sql_userwarehousing.ToString().Substring(0, sql_userwarehousing.Length - 1)); //向用户入库详细表中添加
                        }
                    }
                    //foreach (var item in para.EnterICCID)
                    //{
                    //    sql_userwarehousing.Append("('" + item.Card_ICCID + "','"+item.Card_ID+"'," + 0 + ",'" + EnterCode + "','"+time+"'),");                        
                    //}                   
                        var iccidse=para.EnterICCID.OrderBy(t => t.ICCID).ToList();
                        var cardid = para.EnterICCID.OrderBy(t => t.Card_ID).ToList();
                        StartICCID = iccidse.Select(t=>t.ICCID).FirstOrDefault();
                        EndICCID = iccidse.Select(t=>t.ICCID).LastOrDefault();
                        StartCardID = cardid.Select(t => t.Card_ID).FirstOrDefault();
                        EndCardID = cardid.Select(t => t.Card_ID).LastOrDefault();
                    adduserstock = "insert into userstock(Company_ID,personnel,StockType,CardNumber,CardEnterNumber,OperatorName,MaterielCode,StartICCID,EndICCID,StartCardID,EndCardID," +
                           "CardXTName,CardTypeName,silentDate,TestDate,Flow,Remark,EnterCode,EnterTime,AddTime,pici,SetmealID,SetmealName,CardPrice,StockCity,StockAdderss,RegionLabel)" +
                           "values('" + para.Company_ID + "','" + para.personnel + "'," + 1 + "," + CardNumber + "," + CardNumber + ",'" + para.OperatorName + "','" + para.MaterielCode + "','" + StartICCID + "','" + EndICCID + "'," +
                           "'"+StartCardID+"','"+EndCardID+"','" + para.CardXTName + "','" + para.CardTypeName + "'," + para.silentDate + "," + para.TestDate + ",'" + para.Flow + "','" + para.Remark + "','" + EnterCode + "','" + time + "','" + time + "'," +
                           "'" + pici + "','" + para.SetmealID + "','" + para.SetmealName + "'," + para.CardPrice + ",'" + para.StockCity + "','" + para.StockAdderss + "','"+para.RegionLabel+"')";                  
                    using (IDbConnection connss = DapperService.MySqlConnection())
                    {       
                            
                            //connss.Execute(sql_userwarehousing.ToString().Substring(0,sql_userwarehousing.Length-1)); //向用户入库详细表中添加
                            connss.Execute(sqlcopy1update);//修改copy1的入库状态
                            connss.Execute(adduserstock);  //向库存表中添加
                            info.flg = "1";
                            info.Msg = "操作成功!";                      
                    }
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "出现错误:" + ex;
            }
            return info;
        }

        ///<summary>
        ///查看入库信息
        /// </summary>
        public GetUserStockDto GetUserStockInfo(GetUserStockPara para)
        {
            GetUserStockDto info = new GetUserStockDto();
            List<UserStock> userStocks = new List<UserStock>();
            string userstock = "select *,DATEDIFF(date_add(userstock.AddTime, interval silentDate+TestDate MONTH),NOW()) IsFormal from userstock where StockType=1 ";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (para.Company_ID == "1556265186243")//如果是奇迹物联
                    {
                        userStocks = conn.Query<UserStock>(userstock).ToList().OrderByDescending(t=>t.EnterTime).ToList();
                    }
                    else
                    {
                        userStocks = conn.Query<UserStock>(userstock).ToList().Where(t=>t.Company_ID==para.Company_ID).ToList().OrderByDescending(t => t.EnterTime).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.CardTypeName))
                    {
                        userStocks = userStocks.Where(t => t.CardTypeName == para.CardTypeName).ToList().OrderByDescending(t => t.EnterTime).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.SetmealName))
                    {
                        userStocks = userStocks.Where(t => t.SetmealName.Contains(para.SetmealName)).ToList().OrderByDescending(t => t.EnterTime).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.MaterielCode))
                    {
                        userStocks = userStocks.Where(t => t.MaterielCode.Contains(para.MaterielCode)).ToList().OrderByDescending(t => t.EnterTime).ToList();
                    }
                    if (para.StatrEnterTime != null)
                    {
                        if (para.StatrEnterTime[0] != null && para.StatrEnterTime[1] != null)
                        {
                            userStocks = userStocks.Where(t => t.EnterTime > Convert.ToDateTime(para.StatrEnterTime[0]) && t.EnterTime <= Convert.ToDateTime(para.StatrEnterTime[1]).AddDays(1)).ToList().OrderByDescending(t => t.EnterTime).ToList();
                        }
                    }
                    info.flg = "1";
                    info.Msg = "查询成功!";
                    info.userStocks = userStocks;
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "查询失败:"+ex;
            }
                    
            return info;
        }

        ///<summary>
        ///修改入库出库信息
        /// </summary>
        public Information UpdateEnterStockInfo(UpdateEnterStockPara para)
        {
            Information info = new Information();
            string UpdateEnterStock = string.Empty;
            if (para.Company_ID == "1556265186243")
            {
                info.Msg = "无权限！";
                info.flg = "-1";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(para.EnterCode))
                {
                    UpdateEnterStock = "update userstock set SetmealName='" + para.SetmealName + "',CardPrice='" + para.CardPrice + "',Flow='" + para.Flow + "'," +
                    "OperatorName='" + para.OperatorName + "',StockCity='" + para.StockCity + "',StockAdderss='" + para.StockAdderss + "',MaterielCode='" + para.MaterielCode + "'," +
                    "CardXTName='" + para.CardXTName + "',CardTypeName='" + para.CardTypeName + "' where Company_ID='" + para.Company_ID + "' and EnterCode='" + para.EnterCode + "'";
                }
                if (!string.IsNullOrWhiteSpace(para.OutCode))
                {
                    UpdateEnterStock = "update userstock set SetmealName='" + para.SetmealName + "',CardPrice='" + para.CardPrice + "',Flow='" + para.Flow + "'," +
                    "OperatorName='" + para.OperatorName + "',StockCity='" + para.StockCity + "',StockAdderss='" + para.StockAdderss + "',MaterielCode='" + para.MaterielCode + "'," +
                    "CardXTName='" + para.CardXTName + "',CardTypeName='" + para.CardTypeName + "' where Company_ID='" + para.Company_ID + "' and OutCode='" + para.OutCode + "'";
                }   
                try
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        conn.Execute(UpdateEnterStock);
                        info.Msg = "操作成功!";
                        info.flg = "1";
                    }
                }
                catch (Exception ex)
                {
                    info.Msg = "操作失败!"+ex;
                    info.flg = "-1";
                }
            }            
            return info;
        }

        ///<summary>
        ///出库操作  输入导出数量或者导入EXCEL方式导出数据
        /// </summary>
        public Information OutStock(OutStockPara para)
        {
            Information info = new Information();
            DateTime time = DateTime.Now;
            string OutCode= Unit.GetTimeStamp(DateTime.Now);//出库单号
            string pici = "CK" + DateTime.Now.ToString("yyyyMMdd");

            int CardOutNumber = para.EnterICCID.Count;
            int CardNumber = 0;
            int OutNumber = 0;
            string iccidstr = string.Empty;
            int i = 0;
            string StartICCID = string.Empty;
            string EndICCID = string.Empty;
            string StartCardID = string.Empty;
            string EndCardID = string.Empty;
            string userstocklist = "select *  from userstock where  StockType=2";
            List<ICCIDS> li = new List<ICCIDS>();
            List<ICCIDS> li_Num = new List<ICCIDS>();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    i = conn.Query<UserStock>(userstocklist).ToList().Where(t => t.OutTime.Year == DateTime.Now.Year && t.OutTime.Month == DateTime.Now.Month && t.OutTime.Day == DateTime.Now.Day).Count();
                    i = i + 1;
                    pici = pici + i.ToString("d3");
                }
                if (para.Company_ID == "1556265186243")
                {
                    info.flg = "-1";
                    info.Msg = "无权限!";
                    return info;
                }
                if (para.EnterICCID.Count > 0)//用户上传excel出库
                {
                    StringBuilder outstockcode = new StringBuilder("select DISTINCT EnterCode from userwarehousing where Card_ICCID in(");
                    StringBuilder sqloutstock = new StringBuilder("select  Card_ICCID from userwarehousing where Card_ICCID in( ");
                    StringBuilder sqloutstocknum = new StringBuilder("select  Card_ICCID from userwarehousing where Card_ICCID in( ");
                    StringBuilder useroutstock = new StringBuilder("insert into useroutstock(Card_ICCID,Card_ID,OutCode,AddTime)values");
                    StringBuilder updateenterstock = new StringBuilder("update userwarehousing set Isout=1,OutCode='" + OutCode + "' where Card_ICCID in(");
                    foreach (var item in para.EnterICCID)
                    {
                        sqloutstock.Append("'" + item.ICCID + "',");
                        outstockcode.Append("'" + item.ICCID + "',");//查找excel表中的出库的入库单号
                        iccidstr +="'"+ item.ICCID + "',";
                        useroutstock.Append("('" + item.ICCID + "','" + item.Card_ID + "','" + OutCode + "','" + time + "'),");
                        updateenterstock.Append("'" + item.ICCID + "',");
                        sqloutstocknum.Append("'" + item.ICCID + "',");
                    }
                    iccidstr = iccidstr.Substring(0, iccidstr.Length - 1);
                    string sql_JD = sqloutstock.ToString();
                    string sql_Num = sqloutstocknum.ToString();
                    string sql_entercode = outstockcode.ToString();
                    sql_JD = sql_JD.Substring(0, sql_JD.Length - 1) + ") and Isout=1";
                    sql_entercode = sql_entercode.Substring(0, sql_entercode.Length - 1) + ") and Isout=0";//查看未出库的卡的入库编码
                    sql_Num = sql_Num.Substring(0, sql_Num.Length - 1) + ") and Isout=0";                   
                    //List<ICCIDS> li_Num = new List<ICCIDS>();
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        li = conn.Query<ICCIDS>(sql_JD).ToList();
                        li_Num = conn.Query<ICCIDS>(sql_Num).ToList();
                    }
                    if (li.Count != 0)
                    {
                        string s = null;
                        foreach (ICCIDS item in li)
                        {
                            s += item.ICCID + ",";
                        }
                        s = s.Substring(0, s.Length - 1);
                        info.Msg = "有数据已经出库:" + s;
                        info.flg = "-1";
                    }
                    else if (li_Num.Count != para.EnterICCID.Count)
                    {
                        List<ICCIDS> list3 = para.EnterICCID.Where(x => !li_Num.Any(x2 => x.ICCID == x2.ICCID)).ToList();
                        string s = null;
                        foreach (ICCIDS item in list3)
                        {
                            s += item.ICCID + ",";
                        }
                        s = s.Substring(0, s.Length - 1);
                        info.Msg = "有数据不符:" + s;
                        info.flg = "-1";
                    }
                    else
                    {
                        //根据EXCEL传入的数据去查找卡信息
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            var entercodelist = conn.Query<UserWarehousing>(sql_entercode).ToList();
                            foreach (var item in entercodelist)
                            {
                                string statrend = "select Card_ICCID,Card_ID from userwarehousing where Card_ICCID in(" + iccidstr + ") and EnterCode='"+item.EnterCode+"'";
                                StartCardID = conn.Query<UserWarehousing>(statrend).ToList().OrderBy(t => t.Card_ID).FirstOrDefault().Card_ID;
                                EndCardID = conn.Query<UserWarehousing>(statrend).ToList().OrderBy(t => t.Card_ID).LastOrDefault().Card_ID;
                                StartICCID = conn.Query<UserWarehousing>(statrend).ToList().OrderBy(t => t.Card_ICCID).FirstOrDefault().Card_ICCID;
                                EndICCID = conn.Query<UserWarehousing>(statrend).ToList().OrderBy(t => t.Card_ICCID).LastOrDefault().Card_ICCID;
                                string sqluserstockinfo = "select * from userstock where EnterCode='" + item.EnterCode + "'";
                                var stocklist = conn.Query<UserStock>(sqluserstockinfo).ToList();
                                foreach (var items in stocklist)
                                {
                                    //添加库存信息出库信息
                                    string addoutstockinfo = "insert into userstock(Company_ID,personnel,StockType,CardOutNumber,OperatorName,MaterielCode,StartICCID,EndICCID,StartCardID,EndCardID," +
                                           "CardXTName,CardTypeName,silentDate,TestDate,Flow,Remark,OutCode,AddTime,EnterTime,OutTime,pici,SetmealID,SetmealName,CardPrice,StockCity,StockAdderss)" +
                                           "values('" + para.Company_ID + "','" + para.personnel + "'," + 2 + "," + CardOutNumber + ",'" + items.OperatorName + "','" + items.MaterielCode + "','"+StartICCID+"','"+EndICCID+"','"+StartCardID+"','"+EndCardID+"'," +
                                           "'" + items.CardXTName + "','" + items.CardTypeName + "'," + items.silentDate + "," + items.TestDate + ",'" + items.Flow + "','" + para.Remark + "','" + OutCode + "','" + time + "'," +
                                           "'" + items.EnterTime + "','" + time + "','" + pici + "','" + items.SetmealID + "','" + items.SetmealName + "'," + para.CardPrice + ",'" + items.StockCity + "','" + items.StockAdderss + "')";
                                    //出库了多少卡
                                    string outnum = "select id from userwarehousing where EnterCode='" + items.EnterCode + "' and Card_ICCID in(" + iccidstr + ")";
                                    OutNumber = conn.Query<UserWarehousing>(outnum).Count();
                                    CardNumber = items.CardNumber - OutNumber;
                                    //修改入库的库存信息
                                    string updatestockinfo = "update userstock set CardNumber=" + CardNumber + " where EnterCode='" + items.EnterCode + "' and Company_ID='" + para.Company_ID + "'";
                                    conn.Execute(addoutstockinfo);
                                    conn.Execute(updatestockinfo);
                                }
                                //修改入库表的状态
                                string updateuserwarehousing = "update userwarehousing set Isout=1,OutCode='"+OutCode+ "' where Card_ICCID in(" + iccidstr + ") and EnterCode='"+item.EnterCode+"'";
                                conn.Execute(updateuserwarehousing);
                            }
                            //向出库表里添加信息
                            conn.Execute(useroutstock.ToString().Substring(0, useroutstock.ToString().Length - 1));
                            //修改入库表的状态
                            //conn.Execute(updateenterstock.ToString().Substring(0, updateenterstock.ToString().Length - 1) + ")");
                        }
                        info.flg = "1";
                        info.Msg = "操作成功!";
                    }
                }
                else  //用户直接去查找信息去出库
                {
                    string sqluserstock = "select * from userstock where EnterCode='" + para.EnterCode + "'";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        if (para.CardOutNumber > conn.Query<UserStock>(sqluserstock).Select(t => t.CardNumber).FirstOrDefault())
                        {
                            info.flg = "-1";
                            info.Msg = "出库数量请不要大于库存剩余数量!";
                            return info;
                        }
                        string sql_GetDate = "select Card_ICCID as ICCID,Card_ID from userwarehousing where Isout=0  and EnterCode='" + para.EnterCode + "' limit 0," + para.CardOutNumber;
                        li = conn.Query<ICCIDS>(sql_GetDate).ToList();
                        var stocklist = conn.Query<UserStock>(sqluserstock).ToList();
                        foreach (var item in stocklist)
                        {
                            StartICCID = li.OrderBy(t => t.ICCID).ToList().FirstOrDefault().ICCID;
                            EndICCID = li.OrderBy(t => t.ICCID).ToList().LastOrDefault().ICCID;
                            StartCardID= li.OrderBy(t => t.Card_ID).ToList().FirstOrDefault().Card_ID;
                            EndCardID = li.OrderBy(t => t.Card_ID).ToList().LastOrDefault().Card_ID;
                            //添加库存信息出库信息
                            string addoutstockinfo = "insert into userstock(Company_ID,personnel,StockType,CardOutNumber,OperatorName,MaterielCode,StartICCID,EndICCID,StartCardID,EndCardID," +
                            "CardXTName,CardTypeName,silentDate,TestDate,Flow,Remark,OutCode,AddTime,EnterTime,OutTime,pici,SetmealID,SetmealName,CardPrice,StockCity,StockAdderss)" +
                            "values('" + para.Company_ID + "','" + para.personnel + "'," + 2 + "," +para.CardOutNumber + ",'" + item.OperatorName + "','" + item.MaterielCode + "','"+StartICCID+"','"+EndICCID+"','"+StartCardID+"','"+EndCardID+"'," +
                            "'" + item.CardXTName + "','" + item.CardTypeName + "'," + item.silentDate + "," + item.TestDate + ",'" + item.Flow + "','" + para.Remark + "','" + OutCode + "','" + time + "'," +
                            "'" + item.EnterTime + "','" + time + "','" + pici + "','" + item.SetmealID + "','" + item.SetmealName + "'," + para.CardPrice + ",'" + item.StockCity + "','" + item.StockAdderss + "')";

                            CardNumber = item.CardNumber - para.CardOutNumber;
                            //修改入库的库存信息
                            string updatestockinfo = "update userstock set CardNumber=" + CardNumber + " where EnterCode='" + item.EnterCode + "' and Company_ID='" + para.Company_ID + "'";
                            conn.Execute(addoutstockinfo);
                            conn.Execute(updatestockinfo);
                        }
                        //向出库表添加信息 修改入库表
                        string outiccidstr = string.Empty;
                        StringBuilder adduseroutstock = new StringBuilder("insert into useroutstock(Card_ICCID,Card_ID,OutCode,AddTime)values");
                        StringBuilder updateenterstock = new StringBuilder("update userwarehousing set Isout=1,OutCode='" + OutCode + "' where Card_ICCID in(");
                        foreach (var items in li)
                        {
                            adduseroutstock.Append("('" + items.ICCID + "','" + items.Card_ID + "','" + OutCode + "','" + time + "'),");
                            updateenterstock.Append("'" + items.ICCID + "',");
                        }
                        //向出库表里添加信息
                        conn.Execute(adduseroutstock.ToString().Substring(0, adduseroutstock.ToString().Length - 1));
                        //修改入库表的状态
                        conn.Execute(updateenterstock.ToString().Substring(0, updateenterstock.ToString().Length - 1)+")");
                        info.Msg = "操作成功!";
                        info.flg = "1";
                    }                    
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "操作失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///查看出库信息  成功出库后的信息
        /// </summary>
        public OutStockDto GetOutStockInfo(GetUserStockPara para)
        {
            OutStockDto info = new OutStockDto();
            List<UserStock> outstock = new List<UserStock>();
            string userstock = "select * from userstock where StockType=2";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (para.Company_ID == "1556265186243")//如果是奇迹物联
                    {
                        outstock = conn.Query<UserStock>(userstock).ToList();
                    }
                    else
                    {
                        outstock = conn.Query<UserStock>(userstock).ToList().Where(t => t.Company_ID == para.Company_ID).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.CardTypeName))
                    {
                        outstock = outstock.Where(t => t.CardTypeName == para.CardTypeName).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.SetmealName))
                    {
                        outstock = outstock.Where(t => t.SetmealName.Contains(para.SetmealName)).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.MaterielCode))
                    {
                        outstock = outstock.Where(t => t.MaterielCode.Contains(para.MaterielCode)).ToList();
                    }
                    if (para.StatrEnterTime != null)
                    {
                        outstock = outstock.Where(t => t.OutTime > Convert.ToDateTime(para.StatrEnterTime[0]) && t.OutTime <= Convert.ToDateTime(para.StatrEnterTime[1]).AddDays(1)).ToList();
                    }
                    //if (para.StatrEnterTime != null && para.EndEnterTime != null)
                    //{
                    //    outstock = outstock.Where(t => t.OutTime > para.StatrEnterTime && t.EnterTime <= para.EndEnterTime.Value.AddDays(1)).ToList();
                    //}
                    info.flg = "1";
                    info.Msg = "查询成功!";
                    info.outstock = outstock;
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "查询失败:" + ex;
            }
            return info;
        }

        ///<summary>
        ///撤销出库信息
        /// </summary>
        public Information CancelOutStock(string Company_ID,string OutCode)
        {
            Information info = new Information();
            try
            {
                if (Company_ID == "1556265186243")
                {
                    info.flg = "-1";
                    info.Msg = "无权限!";
                }
                else
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        //获取出库时间
                        string sqlouttime = "select OutTime from userstock where OutCode='"+OutCode+"'";
                        DateTime outtime = conn.Query<UserStock>(sqlouttime).Select(t => t.OutTime).FirstOrDefault();
                        DateTime time = DateTime.Now;
                        
                        //if (outtime!=null && outtime)
                        //{

                        //}
                        string sql = "DELETE from userstock where OutCode='" + OutCode + "' and Company_ID='" + Company_ID + "'";//删除库存信息中出库的信息
                        //取出出库数量
                        //string sqloutnum = "select CardOutNumber from userstock where OutCode='"+OutCode+"'";
                        //int CardOutNumber = conn.Query<UserStock>(sqloutnum).Select(t => t.CardOutNumber).FirstOrDefault();
                        string sqlEnterCode = "select DISTINCT EnterCode from userwarehousing where OutCode='" + OutCode + "'";
                        var EnterCodeList = conn.Query<UserWarehousing>(sqlEnterCode).ToList();//找到入库单号 出库导入excel可能是从不同的入库单里面出库的
                        foreach (var item in EnterCodeList)
                        {
                            string sqlCardNumber = "select CardNumber from userstock where EnterCode='" + item.EnterCode + "'";
                            string sqloutnum = "select id from userwarehousing where Isout=1 and OutCode='" + OutCode + "'";
                            int CardOutNumber = conn.Query<UserWarehousing>(sqloutnum).Count();
                            int CardNumber = conn.Query<UserStock>(sqlCardNumber).Select(t => t.CardNumber).FirstOrDefault() + CardOutNumber;
                            //修改入库信息的卡剩余数量
                            string updateuserstock = "update userstock set CardNumber=" + CardNumber + " where EnterCode='" + item.EnterCode + "' and Company_ID='" + Company_ID + "'";
                            conn.Execute(updateuserstock);
                        }
                        //修改入库的卡的状态
                        string updateuserwarehousing = "update userwarehousing set Isout=0,OutCode='' where OutCode='" + OutCode + "'";
                        //删除出库表信息
                        string deleoutstock = "delete from useroutstock where OutCode='" + OutCode + "'";
                        conn.Execute(sql);
                        conn.Execute(updateuserwarehousing);
                        conn.Execute(deleoutstock);
                        info.flg = "1";
                        info.Msg = "操作成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "发生错误，请联系管理员"+ex;
            }
            return info;
        }

        ///<summary>
        ///查看入库出库卡的详细信息（库存中每一张卡的信息）
        /// </summary>
        public GetUserStockDetailDto GetOutEnterStockDetail(string OutCode,string EnterCode)
        {
            GetUserStockDetailDto info = new GetUserStockDetailDto();
            List<StockCardInfo> stockCards = new List<StockCardInfo>();
            string CardTypeName = string.Empty; ;
            string CardXTName = string.Empty;
            string Flow = string.Empty;
            string SetmealName = string.Empty;
            string MaterielName = string.Empty;//物料名称
            string sqlwarehousing = "select * from userwarehousing";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var warehousing = conn.Query<UserWarehousing>(sqlwarehousing).ToList();
                    if (!string.IsNullOrWhiteSpace(OutCode))
                    {
                        string sqluserstock = "select * from userstock where OutCode='"+OutCode+"'";
                        warehousing = warehousing.Where(t => t.OutCode == OutCode).OrderBy(t=>t.Card_ICCID).ToList();
                        var userstockinfo = conn.Query<UserStock>(sqluserstock).ToList();
                        foreach (var item in userstockinfo)
                        {
                            foreach (var items in warehousing)
                            {
                                string cardsetmeal = "select * from setmeal where SetmealID='" + item.SetmealID + "'";
                                var listsetmeal = conn.Query<setmeal>(cardsetmeal).FirstOrDefault();
                                if (listsetmeal != null)
                                {
                                    Flow = listsetmeal.Flow.ToString();
                                    SetmealName = listsetmeal.PackageDescribe;
                                    string sqlcardtype = "select * from cardtype where CardTypeID='" + listsetmeal.CardTypeID + "'";
                                    CardTypeName = conn.Query<UserStock>(sqlcardtype).Select(t => t.CardTypeName).FirstOrDefault();
                                    string sqlcardxingtai = "select * from card_xingtai where CardXTID='" + listsetmeal.CardXTID + "'";
                                    CardXTName = conn.Query<UserStock>(sqlcardxingtai).Select(t => t.CardXTName).FirstOrDefault();
                                    MaterielName = listsetmeal.PartNumber;
                                }
                                StockCardInfo userStock = new StockCardInfo();
                                userStock.Card_ICCID = items.Card_ICCID;
                                userStock.Card_ID = items.Card_ID;
                                userStock.CardPrice = item.CardPrice;
                                userStock.CardTypeName = CardTypeName;
                                userStock.CardXTName = CardXTName;
                                userStock.Company_ID = item.Company_ID;
                                userStock.Flow = Flow;
                                userStock.MaterielCode = item.MaterielCode;
                                userStock.OperatorName = "中国移动";
                                userStock.OutPurpose = item.OutPurpose;
                                userStock.personnel = item.personnel;
                                userStock.pici = item.pici;
                                userStock.Remark = item.Remark;
                                userStock.SetmealName = SetmealName;
                                userStock.silentDate = item.silentDate;
                                userStock.StockAdderss = item.StockAdderss;
                                userStock.StockCity = item.StockCity;
                                userStock.TestDate = item.TestDate;
                                userStock.MaterielName = MaterielName;
                                stockCards.Add(userStock);
                            }
                        } 
                    }
                    if (!string.IsNullOrWhiteSpace(EnterCode))
                    {
                        var warehousings = conn.Query<UserWarehousing>(sqlwarehousing).ToList();
                        string sqluserstock = "select * from userstock where EnterCode='" + EnterCode + "'";
                        warehousings = warehousings.Where(t => t.EnterCode == EnterCode).OrderBy(t => t.Card_ICCID).ToList();
                        var userstockinfo = conn.Query<UserStock>(sqluserstock).ToList();
                        foreach (var item in userstockinfo)
                        {
                            foreach (var items in warehousings)
                            {
                                string cardsetmeal = "select * from setmeal where SetmealID='"+item.SetmealID+"'";
                                var listsetmeal = conn.Query<setmeal>(cardsetmeal).FirstOrDefault();
                                if (listsetmeal != null)
                                {
                                    Flow = listsetmeal.Flow.ToString();
                                    SetmealName = listsetmeal.PackageDescribe;
                                    string sqlcardtype = "select * from cardtype where CardTypeID='"+listsetmeal.CardTypeID+"'";
                                    CardTypeName = conn.Query<UserStock>(sqlcardtype).Select(t => t.CardTypeName).FirstOrDefault();
                                    string sqlcardxingtai = "select * from card_xingtai where CardXTID='" + listsetmeal.CardXTID + "'";
                                    CardXTName = conn.Query<UserStock>(sqlcardxingtai).Select(t => t.CardXTName).FirstOrDefault();
                                    MaterielName = listsetmeal.PartNumber;
                                }
                                StockCardInfo userStock = new StockCardInfo();
                                userStock.Card_ICCID = items.Card_ICCID;
                                userStock.Card_ID = items.Card_ID;
                                userStock.CardPrice = item.CardPrice;
                                userStock.CardTypeName = CardTypeName;
                                userStock.CardXTName = CardXTName;
                                userStock.Company_ID = item.Company_ID;
                                userStock.Flow = Flow;
                                userStock.MaterielCode = item.MaterielCode;
                                userStock.OperatorName = "中国移动";
                                userStock.OutPurpose = item.OutPurpose;
                                userStock.personnel = item.personnel;
                                userStock.pici = item.pici;
                                userStock.Remark = item.Remark;
                                userStock.SetmealName = SetmealName;
                                userStock.silentDate = item.silentDate;
                                userStock.StockAdderss = item.StockAdderss;
                                userStock.StockCity = item.StockCity;
                                userStock.TestDate = item.TestDate;
                                userStock.MaterielName = MaterielName;
                                stockCards.Add(userStock);
                            }
                        }
                    }
                    info.flg = "1";
                    info.Msg = "查看成功!";
                    info.stockCards = stockCards;
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "出现错误:"+ex;
            }
            return info;
        }


        ///<summary>
        ///导出入库信息导出出库信息导出目前库存信息
        /// </summary>
        public GetUserStockDetailDto ExportUserStock(int ExportType,string Code)
        {
            //ExportType  1:入库信息   2:出库信息  3:剩余库存信息
            GetUserStockDetailDto info = new GetUserStockDetailDto();
            List<StockCardInfo> stockCards = new List<StockCardInfo>();
            string sqlwarehousing = "select * from userwarehousing";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var warehousing = conn.Query<UserWarehousing>(sqlwarehousing).ToList();
                    if (ExportType==2)
                    {
                        string sqluserstock = "select * from userstock where OutCode='" + Code + "'";
                        warehousing = warehousing.Where(t => t.OutCode == Code).OrderBy(t => t.Card_ICCID).ToList();
                        var userstockinfo = conn.Query<UserStock>(sqluserstock).ToList();
                        var item = conn.Query<UserStock>(sqluserstock).FirstOrDefault();
                        //foreach (var item in userstockinfo)
                        //{
                            foreach (var items in warehousing)
                            {
                                StockCardInfo userStock = new StockCardInfo();
                                userStock.Card_ICCID = items.Card_ICCID;
                                userStock.Card_ID = items.Card_ID;
                                userStock.CardPrice = item.CardPrice;
                                userStock.CardTypeName = item.CardTypeName;
                                userStock.CardXTName = item.CardXTName;
                                userStock.Company_ID = item.Company_ID;
                                userStock.Flow = item.Flow;
                                userStock.MaterielCode = item.MaterielCode;
                                userStock.OperatorName = item.OperatorName;
                                userStock.OutPurpose = item.OutPurpose;
                                userStock.personnel = item.personnel;
                                userStock.pici = item.pici;
                                userStock.Remark = item.Remark;
                                userStock.SetmealName = item.SetmealName;
                                userStock.silentDate = item.silentDate;
                                userStock.StockAdderss = item.StockAdderss;
                                userStock.StockCity = item.StockCity;
                                userStock.TestDate = item.TestDate;
                                stockCards.Add(userStock);
                            }
                        //}
                    }
                    if (ExportType==1)
                    {
                        var warehousings = conn.Query<UserWarehousing>(sqlwarehousing).ToList();
                        string sqluserstockinfo = "select * from userstock where EnterCode='" + Code + "'";
                        string sqluserstock = "select * from userwarehousing where EnterCode='" + Code + "'";
                        warehousings = warehousings.Where(t => t.EnterCode == Code).OrderBy(t => t.Card_ICCID).ToList();
                        var userstockinfo = conn.Query<UserStock>(sqluserstock).ToList();
                        var item = conn.Query<UserStock>(sqluserstockinfo).FirstOrDefault();
                        foreach (var items in warehousings)
                        {
                                StockCardInfo userStock = new StockCardInfo();
                                userStock.Card_ICCID = items.Card_ICCID;
                                userStock.Card_ID = items.Card_ID;
                                userStock.CardPrice = item.CardPrice;
                                userStock.CardTypeName = item.CardTypeName;
                                userStock.CardXTName = item.CardXTName;
                                userStock.Company_ID = item.Company_ID;
                                userStock.Flow = item.Flow;
                                userStock.MaterielCode = item.MaterielCode;
                                userStock.OperatorName = item.OperatorName;
                                userStock.OutPurpose = item.OutPurpose;
                                userStock.personnel = item.personnel;
                                userStock.pici = item.pici;
                                userStock.Remark = item.Remark;
                                userStock.SetmealName = item.SetmealName;
                                userStock.silentDate = item.silentDate;
                                userStock.StockAdderss = item.StockAdderss;
                                userStock.StockCity = item.StockCity;
                                userStock.TestDate = item.TestDate;
                                stockCards.Add(userStock);
                        }
                        
                    }
                    if (ExportType == 3)
                    {
                        var warehousings = conn.Query<UserWarehousing>(sqlwarehousing).ToList();
                        string sqluserstockinfo = "select * from userstock where EnterCode='" + Code + "'";
                        string sqluserstock = "select * from userwarehousing where EnterCode='" + Code + "' and Isout=0";
                        warehousings = warehousings.Where(t => t.EnterCode == Code && t.Isout==0).OrderBy(t => t.Card_ICCID).ToList();
                        //var userstockinfo = conn.Query<UserStock>(sqluserstock).ToList();
                        var item = conn.Query<UserStock>(sqluserstockinfo).FirstOrDefault();
                        //foreach (var item in userstockinfo)
                        //{
                            foreach (var items in warehousings)
                            {
                                StockCardInfo userStock = new StockCardInfo();
                                userStock.Card_ICCID = items.Card_ICCID;
                                userStock.Card_ID = items.Card_ID;
                                userStock.CardPrice = item.CardPrice;
                                userStock.CardTypeName = item.CardTypeName;
                                userStock.CardXTName = item.CardXTName;
                                userStock.Company_ID = item.Company_ID;
                                userStock.Flow = item.Flow;
                                userStock.MaterielCode = item.MaterielCode;
                                userStock.OperatorName = item.OperatorName;
                                userStock.OutPurpose = item.OutPurpose;
                                userStock.personnel = item.personnel;
                                userStock.pici = item.pici;
                                userStock.Remark = item.Remark;
                                userStock.SetmealName = item.SetmealName;
                                userStock.silentDate = item.silentDate;
                                userStock.StockAdderss = item.StockAdderss;
                                userStock.StockCity = item.StockCity;
                                userStock.TestDate = item.TestDate;
                                stockCards.Add(userStock);
                            }
                       //}
                    }
                    info.flg = "1";
                    info.Msg = "查看成功!";
                    info.stockCards = stockCards;
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "操作失败:"+ex;
            }
            return info;

        }

        #region 销售订单
        ///<summary>
        ///创建订单/合同
        /// </summary>
        public Information AddContractorderInfo(contractorder order)
        {
            Information info = new Information();
            string CompanyName = string.Empty;   
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //判断合同编号是否重复
                    string contractsql = "select ContractNo from contractorder where ContractNo='"+order.ContractNo+"' and status=1";
                    var listcno = conn.Query<contractorder>(contractsql).ToList();
                    if (listcno.Count > 0)
                    {
                        info.Msg = "合同编号已经存在，请重新添加!";
                        info.flg = "-1";
                        return info;
                    }
                    else
                    {
                        string sqlcom = "select CompanyName from company where CompanyID='" + order.CustomCompanyID + "'";
                        CompanyName = conn.Query<contractorder>(sqlcom).Select(t => t.CompanyName).FirstOrDefault();
                        string insertorder = "insert into contractorder (ContractNo,CustomCompanyID,Company_ID,MaterialCode,CardTypeID,CardXTID,Number,Price,TotalPrice,Taxation,Remar,CompanyName,pic,AddTime) " +
                            "values ('" + order.ContractNo + "','" + order.CustomCompanyID + "','" + order.Company_ID + "','" + order.MaterialCode + "','" + order.CardTypeID + "','" + order.CardXTID + "'," +
                            "'" + order.Number + "'," + order.Price + "," + order.TotalPrice + "," + order.Taxation + ",'" + order.Remar + "','" + CompanyName + "','" + order.pic + "','" + DateTime.Now + "')";
                        conn.Execute(insertorder);
                        info.Msg = "添加成功!";
                        info.flg = "1";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                info.Msg = "添加失败:"+ex;
                info.flg = "-1";
            }
            return info;
        }


        ///<summary>
        ///修改订单/合同
        /// </summary>
        public Information UpdateContractorderInfo(contractorder order)
        {
            Information info = new Information();
            string CompanyName = string.Empty;
            string ContractNo = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //判断合同编号是否重复
                    string contractsql = "select ContractNo from contractorder where Id="+order.Id+" ";
                    ContractNo = conn.Query<contractorder>(contractsql).Select(t => t.ContractNo).FirstOrDefault();
                    string sqlcom = "select CompanyName from company where CompanyID='" + order.CustomCompanyID + "'";
                    CompanyName = conn.Query<contractorder>(sqlcom).Select(t => t.CompanyName).FirstOrDefault();
                    if (ContractNo == order.ContractNo)
                    {
                        string updateorder = "update contractorder set ContractNo='"+ order.ContractNo + "',CustomCompanyID='"+ order.CustomCompanyID+ "',MaterialCode='"+order.MaterialCode+"'," +
                            "CardTypeID='"+ order.CardTypeID+ "',CardXTID='"+order.CardXTID+"',Number='"+order.Number+"',Price='"+order.Price+"',TotalPrice='"+order.TotalPrice+"'," +
                            "Taxation='"+order.Taxation+"',Remar='"+order.Remar+"',CompanyName='"+CompanyName+"',pic='"+order.pic+ "' where Id=" + order.Id+" ";
                        conn.Execute(updateorder);
                        info.Msg = "修改信息成功!";
                        info.flg = "1";
                    }
                    if (ContractNo!=order.ContractNo)
                    {
                        string contrac = "select ContractNo from contractorder where ContractNo=" + order.ContractNo + " and status=1";
                        var listcontrac = conn.Query<contractorder>(contrac).ToList();
                        if (listcontrac.Count > 0)
                        {
                            info.Msg = "合同编号已经存在，请重新添加!";
                            info.flg = "-1";
                            return info;
                        }
                        else
                        {
                            string updateorder = "update contractorder set ContractNo='" + order.ContractNo + "',CustomCompanyID='" + order.CustomCompanyID + "',MaterialCode='" + order.MaterialCode + "'," +
                            "CardTypeID='" + order.CardTypeID + "',CardXTID='" + order.CardXTID + "',Number='" + order.Number + "',Price='" + order.Price + "',TotalPrice='" + order.TotalPrice + "'," +
                            "Taxation='" + order.Taxation + "',Remar='" + order.Remar + "',CompanyName='" + CompanyName + "',pic='" + order.pic + "'  where Id=" + order.Id + " ";
                            conn.Execute(updateorder);
                            info.Msg = "修改信息成功!";
                            info.flg = "1";
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                info.Msg = "添加失败:" + ex;
                info.flg = "-1";
            }
            return info;
        }

        ///<summary>
        ///删除订单/合同
        /// </summary>
        public Information DelContractorderInfo(int Id)
        {
            Information info = new Information();
            try
            {
                string updatestatus = "update contractorder set status=0 where Id="+Id+"";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(updatestatus);
                    info.flg = "1";
                    info.Msg = "删除成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "删除失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///查看订单
        ///Company_ID  当前登录客户的companyid
        ///CompanyName 查找的用户的名称
        /// </summary>
        public GetContractorderDto GetContractorderInfo(string Company_ID,string CompanyName)
        {
            GetContractorderDto dto = new GetContractorderDto();
            List<contractorder> listdto = new List<contractorder>();
            string CardTypeName = string.Empty;
            string CardXTName = string.Empty;
            try
            {
                string sql = "select * from contractorder where status=1 and  Company_ID='" + Company_ID+ "' or CustomCompanyID='"+ Company_ID + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var lists = conn.Query<contractorder>(sql).ToList();
                    if (!string.IsNullOrWhiteSpace(CompanyName))
                    {
                        lists = lists.Where(t => t.CompanyName.Contains(CompanyName)).ToList(); ;
                    }
                    foreach (var item in lists)
                    {
                        contractorder order = new contractorder();
                        string sqlxt = "select CardXTName  from card_xingtai where  CardXTID='"+item.CardXTID+"'";
                        CardXTName = conn.Query<contractorder>(sqlxt).Select(t => t.CardXTName).FirstOrDefault();
                        string sqltype = "select CardTypeName  from cardtype where  CardTypeID='" + item.CardTypeID + "'";
                        CardTypeName = conn.Query<contractorder>(sqltype).Select(t => t.CardTypeName).FirstOrDefault();
                        order.Id = item.Id;
                        order.AddTime = item.AddTime;
                        order.CardTypeID = item.CardTypeID;
                        order.CardTypeName = CardTypeName;
                        order.CardXTName = CardXTName;
                        order.CardXTID = item.CardXTID;
                        order.CompanyName = item.CompanyName;
                        order.Company_ID = item.Company_ID;
                        order.ContractNo = item.ContractNo;
                        order.CustomCompanyID = item.CustomCompanyID;
                        order.MaterialCode = item.MaterialCode;
                        order.Number = item.Number;
                        order.pic = item.pic;
                        order.Price = item.Price;
                        order.Remar = item.Remar;
                        order.Taxation = item.Taxation;
                        order.TotalPrice = item.TotalPrice;
                        listdto.Add(order);
                    }
                    dto.orders = listdto;
                    dto.Msg = "成功!";
                    dto.flg = "1";
                }
            }
            catch (Exception ex)
            {
                dto.Msg = "失败:"+ex;
                dto.flg = "-1";
            }
            return dto;
        }

        ///<summary>
        ///查看销售订单详细信息
        /// </summary>
        public ContractorderDetailDto GetContractorderDetail(string ContractNo,string Value)
        {
            ContractorderDetailDto dto = new ContractorderDetailDto();
            try
            {
                string sql = "select t1.Card_ID,t1.Card_ICCID,t2.CompanyName,t2.Price,t2.ContractNo from  card_copy1 t1 left join contractorder t2 on t1.ContractNo=t2.ContractNo WHERE t2.ContractNo='"+ContractNo+"'";
                if (!string.IsNullOrWhiteSpace(Value))
                {
                    sql += " and t1.Card_ID='" + Value + "' or t1.Card_ICCID='" + Value + "'";
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    dto.details = conn.Query<ContractorderDetail>(sql).ToList();
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败："+ex;
            }
            return dto;
        }


        ///<summary>
        ///查看账单信息
        /// </summary>
        public OrderManageDto GetOrderManageInfo(string Company_ID,string CompanyName)
        {
            OrderManageDto dto = new OrderManageDto();
            try
            {
                string sqlorder = string.Empty;
                if (string.IsNullOrWhiteSpace(CompanyName))
                {
                    sqlorder = "select ContractNo from contractorder where CustomCompanyID='" + Company_ID + "' or Company_ID='" + Company_ID + "' and status=1";
                }
                else
                {
                    sqlorder = "select ContractNo from contractorder where CustomCompanyID='" + Company_ID + "' or Company_ID='" + Company_ID + "' and CompanyName like '%"+CompanyName+"%' and status=1";
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    List<OrderManage> orders = new List<OrderManage>();
                    var listorder = conn.Query<OrderManage>(sqlorder).ToList();
                    foreach (var item in listorder)
                    {
                        string sql = "select count(1) as CardNumber, t2.TotalPrice,t2.Price,t2.ContractNo,t2.CardXTID,t2.CardTypeID ,t2.CompanyName" +
                            " from card_copy1 t1 left join contractorder t2 on t1.ContractNo=t2.ContractNo where t1.ContractNo='"+item.ContractNo+ "' and TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=30 and t1.status=1";
                        var listcon = conn.Query<OrderManage>(sql).FirstOrDefault();
                        if (listcon != null)
                        {
                            if (listcon.CardNumber > 0)
                            {
                                OrderManage manage = new OrderManage();
                                manage.CardNumber = listcon.CardNumber;
                                manage.ContractNo = listcon.ContractNo;
                                manage.TotalPrice = listcon.TotalPrice;
                                manage.Price = listcon.Price;
                                manage.CompanyName = listcon.CompanyName;
                                string sqlxt = "select CardXTName  from card_xingtai where  CardXTID='" + listcon.CardXTID + "'";
                                manage.CardXTName = conn.Query<contractorder>(sqlxt).Select(t => t.CardXTName).FirstOrDefault();
                                string sqltype = "select CardTypeName  from cardtype where  CardTypeID='" + listcon.CardTypeID + "'";
                                manage.CardTypeName = conn.Query<contractorder>(sqltype).Select(t => t.CardTypeName).FirstOrDefault();
                                orders.Add(manage);
                            }
                           
                        }
                    }
                    dto.orders = orders;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.orders = null;
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }

        ///<summary>
        ///查看账单详细信息
        ///Value 卡号或ICCID
        /// </summary>
        public OrderDetailDto GetOrderDetailInfo(string ContractNo,string Value)
        {
            OrderDetailDto dto = new OrderDetailDto();
            string sql = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (string.IsNullOrWhiteSpace(Value))
                    {
                        sql = "select t2.CompanyName,t1.Card_ICCID,t1.Card_ID,t1.Card_EndTime from card_copy1 t1 left join contractorder t2 on t1.ContractNo=t2.ContractNo " +
                        " where t1.ContractNo='"+ContractNo+"' and TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=30 and t1.status=1";
                    }
                    else
                    {
                    sql = "select t2.CompanyName,t1.Card_ICCID,t1.Card_ID,t1.Card_EndTime from card_copy1 t1 left join contractorder t2 on t1.ContractNo=t2.ContractNo " +
                    " where t1.ContractNo='" + ContractNo + "' and Card_ICCID='" + Value + "' or Card_ID='" + Value + "' and TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=30 and t1.status=1";
                    }
                    dto.details = conn.Query<OrderDetail>(sql).ToList();
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.details = null;
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }
        #endregion


        #region 采购订单相关接口
        ///<summary>
        ///创建运营商接口
        /// </summary>
        public Information AddAccountInfo(accounts para)
        {
            Information info = new Information();
            string accountid = Unit.GetTimeStamp(DateTime.Now);
            string operatorsName = string.Empty;
            if (para.Platform == 1)
            {
                operatorsName = "奇迹";
            }
            if (para.Platform == 11)
            {
                operatorsName = "移动";
            }
            if (para.Platform == 21)
            {
                operatorsName = "电信";
            }
            if (para.Platform == 31)
            {
                operatorsName = "联通";
            }
            try
            {
                string companyname = string.Empty;
                string sqlcompany = "select CompanyName from company where CompanyID='"+para.Company_ID+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    companyname = conn.Query<Company>(sqlcompany).Select(t => t.CompanyName).FirstOrDefault();
                    string addaccount = "insert into accounts (Company_ID,APPID,PASSWORD,accountName,accountID,TOKEN,TRANSID,Remark,URL,Platform,companyname,UserId,UserName,APIkey,Contacts,Job," +
                    "Email,Pic,BillName,DutyParagraph,AddressPhone,Blank,BlankNo,operatorsName,PlatformFlg,AddTime) values ('" + para.Company_ID+ "','" + para.APPID + "','" + para.PASSWORD + "','" + para.accountName + "'," +
                    "'" + accountid + "','" + para.TOKEN + "','" + para.TRANSID + "','" + para.Remark + "','" + para.url + "','" + para.Platform + "','"+companyname+"'," +
                    "'" + para.UserId + "','" + para.UserName + "','" + para.APIkey + "','" + para.Contacts + "','" + para.Job + "','" + para.Email + "'," +
                    "'" + para.Pic + "','" + para.BillName + "','" + para.DutyParagraph + "','" + para.AddressPhone + "','" + para.Blank + "','" + para.BlankNo + "','"+operatorsName+"'," +
                    "'"+para.Platform+"','" + DateTime.Now + "')";
                
                    conn.Execute(addaccount);
                    info.flg = "1";
                    info.Msg = "添加成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "添加失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///添加API信息
        /// </summary>
        public Information AddOperatorApi(accounts para)
        {
            Information info = new Information();
            string accountid = Unit.GetTimeStamp(DateTime.Now);
            string companyid = string.Empty;
            try
            {
                
                string updateaccount = "update accounts set UserId='"+para.UserId+"',UserName='"+para.UserName+"',APIkey='"+para.APIkey+"',APPID='"+para.APPID+ "',PASSWORD='"+para.PASSWORD+ "',accountID='"+accountid+"',selectaccountname='',selectaccountID='' where accountID='" + para.accountID+ "' and  Company_ID='" + para.Company_ID + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string accountsql = "select Company_ID from accounts where accountID='"+para.Company_ID+"'";
                    companyid = conn.Query<accounts>(accountsql).Select(t => t.Company_ID).FirstOrDefault();
                    if (para.PlatformFlg == 1)
                    {
                        accountsql = "select APPID,PASSWORD,Platform,Isqiji,UserId,UserName,APIkey,accountID,TOKEN,accountName  from accounts where  accountID='" + para.selectaccountID+"'";
                        var listaccount = conn.Query<accounts>(accountsql).FirstOrDefault();
                        if (listaccount != null)
                        {
                            updateaccount = "update accounts set UserId='"+listaccount.UserId+"',UserName='"+listaccount.UserName+"',Isqiji=1,PASSWORD='"+listaccount.PASSWORD+"'," +
                            "accountID='"+listaccount.accountID+"', APIkey='"+listaccount.APIkey+ "',TOKEN='"+listaccount.TOKEN+ "',selectaccountID='"+para.selectaccountID+"'," +
                            "APPID='"+listaccount.APPID+"',Platform='"+listaccount.Platform+ "',selectaccountname='"+listaccount.accountName+ "' where accountID='" + para.accountID+"' and Company_ID='"+para.Company_ID+"'";
                        }   
                    }                    
                    conn.Execute(updateaccount);
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
        ///查看运营商和API信息
        /// </summary>
        public GetAccountDtoInfos GetAccountInfo(AccountPara para)
        {
            GetAccountDtoInfos infos = new GetAccountDtoInfos();
            string ss = string.Empty;
            try
            {
                string sql = "select * from accounts where Company_ID='"+para.Company_ID+ "' and status=1";
                if (!string.IsNullOrWhiteSpace(para.CompanyName))
                {
                    ss = " and accountName like '%"+ para.CompanyName + "%'";
                    sql = sql +ss;
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    infos.accounts = conn.Query<accountdto>(sql).ToList();
                    infos.flg = "1";
                    infos.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                infos.flg = "-1";
                infos.Msg = "失败:"+ex;
            }
            return infos;
        }

        ///<summary>
        ///查看奇迹对接的运营商信息
        /// </summary>
        public qijiaccountinfo GetQijiAccountInfo(string CompanyID)
        {
            qijiaccountinfo info = new qijiaccountinfo();
            try
            {
                string sqlacc = "select accountID,accountName from accounts where Company_ID='"+CompanyID+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    info.accounts = conn.Query<qijiaccount>(sqlacc).ToList();
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
        ///编辑供应商信息
        /// </summary>
        public Information UpdateAccountInfo(accounts para)
        {
            Information info = new Information();
            string operatorsName = string.Empty;
            try
            {
                if (para.PlatformFlg == 1)
                {
                    operatorsName = "奇迹";
                }
                if (para.PlatformFlg == 11)
                {
                    operatorsName = "移动";
                }
                if (para.PlatformFlg == 21)
                {
                    operatorsName = "电信";
                }
                if (para.PlatformFlg == 31)
                {
                    operatorsName = "联通";
                }
                string updateaccount = "update accounts set APPID='"+para.APPID+"',PASSWORD='"+para.PASSWORD+"',TOKEN='"+para.TOKEN+"',TRANSID='"+para.TRANSID+"',Remark='"+para.Remark+"'," +
                    "URL='"+para.url+"',UserId='"+para.UserId+"',UserName='"+para.UserName+"',APIkey='"+para.APIkey+"',Contacts='"+para.Contacts+"',Job='"+para.Job+"',Email='"+para.Email+"'," +
                    "Pic='"+para.Pic+"',BillName='"+para.BillName+"',DutyParagraph='"+para.DutyParagraph+"',AddressPhone='"+para.AddressPhone+"',Blank='"+para.Blank+"',BlankNo='"+para.BlankNo+"'," +
                    "  operatorsName='"+operatorsName+ "',accountName='"+para.accountName+"',PlatformFlg=" + para.PlatformFlg + ",Platform="+para.PlatformFlg + " where accountID='" + para.accountID+ "' and Company_ID='"+para.Company_ID+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(updateaccount);
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
        ///删除供应商信息
        /// </summary>
        public Information DeleteAccountInfo(int Id)
        {
            Information info = new Information();
            try
            {
                string delaccount = "update accounts set status=0 where Id="+Id+"";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(delaccount);
                    info.flg = "1";
                    info.Msg = "删除成功";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "删除失败:"+ex;
            }
            return info;
        }


        ///<summary>
        ///创建采购订单
        /// </summary>
        public Information AddPurchaseInfo(purchase para)
        {
            Information info = new Information();
            string OperatorName = string.Empty;
            string sqloperator = "select accountName as OperatorName from accounts where Company_ID='" + para.CompanyID + "' and accountID='"+para.accountID+"'";
           
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    OperatorName = conn.Query<purchase>(sqloperator).Select(t => t.OperatorName).FirstOrDefault();
                    string addpruchase = "insert into purchase (PurchaseNo,OperatorName,CompanyID,accountID,ProductModel,Price,Number,TotalPrice,CardTypeID,CardXTID,Pic,Remark,AddTime) " +
                        "values ('" + para.PurchaseNo + "','" + OperatorName + "','" + para.CompanyID + "','" + para.accountID + "','" + para.ProductModel + "'," + para.Price + "" +
                        "," + para.Number + "," + para.TotalPrice + "," + para.CardTypeID + "," + para.CardXTID + ",'" + para.Pic + "','" + para.Remark + "','" + DateTime.Now + "')";
                    conn.Execute(addpruchase);
                    info.flg = "1";
                    info.Msg = "添加成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "添加失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///修改采购单
        /// </summary>
        public Information UpdatePurchaseInfo(purchase para)
        {
            Information info = new Information();
            string PurchaseNo = string.Empty;
            string OperatorName = string.Empty;
            try
            {
                //判断合同编号是否重复
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string contractsql = "select PurchaseNo from purchase where Id=" + para.Id + " ";
                    PurchaseNo = conn.Query<purchase>(contractsql).Select(t => t.PurchaseNo).FirstOrDefault();
                    string sqlcom = "select accountName as OperatorName from accounts where Company_ID='" + para.CompanyID + "' and accountID='" + para.accountID + "'";
                    OperatorName = conn.Query<purchase>(sqlcom).Select(t => t.OperatorName).FirstOrDefault();
                    if (PurchaseNo == para.PurchaseNo)
                    {
                        string updateorder = "update purchase set PurchaseNo='" + para.PurchaseNo + "',OperatorName='" + OperatorName + "',accountID='" +para.accountID + "'," +
                            "ProductModel='" + para.ProductModel + "',Price='" +para.Price + "',Number='" + para.Number + "',TotalPrice='" +para.TotalPrice + "'," +
                            "CardTypeID='" + para.CardTypeID + "',CardXTID='" + para.CardXTID + "',Pic='" + para.Pic + "',Remark='" +para.Remark+ "' where Id=" + para.Id + " ";
                        conn.Execute(updateorder);
                        info.Msg = "修改信息成功!";
                        info.flg = "1";
                    }
                    if (PurchaseNo != para.PurchaseNo)
                    {
                        string contrac = "select PurchaseNo from purchase where PurchaseNo=" + para.PurchaseNo + " and status=1";
                        var listcontrac = conn.Query<contractorder>(contrac).ToList();
                        if (listcontrac.Count > 0)
                        {
                            info.Msg = "合同编号已经存在，请重新添加!";
                            info.flg = "-1";
                            return info;
                        }
                        else
                        {
                            string updateorder = "update purchase set PurchaseNo='" + para.PurchaseNo + "',OperatorName='" + OperatorName + "',accountID='" + para.accountID + "'," +
                             "ProductModel='" + para.ProductModel + "',Price='" + para.Price + "',Number='" + para.Number + "',TotalPrice='" + para.TotalPrice + "'," +
                             "CardTypeID='" + para.CardTypeID + "',CardXTID='" + para.CardXTID + "',Pic='" + para.Pic + "',Remark='" + para.Remark + "' where Id=" + para.Id + " ";
                            conn.Execute(updateorder);
                            info.Msg = "修改信息成功!";
                            info.flg = "1";
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

        ///<summary>
        ///删除采购单
        /// </summary>
        public Information DeletePurchaseInfo(int Id)
        {
            Information info = new Information();
            try
            {
                string sqldel = "update purchase set status=0 where Id=" + Id+"";
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
                info.Msg = "删除失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///查看采购单信息
        /// </summary>
        public PurchaseDto GetPurchaseInfo(string PurchaseNo, string CompanyID)
        {
            PurchaseDto dto = new PurchaseDto();
            string CardTypeName = string.Empty;
            string CardXTName = string.Empty;
            string ActivationDate=string.Empty ;
            string EndTimeDate = string.Empty; ;
            string activeandendtime = string.Empty;
            List<PurchaseInfos> purchases = new List<PurchaseInfos>();
            try
            {
                string sql = "select * from purchase where CompanyID='"+CompanyID+"' and status=1";
                if (!string.IsNullOrWhiteSpace(PurchaseNo))
                {
                    sql = sql + "  and PurchaseNo like '%" + PurchaseNo + "%'";
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var purchaselist = conn.Query<PurchaseInfos>(sql).ToList();
                    foreach (var item in purchaselist)
                    {
                        PurchaseInfos pur = new PurchaseInfos();
                        string typename = "select CardTypeName from cardtype  where CardTypeID='" + item.CardTypeID+"'";
                        CardTypeName = conn.Query<PurchaseInfos>(typename).Select(t => t.CardTypeName).FirstOrDefault();
                        string xtname = "select CardXTName from card_xingtai  where CardXTID='" + item.CardXTID + "'";
                        CardXTName = conn.Query<PurchaseInfos>(xtname).Select(t => t.CardXTName).FirstOrDefault();
                        if (CompanyID == "1556265186243")//奇迹物联
                        {
                            activeandendtime = "select date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate,date_format( Card_EndTime, '%Y-%m-%d %H:%i:%s') as Card_EndTime from card where PurchaseNo='" + item.PurchaseNo + "' and status=1";
                        }
                        else
                        {
                            activeandendtime = "select date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate,date_format( Card_EndTime, '%Y-%m-%d %H:%i:%s') as Card_EndTime from card_copy1 where PurchaseNo='" + item.PurchaseNo + "' and status=1";
                        }
                        var listtiem = conn.Query<PurchaseInfos>(activeandendtime).FirstOrDefault();
                        if (listtiem != null)
                        {
                            ActivationDate = listtiem.Card_ActivationDate;
                            EndTimeDate = listtiem.Card_EndTime;
                        }
                        pur.Id = item.Id;
                        pur.accountID = item.accountID;
                        pur.Number = item.Number;
                        pur.OperatorName = item.OperatorName;
                        pur.Pic = item.Pic;
                        pur.Price = item.Price;
                        pur.ProductModel = item.ProductModel;
                        pur.PurchaseNo = item.PurchaseNo;
                        pur.Remark = item.Remark;
                        pur.TotalPrice = item.TotalPrice;
                        pur.AddTime = item.AddTime;
                        pur.CardTypeName = CardTypeName;
                        pur.CardXTName = CardXTName;
                        pur.Card_ActivationDate = ActivationDate;
                        pur.Card_EndTime = EndTimeDate;
                        pur.CardTypeID = item.CardTypeID;
                        pur.CardXTID = item.CardXTID;
                        purchases.Add(pur);
                    }
                    dto.purchases = purchases;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }

        ///<summary>
        ///查看采购单详情
        /// </summary>
        public PurchaseDetailDto GetPurchaseDetaliInfo(string PurchaseNo)
        {
            PurchaseDetailDto dto = new PurchaseDetailDto();
            try
            {
                string sqlpur = "select CompanyID from purchase where PurchaseNo='"+PurchaseNo+"'";
                string sql = string.Empty; 
                string CompanyID = string.Empty;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    CompanyID = conn.Query<PurchaseInfos>(sqlpur).Select(t => t.CompanyID).FirstOrDefault();
                    if (CompanyID == "1556265186243")//奇迹物联
                    {
                        sql= "select Card_ICCID,Card_ID from card where PurchaseNo='" + PurchaseNo + "'";
                    }
                    else
                    {
                        sql = "select Card_ICCID,Card_ID from card_copy1 where PurchaseNo='" + PurchaseNo + "'";
                    }
                    dto.DetailInfos = conn.Query<PurchaseDetailInfo>(sql).ToList();
                    dto.flg = "1";
                    dto.Msg = "成功!";
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
    }
}
