using Dapper;
using Esim7.CMCC.CMCCDAL;
using Esim7.CMCC.CMCCModel;
using Esim7.CT;
using Esim7.CT.CTDAL;
using Esim7.CUCC.CUCCDAL;
using Esim7.CUCC.CUCCModel;
using Esim7.Dto;
using Esim7.Models;
using Esim7.UNity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Esim7.Action
{
    /// <summary>
    /// 三合一卡
    /// </summary>
    public class ThreeCardAction
    {
        ///<summary>
        ///入库上传上传 奇迹将卡上传至三合一表中
        /// </summary>
        public Information AddQjThreeCardInfo(ThreeCardStockPara para)
        {
            DateTime time = DateTime.Now;
            DateTime ActivateDate = para.ActivateDate.AddHours(8);
            DateTime EndDate = para.EndDate.AddHours(8);
            int cardnumber = para.ICCIDS.Count;//统计卡数量
            string companycardnum = string.Empty;
            string sqlcompanynum = string.Empty;//获取用户卡数量
            Information info = new Information();
            #region   判重   
            string BatchID = "ThreeRK" + DateTime.Now.ToString("yyyyMMdd:hhmmss");
            StringBuilder Sql_JudgeRepet = new StringBuilder("select  SN from product_threecard where SN in( ");
            int OperatorsFlg = 0;
            if (para.Platform == "10" || para.Platform == "11")
            {
                OperatorsFlg = 1;
            }
            if (para.Platform == "20" || para.Platform == "21")
            {
                OperatorsFlg = 2;
            }
            if (para.Platform == "30" || para.Platform == "31")
            {
                OperatorsFlg = 3;
            }
            if (para.Platform == "41" )
            {
                OperatorsFlg = 4;
            }
            foreach (CardSN item in para.ICCIDS)
            {
                Sql_JudgeRepet.Append("'" + item.SN + "',");
            }
            string sql_JD = Sql_JudgeRepet.ToString();
            sql_JD = sql_JD.Substring(0, sql_JD.Length - 1) + ")";
            List<CardSN> li = new List<CardSN>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                li = conn.Query<CardSN>(sql_JD).ToList();
            }
            #endregion
            if (li.Count != 0)
            {
                string s = "";
                foreach (CardSN item in li)
                {
                    s += item.SN + ",";
                }
                s = s.Substring(0, s.Length - 1);
                info.Msg= "数据重复:" + s;
                info.flg = "-1";
                return info;
            }
            else
            {
                var ICCID = para.ICCIDS.GroupBy(c => c.SN).Select(c => c.First());
                //表taocan 中的SetmaelID
                string SetmealID = Unit.GetTimeStamp(DateTime.Now);
                //添加到套餐  sql
                StringBuilder sql_Taocan = new StringBuilder("INSERT INTO taocan (SetmealID,testDate,silentDate,OpeningDate,OperatorsID,Remarks,SetmealID2)" +
                               "values('" + SetmealID + "','" + para.testDate + "','" + para.silentDate + "','" + para.OpeningDate + "','" + para.OperatorsID + "','" + para.Remarks + "','" + para.SetmealID2 + "')");
                var Card_ICCIDStart = para.ICCIDS.First().SN;
                var Card_ICCIDEND = para.ICCIDS.Last().SN;
                StringBuilder sql_Product_Excel = new StringBuilder("INSERT INTO product_ThreeCard  (SN,BatchID, AccountID, isout,AddTime)VALUES");
                StringBuilder Sql_Card = new StringBuilder("Insert into Three_Card (SN,pici,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,OperatorsFlg,AddTime,Card_ActivationDate,RenewDate,Card_EndTime,Card_Remarks)VALUES");
                //StringBuilder Sql_Card = new StringBuilder("Insert into Three_Card (SN,pici,accountsID,Card_CompanyID,status,Platform,SetMealID,SetMealID2,OperatorsFlg,AddTime)VALUES");
                StringBuilder Sql_fkthreecard = new StringBuilder("Insert into fk_threecard (SN,Card_CompanyID,CMCC_CardICCID,CUCC_CardICCID,CT_CardICCID,AddTime)VALUES");
                int i = 0;
                foreach (CardSN item in ICCID)
                {
                    sql_Product_Excel.Append("('" + item.SN + "','" + BatchID + "','" + para.AccountID + "','0','"+time+"'),");
                    Sql_Card.Append("('" + item.SN + "','" + BatchID + "','" + para.AccountID + "','1556265186243','1','" + para.Platform + "','" + SetmealID + "','" + para.SetmealID2 + "'," + OperatorsFlg + ",'" + time + "','" + ActivateDate + "','" + EndDate + "','" + EndDate + "','"+para.Remarks+"'),");
                    //Sql_Card.Append("('" + item.SN + "','" + BatchID + "','" + para.AccountID + "','1556265186243','1','" + para.Platform + "','" + SetmealID + "','" + para.SetmealID2 + "'," + OperatorsFlg + ",'" + time + "'),");
                    Sql_fkthreecard.Append("('" + item.SN + "','1556265186243','" + item.CMCC_ICCID + "','" + item.CUCC_ICCID + "','" + item.CT_ICCID + "','" + time + "'),");
                    i++;
                }
                StringBuilder sql_Product = new StringBuilder(@"INSERT INTO product  (SetMealID, StorageRoomID, ProductNumber, ICCIDStart, ICCIDEnd,  operatorID, batchID, Remark, AccountID, isExcel, ProductNumbers,AddTime)
                             VALUES(" + "'" + SetmealID + "','" + para.StorageRoomID + "'," + i + ",'" + Card_ICCIDStart + "','" + Card_ICCIDEND + "','" + para.operatorID + "','" + BatchID + "','" + para.Remarks + "','" + para.AccountID + "',1 ," + ICCID.ToList().Count + ",'"+time+"') ");
                string sql_PE = sql_Product_Excel.ToString();
                sql_PE = sql_PE.Substring(0, sql_PE.Length - 1);
                string Sql_CD = Sql_Card.ToString();
                Sql_CD = Sql_CD.Substring(0, Sql_CD.Length - 1);
                string Sql_fkthree = Sql_fkthreecard.ToString();
                Sql_fkthree = Sql_fkthree.Substring(0,Sql_fkthreecard.Length-1);
                using (IDbConnection connss = DapperService.MySqlConnection())
                {
                    try
                    {
                        string sql_pr = sql_Product.ToString();
                        string sql_tao = sql_Taocan.ToString();
                        var InnerTaocan = connss.Execute(sql_Taocan.ToString()); //向套餐详细信息log添加
                        var INnerProduct = connss.Execute(sql_Product.ToString());  //向库存log中添加
                        var InnerProdoct_excel = connss.Execute(sql_PE);  //向库存添加
                        var InnerCard = connss.Execute(Sql_CD);    //向奇迹三合一卡表中添加
                        var innerfkthreecard = connss.Execute(Sql_fkthree);//向三合一卡表关联表中添加
                        sqlcompanynum = "select CompanyTolCardNum as Number from company where CompanyID='1556265186243'";//获取用户卡数量
                        companycardnum = connss.Query<Company>(sqlcompanynum).Select(t => t.Number).FirstOrDefault();//获取数量
                        cardnumber = Convert.ToInt32(companycardnum) + cardnumber;
                        //修改用户卡数据
                        string updateCompanyCardNmber = "update company set CompanyTolCardNum=" + cardnumber + " where CompanyID='1556265186243'";
                        connss.Execute(updateCompanyCardNmber);//修改奇迹物联卡数量
                        info.Msg = "成功";
                        info.flg = "1";
                    }
                    catch (Exception ex)
                    {
                        StringBuilder RollBACK_sql_Taocan = new StringBuilder("delete from taocan  where SetmealID='" + SetmealID + "' ");
                        StringBuilder RollBACK_sql_Product = new StringBuilder("delete from product where SetmealID='" + SetmealID + "' ");
                        StringBuilder RollBACK_sql_product_threecard = new StringBuilder("delete from product_threecard where BatchID='" + BatchID + "' ");
                        StringBuilder RollBACK_Sql_three_card = new StringBuilder("delete from three_card where AddTime='" + time + "' ");
                        StringBuilder RollBACK_Sql_fk_threecard = new StringBuilder("delete from fk_threecard where AddTime='" + time + "' ");
                        connss.Execute(RollBACK_sql_Taocan.ToString());
                        connss.Execute(RollBACK_sql_Product.ToString());
                        connss.Execute(RollBACK_sql_product_threecard.ToString());
                        connss.Execute(RollBACK_Sql_three_card.ToString());
                        connss.Execute(RollBACK_Sql_fk_threecard.ToString());
                        info.Msg = "出现错误请联系管理人员" + "";
                        Console.WriteLine("ERROR：" + ex.ToString());
                        info.flg = "-1";
                        throw ex;
                    }
                }
               
            }
            return info;
        }

        ///<summary>
        ///修改三合一卡套餐
        /// </summary>
        public Information UpdateThreeCardSetmalInfo(UpdateThreeCardSetmeal para)
        {
            Information info = new Information();
            try
            {
                StringBuilder UpdatePackAge = new StringBuilder(" update  three_card set SetMealID2='" + para.SetmealID + "' where SN in(");
                StringBuilder UpdatePackAgecopy = new StringBuilder(" update  three_cardcopy set SetMealID2='" + para.SetmealID + "' where SN in(");
                foreach (ThreeCardCopySN item in para.Cards)
                {
                    UpdatePackAge.Append("'" + item.SN + "',");
                    UpdatePackAgecopy.Append("'" + item.SN + "',");
                }
                string Sql = UpdatePackAge.ToString();
                string Sqlcopy = UpdatePackAgecopy.ToString();
                Sql = Sql.Substring(0, Sql.Length - 1) + ")";
                Sqlcopy = Sqlcopy.Substring(0, Sqlcopy.Length - 1) + ")";
                using (IDbConnection connss = DapperService.MySqlConnection())
                {
                    connss.Execute(Sql);
                    connss.Execute(Sqlcopy);
                    info.flg = "1";
                    info.Msg = "成功";
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
        ///给用户分配三合一卡
        /// </summary>
        public Information ThreeCardToCustom(ThreeCardCopyPara para)
        {
            Information info = new Information();
            DateTime time = DateTime.Now;
            //Card_ActivationDate,Card_Remarks,,Card_totalflow,pici,,CopyID,SetMealID2,OperatorsFlg,accountsID
            string Card_Remarks=string.Empty;
            string Card_totalflow = string.Empty;
            string pici = string.Empty;
            string SetMealID2 = string.Empty;
            int OperatorsFlg = 0;
            string accountsID = string.Empty;
            string sqlThreeCard = "select * from three_card where SN='"+para.Cards[0].SN+"'";
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
                DateTime ActivateDate = para.ActivateDate.AddHours(8);
                DateTime EndDate = para.EndDate.AddHours(8);
                StringBuilder Sql_JudgeRepet = new StringBuilder("select  SN from Three_CardCopy where SN in( ");
                para.Cards.RemoveAll(x => x.SN == "" || x.SN == null);
                Molde_ForCustom f = new Molde_ForCustom();
                List<Three_Card> List_Card = new List<Three_Card>();
                string Company = para.CompanyID;
                foreach (ThreeCardCopySN item in para.Cards)
                {
                    Sql_JudgeRepet.Append("'" + item.SN + "',");
                }
                string CopyID = Unit.GetTimeStamp(DateTime.Now);
                string sql_JD = Sql_JudgeRepet.ToString();
                sql_JD = sql_JD.Substring(0, sql_JD.Length - 1) + ")";
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
                        s += item.SN + ",";
                    }
                    s = s.Substring(0, s.Length - 1);
                    info.Msg = "数据重复:" + s;
                    info.flg = "-1";
                    return info;
                }
                else
                {
                    //将信息添加到用户三合一卡表中
                    StringBuilder InnerThreeCardCopy = new StringBuilder("insert into three_cardcopy(SN,Card_OpenDate,Card_ActivationDate,Card_CompanyID,Card_totalflow," +
                        "pici,Card_EndTime,CopyID,SetMealID2,OperatorsFlg,accountsID,AddTime)  values");
                    //更新奇迹三合一卡copyid
                    StringBuilder UpdateThreeCard = new StringBuilder("update three_card set CopyID='"+CopyID+ "' where SN in (");
                    foreach(var items in para.Cards)
                    {
                        InnerThreeCardCopy.Append("('"+items.SN+"','"+para.ActivateDate+"','"+para.ActivateDate+"','"+para.CompanyID+"','"+Card_totalflow+"','"+pici+"'," +
                            "'"+para.EndDate+"','"+CopyID+"','"+SetMealID2+"','"+OperatorsFlg+"','"+accountsID+"','"+time+"'),");
                        UpdateThreeCard.Append("'" + items.SN + "',");
                    }
                    string threecardcopy = InnerThreeCardCopy.ToString().Substring(0, InnerThreeCardCopy.ToString().Length - 1);
                    string updatethreecard = UpdateThreeCard.ToString().Substring(0, UpdateThreeCard.ToString().Length - 1) + ")";
                    try
                    {
                        using (IDbConnection Conn = DapperService.MySqlConnection())
                        {
                            Conn.Execute(threecardcopy);
                            Conn.Execute(updatethreecard);
                            info.flg = "1";
                            info.Msg = "分配成功!";
                        }
                    }
                    catch (Exception ex)
                    {
                        string delthreecardcopy = "delete three_cardcopy where AddTime='"+time+"'";
                        using (IDbConnection Conn = DapperService.MySqlConnection()) 
                        {
                            Conn.Execute(delthreecardcopy);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "给用户分配卡出错:"+ex;
            }
            return info;
        }

        ///<summary>
        ///查看三合一卡信息(用户和奇迹物联共用)
        /// </summary>
        public ThreeCardInfoDto GetThreeCardInfo(GetThreeCardPara para)
        {
            ThreeCardInfoDto dto = new ThreeCardInfoDto();
            string sqlthreecard = "";
            string sql = "";
            string s = " limit " + (para.PagNumber - 1) * para.Num + "," + para.Num;
            try
            {
                if (para.Company_ID == "1556265186243")//奇迹物联
                {
                    sqlthreecard = "select t1.SN,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,date_format(t1.RenewDate, '%Y-%m-%d') as RenewDate,date_format(t1.Card_EndTime,'%Y-%m-%d') as Card_EndTime,t2.PackageDescribe as SetmealName,CONVERT(t1.Card_totalflow,DECIMAL(9,2)) as Card_totalflow," +
                       "t1.Card_Remarks,t3.CardTypeName as Card_Type,date_format(t1.Card_ActivationDate,'%Y-%m-%d') as Card_ActivationDate,date_format(t1.CustomerActivationDate,'%Y-%m-%d') as CustomerActivationDate," +
                       "date_format(t1.CustomerEndTime,'%Y-%m-%d') as CustomerEndTime,t1.CustomerCompany,t1.AddTime from three_card t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID" +
                       " left join cardType t3 on t2.CardTypeID = t3.CardTypeID where t1.Card_CompanyID = '" + para.Company_ID + "' and t1.status=1 ";
                    sql = "select t1.SN,t1.CustomerCompany from three_card t1 where t1.Card_CompanyID = '" + para.Company_ID + "' and t1.status=1 ";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        if (conn.Query<Three_Card>(sql).ToList().Count > 0)
                        {
                            dto.Total = conn.Query<ThreeCardInfo>(sql).ToList().Count;
                            if (!string.IsNullOrWhiteSpace(para.CustomerCompany))
                            {
                                sql = sql + "and t1.CustomerCompany like '%" + para.CustomerCompany + "%'";
                                sqlthreecard = sqlthreecard + "and t1.CustomerCompany like '%" + para.CustomerCompany + "%' ";
                                dto.Cards = conn.Query<ThreeCardInfo>(sqlthreecard).ToList().OrderByDescending(t => t.AddTime).ToList();
                                dto.Total = conn.Query<ThreeCardInfo>(sql).ToList().Where(t => t.CustomerCompany.Contains(para.CustomerCompany)).ToList().Count;
                            }
                            if (!string.IsNullOrWhiteSpace(para.SN))
                            {
                                if (para.SN.Length != 16)
                                {
                                    string sqlsn = "select SN from fk_threecard where CMCC_CardICCID='" + para.SN + "' || CUCC_CardICCID='" + para.SN + "' || CT_CardICCID='" + para.SN + "'";
                                    para.SN = conn.Query<GetThreeCardPara>(sqlsn).Select(t => t.SN).FirstOrDefault();
                                }
                                sqlthreecard = sqlthreecard + "and t1.SN like '%" + para.SN + "%' ";
                                dto.Cards = conn.Query<ThreeCardInfo>(sqlthreecard).ToList().OrderByDescending(t => t.AddTime).ToList();
                                dto.Total = conn.Query<ThreeCardInfo>(sql).ToList().Where(t => t.SN.Contains(para.SN)).ToList().Count;
                            }
                            if (string.IsNullOrWhiteSpace(para.CustomerCompany) && string.IsNullOrWhiteSpace(para.CustomerCompany))
                            {
                                sqlthreecard = sqlthreecard + s;
                                dto.Cards = conn.Query<ThreeCardInfo>(sqlthreecard).ToList().OrderByDescending(t => t.AddTime).ToList();
                            }
                            if (!string.IsNullOrWhiteSpace(para.CustomerCompany) || !string.IsNullOrWhiteSpace(para.CustomerCompany))
                            {
                                sqlthreecard = sqlthreecard + s;
                                dto.Cards = conn.Query<ThreeCardInfo>(sqlthreecard).ToList().OrderByDescending(t => t.AddTime).ToList();
                            }
                        }
                    }
                    dto.Total = dto.Total;
                    dto.flg = "1";
                    dto.Msg = "查询成功!";
                }
                else
                {
                    sqlthreecard = "select t1.SN,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,date_format(t1.Card_EndTime,'%Y-%m-%d') as RenewDate,date_format(t1.Card_EndTime,'%Y-%m-%d') as Card_EndTime,t2.PackageDescribe as SetmealName,CONVERT(t1.Card_totalflow,DECIMAL(9,2)) as Card_totalflow," +
                         "t1.Card_Remarks,t3.CardTypeName as Card_Type,date_format(t1.Card_ActivationDate,'%Y-%m-%d') as Card_ActivationDate,t1.AddTime from three_cardcopy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID" +
                         " left join cardType t3 on t2.CardTypeID = t3.CardTypeID where t1.Card_CompanyID = '" + para.Company_ID + "' and t1.status=1 " ;
                    sql = "select t1.SN,date_format(t1.Card_OpenDate, '%Y-%m-%d') as Card_OpenDate,date_format(t1.Card_EndTime,'%Y-%m-%d') as RenewDate,date_format(t1.Card_EndTime,'%Y-%m-%d') as Card_EndTime,t2.PackageDescribe as SetmealName,CONVERT(t1.Card_totalflow,DECIMAL(9,2)) as Card_totalflow," +
                         "t1.Card_Remarks,t3.CardTypeName as Card_Type,date_format(t1.Card_ActivationDate,'%Y-%m-%d') as Card_ActivationDate from three_cardcopy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID" +
                         " left join cardType t3 on t2.CardTypeID = t3.CardTypeID where t1.Card_CompanyID = '" + para.Company_ID + "' and t1.status=1 ";
                    string sqlss= " select * from three_cardcopy where Card_CompanyID = '" + para.Company_ID+ "' and status=1 ";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        if (conn.Query<Three_Card>(sqlss).ToList().Count > 0)
                        {
                            string sqlthrcard = sqlthreecard + s;
                            dto.Cards = conn.Query<ThreeCardInfo>(sqlthrcard).ToList().OrderByDescending(t => t.AddTime).ToList();
                            dto.Total = conn.Query<ThreeCardInfo>(sqlss).ToList().Count;
                            //if (!string.IsNullOrWhiteSpace(para.CardRemark))
                            //{
                            //    dto.Cards = conn.Query<ThreeCardInfo>(sqlthreecard).ToList().Where(t => t.SN.Contains(para.CardRemark)).ToList().OrderByDescending(t => t.AddTime).ToList();
                            //    dto.Total = conn.Query<ThreeCardInfo>(sql).ToList().Where(t => t.Card_Remarks.Contains(para.CardRemark)).ToList().Count;
                            //}
                            if (!string.IsNullOrWhiteSpace(para.SN))
                            {
                                if (para.SN.Length != 16)
                                {
                                    string sqlsn = "select SN from fk_threecard where CMCC_CardICCID='" + para.SN + "' || CUCC_CardICCID='" + para.SN + "' || CT_CardICCID='" + para.SN + "'";
                                    para.SN = conn.Query<GetThreeCardPara>(sqlsn).Select(t => t.SN).FirstOrDefault();
                                }
                                sqlthreecard += "and t1.SN like'%" + para.SN + "%' " +s;
                                dto.Cards = conn.Query<ThreeCardInfo>(sqlthreecard).ToList().Where(t => t.SN.Contains(para.SN)).ToList().OrderByDescending(t => t.AddTime).ToList();
                                //dto.Cards = conn.Query<ThreeCardInfo>(sqlthreecard).ToList().Where(t => t.SN.Contains(para.SN)).ToList().OrderByDescending(t => t.AddTime).ToList();
                                dto.Total = conn.Query<ThreeCardInfo>(sql).ToList().Where(t => t.SN.Contains(para.SN)).ToList().Count;
                            }
                        }
                        else
                        {
                            dto.Cards = null;
                            dto.Total = 0;
                        }
                    }
                    dto.flg = "1";
                    dto.Msg = "查询成功!";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "查询失败:"+ex;
            }
            return dto;
        }

        ///<summary>
        ///查看三合一卡详细信息
        /// </summary>
        public ThreeDetail GetThreeCardDetailInfo(string SN)
        {
            ThreeDetail t = new ThreeDetail();
            CTAPIDAL ct = new CTAPIDAL();
            CUCCAPIDAL cucc = new CUCCAPIDAL();
            CMCCAPIDAL cmcc = new CMCCAPIDAL();
            RootCTStatus rt = new RootCTStatus();//电信卡状态接收
            RootCTCardWorkStatus wt = new RootCTCardWorkStatus();//电信卡工作状态接收
            CuccCardWorkStatus ws = new CuccCardWorkStatus();//联通卡工作状态接收
            CUCCCardStatus cs = new CUCCCardStatus();//联通卡状态接收
            CuccCardFlow cf = new CuccCardFlow();//联通卡流量接收
            CMCCRootToken crt = new CMCCRootToken();//移动token接收
            NewCMCCCardStatus ncs = new NewCMCCCardStatus();//移动新平台卡状态接收
            NewCMCCCardWorkStatus nws = new NewCMCCCardWorkStatus();//移动新平台卡工作状态接收
            NewCMCCCardMonthFlow nwf = new NewCMCCCardMonthFlow();//移动卡新平台卡月使用流量
            OldCmccCardStatus ocs = new OldCmccCardStatus();//移动老平台卡状态接收
            OldCmccCardWorkStatus ocw = new OldCmccCardWorkStatus();//移动老平台卡工作状态
            OldCmccCardMonthFlow ocf = new OldCmccCardMonthFlow();//移动老平台卡月使用流量
            string CMCCICCID = string.Empty;
            string CUCCICCID = string.Empty;
            string CTICCID = string.Empty;
            ThreeCardInfos info = new ThreeCardInfos();
            List<ThreeCardInfos> data = new List<ThreeCardInfos>();
            try
            {
                string sqlthreecardiccid = "select  CMCC_CardICCID,CUCC_CardICCID,CT_CardICCID from fk_threecard where SN='"+SN+"'";//获取三个卡的iccid
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var threeiccid = conn.Query<ThreeCardInfos>(sqlthreecardiccid).FirstOrDefault();
                    if (threeiccid != null)
                    {
                        info.CMCC_CardICCID = threeiccid.CMCC_CardICCID;
                        info.CUCC_CardICCID = threeiccid.CUCC_CardICCID;
                        info.CT_CardICCID = threeiccid.CT_CardICCID;
                        if (!string.IsNullOrWhiteSpace(threeiccid.CT_CardICCID))//电信卡信息
                        {
                            //获取电信卡号
                            info.CT_CardId = ct.GetCTCard_ID(threeiccid.CT_CardICCID);

                            //获取电信卡状态
                            string CT_CardStatusName = ct.GetStatusName(info.CT_CardId); //1：可激活 2：测试激活 3:测试去激活 4:在用 5:停机 6:运营商状态管理
                            if (!string.IsNullOrWhiteSpace(CT_CardStatusName))
                            {
                                info.CT_CardState = CT_CardStatusName;
                            }
                            else
                            {
                                info.CT_CardState = "暂未查询到状态";
                            }
                            //获取电信卡工作状态
                            wt = ct.GetCTCardWorkStatus(threeiccid.CT_CardICCID);
                            if (wt.resultCode == "0")
                            {
                                threeiccid.CT_WorkState = wt.description.result;
                                if (threeiccid.CT_WorkState == "0")
                                {
                                    info.CT_WorkState = "在线";
                                }
                                if (threeiccid.CT_WorkState == "-1")
                                {
                                    info.CT_WorkState = "离线";
                                }
                                if (threeiccid.CT_WorkState == "-2")
                                {
                                    info.CT_WorkState = "未查询到会话信息";
                                }
                                
                            }
                            else
                            {
                                info.CT_WorkState = "未查询到会话信息";
                            }
                            
                            info.CT_Flow = ct.GetCuccCardMonthFlow(threeiccid.CT_CardICCID);//电信卡月使用流量
                        }
                        if (!string.IsNullOrWhiteSpace(threeiccid.CUCC_CardICCID))//联通卡信息
                        {

                            ws = cucc.GetCuccCardWorkStatus(threeiccid.CUCC_CardICCID);//联通卡工作状态
                            if (ws != null)
                            {
                                if (string.IsNullOrWhiteSpace(ws.dateSessionStarted))
                                {
                                    info.CUCC_WorkState = "离线";
                                }
                                if (!string.IsNullOrWhiteSpace(ws.dateSessionStarted))
                                {
                                    info.CUCC_WorkState = "在线";
                                }
                            }
                            else
                            {
                                info.CUCC_WorkState = "暂时未查询到卡的工作状态信息";
                            }
                            cs = cucc.CuccCardStatus(threeiccid.CUCC_CardICCID);//联通卡状态
                            if (cs != null)
                            {
                                if (cs.status == "TEST_READY")
                                {
                                    info.CUCC_CardState = "可测试";
                                }
                                if (cs.status == "ACTIVATED")
                                {
                                    info.CUCC_CardState = "已激活";
                                }
                                if (cs.status == "ACTIVATION_READY")
                                {
                                    info.CUCC_CardState = "可激活";
                                }
                                if (cs.status == "DEACTIVATED")
                                {
                                    info.CUCC_CardState = "已停用";
                                }
                                if (cs.status == "INVENTORY")
                                {
                                    info.CUCC_CardState = "库存";
                                }
                                if (cs.status == "PURGED")
                                {
                                    info.CUCC_CardState = "已清除";
                                }
                                if (cs.status == "RETIRED")
                                {
                                    info.CUCC_CardState = "已注销";
                                }
                                info.CUCC_CardId = cs.msisdn;//联通卡号
                            }
                            else
                            {
                                info.CUCC_CardState = "暂未查询到卡状态信息";
                                info.CUCC_CardId = "暂未查询到卡号";//联通卡号
                            }
                            cf = cucc.GetCuccCardFlow(threeiccid.CUCC_CardICCID);
                            if (cf != null)
                            {
                                decimal ctdDataUsage = 0;
                                decimal cuccflow = 0;
                                ctdDataUsage = cf.ctdDataUsage;
                                cuccflow = Math.Round(ctdDataUsage / 1024, 3);
                                cuccflow = Math.Round(cuccflow / 1024, 3);
                                info.CUCC_Flow = cuccflow.ToString() + "MB";
                            }
                            else
                            {
                                info.CUCC_Flow = "暂未查询到流量信息";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(threeiccid.CMCC_CardICCID))//移动卡信息
                        {
                            //移动卡
                            string sqlplatform = "select Platform,Card_ID from card where Card_ICCID='" + threeiccid.CMCC_CardICCID + "'";
                            string sqlaccountsid = "select accountsID from card where Card_ICCID='" + threeiccid.CMCC_CardICCID + "'";
                            string Platform = conn.Query<Card>(sqlplatform).Select(s => s.Platform).FirstOrDefault();
                            string accountsID = conn.Query<Card2>(sqlaccountsid).Select(s => s.accountsID).FirstOrDefault();
                            string sqlaccounts = "select * from accounts where accountID='" + accountsID + "'";
                            var sqlaccountinfo = conn.Query<accounts>(sqlaccounts).FirstOrDefault();
                            string token = string.Empty;
                            string appid = string.Empty;
                            info.CMCC_CardId = conn.Query<Card>(sqlplatform).Select(a => a.Card_ID).FirstOrDefault();
                            if (Platform == "10")
                            {
                                string EBID;
                                if (sqlaccountinfo != null)
                                {
                                    EBID = "0001000000009";//卡的状态  00-正常；01-单向停机；02-停机；03-预销号；04-销号；05-过户；06-休眠；07-待激活；99-号码不存在
                                    ocs = cmcc.GetOldCmccCardStatus(sqlaccountinfo.APPID, sqlaccountinfo.TRANSID, EBID, sqlaccountinfo.TOKEN, threeiccid.CMCC_CardICCID);
                                    if (ocs.status == "0")
                                    {
                                        if (ocs.result[0].STATUS == "00")
                                        {
                                            info.CMCC_CardState = "正常";
                                        }
                                        if (ocs.result[0].STATUS == "01")
                                        {
                                            info.CMCC_CardState = "单项停机";
                                        }
                                        if (ocs.result[0].STATUS == "02")
                                        {
                                            info.CMCC_CardState = "停机";
                                        }
                                        if (ocs.result[0].STATUS == "03")
                                        {
                                            info.CMCC_CardState = "预销号";
                                        }
                                        if (ocs.result[0].STATUS == "04")
                                        {
                                            info.CMCC_CardState = "销号";
                                        }
                                        if (ocs.result[0].STATUS == "05")
                                        {
                                            info.CMCC_CardState = "过户";
                                        }
                                        if (ocs.result[0].STATUS == "06")
                                        {
                                            info.CMCC_CardState = "休眠";
                                        }
                                        if (ocs.result[0].STATUS == "07")
                                        {
                                            info.CMCC_CardState = "待激活";
                                        }
                                        if (ocs.result[0].STATUS == "99")
                                        {
                                            info.CMCC_CardState = "号码不存在";
                                        }
                                    }
                                    EBID = "0001000000008";//卡的工作状态
                                    ocw = cmcc.GetOldCmccCardWorkStatus(sqlaccountinfo.APPID, sqlaccountinfo.TRANSID, EBID, sqlaccountinfo.TOKEN, threeiccid.CMCC_CardICCID);
                                    if (ocw.status == "0")
                                    {
                                        if (ocw.result[0].GPRSSTATUS == "00")
                                        {
                                            info.CMCC_WorkState = "在线";
                                        }
                                        if (ocw.result[0].GPRSSTATUS == "01")
                                        {
                                            info.CMCC_WorkState = "离线";
                                        }
                                    }
                                    EBID = "0001000000012";//卡的月流量
                                    ocf = cmcc.GetOldCmccCardMonthFlow(sqlaccountinfo.APPID, sqlaccountinfo.TRANSID, EBID, sqlaccountinfo.TOKEN, threeiccid.CMCC_CardICCID);
                                    if (ocf.status == "0")
                                    {
                                        decimal oldflow = Convert.ToDecimal(ocf.result[0].total_gprs);
                                        oldflow = Math.Round(oldflow / 1024, 2);
                                        info.CMCC_Flow = oldflow.ToString() + "MB";
                                    }

                                }
                            }
                            if (Platform == "11")
                            {
                                crt = cmcc.GetToken(threeiccid.CMCC_CardICCID);//新平台移动卡的状态
                                if (crt.status == "0")
                                {
                                    token = crt.result[0].token;
                                }
                                if (sqlaccountinfo != null)
                                {
                                    appid = sqlaccountinfo.APPID;
                                }
                                ncs = cmcc.GetNewCmccCardStatus(token, appid, threeiccid.CMCC_CardICCID);
                                if (ncs.status == "0")//移动新平台卡状态
                                {
                                    if (ncs.result[0].cardStatus == "1")
                                    {
                                        info.CMCC_CardState = "待激活";
                                    }
                                    if (ncs.result[0].cardStatus == "2")
                                    {
                                        info.CMCC_CardState = "已激活";
                                    }
                                    if (ncs.result[0].cardStatus == "4")
                                    {
                                        info.CMCC_CardState = "停机";
                                    }
                                    if (ncs.result[0].cardStatus == "6")
                                    {
                                        info.CMCC_CardState = "可测试";
                                    }
                                    if (ncs.result[0].cardStatus == "7")
                                    {
                                        info.CMCC_CardState = "库存";
                                    }
                                    if (ncs.result[0].cardStatus == "8")
                                    {
                                        info.CMCC_CardState = "预销户";
                                    }
                                    if (ncs.result[0].cardStatus == "9")
                                    {
                                        info.CMCC_CardState = "已销户";
                                    }
                                }
                                crt = cmcc.GetToken(threeiccid.CMCC_CardICCID);//新平台移动卡的工作状态
                                if (crt.status == "0")
                                {
                                    token = crt.result[0].token;
                                }
                                nws = cmcc.GetNewCmccCardWrokStatus(token, appid, threeiccid.CMCC_CardICCID);
                                if (nws.status == "0")
                                {
                                    if (nws.result[0].simSessionList[0].status == "00")
                                    {
                                        info.CMCC_WorkState = "离线";
                                    }
                                    if (nws.result[0].simSessionList[0].status == "01")
                                    {
                                        info.CMCC_WorkState = "在线";
                                    }
                                }
                                crt = cmcc.GetToken(threeiccid.CMCC_CardICCID);//新平台移动卡的工作状态
                                if (crt.status == "0")
                                {
                                    token = crt.result[0].token;
                                }
                                nwf = cmcc.GetNewCmccCardMonthFlow(token, appid, threeiccid.CMCC_CardICCID);
                                if (nwf.status == "0")
                                {
                                    decimal newcuccflow = Convert.ToDecimal(nwf.result[0].dataAmount);
                                    newcuccflow = Math.Round(newcuccflow / 1024, 2);
                                    info.CMCC_Flow = newcuccflow.ToString() + "MB";
                                }
                            }
                        }
                    }
                    data.Add(info);
                    t.Data = data;
                    t.flg = "1";
                    t.Msg = "查询成功!";
                }               
            }
            catch (Exception ex)
            {
                t.flg = "-1";
                t.Msg = "查询失败!";
            }
            return t;
        }

        public string  sss(duibijihepara para)
        {
            string value = "";
            string sqlcard = "select Card_ICCID from card";
            List<string> listcard = new List<string>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                listcard = conn.Query<Card>(sqlcard).Select(t=>t.Card_ICCID).ToList();
                
                if (para.iccids.Count > 0)
                {
                    foreach (var item in para.iccids)
                    {
                        
                        if (!listcard.Contains(item.ICCID))
                        {
                            //IWorkbook workbook = new HSSFWorkbook();
                            ////创建工作表
                            //ISheet sheet = workbook.CreateSheet("sss1");
                            //IRow row0 = sheet.CreateRow(0);
                            //row0.CreateCell(0).SetCellValue("ICCID");
                            ////创建行row
                            //IRow row = sheet.CreateRow(i);
                            //i++;
                            //row.CreateCell(0).SetCellValue(item.ICCID);
                            value += item.ICCID + ",";
                        }
                        
                    }
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var s = value.Substring(0, value.Length - 1).Split(',');
                        if (s.Length > 0)
                        {
                            //创建工作簿对象
                            IWorkbook workbook = new HSSFWorkbook();
                            //创建工作表
                            ISheet sheet = workbook.CreateSheet("onesheet");
                            IRow row0 = sheet.CreateRow(0);
                            row0.CreateCell(0).SetCellValue("ICCID");
                            for (int i = 0; i < s.Length; i++)
                            {
                                //创建行row
                                int j = i + 1;
                                IRow row = sheet.CreateRow(j);
                                row.CreateCell(0).SetCellValue(s[i]);

                            }
                            string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xlsx";
                            string FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                            using (FileStream url = File.OpenWrite(FilePath))
                            {
                                //导出Excel文件
                                workbook.Write(url);
                            };
                        }
                    }
                }
            }               
             return value;
        }

        ///<summary>
        ///返回数据layui测试
        /// </summary>
        public TestLayuiModel testlayui(string Card_CompanyID, int PagNumber, int Num)
        {
            TestLayuiModel c = new TestLayuiModel();
            List<Infoqqq> li = new List<Infoqqq>();
            string s = "";
            if (Card_CompanyID == "1556265186243")
            {
                s = " ";
                s += " limit " + (PagNumber - 1) * Num + "," + Num;

                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI, (case t1.Card_State when '00' then '正常' else '其他' end  ) as Card_State,
                                (case t1.Card_WorkState when '01' then '在线' else '离线' end  ) as Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate    ,t5.OperatorName as operatorsName
                                from card   t1 left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status=1" + s;


                    Infoqqq card = new Infoqqq();
                    li = conn.Query<Infoqqq>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList();
                }
                int i = 0;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @" SELECT   id   from card t1 where t1.status=1  ";
                    i = conn.Query<string>(sql2).ToList().Count;
                }
                c.code = 0;
                c.data = li;
            }
            return c;
        }

        ///<summary>
        ///导出三合一卡信息
        /// </summary>
        public FilesPath ThreeExportData(string Card_CompanyID)
        {
            FilesPath filesPath = new FilesPath();
            string FileName = string.Empty;
            string FilePath = string.Empty;
            string sqlthreecard = string.Empty;
            Card_API c = new Card_API();
            List<Card> li = new List<Card>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                if (Card_CompanyID == "1556265186243")
                {
                    sqlthreecard = "select t1.SN,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,t1.RenewDate,date_format(t1.Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,t2.PackageDescribe as SetmealName,CONVERT(t1.Card_totalflow,DECIMAL(9,2)) as Card_totalflow," +
                          "t1.Card_Remarks,t3.CardTypeName as cardType,date_format(t1.Card_ActivationDate,'%Y-%m-%d %H:%i:%s') as Card_ActivationDate,date_format(t1.CustomerActivationDate,'%Y-%m-%d %H:%i:%s') as CustomerActivationDate," +
                          "date_format(t1.CustomerEndTime,'%Y-%m-%d %H:%i:%s') as CustomerEndTime,t4.CMCC_CardICCID,t1.CustomerCompany,t4.CUCC_CardICCID,t4.CT_CardICCID from three_card t1 left join fk_threecard t4 on t1.SN=t4.SN left join setmeal t2 on t1.SetMealID2 = t2.SetmealID" +
                          " left join cardType t3 on t2.CardTypeID = t3.CardTypeID where t1.Card_CompanyID = '" + Card_CompanyID + "' and t1.status=1";
                    li = conn.Query<Card>(sqlthreecard).ToList();
                    //创建工作簿对象
                    IWorkbook workbook = new HSSFWorkbook();
                    //创建工作表
                    ISheet sheet = workbook.CreateSheet("onesheet");
                    IRow row0 = sheet.CreateRow(0);
                    row0.CreateCell(0).SetCellValue("SN");
                    row0.CreateCell(1).SetCellValue("移动卡ICCID");
                    row0.CreateCell(2).SetCellValue("电信卡ICCID");
                    row0.CreateCell(3).SetCellValue("联通卡ICCID");
                    row0.CreateCell(4).SetCellValue("开户日期");
                    row0.CreateCell(5).SetCellValue("续费日期");
                    row0.CreateCell(6).SetCellValue("客户开户日期");
                    row0.CreateCell(7).SetCellValue("客户续费日期");
                    row0.CreateCell(8).SetCellValue("客户名称");
                    row0.CreateCell(9).SetCellValue("套餐名称");
                    row0.CreateCell(10).SetCellValue("卡类型");
                    row0.CreateCell(11).SetCellValue("使用流量");
                    row0.CreateCell(12).SetCellValue("备注");
                    int count = li.Count + 1;
                    for (int r = 1; r < count; r++)
                    {
                        //创建行row
                        IRow row = sheet.CreateRow(r);
                        row.CreateCell(0).SetCellValue(li[r - 1].SN);
                        row.CreateCell(1).SetCellValue(li[r - 1].CMCC_CardICCID);
                        row.CreateCell(2).SetCellValue(li[r - 1].CT_CardICCID.Substring(0,19));
                        row.CreateCell(3).SetCellValue(li[r - 1].CUCC_CardICCID);
                        row.CreateCell(4).SetCellValue(li[r - 1].Card_ActivationDate);
                        row.CreateCell(5).SetCellValue(li[r - 1].Card_EndTime);
                        row.CreateCell(6).SetCellValue(li[r - 1].CustomerActivationDate);
                        row.CreateCell(7).SetCellValue(li[r - 1].CustomerEndTime);
                        row.CreateCell(8).SetCellValue(li[r - 1].CustomerCompany);
                        row.CreateCell(9).SetCellValue(li[r - 1].SetmealName);
                        row.CreateCell(10).SetCellValue(li[r - 1].cardType);
                        row.CreateCell(11).SetCellValue(li[r - 1].Card_totalflow);
                        row.CreateCell(12).SetCellValue(li[r - 1].Card_Remarks);
                    }
                    //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
                    //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
                    //自动生成文件名称
                    FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
                    FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                    //FilePath=@"D:Files/" + FileName;
                    using (FileStream url = File.OpenWrite(FilePath))
                    {
                        //导出Excel文件
                        workbook.Write(url);
                    };
                    //ProcessRequest1(li);
                    filesPath.Path = FilePath;
                    filesPath.Flage = "1";
                    filesPath.Message= "Success";
                }
                else
                {
                    sqlthreecard = "select t1.SN,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,t1.RenewDate,date_format(t1.Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,t2.PackageDescribe as SetmealName,CONVERT(t1.Card_totalflow,DECIMAL(9,2)) as Card_totalflow," +
                         "t1.Card_Remarks,t3.CardTypeName as cardType,date_format(t1.Card_ActivationDate,'%Y-%m-%d %H:%i:%s') as Card_ActivationDate,t4.CMCC_CardICCID,t4.CUCC_CardICCID,t4.CT_CardICCID" +
                         " from three_cardcopy t1  left join fk_threecard t4 on t1.SN=t4.SN  left join setmeal t2 on t1.SetMealID2 = t2.SetmealID" +
                         " left join cardType t3 on t2.CardTypeID = t3.CardTypeID where t1.Card_CompanyID = '" + Card_CompanyID+ "' and t1.status=1";
                    li = conn.Query<Card>(sqlthreecard).ToList();
                    //创建工作簿对象
                    IWorkbook workbook = new HSSFWorkbook();
                    //创建工作表
                    ISheet sheet = workbook.CreateSheet("onesheet");
                    IRow row0 = sheet.CreateRow(0);
                    row0.CreateCell(0).SetCellValue("SN");
                    row0.CreateCell(1).SetCellValue("移动卡ICCID");
                    row0.CreateCell(2).SetCellValue("电信卡ICCID");
                    row0.CreateCell(3).SetCellValue("联通卡ICCID");
                    row0.CreateCell(4).SetCellValue("开户日期");
                    row0.CreateCell(5).SetCellValue("续费日期");
                    row0.CreateCell(6).SetCellValue("套餐名称");
                    row0.CreateCell(7).SetCellValue("卡类型");
                    row0.CreateCell(8).SetCellValue("使用流量");
                    row0.CreateCell(9).SetCellValue("备注");
                    int count = li.Count + 1;
                    for (int r = 1; r < count; r++)
                    {
                        //创建行row
                        IRow row = sheet.CreateRow(r);
                        row.CreateCell(0).SetCellValue(li[r - 1].SN);
                        row.CreateCell(1).SetCellValue(li[r - 1].CMCC_CardICCID);
                        row.CreateCell(2).SetCellValue(li[r - 1].CT_CardICCID.Substring(0,19));
                        row.CreateCell(3).SetCellValue(li[r - 1].CUCC_CardICCID);
                        row.CreateCell(4).SetCellValue(li[r - 1].Card_ActivationDate);
                        row.CreateCell(5).SetCellValue(li[r - 1].Card_EndTime);
                        row.CreateCell(6).SetCellValue(li[r - 1].SetmealName);
                        row.CreateCell(7).SetCellValue(li[r - 1].cardType);
                        row.CreateCell(8).SetCellValue(li[r - 1].Card_totalflow);
                        row.CreateCell(9).SetCellValue(li[r - 1].Card_Remarks);
                    }
                    //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
                    //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
                    //自动生成文件名称
                    FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
                    FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                    //FilePath=@"D:Files/" + FileName;
                    using (FileStream url = File.OpenWrite(FilePath))
                    {
                        //导出Excel文件
                        workbook.Write(url);
                    };
                    filesPath.Path = FilePath;
                    filesPath.Flage = "1";
                    filesPath.Message = "Success";
                }
            }
            return filesPath;
        }
    }
}