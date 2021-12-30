using Dapper;
using Esim7.CMCC.CMCCDAL;
using Esim7.CMCC.CMCCModel;
using Esim7.CT;
using Esim7.CT.CTDAL;
using Esim7.CUCC.CUCCDAL;
using Esim7.CUCC.CUCCModel;
using Esim7.Dto;
using Esim7.IOT.IOTModel;
using Esim7.LargeFlow.LargeFlowDAL;
using Esim7.LargeFlow.LargeFlowModel;
using Esim7.Models;
using Esim7.Models.APIModel;
using Esim7.Models.Para;
using Esim7.UNity;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using static Esim7.Models.APIModel.CmccApiModel;
using static Esim7.SimModel.API;

namespace Esim7.Action
{
    public class CardAction
    {       
        /// <summary>
        /// 查询卡接口 各种类型  单卡查询
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Card_CompanyID"></param>
        /// <returns></returns>
        public static List<Card> GetCards(string value, string Card_CompanyID)
        {
            List<Card> card = new List<Card>();
            int where = value.Length;
            //string s = "";
            //if (value.Length != 20)
            //{
            //    return card;
            //}           
            //switch (where)
            //{

            //    case 15:
            //        s = " and t1.Card_IMSI=" + "'" + value + "'";
            //        break;
            //    case 16:
            //        s = " and  t1.Card_IMEI=" + "'" + value + "'";
            //        break;
            //    case 20:
            //        s = " and  t1.Card_ICCID=" + "'" + value + "'";
            //        break;
            //    case 13:
            //        s = "and   t1.Card_ID=" + "'" + value + "'";
            //        break;

            //    default:
            //        s = "";
            //        break;

            //}
            string sss = " t1.Card_CompanyID=" + Card_CompanyID;
            string s2 = "";
            switch (where)
            {
                case 15:
                    s2 = sss + "  and t1.Card_IMSI='" + value + "'" + "  or  t1.Card_IMEI='" + value + "'";
                    break;
                case 20:
                    s2 = sss + " and  t1.Card_ICCID='" + value + "'"; ;
                    break;
                case 13:
                    s2 = sss + " and   t1.Card_ID='" + value + "'"; ;
                    break;
                default:
                    s2 = "";
                    break;
            }
            //string sql2 = "";
            // List<Card> li = new List<Card>();
            // List<CardAndAPI> Li2 = new List<CardAndAPI>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                if(Card_CompanyID == "1556265186243")
                {
                    #region 之前的sql
//                    sql2 = @"SELECT  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,
//t1.Card_IMSI,t1.Card_Type,
//t1.Card_State,t1.Card_WorkState,  
//date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
//date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
//t1.Card_Remarks  as Card_Remrarks, t1.status, t1.Card_CompanyID
//,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate
//from
//card t1
//LEFT JOIN accounts t2 on
//t2.accountID = t1.accountsID

//left join  operators t3 on t3.operatorsID = t2.operatorsID
//left join outofstock t4 on t4.OutofstockID = t1.OutofstockID

//left join taocan t5 on t5.SetMealID = t4.SetMealID
//left join cardtype t6 on t6.CardTypeID = t5.CardTypeID
//left join card_xingtai t7 on t7.CardXTID = t5.CardxingtaiID

//left join city t8 on t8.cityID = t2.cityID

// where t1.status = 1 and  " + s2;
                    #endregion
                    string sql3 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState, date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate,t5.OperatorName as operatorsName
                                from card   t1 left join accounts t6 on  t6.AccountID=t1.accountsID left  join setmeal t2 on t2.SetmealID=t1.setmealId2
                                left join cardtype t3 on t3.CardTypeID=t2.CardTypeID left join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left  join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1 and " + s2;
                    return conn.Query<Card>(sql3).ToList();
                }
                else
                {
                    #region old sql
//                    sql2 = @"SELECT  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,
//t1.Card_IMSI,t1.Card_Type,
//t1.Card_State,t1.Card_WorkState,  
//date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
//date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
//t1.Card_Remarks  as  Card_Remrarks, t1.status, t1.Card_CompanyID
//,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate
//from
//card_copy1 t1
//LEFT JOIN accounts t2 on
//t2.accountID = t1.accountsID

//left join  operators t3 on t3.operatorsID = t2.operatorsID
//left join outofstock t4 on t4.OutofstockID = t1.OutofstockID

//left join taocan t5 on t5.SetMealID = t4.SetMealID
//left join cardtype t6 on t6.CardTypeID = t5.CardTypeID
//left join card_xingtai t7 on t7.CardXTID = t5.CardxingtaiID

//left join city t8 on t8.cityID = t2.cityID

// where  " + s2;

                    #endregion
                    string sql3 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate   ,t5.OperatorName as operatorsName
                                from card_copy1  t1 left join accounts t6 on  t6.AccountID=t1.accountsID left  join setmeal t2 on t2.SetmealID=t1.setmealId2
                                left join cardtype t3 on t3.CardTypeID=t2.CardTypeID left join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left  join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1 and " + s2;
                    return conn.Query<Card>(sql3).ToList();
                }
            }
        }
        /// <summary>
        /// 查询接口没有参数
        /// </summary>
        /// <returns></returns>
        public static List<Card> GetCards2()
        {
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = "select Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate,date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card";
                Card card = new Card();
                List<Card> li = new List<Card>();
                return conn.Query<Card>(sql2).ToList();
            }
        }
        /// <summary>
        /// 查询卡数量 按公司ID
        /// </summary>
        /// <param name="Card_CompanyID">公司ID</param>
        /// <param name="PagNumber">页数</param>
        /// <param name="Num">每页数量</param>
        /// <returns></returns>
        public static Card_API GetCardsForCompany(string Card_CompanyID, int PagNumber, int Num)
        { 
            Card_API c = new Card_API();
            List<Card> li = new List<Card>();
            string s = "";
            if (Card_CompanyID == "1556265186243")
            {
                s = " ";
                s += " limit " + (PagNumber - 1) * Num + "," + Num;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate    ,t5.OperatorName as operatorsName
                                from card   t1 left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status=1" + s;
                    Card card = new Card();  
                    li = conn.Query<Card>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList(); 
                }
                #region 注释

                //#region 获取开卡日期
                //Root_CMIOT_API2110 root_CMIOT_API2110 = new Root_CMIOT_API2110();
                //List<ResultItem_CMIOT_API2110> list2110 = new List<ResultItem_CMIOT_API2110>();
                //root_CMIOT_API2110 = APIACTION.GetCMIOT_API2110(item.Card_IMSI);


                //list2110 = root_CMIOT_API2110.result;

                //foreach (ResultItem_CMIOT_API2110 items in list2110)
                //{
                //    item.Card_Opendate = items.openTime;

                //}

                //#endregion

                //#region 使用流量
                //Root_CMIOT_API2005 root_CMIOT_API2020 = new Root_CMIOT_API2005();

                //List<ResultItem_CMIOT_API2005> list2020 = new List<ResultItem_CMIOT_API2005>();
                //root_CMIOT_API2020 = APIACTION.Get_CMIOT_API2005(item.Card_IMSI);

                //list2020 = root_CMIOT_API2020.result;


                //foreach (ResultItem_CMIOT_API2005 items in list2020)
                //{
                //   // item.Card_Monthlyusageflow = (double.Parse(items.total_gprs)/1024).ToString();

                //    item.Card_Monthlyusageflow = items.total_gprs;
                //}

                //#endregion


                //#region  获取工作状态   00-离线       01 - 在线


                //Root_CMIOT_API12001 root_CMIOT_API12001 = new Root_CMIOT_API12001();
                //List<CMIOT_API12001> List2001 = new List<CMIOT_API12001>();


                //root_CMIOT_API12001 = APIACTION.GetCMIOT_API12001(item.Card_IMSI);
                //List2001 = root_CMIOT_API12001.result;
                //foreach (CMIOT_API12001 items in List2001)
                //{

                //    item.Card_WorkState = items.GPRSSTATUS;



                //}

                //#endregion


                //#region 获取物联网卡状态
                //List<ResultItem_CMIOT_API2002> list2002 = new List<ResultItem_CMIOT_API2002>();
                //Root_CMIOT_API2002 root_CMIOT_API2002 = new Root_CMIOT_API2002();
                //root_CMIOT_API2002 = APIACTION.GetCMIOT_API2002(item.Card_IMSI);
                //list2002 = root_CMIOT_API2002.result;
                //foreach (ResultItem_CMIOT_API2002 items in list2002)
                //{
                //    item.Card_State = items.STATUS;
                //}

                #endregion
                int i = 0;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {  
                    string sql2 = @" SELECT   id   from card t1 where t1.status=1  ";                   
                    i = conn.Query<string>(sql2).ToList().Count;
                }
                c.total = i.ToString();
                c.CardAndAPI = li;
            }
            else
            {
                // s= "and Card_CompanyID=@Card_CompanyID  limit " + num + ",20";
                s = "and t7.Card_CompanyID=@Card_CompanyID    ";
                s += " limit " + (PagNumber - 1) * Num + "," + Num;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                    string sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState,  date_format(t7.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
                                date_format(t7.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate , t7.AssignType,
                                t7.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t7.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate   ,t5.OperatorName as operatorsName
                                from card   t1 inner join  card_copy1  t7 on t7.Card_ICCID=t1.Card_ICCID left  join accounts t6 on  t6.AccountID=t1.accountsID 
                                left   join setmeal t2 on t2.SetmealID=t1.setmealId2 left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID
                                left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID left join operator t5 on t5.OperatorID=t2.OperatorID where t7.status=1   " + s;
                    Card card = new Card();
                    //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
                    li = conn.Query<Card>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList();
                }
                s = "and Card_CompanyID=@Card_CompanyID     ";
                int i = 0;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                    string sql2 = @"SELECT   id from card_copy1 t1 where t1.status=1 " + s;
                    //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
                    i = conn.Query<string>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList().Count;
                }
                c.total = i.ToString();
                c.CardAndAPI = li;
            }
            return c;
        }
        ///<summary>
        ///OperatorsFlg 1：移动 2：电信 3：联通
        /// </summary>

        //public static FilesPath ExportData(string Card_CompanyID,string OperatorsFlg)
        //{
        //    FilesPath filesPath = new FilesPath();
        //    string FileName = string.Empty;
        //    int LastNumber = 0;
        //    int Number = 1100000000;
        //    Card_API c = new Card_API();
        //    List<Card> li = new List<Card>();
        //    int TotalNum = 0;//总条数
        //    int ExportNum = 0;//导出的条数
        //    string s = "";
        //    string sql2 = string.Empty;
        //    int i = 0;
        //    if (Card_CompanyID == "1556265186243")
        //    {
        //        using (IDbConnection conn = DapperService.MySqlConnection())
        //        {
        //            if (OperatorsFlg == "1" || string.IsNullOrWhiteSpace(OperatorsFlg))//移动
        //            {
        //                sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,
        //                        t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState, date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
        //                        date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
        //                        t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
        //                        ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate    ,t5.OperatorName as operatorsName
        //                        from card   t1 left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
        //                        left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
        //                        left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status=1";// + s;
        //            }
        //            if (OperatorsFlg == "2")//电信
        //            {
        //                sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,
        //                        t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState, date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
        //                        date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
        //                        t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
        //                        ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate    ,t5.OperatorName as operatorsName
        //                        from ct_card   t1 left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
        //                        left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
        //                        left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status=1";// + s;
        //            }
        //            if (OperatorsFlg == "3")//联通
        //            {
        //                sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,
        //                        t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState, date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
        //                        date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
        //                        t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
        //                        ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate    ,t5.OperatorName as operatorsName
        //                        from cucc_card   t1 left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
        //                        left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
        //                        left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status=1";// + s;
        //            }
        //            Card card = new Card();
        //            li = conn.Query<Card>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList();
        //            i = li.Count;
        //        }
        //        c.total = i.ToString();
        //        TotalNum = Convert.ToInt32(c.total);
        //        ExportNum = 1 * Number;
        //        LastNumber = TotalNum - ExportNum;
        //        //if (TotalNum > ExportNum)
        //        //{
        //        //    c.ExportState = true;
        //        //}
        //        //if (LastNumber <= Number)
        //        //{
        //        //    c.ExportState = false;
        //        //}
        //        c.CardAndAPI = li;
        //        ProcessRequest(li);
        //    }
        //    else
        //    {
        //        s = "and t1.Card_CompanyID=@Card_CompanyID    ";
        //        using (IDbConnection conn = DapperService.MySqlConnection())
        //        {
        //            if (OperatorsFlg == "1" || string.IsNullOrWhiteSpace(OperatorsFlg))//移动
        //            {
        //                sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
        //                        t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,        
        //                        date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
        //                        t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
        //                        ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate   ,t5.OperatorName as operatorsName
        //                        from card_copy1   t1 left join  accounts t6 on  t6.AccountID=t1.accountsID 
        //                        left   join setmeal t2 on t2.SetmealID=t1.setmealId2  left join cardtype t3 on t3.CardTypeID=t2.CardTypeID
        //                        left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID left join operator t5 on t5.OperatorID=t2.OperatorID where t1.status=1   " + s;
        //            }
        //            if (OperatorsFlg == "2")//电信
        //            {
        //                sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
        //                        t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,        
        //                        date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
        //                        t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
        //                        ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate   ,t5.OperatorName as operatorsName
        //                        from ct_cardcopy   t1 left join  accounts t6 on  t6.AccountID=t1.accountsID 
        //                        left   join setmeal t2 on t2.SetmealID=t1.setmealId2  left join cardtype t3 on t3.CardTypeID=t2.CardTypeID
        //                        left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID left join operator t5 on t5.OperatorID=t2.OperatorID where t1.status=1   " + s;
        //            }
        //            if (OperatorsFlg == "3")//联通
        //            {
        //                sql2 = @"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
        //                        t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,        
        //                        date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
        //                        t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
        //                        ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate   ,t5.OperatorName as operatorsName
        //                        from cucc_cardcopy   t1 left join  accounts t6 on  t6.AccountID=t1.accountsID 
        //                        left   join setmeal t2 on t2.SetmealID=t1.setmealId2  left join cardtype t3 on t3.CardTypeID=t2.CardTypeID
        //                        left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID left join operator t5 on t5.OperatorID=t2.OperatorID where t1.status=1   " + s;
        //            }
        //            //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                    
        //            Card card = new Card();
        //            li = conn.Query<Card>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList();
        //            i = li.Count;
        //        }
        //        c.total = i.ToString();
        //        TotalNum = Convert.ToInt32(c.total);
        //        ExportNum = 1 * Number;
        //        LastNumber = TotalNum - ExportNum;
        //        //if (TotalNum > ExportNum)
        //        //{
        //        //    c.ExportState = true;
        //        //}
        //        //if (LastNumber <= Number)
        //        //{
        //        //    c.ExportState = false;
        //        //}
        //        c.CardAndAPI = li;
        //        ProcessRequest(li);
        //    }
        //    filesPath.Path = ProcessRequest(li);
        //    return filesPath;
        //}

        /// <summary>
        /// 写入excel
        /// </summary>
        //public static string ProcessRequest(List<Card> cards)
        //{
        //    var FilePath = "";
        //    string Card_State = string.Empty;
        //    string Card_WorkState = string.Empty;
        //    List<Card> listUser = cards;
        //    if (cards.Count <= 65535)
        //    {
        //        //创建工作簿对象
        //        IWorkbook workbook = new HSSFWorkbook();
        //        //创建工作表
        //        ISheet sheet = workbook.CreateSheet("onesheet");
        //        IRow row0 = sheet.CreateRow(0);
        //        row0.CreateCell(0).SetCellValue("物联卡号");
        //        row0.CreateCell(1).SetCellValue("ICCID");
        //        row0.CreateCell(2).SetCellValue("IMEI");
        //        row0.CreateCell(3).SetCellValue("开户日期");
        //        row0.CreateCell(4).SetCellValue("续费日期");
        //        row0.CreateCell(5).SetCellValue("卡类型");
        //        row0.CreateCell(6).SetCellValue("运营商");
        //        row0.CreateCell(7).SetCellValue("套餐名称");
        //        row0.CreateCell(8).SetCellValue("卡状态");
        //        row0.CreateCell(9).SetCellValue("工作状态");
        //        row0.CreateCell(10).SetCellValue("备注");
        //        row0.CreateCell(11).SetCellValue("使用流量");
        //        row0.CreateCell(12).SetCellValue("IMSI");
        //        int count = listUser.Count + 1;
        //        for (int r = 1; r < count; r++)
        //        {
        //            if (listUser[r - 1].Card_State == "00")
        //            {
        //                Card_State = "正常";
        //            }
        //            if (listUser[r - 1].Card_State == "01")
        //            {
        //                Card_State = "单项停机";
        //            }
        //            if (listUser[r - 1].Card_State == "02" || listUser[r-1].Card_State=="4")
        //            {
        //                Card_State = "停机";
        //            }
        //            if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
        //            {
        //                Card_State = "预销户";
        //            }
        //            if (listUser[r - 1].Card_State == "1")
        //            {
        //                Card_State = "待激活";
        //            }
        //            if (listUser[r - 1].Card_State == "2")
        //            {
        //                Card_State = "已激活";
        //            }
        //            if (listUser[r - 1].Card_State == "6")
        //            {
        //                Card_State = "可测试";
        //            }
        //            if (listUser[r - 1].Card_State == "7")
        //            {
        //                Card_State = "库存";
        //            }
        //            if (listUser[r - 1].Card_WorkState == "00")
        //            {
        //                Card_WorkState = "离线";
        //            }
        //            if (listUser[r - 1].Card_WorkState == "01")
        //            {
        //                Card_WorkState = "在线";
        //            }
        //            //创建行row
        //            IRow row = sheet.CreateRow(r);
        //            row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
        //            row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
        //            row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
        //            row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
        //            row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
        //            row.CreateCell(5).SetCellValue(listUser[r - 1].CardTypeName);
        //            row.CreateCell(6).SetCellValue(listUser[r - 1].operatorsName);
        //            row.CreateCell(7).SetCellValue(listUser[r - 1].SetmealName);
        //            row.CreateCell(8).SetCellValue(Card_State);
        //            row.CreateCell(9).SetCellValue(Card_WorkState);
        //            row.CreateCell(10).SetCellValue(listUser[r - 1].Card_Remarks);
        //            if (!string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
        //            {
        //                decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
        //                decimal flows = s / 1024;
        //                decimal flowmb = Math.Round(flows, 2);
        //                string strflownum = flowmb.ToString();
        //                row.CreateCell(11).SetCellValue(strflownum);
        //            }
        //            if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
        //            {
        //                row.CreateCell(11).SetCellValue("0");
        //            }
        //            row.CreateCell(12).SetCellValue(listUser[r - 1].Card_IMSI);
        //        }
        //        //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
        //        //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
        //        //自动生成文件名称
        //        string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
        //        FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
        //        //FilePath=@"D:Files/" + FileName;
        //        using (FileStream url = File.OpenWrite(FilePath))
        //        {
        //            //导出Excel文件
        //            workbook.Write(url);
        //        };
        //    }
        //    else
        //    {
        //        //创建工作簿对象
        //        IWorkbook workbook = new HSSFWorkbook();
        //        int sheetnum = (cards.Count / 65535) + 1;
        //        for (int i = 0; i < sheetnum; i++)
        //        {
        //            //创建工作表
        //            if (i == 0)
        //            {
        //                ISheet sheet = workbook.CreateSheet("sheet1");
        //                IRow row0 = sheet.CreateRow(0);
        //                row0.CreateCell(0).SetCellValue("物联卡号");
        //                row0.CreateCell(1).SetCellValue("ICCID");
        //                row0.CreateCell(2).SetCellValue("IMEI");
        //                row0.CreateCell(3).SetCellValue("开户日期");
        //                row0.CreateCell(4).SetCellValue("续费日期");
        //                row0.CreateCell(5).SetCellValue("卡类型");
        //                row0.CreateCell(6).SetCellValue("运营商");
        //                row0.CreateCell(7).SetCellValue("套餐名称");
        //                row0.CreateCell(8).SetCellValue("卡状态");
        //                row0.CreateCell(9).SetCellValue("工作状态");
        //                row0.CreateCell(10).SetCellValue("备注");
        //                row0.CreateCell(11).SetCellValue("使用流量");
        //                row0.CreateCell(12).SetCellValue("IMSI");
        //                for (int r = 1; r < 65535; r++)
        //                {
        //                    if (listUser[r - 1].Card_State == "00")
        //                    {
        //                        Card_State = "正常";
        //                    }
        //                    if (listUser[r - 1].Card_State == "01")
        //                    {
        //                        Card_State = "单项停机";
        //                    }
        //                    if (listUser[r - 1].Card_State == "02" || listUser[r - 1].Card_State == "4")
        //                    {
        //                        Card_State = "停机";
        //                    }
        //                    if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
        //                    {
        //                        Card_State = "预销户";
        //                    }
        //                    if (listUser[r - 1].Card_State == "1")
        //                    {
        //                        Card_State = "待激活";
        //                    }
        //                    if (listUser[r - 1].Card_State == "2")
        //                    {
        //                        Card_State = "已激活";
        //                    }
        //                    if (listUser[r - 1].Card_State == "6")
        //                    {
        //                        Card_State = "可测试";
        //                    }
        //                    if (listUser[r - 1].Card_State == "7")
        //                    {
        //                        Card_State = "库存";
        //                    }
        //                    if (listUser[r - 1].Card_WorkState == "00")
        //                    {
        //                        Card_WorkState = "离线";
        //                    }
        //                    if (listUser[r - 1].Card_WorkState == "01")
        //                    {
        //                        Card_WorkState = "在线";
        //                    }
        //                    //创建行row
        //                    IRow row = sheet.CreateRow(r);
        //                    row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
        //                    row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
        //                    row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
        //                    row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
        //                    row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
        //                    row.CreateCell(5).SetCellValue(listUser[r - 1].CardTypeName);
        //                    row.CreateCell(6).SetCellValue(listUser[r - 1].operatorsName);
        //                    row.CreateCell(7).SetCellValue(listUser[r - 1].SetmealName);
        //                    row.CreateCell(8).SetCellValue(Card_State);
        //                    row.CreateCell(9).SetCellValue(Card_WorkState);
        //                    row.CreateCell(10).SetCellValue(listUser[r - 1].Card_Remarks);
        //                    if (!string.IsNullOrWhiteSpace(listUser[r-1].Card_Monthlyusageflow))
        //                    {
        //                        int flownum = int.Parse(listUser[r - 1].Card_Monthlyusageflow);
        //                        decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
        //                        decimal flows = s / 1024;
        //                        decimal flowmb = Math.Round(flows, 2);
        //                        string strflownum = flowmb.ToString();
        //                        row.CreateCell(11).SetCellValue(strflownum);
        //                    }
        //                    if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
        //                    {
        //                        row.CreateCell(11).SetCellValue("0");
        //                    }
        //                    row.CreateCell(12).SetCellValue(listUser[r - 1].Card_IMSI);
        //                }
        //            }
        //                if (i > 0)
        //                {
        //                    int j = i + 1;
        //                    string name = "sheet" + j.ToString();
        //                    ISheet sheets = workbook.CreateSheet(name);
        //                    IRow row0 = sheets.CreateRow(1);
        //                    row0.CreateCell(0).SetCellValue("物联卡号");
        //                    row0.CreateCell(1).SetCellValue("ICCID");
        //                    row0.CreateCell(2).SetCellValue("IMEI");
        //                    row0.CreateCell(3).SetCellValue("开户日期");
        //                    row0.CreateCell(4).SetCellValue("续费日期");
        //                    row0.CreateCell(5).SetCellValue("卡类型");
        //                    row0.CreateCell(6).SetCellValue("运营商");
        //                    row0.CreateCell(7).SetCellValue("套餐名称");
        //                    row0.CreateCell(8).SetCellValue("卡状态");
        //                    row0.CreateCell(9).SetCellValue("工作状态");
        //                    row0.CreateCell(10).SetCellValue("备注");
        //                    row0.CreateCell(11).SetCellValue("使用流量");
        //                    row0.CreateCell(12).SetCellValue("IMSI");
        //                listUser = cards.ToList().Skip(65534).ToList();
        //                //for (int r = 1; r <cards.Count()-65535; r++)
        //                for (int r = 1; r < listUser.Count() - 1; r++)
        //                {
        //                    if (listUser[r - 1].Card_State == "00")
        //                    {
        //                        Card_State = "正常";
        //                    }
        //                    if (listUser[r - 1].Card_State == "01")
        //                    {
        //                        Card_State = "单项停机";
        //                    }
        //                    if (listUser[r - 1].Card_State == "02" || listUser[r - 1].Card_State == "4")
        //                    {
        //                        Card_State = "停机";
        //                    }
        //                    if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
        //                    {
        //                        Card_State = "预销户";
        //                    }
        //                    if (listUser[r - 1].Card_State == "1")
        //                    {
        //                        Card_State = "待激活";
        //                    }
        //                    if (listUser[r - 1].Card_State == "2")
        //                    {
        //                        Card_State = "已激活";
        //                    }
        //                    if (listUser[r - 1].Card_State == "6")
        //                    {
        //                        Card_State = "可测试";
        //                    }
        //                    if (listUser[r - 1].Card_State == "7")
        //                    {
        //                        Card_State = "库存";
        //                    }
        //                    if (listUser[r - 1].Card_WorkState == "00")
        //                    {
        //                        Card_WorkState = "离线";
        //                    }
        //                    if (listUser[r - 1].Card_WorkState == "01")
        //                    {
        //                        Card_WorkState = "在线";
        //                    }
        //                    //创建行row
        //                    IRow row = sheets.CreateRow(r);
        //                    row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
        //                    row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
        //                    row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
        //                    row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
        //                    row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
        //                    row.CreateCell(5).SetCellValue(listUser[r - 1].CardTypeName);
        //                    row.CreateCell(6).SetCellValue(listUser[r - 1].operatorsName);
        //                    row.CreateCell(7).SetCellValue(listUser[r - 1].SetmealName);
        //                    row.CreateCell(8).SetCellValue(Card_State);
        //                    row.CreateCell(9).SetCellValue(Card_WorkState);
        //                    row.CreateCell(10).SetCellValue(listUser[r - 1].Card_Remarks);
        //                    if (!string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
        //                    {
        //                        int flownum = int.Parse(listUser[r - 1].Card_Monthlyusageflow);
        //                        decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
        //                        decimal flows = s / 1024;
        //                        decimal flowmb = Math.Round(flows, 2);
        //                        string strflownum = flowmb.ToString();
        //                        row.CreateCell(11).SetCellValue(strflownum);
        //                    }
        //                    if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
        //                    {
        //                        row.CreateCell(11).SetCellValue("0");
        //                    }
        //                    row.CreateCell(12).SetCellValue(listUser[r - 1].Card_IMSI);
        //                }
        //            }                    
        //        }
        //        //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
        //        //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
        //        //自动生成文件名称
        //           string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
        //            FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
        //           // FilePath = @"D:Files/" + FileName;
        //        using (FileStream url = File.OpenWrite(FilePath))
        //        {
        //                //导出Excel文件
        //                workbook.Write(url);
        //        };                
        //    }
        //    return FilePath;
        //}

        public class Message
        {
           
           public string ICCID { get; set; }
            public string IMEI { get; set; }
            public string SN { get; set; }
            //public string Card_ID { get; set; }
            //public string Card_CompanyID { get; set; }
            //public string accountsID { get; set; }
            //public string SetmealID { get; set; }
            //public decimal YearFlow { get; set; }
        }
        public class Cards
        {
            public string OperatorsFlg { get; set; }
            public string CompanyID { get; set; }
            public List<Message> List_Cards { get; set; }
            
        }
        //public static Card_API2 Query_Piliang()
        //{
        //    Card_API2 c = new Card_API2();
        //    List<Card> li = new List<Card>();
        //    try
        //    {
        //        Cards list = new Cards();
        //        HttpRequest request = HttpContext.Current.Request;
        //        Stream postData = request.InputStream;
        //        StreamReader sRead = new StreamReader(postData);
        //        string postContent = sRead.ReadToEnd();
        //        sRead.Close();
        //        list = JsonConvert.DeserializeObject<Cards>(postContent);
        //        List<string> ll = new List<string>();
        //        string striccid = string.Empty;
        //        using (IDbConnection conn = DapperService.MySqlConnection())
        //        {
        //            foreach (Message item in list.List_Cards)
        //            {
        //                decimal flow = 0;
        //                flow = item.YearFlow * 1024;
        //                string add = "insert into  cmccyearflow (Card_ID,Card_ICCID,Card_CompanyID,Platform,SetmealID,accountsID,Years,Months,YearFlow,SetmealType,UpdateTime) " +
        //                    "values('"+item.Card_ID+"','"+item.ICCID+"','"+item.Card_CompanyID+"','11','"+item.SetmealID+"','"+item.accountsID+"','"+DateTime.Now.Year+"','"+DateTime.Now.Month+"',"+item.YearFlow+",'1','"+DateTime.Now+"') ";
        //                conn.Execute(add);
        //                //string sqlupdate = "update cmccyearflow set YearFlow=" + flow + " where UpdateTime < '2021-07-30 21:00:00' and Card_ICCID='" + item.ICCID + "'";
        //                //conn.Execute(sqlupdate);
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return c;
        //}

        public static Card_API2 Query_Piliang()
        {
            Card_API2 c = new Card_API2();
            List<Card> li = new List<Card>();
            try
            {
                Cards list = new Cards();
                HttpRequest request = HttpContext.Current.Request;
                Stream postData = request.InputStream;
                StreamReader sRead = new StreamReader(postData);
                string postContent = sRead.ReadToEnd();
                sRead.Close();
                list = JsonConvert.DeserializeObject<Cards>(postContent);
                List<string> ll = new List<string>();
                string striccid = string.Empty;
                foreach (Message item in list.List_Cards)
                {
                    if (item.ICCID != null)
                    {
                        item.ICCID = Regex.Replace(item.ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                        ll.Add(item.ICCID);
                        striccid += "'" + item.ICCID + "',";
                    }
                    if (item.IMEI != null)
                    {
                        item.IMEI = Regex.Replace(item.IMEI, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                        ll.Add(item.IMEI);
                    }
                    if (item.SN != null)
                    {
                        item.SN = Regex.Replace(item.SN, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                        ll.Add(item.SN);
                    }
                }
                //string sss = striccid.Substring(0, striccid.Length - 1);
                //c.Message = sss;
                if (list.List_Cards.Count <= 20000)
                {
                    if (list.CompanyID == "1556265186243")
                    {
                        if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    from card t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                                    left join outofstock t4 on  t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID
                                    left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID
                                    where t1.status=1  and (Card_ICCID in @Card_ICCID  or card_id in @Card_ICCID or Card_IMSI in @Card_ICCID or Card_IMEI in @Card_ICCID )";
                                c.Cards = conn.Query<Card>(sql2, new { Card_ICCID = ll }).ToList();
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success"+ striccid;
                                c.status = "1";
                            }
                        }
                        if (list.OperatorsFlg == "2")//电信
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    from ct_card t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                                    left join outofstock t4 on  t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID
                                    left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID
                                    where t1.status=1  and (Card_ICCID in @Card_ICCID  or card_id in @Card_ICCID or Card_IMSI in @Card_ICCID or Card_IMEI in @Card_ICCID )";
                                c.Cards = conn.Query<Card>(sql2, new { Card_ICCID = ll }).ToList();
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "1";
                            }
                        }
                        if (list.OperatorsFlg == "3")//联通
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    from cucc_card t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                                    left join outofstock t4 on  t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID
                                    left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID
                                    where t1.status=1  and (Card_ICCID in @Card_ICCID  or card_id in @Card_ICCID or Card_IMSI in @Card_ICCID or Card_IMEI in @Card_ICCID )";
                                c.Cards = conn.Query<Card>(sql2, new { Card_ICCID = ll }).ToList();
         
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "1";
                            }
                        }
                        if (list.OperatorsFlg == "4")//全网通卡
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string sql2 = @"select t1.SN,t2.Flow,t2.CardXTID,t1.Card_Remarks,t2.PackageDescribe,t2.CardTypeID,t3.CardTypeName,t4.CardXTName,date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate,
                                               t1.RenewDate from three_card t1 left join setmeal t2 on t1.SetMealID2=t2.SetmealID left join cardtype t3 
                                               on t3.CardTypeID=t2.CardTypeID left join card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                               where (SN in @Card_ICCID)";
                                c.Cards = conn.Query<Card>(sql2, new { Card_ICCID = ll }).ToList();
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "1";
                            }
                        }
                        if (list.OperatorsFlg == "5")//漫游卡
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    from roamcard t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                                    left join outofstock t4 on  t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID
                                    left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID
                                    where t1.status=1  and (Card_ICCID in @Card_ICCID  or card_id in @Card_ICCID or Card_IMSI in @Card_ICCID or Card_IMEI in @Card_ICCID )";
                                //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
                                c.Cards = conn.Query<Card>(sql2, new { Card_ICCID = ll }).ToList();
                                foreach (var item in c.Cards)
                                {
                                    Card card = new Card();
                                    card.cardType = item.cardType;
                                    card.CardTypeName = item.CardTypeName;
                                    card.Card_ID = item.Card_ID;
                                    card.Card_State = item.Card_State;
                                    card.Card_ActivationDate = item.Card_ActivationDate;
                                    card.Card_OpenDate = item.Card_OpenDate;
                                    card.Card_State = item.Card_State;
                                    card.Card_WorkState = item.Card_WorkState;
                                    card.Card_Remarks = item.Card_Remarks;
                                    card.Card_ICCID = item.Card_ICCID;
                                    card.RenewDate = item.RenewDate;
                                    card.Card_IMEI = item.Card_IMEI;
                                    li.Add(card);
                                }
                                c.Cards = li;
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "1";
                            }
                        }
                    }
                    else
                    {
                        if (list.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(list.OperatorsFlg))//移动
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string ss = " and Card_CompanyID=@Card_CompanyID    group by  t1.Card_ICCID";
                                //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                                string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    from card_copy1 t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                                    left join outofstock t4 on   t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID
                                    left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID
                                        where t1.status=1   and (Card_ICCID in @Card_ICCID  or card_id in @Card_ICCID or Card_IMSI in @Card_ICCID  or Card_IMEI in @Card_ICCID ) " + ss;
                                Card card = new Card();
                                //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
                                c.Cards = conn.Query<Card>(sql2, new { Card_CompanyID = list.CompanyID, Card_ICCID = ll }).ToList();
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "0";
                            }
                        }
                        if (list.OperatorsFlg == "2")//电信
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string ss = " and Card_CompanyID=@Card_CompanyID    group by  t1.Card_ICCID";
                                //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                                string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    from ct_cardcopy t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                                    left join outofstock t4 on   t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID
                                    left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID
                                        where t1.status=1   and (Card_ICCID in @Card_ICCID  or card_id in @Card_ICCID or Card_IMSI in @Card_ICCID  or Card_IMEI in @Card_ICCID ) " + ss;
                                Card card = new Card();
                                //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
                                c.Cards = conn.Query<Card>(sql2, new { Card_CompanyID = list.CompanyID, Card_ICCID = ll }).ToList();
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "0";
                            }
                        }
                        if (list.OperatorsFlg == "3")//联通
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string ss = " and Card_CompanyID=@Card_CompanyID    group by  t1.Card_ICCID";
                                //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                                string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    from cucc_cardcopy t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                                    left join outofstock t4 on   t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID
                                    left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID
                                        where t1.status=1   and (Card_ICCID in @Card_ICCID  or card_id in @Card_ICCID or Card_IMSI in @Card_ICCID  or Card_IMEI in @Card_ICCID ) " + ss;
                                Card card = new Card();
                                //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
                                c.Cards = conn.Query<Card>(sql2, new { Card_CompanyID = list.CompanyID, Card_ICCID = ll }).ToList();
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "0";
                            }
                        }
                        if (list.OperatorsFlg == "4")//全网通卡
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string ss = " and Card_CompanyID=@Card_CompanyID    group by  t1.SN";
                                string sql2 = @"select t1.SN,t2.Flow,t2.CardXTID,t1.Card_Remarks,t2.PackageDescribe,t2.CardTypeID,t3.CardTypeName,t4.CardXTName,t1.Card_ActivationDate,
                                               t1.RenewDate from three_cardcopy t1 left join setmeal t2 on t1.SetMealID2=t2.SetmealID left join cardtype t3 
                                               on t3.CardTypeID=t2.CardTypeID left join card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                               where (SN in @Card_ICCID)" + ss;
                                Card card = new Card();
                                c.Cards = conn.Query<Card>(sql2, new { Card_CompanyID = list.CompanyID, Card_ICCID = ll }).ToList();
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "0";
                            }
                        }
                        if (list.OperatorsFlg == "5")//漫游卡
                        {
                            using (IDbConnection conn = DapperService.MySqlConnection())
                            {
                                string ss = " and Card_CompanyID=@Card_CompanyID    group by  t1.Card_ICCID";
                                //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                                string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    from roamcard_copy t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                                    left join outofstock t4 on   t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID
                                    left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID
                                        where t1.status=1   and (Card_ICCID in @Card_ICCID  or card_id in @Card_ICCID or Card_IMSI in @Card_ICCID  or Card_IMEI in @Card_ICCID ) " + ss;
                                Card card = new Card();
                                //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
                                c.Cards = conn.Query<Card>(sql2, new { Card_CompanyID = list.CompanyID, Card_ICCID = ll }).ToList();
                                c.Total = c.Cards.Count.ToString();
                                c.Message = "Success";
                                c.status = "0";
                            }
                        }
                    }
                }
                else
                {
                    c.Cards = null;
                    c.Total = "0";
                    c.Message = "查询数量不能大于20000";
                    c.status = "1";
                }
            }
            catch (Exception ex)
            {
                c.status = "500";
                c.Message = "服务器出问题请联系管理员" + ex.ToString();
            }
            return c;
        }
        public class Remark
        {
            //public string Card_ICCID { get; set; }
           // public string Card_Remarks { get; set; }
           public string ICCID { get; set; }
           public string SN {get;set;}
           public string 备注 { get; set; }
        }
        public class Remarks
        {
            //OperatorsFlg 1：移动 2:电信 3:联通 4：全网通 5：漫游
            public string OperatorsFlg { get; set; }
            public string CompanyID { get; set; }
            public List<Remark> remarks { get; set; }
        }

        public class ReturnMessage
        {
            public string status { get; set; }
            public string Message { get; set; }
        }
        public static ReturnMessage Update_Remarks()
        {
            Remarks r = new Remarks();
            ReturnMessage returnMessage = new ReturnMessage();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;           
            StreamReader sRead = new StreamReader(postData);            
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            r = JsonConvert.DeserializeObject<Remarks>(postContent);
            string sql = string.Empty;
            //update   card
            if (r.remarks.Count <= 20000)
            {
                if (r.CompanyID == "1556265186243")
                {
                    if (r.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(r.OperatorsFlg))
                    {
                        sql = "update card set Card_Remarks=@备注 where Card_ICCID=@ICCID ";
                    }
                    if (r.OperatorsFlg == "2")
                    {
                        sql = "update ct_card set Card_Remarks=@备注 where Card_ICCID=@ICCID ";
                    }
                    if (r.OperatorsFlg == "3")
                    {
                        sql = "update cucc_card set Card_Remarks=@备注 where Card_ICCID=@ICCID ";
                    }
                    if (r.OperatorsFlg == "4")
                    {
                        sql = "update three_card set Card_Remarks=@备注 where SN=@SN ";
                    }
                    if (r.OperatorsFlg == "5")
                    {
                        sql = "update roamcard set Card_Remarks=@备注 where Card_ICCID=@ICCID ";
                    }
                    //var sql = "update card set Card_Remarks=@Card_Remarks where Card_ICCID=@Card_ICCID ";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        int i = conn.Execute(sql, r.remarks);
                        returnMessage.status = "0";
                        returnMessage.Message = "生效" + i.ToString() + "条数据";
                    }
                }
                else
                {
                    if (r.OperatorsFlg == "1" || string.IsNullOrWhiteSpace(r.OperatorsFlg))
                    {
                        sql = "update card_copy1 set Card_Remarks=@备注 where Card_ICCID=@ICCID  and Card_CompanyID='" + r.CompanyID + "'";
                    }
                    if (r.OperatorsFlg == "2")
                    {
                        sql = "update ct_cardcopy set Card_Remarks=@备注 where Card_ICCID=@ICCID  and Card_CompanyID='" + r.CompanyID + "'";
                    }
                    if (r.OperatorsFlg == "3")
                    {
                        sql = "update cucc_cardcopy set Card_Remarks=@备注 where Card_ICCID=@ICCID  and Card_CompanyID='" + r.CompanyID + "'";
                    }
                    if (r.OperatorsFlg == "4")
                    {
                        sql = "update three_cardcopy set Card_Remarks=@备注 where SN=@SN  and Card_CompanyID='" + r.CompanyID + "'";
                    }
                    if (r.OperatorsFlg == "5")
                    {
                        sql = "update roamcard_copy set Card_Remarks=@备注 where Card_ICCID=@ICCID  and Card_CompanyID='" + r.CompanyID + "'";
                    } 
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        int i = conn.Execute(sql, r.remarks);                        
                        returnMessage.Message = "生效" + i.ToString() + "条数据";
                        returnMessage.status = "0";
                    }
                }
            }
            else
            {
                returnMessage.status = "1";
                returnMessage.Message = "数量不能大于20000";
            }
            return returnMessage;
        }

         

        /// <summary>
        /// 更新IMEI 以ICCID为基础  
        /// </summary>
        public class Mate_IMEI
        {     
            /// <summary>
            /// ICCID
            /// </summary>
            public string ICCID { get; set; }
            /// <summary>
            /// IMEI
            /// </summary>
            public string IMEI { get; set; }
        }
        /// <summary>
        ///  POST 接受信息
        /// </summary>
        public class Mate_IMEIS
        {
            public string OperatorsFlg { get; set; }
            public List<Mate_IMEI> mate_IMEIs { get; set; }
        }
        /// <summary>
        /// 更新 IMEI
        /// </summary>
        /// <returns></returns>
        public static ReturnMessage Update_IMEI()
        {
            ReturnMessage returnMessage = new ReturnMessage();
            Mate_IMEIS r = new Mate_IMEIS();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            r = JsonConvert.DeserializeObject<Mate_IMEIS>(postContent);
            bool flag = true;
            if (r.mate_IMEIs.Count <= 20000)
            {
                StringBuilder card_IMei = new StringBuilder("");
                StringBuilder card_IMei2 = new StringBuilder("");
                if (string.IsNullOrWhiteSpace(r.OperatorsFlg) || r.OperatorsFlg == "1")//移动
                {
                    card_IMei.Append(" update card set Card_IMEI= case Card_ICCID ");
                    card_IMei2.Append("update card_copy1 set Card_IMEI= case Card_ICCID");
                }
                if (r.OperatorsFlg == "2")//电信
                {
                    card_IMei.Append(" update ct_card set Card_IMEI= case Card_ICCID ");
                    card_IMei2.Append("update ct_cardcopy set Card_IMEI= case Card_ICCID");
                }
                if (r.OperatorsFlg == "3")//联通
                {
                    card_IMei.Append(" update cucc_card set Card_IMEI= case Card_ICCID ");
                    card_IMei2.Append("update cucc_cardcopy set Card_IMEI= case Card_ICCID");
                }
                if (r.OperatorsFlg == "5")//漫游
                {
                    card_IMei.Append(" update roamcard set Card_IMEI= case Card_ICCID ");
                    card_IMei2.Append("update roamcard_copy set Card_IMEI= case Card_ICCID");
                }
                //StringBuilder card_IMei = new StringBuilder(" update card set Card_IMEI= case Card_ICCID ");
                //StringBuilder card_IMei2 = new StringBuilder(" update card_copy1 set Card_IMEI= case Card_ICCID ");
                StringBuilder Card_ICCID = new StringBuilder("  (");
                int ii = 1;
                foreach (Mate_IMEI item in r.mate_IMEIs)
                {
                    if (item.ICCID.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Length == 20 || item.IMEI.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Length == 15)
                    {
                        item.IMEI = item.IMEI.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Substring(item.IMEI.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Length - 15, 15);
                        card_IMei.Append("  WHEN  '" + item.ICCID.Trim() + "' THEN '" + item.IMEI.Trim() + "'");
                        card_IMei2.Append("  WHEN  '" + item.ICCID.Trim() + "' THEN '" + item.IMEI.Trim() + "'");
                    }
                    else
                    {
                        returnMessage.status = "1";
                        returnMessage.Message = "IMEI位数不正确 提示：15位或20位";
                        flag = false;
                        break;
                    }
                    if (ii != r.mate_IMEIs.Count)
                    {
                        Card_ICCID.Append("'" + item.ICCID.Trim() + "'" + ",");
                        ii++;
                    }
                    else
                    {
                        Card_ICCID.Append("'" + item.ICCID.Trim() + "'");
                    }
                }
                if (flag)
                {
                    if (r.mate_IMEIs.Count <= 20000 && flag && ii == r.mate_IMEIs.Count)
                    {
                        card_IMei.Append(" end where Card_ICCID in  ");
                        card_IMei2.Append(" end where Card_ICCID in  ");
                        Card_ICCID.Append(")");
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            string s1 = card_IMei.ToString() + Card_ICCID.ToString();
                            string s2 = card_IMei2.ToString() + Card_ICCID.ToString();
                            int i = conn.Execute(s1);
                            returnMessage.status = "0";
                            returnMessage.Message = "公海数据提交成功";
                            if (string.IsNullOrWhiteSpace(r.OperatorsFlg) || r.OperatorsFlg == "1")
                            {
                                string sqlupdatecard = "update card set UploadimeiFlg='1' where Card_ICCID in " + Card_ICCID.ToString() + "";
                                conn.Execute(sqlupdatecard);
                            }
                        }
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            string s2 = card_IMei2.ToString() + Card_ICCID.ToString();
                            int i = conn.Execute(s2);
                            returnMessage.status = "0";
                            returnMessage.Message += "——客户数据提交成功";
                            if (string.IsNullOrWhiteSpace(r.OperatorsFlg) || r.OperatorsFlg == "1")
                            {
                                string sqlupdatecardcopy = "update card_copy1 set UploadimeiFlg='1' where Card_ICCID in " + Card_ICCID.ToString() + " ";
                                conn.Execute(sqlupdatecardcopy);
                            }   
                        }
                    }
                }
            }
            else
            {
                returnMessage.status = "1";
                returnMessage.Message = "数据不能为空或数据不正确";
            }
            return returnMessage;           
        }

        /// <summary>
        /// 条件查询  返回信息
        /// </summary>
        public class Query_RequirementMessage
        {
            /// <summary>
            /// 卡信息
            /// </summary>
            public List<Card> Cards { get; set; }
            /// <summary>
            /// 状态标志 1 成功 0 失败
            /// </summary>
            public string flg { get; set; }
            /// <summary>
            /// 协议信息提示
            /// </summary>
            public string Msg { get; set; }
            /// <summary>
            /// 查询总数
            /// </summary>
            public string Total { get; set; }
        }     

        /// <summary>
        /// POST  BODY    条件查询     类
        /// </summary>
        public class Query_Parameter
        {
            ///<summary>
            ///运营商编码
            /// </summary>
            public string accountID { get; set; }
            /// <summary>
            /// 基本属相 ICCID  IMSI 等
            /// </summary>
            public string Card_Mess { get; set; }
            ///<summary>
            ///分配给客户的公司名称
            /// </summary>
            public string CustomerCompany { get; set; }
            /// <summary>
            /// 工作状态
            /// </summary>
            public string Card_WorkState { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Card_Remarks { get; set; }
            /// <summary>
            /// 公司内码
            /// </summary>
            public string Card_CompanyID { get; set; }
            /// <summary>
            /// 卡状态
            /// </summary>
            public string Card_State { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string Card_OpenDate_start { get; set; }
            /// <summary>
            /// 终止时间
            /// </summary>
            public string Card_OpenDate_end { get; set; }
            ///<summary>
            ///运营商 1:移动 2:电信 3:联通
            /// </summary>
            public string Card_OperatorsFlg { get; set; }
            public int PagNumber { get; set; }
            public int Num { get; set; }

            public List<Message> cards { get; set; }

        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Query_RequirementMessage Query_requirement()
        {
            Query_RequirementMessage r2 = new Query_RequirementMessage();
            Query_Parameter r = new Query_Parameter();
            HttpRequest request = HttpContext.Current.Request;
            Stream postData = request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            r = JsonConvert.DeserializeObject<Query_Parameter>(postContent);
            StringBuilder sql = new StringBuilder("");
            string ss= " limit " + (r.PagNumber - 1) * r.Num + "," + r.Num;
            string cardcountnum = "select count(id) as Total from ";
            string iccids = string.Empty;
            if (r.Card_CompanyID == "1556265186243")
            {
                if (r.Card_OperatorsFlg == "1")//移动
                {
                    sql.Append(@"select  case when RegionLimitStatus='0' then '未超出管控区域' when RegionLimitStatus='1' then '已超出管控区域' else '暂无数据' end RegionLimitStatus , t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.CustomerCompany,t1.IsSeparate,
                                t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate , t6.Remark,t1.Scene,
                                date_format(t1.CustomerActivationDate, '%Y-%m-%d') as CustomerActivationDate,date_format(t1.CustomerEndTime, '%Y-%m-%d') as CustomerEndTime,   
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate,t1.RegionLabel from card   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1  ");
                    cardcountnum += "card where status=1";
                }
                if (r.Card_OperatorsFlg == "2")//电信
                {
                    sql.Append(@"select  case when RegionLimitStatus='0' then '未超出管控区域' when RegionLimitStatus='1' then '已超出管控区域' else '暂无数据' end RegionLimitStatus , t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.CustomerCompany,
                                t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate , t6.Remark,t1.Scene,
                                date_format(t1.CustomerActivationDate, '%Y-%m-%d') as CustomerActivationDate,date_format(t1.CustomerEndTime, '%Y-%m-%d') as CustomerEndTime,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate,t1.RegionLabel from ct_card   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1  ");
                    cardcountnum += "ct_card where status=1";
                }
                if (r.Card_OperatorsFlg == "3")//联通
                {
                    sql.Append(@"select  case when RegionLimitStatus='0' then '未超出管控区域' when RegionLimitStatus='1' then '已超出管控区域' else '暂无数据' end RegionLimitStatus , t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.CustomerCompany,
                                t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate, t6.Remark,t1.Scene,
                                date_format(t1.CustomerActivationDate, '%Y-%m-%d') as CustomerActivationDate,date_format(t1.CustomerEndTime, '%Y-%m-%d') as CustomerEndTime,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate,t1.RegionLabel from cucc_card   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1  ");
                    cardcountnum += "cucc_card where status=1";
                }
            }
            if (r.Card_CompanyID != "1556265186243")
            {
                if (r.Card_OperatorsFlg == "1")//移动
                {
                    sql.Append(@"select  DISTINCT case when RegionLimitStatus='0' then '未超出管控区域' when RegionLimitStatus='1' then '已超出管控区域' else '暂无数据' end RegionLimitStatus ,  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID, t1.Card_IMSI,t1.Card_Type,t1.IsSeparate,
                                t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate ,t1.Scene,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate,t1.RegionLabel from card_copy1   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where ");
                    sql.Append("  t1.status=1 and  t1.Card_CompanyID='" + r.Card_CompanyID + "'");
                    cardcountnum += "card_copy1 where Card_CompanyID='" + r.Card_CompanyID + "' and status=1";
                }
                if (r.Card_OperatorsFlg == "2")//电信
                {
                    sql.Append(@"select DISTINCT case when RegionLimitStatus='0' then '未超出管控区域' when RegionLimitStatus='1' then '已超出管控区域' else '暂无数据' end RegionLimitStatus ,  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID, t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate ,t1.Scene,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate,t1.RegionLabel from ct_cardcopy   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where ");
                    sql.Append("  t1.status=1 and  t1.Card_CompanyID='" + r.Card_CompanyID + "'");
                    cardcountnum += "ct_cardcopy where Card_CompanyID='" + r.Card_CompanyID + "' and status=1";
                }
                if (r.Card_OperatorsFlg == "3")//联通
                {
                    sql.Append(@"select DISTINCT case when RegionLimitStatus='0' then '未超出管控区域' when RegionLimitStatus='1' then '已超出管控区域' else '暂无数据' end RegionLimitStatus ,  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID, t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate ,t1.Scene,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate,t1.RegionLabel from cucc_cardcopy   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where ");
                    sql.Append("  t1.status=1 and  t1.Card_CompanyID='" + r.Card_CompanyID + "'");
                    cardcountnum += "cucc_cardcopy where Card_CompanyID='" + r.Card_CompanyID + "' and status=1";
                }  
            }
            if (!string.IsNullOrWhiteSpace(r.Card_Mess))
            {
                if (r.Card_Mess.Length != 13 && r.Card_Mess.Length != 15 && r.Card_Mess.Length != 20 && r.Card_Mess.Length != 19 && r.Card_Mess.Length!=14)
                {
                    r2.Msg = "查询失败,请输入正确的ICCID、物联网卡号、IMEI";
                    r2.flg = "0";
                    r2.Total = "0";
                    return r2;
                }
                switch (r.Card_Mess.Length)
                {
                    case 14:
                        sql.Append(" and   t1.Card_IMEI like '%" + r.Card_Mess + "%'");
                        cardcountnum += " and  Card_IMEI like '%" + r.Card_Mess + "%'";
                        break;
                    case 15:
                        sql.Append(" and  t1.Card_IMSI='" + r.Card_Mess + "'" + " or  t1.Card_IMEI='" + r.Card_Mess + "'");
                        cardcountnum += " and  Card_IMSI='" + r.Card_Mess + "'" + " or  Card_IMEI='" + r.Card_Mess + "'";
                        break;
                    case 20:
                        sql.Append("  and  t1.Card_ICCID='" + r.Card_Mess + "'");
                        cardcountnum += "  and  Card_ICCID='" + r.Card_Mess + "'";
                        break;
                    case 19:   //电信卡ICCID  19位
                        sql.Append("  and   t1.Card_ICCID like '%" + r.Card_Mess + "%'");
                        cardcountnum += "  and   Card_ICCID like '%" + r.Card_Mess + "%'";
                        break;
                    case 13:
                        sql.Append("  and   t1.Card_ID='" + r.Card_Mess + "'");
                        cardcountnum += "  and   Card_ID='" + r.Card_Mess + "'";
                        break;
                    default:
                        sql.Append(" ");
                        break;
                }
            }
            //else
            //{
                if (r.cards != null)
                {
                    if (r.cards.Count > 0)
                    {
                        foreach (var item in r.cards)
                        {
                            iccids += "'" + item.ICCID + "',";
                        }
                        iccids = iccids.Substring(0, iccids.Length - 1);
                        sql.Append(" and  t1.Card_ICCID in (" + iccids + ")");
                        cardcountnum += " and  Card_ICCID in(" + iccids + ")";
                    }
                }
                if (!string.IsNullOrWhiteSpace(r.Card_WorkState))
                {
                    sql.Append(" and  t1.Card_WorkState='" + r.Card_WorkState + "'");
                    cardcountnum += " and  Card_WorkState='" + r.Card_WorkState + "'";
                }
                if (!string.IsNullOrWhiteSpace(r.Card_State))
                {
                    if(r.Card_State == "1")//待激活
                    {
                        sql.Append(" and  (t1.Card_State='1' or t1.Card_State='07') and t1.Card_CompanyID='" + r.Card_CompanyID + "'");
                        cardcountnum += " and (Card_State='1' or Card_State='07') and Card_CompanyID='" + r.Card_CompanyID + "'";
                        if (!string.IsNullOrWhiteSpace(r.Card_Mess))
                        {
                            if (r.Card_Mess.Length == 15)//imei
                            {
                                sql.Append(" and  t1.Card_IMEI='" + r.Card_Mess + "'");
                                cardcountnum += " and Card_IMEI='" + r.Card_Mess + "'";
                            }
                            if (r.Card_Mess.Length == 20 || r.Card_Mess.Length == 19)//imei
                            {
                                sql.Append(" and   t1.Card_ICCID='" + r.Card_Mess + "'");
                                cardcountnum += " and  Card_ICCID='" + r.Card_Mess + "'";
                            }
                            if (r.Card_Mess.Length == 13)//imei
                            {
                                sql.Append(" and  t1.Card_ID='" + r.Card_Mess + "'");
                                cardcountnum += " and  Card_ID='" + r.Card_Mess + "'";
                            }
                        }
                    }
                    if(r.Card_State == "02")//停机
                    {
                        sql.Append(" and  (t1.Card_State='02' or t1.Card_State='4') and t1.Card_CompanyID='"+r.Card_CompanyID+"'");
                        cardcountnum += " and (Card_State='02' or Card_State='4') and Card_CompanyID='" + r.Card_CompanyID + "'";
                        if (!string.IsNullOrWhiteSpace(r.Card_Mess))
                        {
                            if (r.Card_Mess.Length == 15)//imei
                            {
                                sql.Append(" and  t1.Card_IMEI='" + r.Card_Mess + "'");
                                cardcountnum += " and Card_IMEI='" + r.Card_Mess + "'";
                            }
                            if (r.Card_Mess.Length == 20 || r.Card_Mess.Length == 19)//imei
                            {
                                sql.Append(" and   t1.Card_ICCID='" + r.Card_Mess + "'");
                                cardcountnum += " and  Card_ICCID='" + r.Card_Mess + "'";
                            }
                            if (r.Card_Mess.Length == 13)//imei
                            {
                                sql.Append(" and  t1.Card_ID='" + r.Card_Mess + "'");
                                cardcountnum += " and  Card_ID='" + r.Card_Mess + "'";
                            }
                        }
                    }
                    if(r.Card_State!="1" && r.Card_State!="02") 
                    {
                        sql.Append(" and  t1.Card_State='" + r.Card_State + "'");
                        cardcountnum += " and Card_State='" + r.Card_State + "'";
                    } 
                }
                //根据运营商编码查询卡数据
                if (!string.IsNullOrWhiteSpace(r.accountID))
                {
                    sql.Append(" and  t1.accountsID='" + r.accountID + "'");
                    cardcountnum += " and accountsID='" + r.accountID + "'";
                }
                if (!string.IsNullOrWhiteSpace(r.Card_Remarks))
                {
                    sql.Append(" and  t1.Card_Remarks like'%" + r.Card_Remarks + "%'");
                    cardcountnum += " and  Card_Remarks like'%" + r.Card_Remarks + "%'";
                }
                if (!string.IsNullOrWhiteSpace(r.CustomerCompany))//根据分配给用户的公司名称查询
                {
                    sql.Append(" and  t1.CustomerCompany like'%" + r.CustomerCompany + "%'");
                    cardcountnum += " and  CustomerCompany like'%" + r.CustomerCompany + "%'";
                }
                if (!string.IsNullOrWhiteSpace(r.Card_OpenDate_start) && !string.IsNullOrWhiteSpace(r.Card_OpenDate_end ))
                {
                    sql.Append("  and   (t1.Card_OpenDate between '" + r.Card_OpenDate_start + "' and '" + r.Card_OpenDate_end + "')");
                    cardcountnum += "  and   (Card_OpenDate between '" + r.Card_OpenDate_start + "' and '" + r.Card_OpenDate_end + "')";
                }
               
                //if (!string.IsNullOrWhiteSpace(r.Card_OperatorsFlg))
                //{
                //    int flgnum = Convert.ToInt32(r.Card_OperatorsFlg);
                //    sql.Append("  and  t1.OperatorsFlg="+flgnum+"");
                //    cardcountnum += "  and  OperatorsFlg=" + flgnum + "";
                //}
                sql.Append(ss);
                
            //}
            try
            {
                if (r.Card_CompanyID == "1556265186243")
                {
                    using (IDbConnection  conn = DapperService.MySqlConnection())
                    {
                        Card card = new Card();
                        string s = sql.ToString();
                        r2.Cards = new List<Card>();
                        r2.Cards = conn.Query<Card>(s).ToList();
                        r2.Msg = "查询成功!";
                        r2.flg = "1";
                        string total = conn.Query<Card>(cardcountnum).Select(t => t.Total).FirstOrDefault().ToString();
                        //string total = r2.Cards.Count().ToString();
                        r2.Total = total;
                       // r2.Total = r2.Cards.Count.ToString();
                       
                    }
                }
                else
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        r2.Cards = new List<Card>();
                        r2.Cards = conn.Query<Card>(sql.ToString()).ToList();
                        r2.Msg = "查询成功!";
                        r2.flg = "1";
                        // r2.Total = r2.Cards.Count.ToString();
                        string total = conn.Query<Card>(cardcountnum).Select(t => t.Total).FirstOrDefault().ToString();
                        r2.Total = total;
                    }
                }
            }
            catch (Exception ex)
            {
                r2.Msg = "调取接口失败请联系管理员" + ex;
                r2.flg = "0";
                r2.Total = "0";
            }
            return r2;
        }


        ///<summary>
        ///导出物联网卡数据
        /// </summary>
        public static FilesPath ExportData(GetCardPara para)
        {
            FilesPath path = new FilesPath();
            try
            {
                StringBuilder sql = new StringBuilder();
                Query_RequirementMessage r2 = new Query_RequirementMessage();
                string Card_State = string.Empty;
                string Card_WorkState = string.Empty;
                if (para.Card_CompanyID == "1556265186243")//奇迹物联
                {
                    if (para.Card_OperatorsFlg == "1")//移动
                    {
                        sql.Append(@"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.CustomerCompany,t1.IsSeparate,
                                t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate , t6.Remark,
                                date_format(t1.CustomerActivationDate, '%Y-%m-%d') as CustomerActivationDate,date_format(t1.CustomerEndTime, '%Y-%m-%d') as CustomerEndTime,   
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate from card   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1  ");
                    }
                    if (para.Card_OperatorsFlg == "2")//电信
                    {
                        sql.Append(@"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.CustomerCompany,
                                t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate , t6.Remark,
                                date_format(t1.CustomerActivationDate, '%Y-%m-%d') as CustomerActivationDate,date_format(t1.CustomerEndTime, '%Y-%m-%d') as CustomerEndTime,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate from ct_card   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1  ");
                    }
                    if (para.Card_OperatorsFlg == "3")//联通
                    {
                        sql.Append(@"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.CustomerCompany,
                                t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate, t6.Remark,
                                date_format(t1.CustomerActivationDate, '%Y-%m-%d') as CustomerActivationDate,date_format(t1.CustomerEndTime, '%Y-%m-%d') as CustomerEndTime,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate from cucc_card   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1  ");
                    }
                }
                else if(para.Card_CompanyID!= "1556265186243")//用户
                {
                    if (para.Card_OperatorsFlg == "1")//移动
                    {
                        sql.Append(@"select DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID, t1.Card_IMSI,t1.Card_Type,t1.IsSeparate,
                                t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate ,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate from card_copy1   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where ");
                        sql.Append("  t1.status=1 and  t1.Card_CompanyID='" + para.Card_CompanyID + "'");
                    }
                    if (para.Card_OperatorsFlg == "2")//电信
                    {
                        sql.Append(@"select DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID, t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate ,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate from ct_cardcopy   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where ");
                        sql.Append("  t1.status=1 and  t1.Card_CompanyID='" + para.Card_CompanyID + "'");
                    }
                    if (para.Card_OperatorsFlg == "3")//联通
                    {
                        sql.Append(@"select DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID, t1.Card_IMSI,t1.Card_Type,
                                t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_OpenDate  ,        
                                date_format(t1.Card_ActivationDate, '%Y-%m-%d') as Card_ActivationDate ,
                                t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                                ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d') as RenewDate from cucc_cardcopy   t1 
                                left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                                left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                                left   join operator t5 on t5.OperatorID=t2.OperatorID where ");
                        sql.Append("  t1.status=1 and  t1.Card_CompanyID='" + para.Card_CompanyID + "'");
                    }
                }
                if (!string.IsNullOrWhiteSpace(para.Card_WorkState))
                {
                    sql.Append(" and  t1.Card_WorkState='" + para.Card_WorkState + "'");
                }
                if (!string.IsNullOrWhiteSpace(para.Card_State))
                {
                    if (para.Card_State == "1")//待激活
                    {
                        sql.Append(" and  (t1.Card_State='1' or t1.Card_State='07') and t1.Card_CompanyID='" + para.Card_CompanyID + "'");
                        
                    }
                    if (para.Card_State == "02")//停机
                    {
                        sql.Append(" and  (t1.Card_State='02' or t1.Card_State='4') and t1.Card_CompanyID='" + para.Card_CompanyID + "'");
                    }
                    if (para.Card_State != "1" && para.Card_State != "02")
                    {
                        sql.Append(" and  t1.Card_State='" + para.Card_State + "'");
                    }
                }
                //根据运营商编码查询卡数据
                if (!string.IsNullOrWhiteSpace(para.accountID))
                {
                    sql.Append(" and  t1.accountsID='" + para.accountID + "'");
                }
                if (!string.IsNullOrWhiteSpace(para.Card_Remarks))
                {
                    sql.Append(" and  t1.Card_Remarks like'%" + para.Card_Remarks + "%'");
                }
                if (!string.IsNullOrWhiteSpace(para.CustomerCompany))//根据分配给用户的公司名称查询
                {
                    sql.Append(" and  t1.CustomerCompany like'%" + para.CustomerCompany + "%'");
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    r2.Cards = conn.Query<Card>(sql.ToString()).ToList();
                    List<Card> listUser = r2.Cards;
                    if (para.Card_CompanyID == "1556265186243")//导出奇迹数据
                    {
                        if (r2.Cards.Count <= 65535)
                        {
                            //创建工作簿对象
                            IWorkbook workbook = new HSSFWorkbook();
                            //创建工作表
                            ISheet sheet = workbook.CreateSheet("onesheet");
                            IRow row0 = sheet.CreateRow(0);
                            row0.CreateCell(0).SetCellValue("物联卡号");
                            row0.CreateCell(1).SetCellValue("ICCID");
                            row0.CreateCell(2).SetCellValue("IMEI");
                            row0.CreateCell(3).SetCellValue("开户日期");
                            row0.CreateCell(4).SetCellValue("续费日期");
                            row0.CreateCell(5).SetCellValue("客户名称");
                            row0.CreateCell(6).SetCellValue("用户开户日期");
                            row0.CreateCell(7).SetCellValue("用户续费日期");
                            row0.CreateCell(8).SetCellValue("运营商");
                            row0.CreateCell(9).SetCellValue("卡类型");
                            row0.CreateCell(10).SetCellValue("套餐名称");
                            row0.CreateCell(11).SetCellValue("备注");
                            row0.CreateCell(12).SetCellValue("卡状态");
                            row0.CreateCell(13).SetCellValue("工作状态");
                            row0.CreateCell(14).SetCellValue("机卡分离");
                            row0.CreateCell(15).SetCellValue("使用流量");
                            row0.CreateCell(16).SetCellValue("IMSI");
                            int count = listUser.Count + 1;
                            for (int r = 1; r < count; r++)
                            {
                                if (listUser[r - 1].Card_State == "00")
                                {
                                    Card_State = "正常";
                                }
                                if (listUser[r - 1].Card_State == "01")
                                {
                                    Card_State = "单项停机";
                                }
                                if (listUser[r - 1].Card_State == "02" || listUser[r - 1].Card_State == "4")
                                {
                                    Card_State = "停机";
                                }
                                if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
                                {
                                    Card_State = "预销户";
                                }
                                if (listUser[r - 1].Card_State == "1")
                                {
                                    Card_State = "待激活";
                                }
                                if (listUser[r - 1].Card_State == "2")
                                {
                                    Card_State = "已激活";
                                }
                                if (listUser[r - 1].Card_State == "6")
                                {
                                    Card_State = "可测试";
                                }
                                if (listUser[r - 1].Card_State == "7")
                                {
                                    Card_State = "库存";
                                }
                                if (listUser[r - 1].Card_WorkState == "00")
                                {
                                    Card_WorkState = "离线";
                                }
                                if (listUser[r - 1].Card_WorkState == "01")
                                {
                                    Card_WorkState = "在线";
                                }
                                //创建行row
                                IRow row = sheet.CreateRow(r);
                                row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
                                row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
                                row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
                                row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
                                row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
                                row.CreateCell(5).SetCellValue(listUser[r - 1].CustomerCompany);
                                row.CreateCell(6).SetCellValue(listUser[r - 1].CustomerActivationDate);
                                row.CreateCell(7).SetCellValue(listUser[r - 1].CustomerEndTime);
                                row.CreateCell(8).SetCellValue(listUser[r - 1].Remark);
                                row.CreateCell(9).SetCellValue(listUser[r - 1].CardTypeName);
                                row.CreateCell(10).SetCellValue(listUser[r - 1].SetmealName);
                                row.CreateCell(11).SetCellValue(listUser[r - 1].Card_Remarks);
                                row.CreateCell(12).SetCellValue(Card_State);
                                row.CreateCell(13).SetCellValue(Card_WorkState);
                                if (string.IsNullOrWhiteSpace(listUser[r - 1].IsSeparate))
                                {
                                    row.CreateCell(14).SetCellValue("暂无数据");
                                }
                                if (listUser[r - 1].IsSeparate == "0")
                                {
                                    row.CreateCell(14).SetCellValue("否");
                                }
                                if (listUser[r - 1].IsSeparate == "1")
                                {
                                    row.CreateCell(14).SetCellValue("是");
                                }
                                if (!string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                {
                                    decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
                                    decimal flows = s / 1024;
                                    decimal flowmb = Math.Round(flows, 2);
                                    string strflownum = flowmb.ToString();
                                    row.CreateCell(15).SetCellValue(strflownum);
                                }
                                if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                {
                                    row.CreateCell(15).SetCellValue("0");
                                }
                                row.CreateCell(16).SetCellValue(listUser[r - 1].Card_IMSI);
                            }
                            //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
                            //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
                            //自动生成文件名称
                            string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
                            path.Path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                            //FilePath=@"D:Files/" + FileName;
                            using (FileStream url = File.OpenWrite(path.Path))
                            {
                                //导出Excel文件
                                workbook.Write(url);
                            };
                            path.Flage = "1";
                            path.Message = "Success";
                        }
                        else if (r2.Cards.Count > 65535)
                        {
                            //创建工作簿对象
                            IWorkbook workbook = new HSSFWorkbook();
                            int sheetnum = (r2.Cards.Count / 65535) + 1;
                            for (int i = 0; i < sheetnum; i++)
                            {
                                //创建工作表
                                if (i == 0)
                                {
                                    ISheet sheet = workbook.CreateSheet("sheet1");
                                    IRow row0 = sheet.CreateRow(0);
                                    row0.CreateCell(0).SetCellValue("物联卡号");
                                    row0.CreateCell(1).SetCellValue("ICCID");
                                    row0.CreateCell(2).SetCellValue("IMEI");
                                    row0.CreateCell(3).SetCellValue("开户日期");
                                    row0.CreateCell(4).SetCellValue("续费日期");
                                    row0.CreateCell(5).SetCellValue("客户名称");
                                    row0.CreateCell(6).SetCellValue("用户开户日期");
                                    row0.CreateCell(7).SetCellValue("用户续费日期");
                                    row0.CreateCell(8).SetCellValue("运营商");
                                    row0.CreateCell(9).SetCellValue("卡类型");
                                    row0.CreateCell(10).SetCellValue("套餐名称");
                                    row0.CreateCell(11).SetCellValue("备注");
                                    row0.CreateCell(12).SetCellValue("卡状态");
                                    row0.CreateCell(13).SetCellValue("工作状态");
                                    row0.CreateCell(14).SetCellValue("机卡分离");
                                    row0.CreateCell(15).SetCellValue("使用流量");
                                    row0.CreateCell(16).SetCellValue("IMSI");
                                    for (int r = 1; r < 65535; r++)
                                    {
                                        if (listUser[r - 1].Card_State == "00")
                                        {
                                            Card_State = "正常";
                                        }
                                        if (listUser[r - 1].Card_State == "01")
                                        {
                                            Card_State = "单项停机";
                                        }
                                        if (listUser[r - 1].Card_State == "02" || listUser[r - 1].Card_State == "4")
                                        {
                                            Card_State = "停机";
                                        }
                                        if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
                                        {
                                            Card_State = "预销户";
                                        }
                                        if (listUser[r - 1].Card_State == "1")
                                        {
                                            Card_State = "待激活";
                                        }
                                        if (listUser[r - 1].Card_State == "2")
                                        {
                                            Card_State = "已激活";
                                        }
                                        if (listUser[r - 1].Card_State == "6")
                                        {
                                            Card_State = "可测试";
                                        }
                                        if (listUser[r - 1].Card_State == "7")
                                        {
                                            Card_State = "库存";
                                        }
                                        if (listUser[r - 1].Card_WorkState == "00")
                                        {
                                            Card_WorkState = "离线";
                                        }
                                        if (listUser[r - 1].Card_WorkState == "01")
                                        {
                                            Card_WorkState = "在线";
                                        }
                                        //创建行row
                                        IRow row = sheet.CreateRow(r);
                                        row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
                                        row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
                                        row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
                                        row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
                                        row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
                                        row.CreateCell(5).SetCellValue(listUser[r - 1].CustomerCompany);
                                        row.CreateCell(6).SetCellValue(listUser[r - 1].CustomerActivationDate);
                                        row.CreateCell(7).SetCellValue(listUser[r - 1].CustomerEndTime);
                                        row.CreateCell(8).SetCellValue(listUser[r - 1].Remark);
                                        row.CreateCell(9).SetCellValue(listUser[r - 1].CardTypeName);
                                        row.CreateCell(10).SetCellValue(listUser[r - 1].SetmealName);
                                        row.CreateCell(11).SetCellValue(listUser[r - 1].Card_Remarks);
                                        row.CreateCell(12).SetCellValue(Card_State);
                                        row.CreateCell(13).SetCellValue(Card_WorkState);
                                        if (string.IsNullOrWhiteSpace(listUser[r - 1].IsSeparate))
                                        {
                                            row.CreateCell(14).SetCellValue("暂无数据");
                                        }
                                        if (listUser[r - 1].IsSeparate == "0")
                                        {
                                            row.CreateCell(14).SetCellValue("否");
                                        }
                                        if (listUser[r - 1].IsSeparate == "1")
                                        {
                                            row.CreateCell(14).SetCellValue("是");
                                        }
                                        if (!string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                        {
                                            decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
                                            decimal flows = s / 1024;
                                            decimal flowmb = Math.Round(flows, 2);
                                            string strflownum = flowmb.ToString();
                                            row.CreateCell(15).SetCellValue(strflownum);
                                        }
                                        if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                        {
                                            row.CreateCell(15).SetCellValue("0");
                                        }
                                        row.CreateCell(16).SetCellValue(listUser[r - 1].Card_IMSI);
                                    }
                                }
                                if (i > 0)
                                {
                                    int j = i + 1;
                                    string name = "sheet" + j.ToString();
                                    ISheet sheets = workbook.CreateSheet(name);
                                    IRow row0 = sheets.CreateRow(1);
                                    row0.CreateCell(0).SetCellValue("物联卡号");
                                    row0.CreateCell(1).SetCellValue("ICCID");
                                    row0.CreateCell(2).SetCellValue("IMEI");
                                    row0.CreateCell(3).SetCellValue("开户日期");
                                    row0.CreateCell(4).SetCellValue("续费日期");
                                    row0.CreateCell(5).SetCellValue("客户名称");
                                    row0.CreateCell(6).SetCellValue("用户开户日期");
                                    row0.CreateCell(7).SetCellValue("用户续费日期");
                                    row0.CreateCell(8).SetCellValue("运营商");
                                    row0.CreateCell(9).SetCellValue("卡类型");
                                    row0.CreateCell(10).SetCellValue("套餐名称");
                                    row0.CreateCell(11).SetCellValue("备注");
                                    row0.CreateCell(12).SetCellValue("卡状态");
                                    row0.CreateCell(13).SetCellValue("工作状态");
                                    row0.CreateCell(14).SetCellValue("机卡分离");
                                    row0.CreateCell(15).SetCellValue("使用流量");
                                    row0.CreateCell(16).SetCellValue("IMSI");
                                    listUser = r2.Cards.ToList().Skip(65534).ToList();
                                    //for (int r = 1; r <cards.Count()-65535; r++)
                                    for (int r = 1; r < listUser.Count() - 1; r++)
                                    {
                                        if (listUser[r - 1].Card_State == "00")
                                        {
                                            Card_State = "正常";
                                        }
                                        if (listUser[r - 1].Card_State == "01")
                                        {
                                            Card_State = "单项停机";
                                        }
                                        if (listUser[r - 1].Card_State == "02" || listUser[r - 1].Card_State == "4")
                                        {
                                            Card_State = "停机";
                                        }
                                        if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
                                        {
                                            Card_State = "预销户";
                                        }
                                        if (listUser[r - 1].Card_State == "1")
                                        {
                                            Card_State = "待激活";
                                        }
                                        if (listUser[r - 1].Card_State == "2")
                                        {
                                            Card_State = "已激活";
                                        }
                                        if (listUser[r - 1].Card_State == "6")
                                        {
                                            Card_State = "可测试";
                                        }
                                        if (listUser[r - 1].Card_State == "7")
                                        {
                                            Card_State = "库存";
                                        }
                                        if (listUser[r - 1].Card_WorkState == "00")
                                        {
                                            Card_WorkState = "离线";
                                        }
                                        if (listUser[r - 1].Card_WorkState == "01")
                                        {
                                            Card_WorkState = "在线";
                                        }
                                        //创建行row
                                        IRow row = sheets.CreateRow(r);
                                        row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
                                        row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
                                        row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
                                        row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
                                        row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
                                        row.CreateCell(5).SetCellValue(listUser[r - 1].CustomerCompany);
                                        row.CreateCell(6).SetCellValue(listUser[r - 1].CustomerActivationDate);
                                        row.CreateCell(7).SetCellValue(listUser[r - 1].CustomerEndTime);
                                        row.CreateCell(8).SetCellValue(listUser[r - 1].Remark);
                                        row.CreateCell(9).SetCellValue(listUser[r - 1].CardTypeName);
                                        row.CreateCell(10).SetCellValue(listUser[r - 1].SetmealName);
                                        row.CreateCell(11).SetCellValue(listUser[r - 1].Card_Remarks);
                                        row.CreateCell(12).SetCellValue(Card_State);
                                        row.CreateCell(13).SetCellValue(Card_WorkState);
                                        if (string.IsNullOrWhiteSpace(listUser[r - 1].IsSeparate))
                                        {
                                            row.CreateCell(14).SetCellValue("暂无数据");
                                        }
                                        if (listUser[r - 1].IsSeparate == "0")
                                        {
                                            row.CreateCell(14).SetCellValue("否");
                                        }
                                        if (listUser[r - 1].IsSeparate == "1")
                                        {
                                            row.CreateCell(14).SetCellValue("是");
                                        }
                                        if (!string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                        {
                                            decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
                                            decimal flows = s / 1024;
                                            decimal flowmb = Math.Round(flows, 2);
                                            string strflownum = flowmb.ToString();
                                            row.CreateCell(15).SetCellValue(strflownum);
                                        }
                                        if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                        {
                                            row.CreateCell(15).SetCellValue("0");
                                        }
                                        row.CreateCell(16).SetCellValue(listUser[r - 1].Card_IMSI);
                                    }
                                }
                            }
                            //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
                            //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
                            //自动生成文件名称
                            string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
                            path.Path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                            // FilePath = @"D:Files/" + FileName;
                            using (FileStream url = File.OpenWrite(path.Path))
                            {
                                //导出Excel文件
                                workbook.Write(url);
                            };
                            path.Flage = "1";
                            path.Message = "Success";
                        }
                    }
                    if (para.Card_CompanyID != "1556265186243")
                    {
                        if (r2.Cards.Count <= 65535)
                        {
                            //创建工作簿对象
                            IWorkbook workbook = new HSSFWorkbook();
                            //创建工作表
                            ISheet sheet = workbook.CreateSheet("onesheet");
                            IRow row0 = sheet.CreateRow(0);
                            row0.CreateCell(0).SetCellValue("物联卡号");
                            row0.CreateCell(1).SetCellValue("ICCID");
                            row0.CreateCell(2).SetCellValue("IMEI");
                            row0.CreateCell(3).SetCellValue("开户日期");
                            row0.CreateCell(4).SetCellValue("续费日期");
                            row0.CreateCell(5).SetCellValue("卡类型");
                            row0.CreateCell(6).SetCellValue("套餐名称");
                            row0.CreateCell(7).SetCellValue("备注");
                            row0.CreateCell(8).SetCellValue("卡状态");
                            row0.CreateCell(9).SetCellValue("工作状态");
                            row0.CreateCell(10).SetCellValue("机卡分离");
                            row0.CreateCell(11).SetCellValue("使用流量");
                            row0.CreateCell(12).SetCellValue("IMSI");
                            int count = listUser.Count + 1;
                            for (int r = 1; r < count; r++)
                            {
                                if (listUser[r - 1].Card_State == "00")
                                {
                                    Card_State = "正常";
                                }
                                if (listUser[r - 1].Card_State == "01")
                                {
                                    Card_State = "单项停机";
                                }
                                if (listUser[r - 1].Card_State == "02" || listUser[r - 1].Card_State == "4")
                                {
                                    Card_State = "停机";
                                }
                                if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
                                {
                                    Card_State = "预销户";
                                }
                                if (listUser[r - 1].Card_State == "1")
                                {
                                    Card_State = "待激活";
                                }
                                if (listUser[r - 1].Card_State == "2")
                                {
                                    Card_State = "已激活";
                                }
                                if (listUser[r - 1].Card_State == "6")
                                {
                                    Card_State = "可测试";
                                }
                                if (listUser[r - 1].Card_State == "7")
                                {
                                    Card_State = "库存";
                                }
                                if (listUser[r - 1].Card_WorkState == "00")
                                {
                                    Card_WorkState = "离线";
                                }
                                if (listUser[r - 1].Card_WorkState == "01")
                                {
                                    Card_WorkState = "在线";
                                }
                                //创建行row
                                IRow row = sheet.CreateRow(r);
                                row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
                                row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
                                row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
                                row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
                                row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
                                row.CreateCell(5).SetCellValue(listUser[r - 1].CardTypeName);
                                row.CreateCell(6).SetCellValue(listUser[r - 1].SetmealName);
                                row.CreateCell(7).SetCellValue(listUser[r - 1].Card_Remarks);
                                row.CreateCell(8).SetCellValue(Card_State);
                                row.CreateCell(9).SetCellValue(Card_WorkState);
                                if (string.IsNullOrWhiteSpace(listUser[r - 1].IsSeparate))
                                {
                                    row.CreateCell(10).SetCellValue("暂无数据");
                                }
                                if (listUser[r - 1].IsSeparate=="0")
                                {
                                    row.CreateCell(10).SetCellValue("否");
                                }
                                if (listUser[r - 1].IsSeparate == "1")
                                {
                                    row.CreateCell(10).SetCellValue("是");
                                }
                                if (!string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                {
                                    decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
                                    decimal flows = s / 1024;
                                    decimal flowmb = Math.Round(flows, 2);
                                    string strflownum = flowmb.ToString();
                                    row.CreateCell(11).SetCellValue(strflownum);
                                }
                                if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                {
                                    row.CreateCell(11).SetCellValue("0");
                                }
                                row.CreateCell(12).SetCellValue(listUser[r - 1].Card_IMSI);
                            }
                            //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
                            //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
                            //自动生成文件名称
                            string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
                            path.Path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                            //FilePath=@"D:Files/" + FileName;
                            using (FileStream url = File.OpenWrite(path.Path))
                            {
                                //导出Excel文件
                                workbook.Write(url);
                            };
                            path.Flage = "1";
                            path.Message = "Success";
                        }
                        else if (r2.Cards.Count > 65535)
                        {
                            //创建工作簿对象
                            IWorkbook workbook = new HSSFWorkbook();
                            int sheetnum = (r2.Cards.Count / 65535) + 1;
                            for (int i = 0; i < sheetnum; i++)
                            {
                                //创建工作表
                                if (i == 0)
                                {
                                    ISheet sheet = workbook.CreateSheet("sheet1");
                                    IRow row0 = sheet.CreateRow(0);
                                    row0.CreateCell(0).SetCellValue("物联卡号");
                                    row0.CreateCell(1).SetCellValue("ICCID");
                                    row0.CreateCell(2).SetCellValue("IMEI");
                                    row0.CreateCell(3).SetCellValue("开户日期");
                                    row0.CreateCell(4).SetCellValue("续费日期");
                                    row0.CreateCell(5).SetCellValue("卡类型");
                                    row0.CreateCell(6).SetCellValue("套餐名称");
                                    row0.CreateCell(7).SetCellValue("备注");
                                    row0.CreateCell(8).SetCellValue("卡状态");
                                    row0.CreateCell(9).SetCellValue("工作状态");
                                    row0.CreateCell(10).SetCellValue("机卡分离");
                                    row0.CreateCell(11).SetCellValue("使用流量");
                                    row0.CreateCell(12).SetCellValue("IMSI");
                                    for (int r = 1; r < 65535; r++)
                                    {
                                        if (listUser[r - 1].Card_State == "00")
                                        {
                                            Card_State = "正常";
                                        }
                                        if (listUser[r - 1].Card_State == "01")
                                        {
                                            Card_State = "单项停机";
                                        }
                                        if (listUser[r - 1].Card_State == "02" || listUser[r - 1].Card_State == "4")
                                        {
                                            Card_State = "停机";
                                        }
                                        if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
                                        {
                                            Card_State = "预销户";
                                        }
                                        if (listUser[r - 1].Card_State == "1")
                                        {
                                            Card_State = "待激活";
                                        }
                                        if (listUser[r - 1].Card_State == "2")
                                        {
                                            Card_State = "已激活";
                                        }
                                        if (listUser[r - 1].Card_State == "6")
                                        {
                                            Card_State = "可测试";
                                        }
                                        if (listUser[r - 1].Card_State == "7")
                                        {
                                            Card_State = "库存";
                                        }
                                        if (listUser[r - 1].Card_WorkState == "00")
                                        {
                                            Card_WorkState = "离线";
                                        }
                                        if (listUser[r - 1].Card_WorkState == "01")
                                        {
                                            Card_WorkState = "在线";
                                        }
                                        //创建行row
                                        IRow row = sheet.CreateRow(r);
                                        row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
                                        row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
                                        row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
                                        row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
                                        row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
                                        row.CreateCell(5).SetCellValue(listUser[r - 1].CardTypeName);
                                        row.CreateCell(6).SetCellValue(listUser[r - 1].SetmealName);
                                        row.CreateCell(7).SetCellValue(listUser[r - 1].Card_Remarks);
                                        row.CreateCell(8).SetCellValue(Card_State);
                                        row.CreateCell(9).SetCellValue(Card_WorkState);
                                        if (string.IsNullOrWhiteSpace(listUser[r - 1].IsSeparate))
                                        {
                                            row.CreateCell(10).SetCellValue("暂无数据");
                                        }
                                        if (listUser[r - 1].IsSeparate == "0")
                                        {
                                            row.CreateCell(10).SetCellValue("否");
                                        }
                                        if (listUser[r - 1].IsSeparate == "1")
                                        {
                                            row.CreateCell(10).SetCellValue("是");
                                        }
                                        if (!string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                        {
                                            decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
                                            decimal flows = s / 1024;
                                            decimal flowmb = Math.Round(flows, 2);
                                            string strflownum = flowmb.ToString();
                                            row.CreateCell(11).SetCellValue(strflownum);
                                        }
                                        if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                        {
                                            row.CreateCell(11).SetCellValue("0");
                                        }
                                        row.CreateCell(12).SetCellValue(listUser[r - 1].Card_IMSI);
                                    }
                                }
                                if (i > 0)
                                {
                                    int j = i + 1;
                                    string name = "sheet" + j.ToString();
                                    ISheet sheets = workbook.CreateSheet(name);
                                    IRow row0 = sheets.CreateRow(1);
                                    row0.CreateCell(0).SetCellValue("物联卡号");
                                    row0.CreateCell(1).SetCellValue("ICCID");
                                    row0.CreateCell(2).SetCellValue("IMEI");
                                    row0.CreateCell(3).SetCellValue("开户日期");
                                    row0.CreateCell(4).SetCellValue("续费日期");
                                    row0.CreateCell(5).SetCellValue("卡类型");
                                    row0.CreateCell(6).SetCellValue("套餐名称");
                                    row0.CreateCell(7).SetCellValue("备注");
                                    row0.CreateCell(8).SetCellValue("卡状态");
                                    row0.CreateCell(9).SetCellValue("工作状态");
                                    row0.CreateCell(10).SetCellValue("机卡分离");
                                    row0.CreateCell(11).SetCellValue("使用流量");
                                    row0.CreateCell(12).SetCellValue("IMSI");
                                    listUser = r2.Cards.ToList().Skip(65534).ToList();
                                    //for (int r = 1; r <cards.Count()-65535; r++)
                                    for (int r = 1; r < listUser.Count() - 1; r++)
                                    {
                                        if (listUser[r - 1].Card_State == "00")
                                        {
                                            Card_State = "正常";
                                        }
                                        if (listUser[r - 1].Card_State == "01")
                                        {
                                            Card_State = "单项停机";
                                        }
                                        if (listUser[r - 1].Card_State == "02" || listUser[r - 1].Card_State == "4")
                                        {
                                            Card_State = "停机";
                                        }
                                        if (listUser[r - 1].Card_State == "03" || listUser[r - 1].Card_State == "8")
                                        {
                                            Card_State = "预销户";
                                        }
                                        if (listUser[r - 1].Card_State == "1")
                                        {
                                            Card_State = "待激活";
                                        }
                                        if (listUser[r - 1].Card_State == "2")
                                        {
                                            Card_State = "已激活";
                                        }
                                        if (listUser[r - 1].Card_State == "6")
                                        {
                                            Card_State = "可测试";
                                        }
                                        if (listUser[r - 1].Card_State == "7")
                                        {
                                            Card_State = "库存";
                                        }
                                        if (listUser[r - 1].Card_WorkState == "00")
                                        {
                                            Card_WorkState = "离线";
                                        }
                                        if (listUser[r - 1].Card_WorkState == "01")
                                        {
                                            Card_WorkState = "在线";
                                        }
                                        //创建行row
                                        IRow row = sheets.CreateRow(r);
                                        row.CreateCell(0).SetCellValue(listUser[r - 1].Card_ID);
                                        row.CreateCell(1).SetCellValue(listUser[r - 1].Card_ICCID);
                                        row.CreateCell(2).SetCellValue(listUser[r - 1].Card_IMEI);
                                        row.CreateCell(3).SetCellValue(listUser[r - 1].Card_OpenDate);
                                        row.CreateCell(4).SetCellValue(listUser[r - 1].RenewDate);
                                        row.CreateCell(5).SetCellValue(listUser[r - 1].CardTypeName);
                                        row.CreateCell(6).SetCellValue(listUser[r - 1].operatorsName);
                                        row.CreateCell(7).SetCellValue(listUser[r - 1].SetmealName);
                                        row.CreateCell(8).SetCellValue(Card_State);
                                        row.CreateCell(9).SetCellValue(Card_WorkState);
                                        row.CreateCell(10).SetCellValue(listUser[r - 1].Card_Remarks);
                                        if (!string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                        {
                                            int flownum = int.Parse(listUser[r - 1].Card_Monthlyusageflow);
                                            decimal s = Convert.ToDecimal(listUser[r - 1].Card_Monthlyusageflow);
                                            decimal flows = s / 1024;
                                            decimal flowmb = Math.Round(flows, 2);
                                            string strflownum = flowmb.ToString();
                                            row.CreateCell(11).SetCellValue(strflownum);
                                        }
                                        if (string.IsNullOrWhiteSpace(listUser[r - 1].Card_Monthlyusageflow))
                                        {
                                            row.CreateCell(11).SetCellValue("0");
                                        }
                                        row.CreateCell(12).SetCellValue(listUser[r - 1].Card_IMSI);
                                    }
                                }
                            }
                            //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
                            //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
                            //自动生成文件名称
                            string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
                            path.Path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                            // FilePath = @"D:Files/" + FileName;
                            using (FileStream url = File.OpenWrite(path.Path))
                            {
                                //导出Excel文件
                                workbook.Write(url);
                            };
                            path.Flage = "1";
                            path.Message = "Success";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return path;
        }


        ///<summary>
        ///根据ICCID电信卡的流量、卡状态、卡工作状态信息  Card_OperatorsFlg 2:电信  3:联通
        /// </summary>
        public static CardStatusFlowInfoDto GetCardStstusFlowInfo(string Card_ICCID,string Card_OperatorsFlg)
        {
            CardStatusFlowInfoDto dto = new CardStatusFlowInfoDto();
            RootCTStatus cTStatus = new RootCTStatus();
            RootCTCardWorkStatus cTCardWorkStatus = new RootCTCardWorkStatus();
            CuccCardFlow cuccflow = new CuccCardFlow();
            CUCCCardStatus cuccstatus = new CUCCCardStatus();
            CuccCardWorkStatus cuccworkststus = new CuccCardWorkStatus();
            CTAPIDAL ctdal = new CTAPIDAL();
            CUCCAPIDAL cuccdal = new CUCCAPIDAL();
            LargeFlowApiDAL lfdal = new LargeFlowApiDAL();
            LargeFlowDetailDto lfdto = new LargeFlowDetailDto();
            YiYuanCardDetailDto yyctdto = new YiYuanCardDetailDto();
            YiYuanWorkStatusRoot yywork = new YiYuanWorkStatusRoot();
            string sqlcucc = "select * from cucc_card where Card_ICCID='"+Card_ICCID+"'";
            string sqlct = "select * from ct_card where Card_ICCID='" + Card_ICCID + "'";
            string Platform = "";
            string CT_CardID = string.Empty;
            string CT_CardStatusName = string.Empty;
            try
            {
                if (Card_OperatorsFlg == "2")//电信
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        Platform = conn.Query<Card2>(sqlct).Select(t => t.Platform).FirstOrDefault();
                        CT_CardID = conn.Query<Card2>(sqlct).Select(t => t.Card_ID).FirstOrDefault();
                    }
                    if (Platform == "21")//普通电信卡
                    {
                        dto.Card_Monthlyusageflow = ctdal.GetCuccCardMonthFlow(Card_ICCID);
                        if (dto.Card_Monthlyusageflow != "暂未查询到月使用流量信息")
                        {
                            int i = dto.Card_Monthlyusageflow.IndexOf('M');
                            dto.Card_Monthlyusageflow = dto.Card_Monthlyusageflow.Substring(0, i);
                            dto.Card_Monthlyusageflow = (Convert.ToDecimal(dto.Card_Monthlyusageflow) * 1024).ToString();
                        }
                        else
                        {
                            dto.Card_Monthlyusageflow = "0";
                        }
                        #region
                        //cTStatus = ctdal.GetCTCardStatus(Card_ICCID);
                        //if (cTStatus.resultCode == "0")//电信卡状态
                        //{
                        //    dto.Card_State = cTStatus.description.simList[0].simStatus[0];//1：可激活 2：测试激活 3:测试去激活 4:在用 5:停机 6:运营商状态管理
                        //    if (dto.Card_State == "1")
                        //    {
                        //        dto.Card_State = "07";
                        //    }
                        //    if (dto.Card_State == "2")//测试激活
                        //    {
                        //        dto.Card_State = "10";
                        //    }
                        //    if (dto.Card_State == "3")
                        //    {
                        //        dto.Card_State = "11";
                        //    }
                        //    if (dto.Card_State == "4")
                        //    {
                        //        dto.Card_State = "00";
                        //    }
                        //    if (dto.Card_State == "5")
                        //    {
                        //        dto.Card_State = "02";
                        //    }
                        //    if (dto.Card_State == "6")
                        //    {
                        //        dto.Card_State = "12";
                        //    }
                        //}
                        //else
                        //{
                        //    dto.Card_State = "";
                        //}
                        #endregion
                        //电信卡状态（准确）
                        CT_CardStatusName = ctdal.GetStatusName(CT_CardID); //1：可激活 2：测试激活 3:测试去激活 4:在用 5:停机 6:运营商状态管理
                        if (!string.IsNullOrWhiteSpace(CT_CardStatusName))
                        {
                            if (CT_CardStatusName == "已激活(测试期)")
                            {
                                dto.Card_State = "10";//测试激活
                            }
                            if (CT_CardStatusName == "未激活(测试期)")
                            {
                                dto.Card_State = "07";//待激活
                            }
                            if (CT_CardStatusName == "测试去激活")
                            {
                                dto.Card_State = "11";
                            }
                            if (CT_CardStatusName == "在用")
                            {
                                dto.Card_State = "00";
                            }
                            if (CT_CardStatusName == "停机")
                            {
                                dto.Card_State = "02";
                            }
                            if (CT_CardStatusName == "运营商状态管理")
                            {
                                dto.Card_State = "12";
                            }
                        }
                        else
                        {
                            dto.Card_State = "";
                        }
                        cTCardWorkStatus = ctdal.GetCTCardWorkStatus(Card_ICCID);
                        if (cTCardWorkStatus.resultCode == "0")//电信卡工作状态
                        {
                            dto.Card_WorkState = cTCardWorkStatus.description.result;
                            if (dto.Card_WorkState == "0")//在线
                            {
                                dto.Card_WorkState = "01";
                            }
                            if (dto.Card_WorkState == "-1")//离线
                            {
                                dto.Card_WorkState = "00";
                            }
                            if (dto.Card_WorkState == "-2")
                            {
                                dto.Card_WorkState = "未查询到会话信息";
                            }
                        }
                        else
                        {
                            dto.Card_WorkState = "未查询到会话信息";
                        }
                    }
                    if(Platform=="51")  //联通大流量卡
                    {
                        lfdto=lfdal.LargeFLowCardDetail(Card_ICCID);
                        if (lfdto.code=="0")
                        {
                            if (lfdto.data.status== "in_stock")//在库
                            {
                                dto.Card_State = "7";
                            }
                            if (lfdto.data.status == "test_ready")//可测试
                            {
                                dto.Card_State = "6";
                            }
                            if (lfdto.data.status == "tested")//测试结束
                            {
                                dto.Card_State = "14";
                            }
                            if (lfdto.data.status == "wait_activated")//待激活
                            {
                                dto.Card_State = "1";
                            }
                            if (lfdto.data.status == "activated")//已激活
                            {
                                dto.Card_State = "2";
                            }
                            if (lfdto.data.status == "stopped")//停卡
                            {
                                dto.Card_State = "4";
                            }
                            if (lfdto.data.status == "disconnected")//断网
                            {
                                dto.Card_State = "15";
                            }
                            if (lfdto.data.status == "canceled")//销卡
                            {
                                dto.Card_State = "9";
                            }
                            dto.Card_WorkState = lfdto.data.online_status;//卡工作状态
                            dto.Card_Monthlyusageflow = lfdto.data.traffic_use.ToString();
                        }
                    }
                    if (Platform == "20")//移远电信卡
                    {
                        yyctdto = ctdal.YiYuanCardDetail(Card_ICCID);//流量和卡状态
                        if (yyctdto != null && yyctdto.resultCode == 0)
                        {
                            dto.Card_Monthlyusageflow = yyctdto.flow.ToString();
                            if (yyctdto.status == "正常")
                            {
                                dto.Card_State = "00";
                            }
                            if (yyctdto.status == "激活")
                            {
                                dto.Card_State = "2";
                            }
                            if (yyctdto.status == "待激活")
                            {
                                dto.Card_State = "1";
                            }
                            if (yyctdto.status == "停机")
                            {
                                dto.Card_State = "4";
                            }
                        }
                        yywork = ctdal.GetYiYuanWorkStatusInfo(Card_ICCID);//卡工作状态
                        if (yywork.resultCode == 0)
                        {
                            if (yywork.realTimeStatus.gprsstatus.Contains("在线"))
                            {
                                yywork.realTimeStatus.gprsstatus = "01";
                            }
                            if (yywork.realTimeStatus.gprsstatus.Contains("离线"))
                            {
                                yywork.realTimeStatus.gprsstatus = "00";
                            }
                            dto.Card_WorkState = yywork.realTimeStatus.gprsstatus;
                        }
                    }
                }
                if (Card_OperatorsFlg == "3")//联通
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        Platform = conn.Query<Card2>(sqlcucc).Select(t => t.Platform).FirstOrDefault();
                    }
                    if (Platform != "51")//普通联通卡
                    {
                        cuccflow = cuccdal.GetCuccCardFlow(Card_ICCID);
                        if (cuccflow != null)
                        {
                            decimal flows = cuccflow.ctdDataUsage / 1024; //B转为KB
                            //dto.Flow = flows.ToString() + "MB";
                            dto.Card_Monthlyusageflow = flows.ToString();
                        }
                        else
                        {
                            dto.Card_Monthlyusageflow = "0";
                        }
                        cuccstatus = cuccdal.CuccCardStatus(Card_ICCID);
                        if (cuccstatus != null)
                        {
                            if (cuccstatus.status == "TEST_READY")//可测试
                            {
                                dto.Card_State = "6";
                            }
                            if (cuccstatus.status == "ACTIVATED")//已激活
                            {
                                dto.Card_State = "2";
                            }
                            if (cuccstatus.status == "ACTIVATION_READY")//可激活
                            {
                                dto.Card_State = "07";
                            }
                            if (cuccstatus.status == "DEACTIVATED")//已停用
                            {
                                dto.Card_State = "02";
                            }
                            if (cuccstatus.status == "INVENTORY")//库存
                            {
                                dto.Card_State = "7";
                            }
                            if (cuccstatus.status == "PURGED")//已清除
                            {
                                dto.Card_State = "13";
                            }
                            if (cuccstatus.status == "RETIRED")//已注销
                            {
                                dto.Card_State = "9";
                            }
                        }
                        else
                        {
                            dto.Card_State = "暂未查询到卡状态信息";
                        }
                        cuccworkststus = cuccdal.GetCuccCardWorkStatus(Card_ICCID);
                        if (cuccworkststus != null)
                        {
                            if (!string.IsNullOrWhiteSpace(cuccworkststus.dateSessionEnded))//离线
                            {
                                dto.Card_WorkState = "00";
                            }
                            if (string.IsNullOrWhiteSpace(cuccworkststus.dateSessionEnded))//在线
                            {
                                dto.Card_WorkState = "01";
                            }
                        }
                        else
                        {
                            dto.Card_WorkState = "暂未查询到卡工作状态信息";
                        }
                    }
                    else //电信大流量卡
                    {
                        lfdto = lfdal.LargeFLowCardDetail(Card_ICCID);
                        if (lfdto.code == "0")
                        {
                            if (lfdto.data.status == "in_stock")//在库
                            {
                                dto.Card_State = "7";
                            }
                            if (lfdto.data.status == "test_ready")//可测试
                            {
                                dto.Card_State = "6";
                            }
                            if (lfdto.data.status == "tested")//测试结束
                            {
                                dto.Card_State = "14";
                            }
                            if (lfdto.data.status == "wait_activated")//待激活
                            {
                                dto.Card_State = "1";
                            }
                            if (lfdto.data.status == "activated")//已激活
                            {
                                dto.Card_State = "2";
                            }
                            if (lfdto.data.status == "stopped")//停卡
                            {
                                dto.Card_State = "4";
                            }
                            if (lfdto.data.status == "disconnected")//断网
                            {
                                dto.Card_State = "15";
                            }
                            if (lfdto.data.status == "canceled")//销卡
                            {
                                dto.Card_State = "9";
                            }
                            dto.Card_WorkState = lfdto.data.online_status;//卡工作状态
                            dto.Card_Monthlyusageflow = lfdto.data.traffic_use.ToString();
                        }
                    }
                }
                dto.status = "0";
                dto.Message = "接口调取成功!";
                dto.Success = true;
            }
            catch(Exception ex)
            {
                dto.status = "-1";
                dto.Message = "接口调取失败!"+ex;
                dto.Success = false;
            }
            return dto;
        }

        ///<summary>
        ///移动新平台APN关停  operType 0：开 1:停
        /// </summary>
        public static Information  APNoffon(CardOffOnPara para)
        {
            Information info = new Information();
            CMCCAPIDAL cmcc = new CMCCAPIDAL();
            string token = string.Empty;
            CMIOT_API23M07 t = new CMIOT_API23M07();//apn开关结果接收类
            CMIOT_API23M03 t1 = new CMIOT_API23M03();//apn名称
            CMCCRootToken crt = new CMCCRootToken();//移动token接收
            string appid = string.Empty;
            string accountsid = string.Empty;
            string apnname = string.Empty;
            StringBuilder cardiccid = new StringBuilder("");
            string sql = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(para.ICCID))
                {
                    cardiccid.Append("'" + para.ICCID + "'");
                    if (para.CompanyID == "1556265186243")//奇迹物联
                    {
                        sql = "select * from card where Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in(" + cardiccid.ToString() + ")";
                    }
                    if (para.CompanyID != "1556265186243")
                    {
                        sql = "select * from card_copy1 where Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in(" + cardiccid.ToString() + ")";
                    }
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        var listcard = conn.Query<Card>(sql).ToList();
                        if (listcard.Count == 0)
                        {
                            info.flg = "-1";
                            info.Msg = "ICCID不是您的,您没有操作权限！";
                            return info;
                        }
                    }
                }
                if (para.cards.Count > 0 && para.cards!=null)
                {
                    foreach (var item in para.cards)
                    {
                        cardiccid.Append( "'"+item.ICCID+"',");
                    }
                    string iccids= cardiccid.ToString().Substring(0, cardiccid.Length - 1).ToString();
                    if (para.CompanyID == "1556265186243")
                    {
                        sql = "select * from card where Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in(" + iccids.ToString() + ")";
                    }
                    if (para.CompanyID != "1556265186243")
                    {
                        sql = "select * from card_copy1 where Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in(" + iccids.ToString() + ")";
                    }
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        var listcard = conn.Query<Card>(sql).ToList();
                        if (listcard.Count !=para.cards.Count)
                        {
                            info.flg = "-1";
                            info.Msg = "Excel中部分ICCID不是您的,您没有操作权限！";
                            return info;
                        }
                    }
                } 
                if (para.cards.Count == 0 && !string.IsNullOrWhiteSpace(para.ICCID))
                {
                    crt = cmcc.GetToken(para.ICCID);//新平台移动卡获取token
                    if (crt.status == "0")
                    {
                        token = crt.result[0].token;
                    }
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sqlaccountid = "select accountsID from card where Card_ICCID='" + para.ICCID + "'";
                        accountsid = conn.Query<Card>(sqlaccountid).Select(s => s.accountsID).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(accountsid))
                        {
                            string sqlappid = "select APPID from accounts where accountID='" + accountsid + "'";
                            appid = conn.Query<accounts>(sqlappid).Select(s => s.APPID).FirstOrDefault();
                        }
                    }
                    t1 = cmcc.GetApnInfo(token, appid, para.ICCID); //涉及apn关停先注释掉 后面有测试卡后在启用
                    if (t1.status == "0")
                    {
                        apnname = t1.result[0].apnList[0].apnName;
                    }
                    t = cmcc.Operateapn(token, appid, para.ICCID, apnname, para.operType);
                    if (t.status == 0)
                    {
                        if (para.operType == "0")
                        {
                            info.flg = "1";
                            info.Msg = "APN开启成功!";
                        }
                        if (para.operType == "1")
                        {
                            info.flg = "1";
                            info.Msg = "APN关闭成功!";
                        }
                    }
                    //info.flg = "1";
                    //info.Msg = "成功";
                    #region 查看卡APN关停信息
                    //https://api.iot.10086.cn/v5/ec/query/apn-info?transid=xxx&token=xxx&iccid=xxx -以iccid进行查询
                    //string sssss = t1.result[0].apnList[0].status;
                    
                    #endregion
                }
                if (para.cards.Count > 0)
                {
                    for (int i = 0; i < para.cards.Count; i++)
                    {
                        crt = cmcc.GetToken(para.cards[i].ICCID);//新平台移动卡获取token
                        if (crt.status == "0")
                        {
                            token = crt.result[0].token;
                        }
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            string sqlaccountid = "select accountsID from card where Card_ICCID='" + para.cards[i].ICCID + "'";
                            accountsid = conn.Query<Card>(sqlaccountid).Select(s => s.accountsID).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(accountsid))
                            {
                                string sqlappid = "select APPID from accounts where accountID='" + accountsid + "'";
                                appid = conn.Query<accounts>(sqlappid).Select(s => s.APPID).FirstOrDefault();
                            }
                        }
                        t1 = cmcc.GetApnInfo(token, appid, para.cards[i].ICCID); //涉及apn关停先注释掉 后面有测试卡后在启用
                        if (t1.status == "0")
                        {
                            apnname = t1.result[0].apnList[0].apnName;
                        }
                        t = cmcc.Operateapn(token, appid, para.cards[i].ICCID, apnname, para.operType);
                        if (t.status == 0)
                        {
                            if (para.operType == "0")
                            {
                                info.flg = "1";
                                info.Msg = "APN开启成功!";
                            }
                            if (para.operType == "1")
                            {
                                info.flg = "1";
                                info.Msg = "APN关闭成功!";
                            }
                        }
                    }
                    //info.flg = "1";
                    //info.Msg = "成功";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败!"+ex;
            }
            return info;
        }

        ///<summary>
        ///删除卡数据
        /// </summary>
        public static Information DeleteCardInfo(CardDelPara para)
        {
            Information info = new Information();
            int status = -1;
            string iccids = "";
            StringBuilder sqlstr = new StringBuilder();
            string sqlcompany = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (para.OperatorID == 1)//移动
                    {
                        if (para.CompanyID == "1556265186243")
                        {
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select Card_ICCID from card where status=1 and Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in (");
                            string sqlnum = "select CompanyTolCardNum  from company where CompanyID='" + para.CompanyID + "'";
                            string CompanyTolCardNum = conn.Query<Company>(sqlnum).Select(t => t.CompanyTolCardNum).FirstOrDefault();
                            int cardnum = Convert.ToInt32(CompanyTolCardNum) - para.Cards.Count;
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.ICCID + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除 
                                {
                                    sqlstr.Append("update card set status= " + status + " where  Card_ICCID in (");
                                    sqlstr.Append(iccids);
                                    string upnum = "update company set CompanyTolCardNum='" + cardnum.ToString() + "' where CompanyID='" + para.CompanyID + "'";
                                    conn.Execute(sqlstr.ToString());
                                    conn.Execute(upnum);
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                        else
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select Card_ICCID from card_copy1 where status=1 and Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in (");
                            string sqlnum = "select CompanyTolCardNum  from company where CompanyID='" + para.CompanyID + "'";
                            string CompanyTolCardNum = conn.Query<Company>(sqlnum).Select(t => t.CompanyTolCardNum).FirstOrDefault();
                            int cardnum = Convert.ToInt32(CompanyTolCardNum) - para.Cards.Count;
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.ICCID + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除 同时删除上传日志中的卡数据和公海中所属用户和用户开卡续费日期数据
                                {
                                    sqlstr.Append("update card_copy1 set status= " + status + " where  Card_ICCID in (");
                                    sqlstr.Append(iccids);
                                    string upnum = "update company set CompanyTolCardNum='" + cardnum.ToString() + "' where CompanyID='" + para.CompanyID + "'";
                                    //删除上传日志汇总的卡数据
                                    string delproduct_excel = "delete from product_excel where ICCID in ("+iccids+"";
                                    //删除公海中所属用户和用户开卡续费日期数据
                                    string delcarddata = "update  card set CustomerCompany=null,CustomerActivationDate=null,CustomerEndTime=null where Card_ICCID in (" + iccids + "";
                                    conn.Execute(sqlstr.ToString());
                                    conn.Execute(upnum);
                                    conn.Execute(delproduct_excel);
                                    conn.Execute(delcarddata);
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                    }
                    if (para.OperatorID == 2)//电信
                    {
                        if (para.CompanyID == "1556265186243")
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select * from ct_card where status=1 and Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in (");
                            string sqlnum = "select CompanyTolCardNum  from company where CompanyID='" + para.CompanyID + "'";
                            string CompanyTolCardNum = conn.Query<Company>(sqlnum).Select(t => t.CompanyTolCardNum).FirstOrDefault();
                            int cardnum = Convert.ToInt32(CompanyTolCardNum) - para.Cards.Count;
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.ICCID + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除
                                {
                                    sqlstr.Append("update ct_card set status= " + status + " where  Card_ICCID in (");
                                    sqlstr.Append(iccids);
                                    string upnum = "update company set CompanyTolCardNum='" + cardnum.ToString() + "' where CompanyID='" + para.CompanyID + "'";
                                    conn.Execute(sqlstr.ToString());
                                    conn.Execute(upnum);
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                        else
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select * from ct_cardcopy where status=1 and Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in (");
                            string sqlnum = "select CompanyTolCardNum  from company where CompanyID='" + para.CompanyID + "'";
                            string CompanyTolCardNum = conn.Query<Company>(sqlnum).Select(t => t.CompanyTolCardNum).FirstOrDefault();
                            int cardnum = Convert.ToInt32(CompanyTolCardNum) - para.Cards.Count;
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.ICCID + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除
                                {
                                    sqlstr.Append("update ct_cardcopy set status= " + status + " where  Card_ICCID in (");
                                    sqlstr.Append(iccids);
                                    string upnum = "update company set CompanyTolCardNum='" + cardnum.ToString() + "' where CompanyID='" + para.CompanyID + "'";
                                    //删除上传日志汇总的卡数据
                                    string delproduct_excel = "delete from product_excel where ICCID in (" + iccids + "";
                                    //删除公海中所属用户和用户开卡续费日期数据
                                    string delcarddata = "update  ct_card set CustomerCompany=null,CustomerActivationDate=null,CustomerEndTime=null where Card_ICCID in (" + iccids + "";
                                    conn.Execute(sqlstr.ToString());
                                    conn.Execute(upnum);
                                    conn.Execute(delproduct_excel);
                                    conn.Execute(delcarddata);
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                    }
                    if (para.OperatorID == 3)//联通
                    {
                        if (para.CompanyID == "1556265186243")
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select * from cucc_card where status=1 and Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in (");
                            string sqlnum = "select CompanyTolCardNum  from company where CompanyID='" + para.CompanyID + "'";
                            string CompanyTolCardNum = conn.Query<Company>(sqlnum).Select(t => t.CompanyTolCardNum).FirstOrDefault();
                            int cardnum = Convert.ToInt32(CompanyTolCardNum) - para.Cards.Count;
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.ICCID + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除
                                {
                                    sqlstr.Append("update cucc_card set status= " + status + " where  Card_ICCID in (");
                                    sqlstr.Append(iccids);
                                    string upnum = "update company set CompanyTolCardNum='" + cardnum.ToString() + "' where CompanyID='" + para.CompanyID + "'";
                                    conn.Execute(sqlstr.ToString());
                                    conn.Execute(upnum);
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                        else
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select * from cucc_cardcopy where status=1 and Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in (");
                            string sqlnum = "select CompanyTolCardNum  from company where CompanyID='" + para.CompanyID + "'";
                            string CompanyTolCardNum = conn.Query<Company>(sqlnum).Select(t => t.CompanyTolCardNum).FirstOrDefault();
                            int cardnum = Convert.ToInt32(CompanyTolCardNum) - para.Cards.Count;
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.ICCID + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除
                                {
                                    sqlstr.Append("update cucc_cardcopy set status= " + status + " where  Card_ICCID in (");
                                    sqlstr.Append(iccids);
                                    string upnum = "update company set CompanyTolCardNum='" + cardnum.ToString() + "' where CompanyID='" + para.CompanyID + "'";
                                    //删除上传日志汇总的卡数据
                                    string delproduct_excel = "delete from product_excel where ICCID in (" + iccids + "";
                                    //删除公海中所属用户和用户开卡续费日期数据
                                    string delcarddata = "update  cucc_card set CustomerCompany=null,CustomerActivationDate=null,CustomerEndTime=null where Card_ICCID in (" + iccids + "";
                                    conn.Execute(sqlstr.ToString());
                                    conn.Execute(upnum);
                                    conn.Execute(delproduct_excel);
                                    conn.Execute(delcarddata);
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                    }
                    if (para.OperatorID == 4)//全网通卡
                    {
                        if (para.CompanyID == "1556265186243")
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select * from three_card where status=1 and Card_CompanyID='" + para.CompanyID + "' and SN in (");
                            string sqlnum = "select CompanyTolCardNum  from company where CompanyID='" + para.CompanyID + "'";
                            string CompanyTolCardNum = conn.Query<Company>(sqlnum).Select(t => t.CompanyTolCardNum).FirstOrDefault();
                            int cardnum = Convert.ToInt32(CompanyTolCardNum) - para.Cards.Count;
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.SN + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除
                                {
                                    sqlstr.Append("update three_card set status= " + status + " where  SN in (");
                                    sqlstr.Append(iccids);
                                    string upnum = "update company set CompanyTolCardNum='" + cardnum.ToString() + "' where CompanyID='" + para.CompanyID + "'";
                                    conn.Execute(upnum);
                                    conn.Execute(sqlstr.ToString());
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                        else
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select * from three_cardcopy where status=1 and Card_CompanyID='" + para.CompanyID + "' and SN in (");
                            string sqlnum = "select CompanyTolCardNum  from company where CompanyID='" + para.CompanyID + "'";
                            string CompanyTolCardNum = conn.Query<Company>(sqlnum).Select(t => t.CompanyTolCardNum).FirstOrDefault();
                            int cardnum = Convert.ToInt32(CompanyTolCardNum) - para.Cards.Count;
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.SN + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除
                                {
                                    sqlstr.Append("update three_cardcopy set status= " + status + " where  SN in (");
                                    sqlstr.Append(iccids);
                                    string upnum = "update company set CompanyTolCardNum='" + cardnum.ToString() + "' where CompanyID='" + para.CompanyID + "'";
                                    //删除上传日志汇总的卡数据
                                    string delproduct_excel = "delete from product_threecard where SN in (" + iccids + "";
                                    //删除公海中所属用户和用户开卡续费日期数据
                                    string delcarddata = "update  three_card set CustomerCompany=null,CustomerActivationDate=null,CustomerEndTime=null where SN in (" + iccids + "";
                                    conn.Execute(upnum);
                                    conn.Execute(sqlstr.ToString());
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                    }
                    if (para.OperatorID ==5)//海外漫游卡
                    {
                        if (para.CompanyID == "1556265186243")
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select * from roamcard where status=1 and Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in (");
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.ICCID + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除
                                {
                                    sqlstr.Append("update roamcard set status= " + status + " where  Card_ICCID in (");
                                    sqlstr.Append(iccids);
                                    conn.Execute(sqlstr.ToString());
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
                            }
                        }
                        else
                        {
                            //sqlcompany= "select count(*) as num from company where ";
                            //查询是否有此数据 或者是否已经删除
                            StringBuilder sqldel = new StringBuilder("select * from roamcard_copy where status=1 and Card_CompanyID='" + para.CompanyID + "' and Card_ICCID in (");
                            if (para.Cards.Count > 0)
                            {
                                foreach (var item in para.Cards)
                                {
                                    iccids += "'" + item.ICCID + "',";
                                }
                                iccids = iccids.Substring(0, iccids.Length - 1);
                                iccids = iccids + ")";
                                sqldel.Append(iccids);
                                var listcard = conn.Query<Card>(sqldel.ToString()).ToList();
                                if (listcard.Count == para.Cards.Count)//数据符合 传入的卡为自己的卡且均没有删除
                                {
                                    sqlstr.Append("update roamcard_copy set status= " + status + " where  Card_ICCID in (");
                                    //删除上传日志汇总的卡数据
                                    string delproduct_excel = "delete from product_excel where ICCID in (" + iccids + "";
                                    //删除公海中所属用户和用户开卡续费日期数据
                                    string delcarddata = "update  roamcard set CustomerCompany=null,CustomerActivationDate=null,CustomerEndTime=null where Card_ICCID in (" + iccids + "";
                                    sqlstr.Append(iccids);
                                    conn.Execute(sqlstr.ToString());
                                    conn.Execute(delcarddata);
                                    conn.Execute(delproduct_excel);
                                    info.flg = "1";
                                    info.Msg = "成功!";
                                }
                                else
                                {
                                    info.flg = "-1";
                                    info.Msg = "传入的卡中有不属于自己的卡，或卡已经删除!";
                                }
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


        ///<summary>
        ///根据卡号获取卡的IMEI和卡的近七天流量使用情况卡的近六个月流量使用情况 Card_OperatorsFlg 1：移动 2:电信
        /// </summary>
        public static CardFlowImeiInfo GetCardFlowImeiInfo(string Card_ID,string Card_OperatorsFlg)
        {
            CardFlowImeiInfo info = new CardFlowImeiInfo();
            try
            {
                if (Card_OperatorsFlg == "1")
                {
                    //查看imei
                    string sql = "select Platform,Card_ICCID,accountsID from card where Card_ID='" + Card_ID+"'";
                    string ICCID = string.Empty;
                    string Platform = string.Empty;
                    string token = string.Empty;
                    CMCCRootToken crt = new CMCCRootToken();
                    CMCCAPIDAL cmcc = new CMCCAPIDAL();
                    List<DayFlow> dayFlows = new List<DayFlow>();
                    List<MonthFlow> monthFlows = new List<MonthFlow>();
                    DayFlow dayflow = new DayFlow();
                    MonthFlow monthflow = new MonthFlow();
                    List<string> flow = new List<string>();
                    List<string> date = new List<string>();
                    List<string> mflow = new List<string>();
                    List<string> mdate = new List<string>();
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        var cardinfo = conn.Query<Card>(sql).FirstOrDefault();
                        if (cardinfo != null)
                        {
                            ICCID = cardinfo.Card_ICCID;
                            Platform = cardinfo.Platform;
                            string accountsID = cardinfo.accountsID;
                            if (Platform == "11")//移动新平台可以查imei老平台不可以
                            {
                                CMIOT_API23S04.Root root = new CMIOT_API23S04.Root();
                                root = Sim_Action.Get_CMIOT_API23S04("MSISDN", Card_ID);
                                info.imei = root.result[0].imei;

                                string sqlappid = "select APPID from accounts where accountID='" + accountsID + "'";
                                string appid = conn.Query<accounts>(sqlappid).Select(s => s.APPID).FirstOrDefault();
                                //获取七天的流量信息
                                crt = cmcc.GetToken(ICCID);//新平台移动卡的状态
                                if (crt.status == "0")
                                {
                                    token = crt.result[0].token;
                                }
                                //获取流量 近七天
                                //https://api.iot.10086.cn/v5/ec/query/sim-data-usage-daily/batch?transid=xxxxxx&token=xxxxx&msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询
                                CMIOT_API25U01 t = new CMIOT_API25U01();
                                string url = @"https://api.iot.10086.cn/v5/ec/query/sim-data-usage-daily/batch?";
                                string Transid = appid + "2019071002415709643582";
                                for (int i = 0; i < 7; i++)
                                {

                                    string queryDate = string.Empty;
                                    int j = i + 1;
                                    DateTime time = DateTime.Now.AddDays(-j);
                                    int month = time.Month;
                                    int day = time.Day;
                                    string datem = string.Empty;
                                    string dated = string.Empty;
                                    if (month < 10)
                                    {
                                        datem = "0" + month.ToString();
                                    }
                                    else
                                    {
                                        datem = month.ToString();
                                    }
                                    if (day < 10)
                                    {
                                        dated = "0" + day.ToString();
                                    }
                                    else
                                    {
                                        dated = day.ToString();
                                    }
                                    queryDate = time.Year.ToString() + datem + dated;
                                    string SS = url + "transid=" + Transid + "&token=" + token + "&msisdns=" + Card_ID + "&queryDate=" + queryDate;
                                    Encoding encoding = Encoding.UTF8;
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                                    request.Method = "GET";
                                    request.Accept = "text/html, application/xhtml+xml, */*";
                                    request.ContentType = "application/json";
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                    {
                                        t = JsonConvert.DeserializeObject<CMIOT_API25U01>(reader.ReadToEnd());
                                        date.Add(time.Year.ToString() + "-" + month.ToString() + "-" + day.ToString());
                                        flow.Add(t.result[0].dataAmountList[0].dataAmount);
                                        dayflow.date = date;
                                        dayflow.flow = flow;
                                        //flow.Flow = 0].dataAmount;
                                    }
                                }
                                dayFlows.Add(dayflow);
                                //获取流量近六个月
                                //https://api.iot.10086.cn/v5/ec/query/sim-data-usage-monthly/batch?transid=xxxxxx&token=xxxxx&msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询
                                for (int i = 0; i < 6; i++)
                                {

                                    int j = i + 1;
                                    DateTime time = DateTime.Now.AddMonths(-j);
                                    int month = time.Month;
                                    string datem = string.Empty;
                                    if (month < 10)
                                    {
                                        datem = "0" + month.ToString();
                                    }
                                    else
                                    {
                                        datem = month.ToString();
                                    }
                                    url = @"https://api.iot.10086.cn/v5/ec/query/sim-data-usage-monthly/batch?";
                                    string queryDate = time.Year.ToString() + datem;
                                    string SS = url + "transid=" + Transid + "&token=" + token + "&msisdns=" + Card_ID + "&queryDate=" + queryDate;
                                    Encoding encoding = Encoding.UTF8;
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                                    request.Method = "GET";
                                    request.Accept = "text/html, application/xhtml+xml, */*";
                                    request.ContentType = "application/json";
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                    {
                                        t = JsonConvert.DeserializeObject<CMIOT_API25U01>(reader.ReadToEnd());
                                        mdate.Add(time.Year.ToString() + "-" + month.ToString());
                                        mflow.Add(t.result[0].dataAmountList[0].dataAmount);
                                        monthflow.date = mdate;
                                        monthflow.flow = mflow;
                                    }
                                }
                                monthFlows.Add(monthflow);
                                info.monthFlows = monthFlows;
                                info.dayFlows = dayFlows;
                                info.flg = "1";
                                info.Msg = "成功!";
                            }
                            if (Platform == "10")//移动老平台
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    int j = i + 1;
                                    DateTime time = DateTime.Now.AddDays(-j);
                                    int month = time.Month;
                                    int day = time.Day;
                                    string datem = string.Empty;
                                    string dated = string.Empty;
                                    if (month < 10)
                                    {
                                        datem = "0" + month.ToString();
                                    }
                                    else
                                    {
                                        datem = month.ToString();
                                    }
                                    if (day < 10)
                                    {
                                        dated = "0" + day.ToString();
                                    }
                                    else
                                    {
                                        dated = day.ToString();
                                    }
                                    date.Add(time.Year.ToString() + "-" + month.ToString()+"-"+day.ToString());
                                    flow.Add("0");
                                    dayflow.date = date;
                                    dayflow.flow = flow;
                                }
                                dayFlows.Add(dayflow);
                                for (int i = 0; i < 6; i++)
                                {
                                    int j = i + 1;
                                    DateTime time = DateTime.Now.AddMonths(-j);
                                    int month = time.Month;
                                    string datem = string.Empty;
                                    if (month < 10)
                                    {
                                        datem = "0" + month.ToString();
                                    }
                                    else
                                    {
                                        datem = month.ToString();
                                    }
                                    mdate.Add(time.Year.ToString() + "-" + month.ToString());
                                    mflow.Add("0");
                                    monthflow.date = mdate;
                                    monthflow.flow = mflow;
                                }
                                monthFlows.Add(monthflow);
                                info.dayFlows = dayFlows;
                                info.monthFlows = monthFlows;
                                info.flg = "1";
                                info.Msg = "无数据";
                            }
                        }                   
                    }   
                }
                if (Card_OperatorsFlg == "2")//电信
                {
                    List<DayFlow> dayFlows = new List<DayFlow>();
                    List<MonthFlow> monthFlows = new List<MonthFlow>();
                    DayFlow dayflow = new DayFlow();
                    MonthFlow monthflow = new MonthFlow();
                    List<string> flow = new List<string>();
                    List<string> date = new List<string>();
                    List<string> mflow = new List<string>();
                    List<string> mdate = new List<string>();
                    //流量查询(时间段)接口
                    string MonthFlow = "";
                    //http://api.ct10649.com:9001/m2m_ec/query.do?method=queryTraffic&access_number=1491000000&user_id= test & passWord = 443 * *****EC6E & sign = 7E7D6 * **edDtl = 0
                    XmlDocument xml = new XmlDocument();
                    string method = "queryTrafficByDate";
                    string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
                    string password = "40P4Xn2bkyyGvmrB";
                    string key1 = "682";
                    string key2 = "NB5";
                    string key3 = "ckb";
                    string SS = string.Empty;
                    string startDate = string.Empty;
                    string endDate = string.Empty;
                    for (int i = 0; i < 7; i++) //七日内流量
                    {
                        int j = i + 1;
                        DateTime time = DateTime.Now.AddDays(-j);
                        startDate = time.Year.ToString() + time.Month.ToString();
                        int month = time.Month;
                        int day = time.Day;
                        string m = string.Empty;
                        string d = string.Empty;
                        if (month < 10)
                        {
                            m = "0" + month.ToString();
                        }
                        else
                        {
                            m = month.ToString();
                        }
                        if (day < 10)
                        {
                            d = "0" + day.ToString();
                        }
                        else
                        {
                            d = day.ToString();
                        }
                        startDate = time.Year.ToString() + m + d;
                        endDate = time.Year.ToString() + m + d;
                        //string iccid = "8986111925804813264";
                        //具体接口参数需参照自管理门户在线文档
                        string[] arr = { user_id, password, method, Card_ID }; //加密数组，数组所需参数根据对应的接口文档
                        DesUtils des = new DesUtils(); //加密工具类实例化
                        string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
                        string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
                        string url = string.Empty;
                        url = "http://api.ct10649.com:9001/m2m_ec/query.do?";
                        SS = url + "method=" + method + "&access_number=" + Card_ID + "&needDtl=0&user_id=" + user_id + "&startDate=" + startDate + "&endDate=" + endDate + "&passWord=" + passwordEnc + "&sign=" + sign;
                        Encoding encoding = Encoding.UTF8;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                        request.Method = "GET";
                        request.Accept = "text/html, application/xhtml+xml, */*";
                        request.ContentType = "application/json";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            string str = reader.ReadToEnd();
                            if (str.Length > 30)
                            {
                                xml.LoadXml(str);//加载数据
                                XmlNodeList xxList = xml.GetElementsByTagName("web:NEW_DATA_TICKET_QRsp"); //取得节点e799bee5baa6e58685e5aeb931333337396362名为row的XmlNode集
                                foreach (XmlNode item in xxList)
                                {
                                    MonthFlow = item["TOTAL_BYTES_CNT"].InnerText;
                                }
                                if (!string.IsNullOrWhiteSpace(MonthFlow))
                                {
                                    MonthFlow = MonthFlow.Substring(0, MonthFlow.IndexOf('M'));
                                    decimal flows = Convert.ToDecimal(MonthFlow) * 1024;
                                    MonthFlow = flows.ToString();
                                }
                                date.Add(time.Year.ToString() + "-" + month.ToString() + "-" + day.ToString());
                                flow.Add(MonthFlow);
                                dayflow.date = date;
                                dayflow.flow = flow;
                            }
                            else
                            {
                                date.Add(time.Year.ToString() + "-" + month.ToString() + "-" + day.ToString());
                                flow.Add("0");
                                dayflow.date = date;
                                dayflow.flow = flow;
                            }
                        }
                    }
                    dayFlows.Add(dayflow);
                    for (int i = 0; i < 6; i++)
                    {
                        int j = i + 1;
                        DateTime time = DateTime.Now.AddMonths(-j);
                        int month = time.Month;
                        string datem = string.Empty;
                        if (month < 10)
                        {
                            datem = "0" + month.ToString();
                        }
                        else
                        {
                            datem = month.ToString();
                        }
                        mdate.Add(time.Year.ToString() + "-" + month.ToString());
                        mflow.Add("0");
                        monthflow.date = mdate;
                        monthflow.flow = mflow;
                    }
                    monthFlows.Add(monthflow);
                    info.monthFlows = monthFlows;
                    info.dayFlows = dayFlows;
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
        ///查看漫游卡信息
        /// </summary>
        public static Query_RequirementMessage GetRoamCardInfo(GetRoamCardPara para)
        {
            Query_RequirementMessage info = new Query_RequirementMessage();
            List<Card> Cards = new List<Card>();
            List<Card> cards = new List<Card>();
            
            StringBuilder sql = new StringBuilder("");
            string ss = " limit " + (para.PagNumber - 1) * para.Num + "," + para.Num;
            string iccids = string.Empty;
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    if (para.Card_CompanyID == "1556265186243")
                    {
                        sql.Append(@"select  t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.CustomerCompany,t1.IsSeparate,
                            t1.Card_IMSI,t1.Card_Type,t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate,        
                            date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate , 
                            date_format(t1.CustomerActivationDate, '%Y-%m-%d %H:%i:%s') as CustomerActivationDate,date_format(t1.CustomerEndTime, '%Y-%m-%d %H:%i:%s') as CustomerEndTime,   
                            t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                            ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate from roamcard   t1 
                            left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                            left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                            left   join operator t5 on t5.OperatorID=t2.OperatorID where t1.status = 1  ");
                    }
                    else
                    {
                        sql.Append(@"select DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID, t1.Card_IMSI,t1.Card_Type,t1.IsSeparate,
                            t1.Card_State,t1.Card_WorkState,  date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_OpenDate  ,        
                            date_format(t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,
                            t1.Card_Remarks  as Card_Remarks, t1.status, t1.Card_CompanyID ,t2.Flow,t2.PackageDescribe as SetmealName 
                            ,t3.CardTypeName as CardTypeName ,t4.CardXTName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate from roamcard_copy   t1 
                            left  join accounts t6 on  t6.AccountID=t1.accountsID left   join setmeal t2 on t2.SetmealID=t1.setmealId2 
                            left  join cardtype t3 on t3.CardTypeID=t2.CardTypeID left  join  card_xingtai t4 on t4.CardXTID=t2.CardXTID
                            left   join operator t5 on t5.OperatorID=t2.OperatorID where ");
                        sql.Append("  t1.status=1 and  t1.Card_CompanyID='" + para.Card_CompanyID + "'");
                    }
                    if (!string.IsNullOrWhiteSpace(para.Card_Mess))
                    {
                        if (para.Card_Mess.Length == 15)
                        {
                            sql.Append("and  t1.Card_IMSI='" + para.Card_Mess + "'" + " or  t1.Card_IMEI='" + para.Card_Mess + "' or t1.Card_ID='" + para.Card_Mess + "'");
                        }
                        if (para.Card_Mess.Length == 20)
                        {
                            sql.Append("and  t1.Card_ICCID='" + para.Card_Mess + "'");
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(para.Card_State))
                    {
                        sql.Append("and  t1.Card_State='"+para.Card_State+"' ");
                    }
                    if (!string.IsNullOrWhiteSpace(para.CustomerCompany) && para.Card_CompanyID== "1556265186243")
                    {
                        sql.Append("and  t1.CustomerCompany like '%" + para.CustomerCompany + "%'");
                    }
                    if (!string.IsNullOrWhiteSpace(para.Card_Remarks))
                    {
                        sql.Append("and  t1.Card_Remarks like '%" + para.Card_Remarks + "%'");
                    }
                    cards = conn.Query<Card>(sql.ToString()).ToList();
                    info.Total = cards.Count.ToString();
                    sql.Append(ss);
                    Cards = conn.Query<Card>(sql.ToString()).ToList();
                    info.Cards = Cards;
                    info.Msg = "查询成功!";
                    info.flg = "1";
                }
                catch (Exception ex)
                {
                    info.Msg = "调取接口失败请联系管理员:" + ex;
                    info.flg = "0";
                    info.Total = "0";
                }
            }
            return info;
        }


        public class CMCCYEAR
        {
            public string Card_ICCID { get; set; }
            public decimal YearFlow { get; set; }
        }

        //老平台数据相加
        public static string sumnum()
        {
            string t = string.Empty;
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql = "select Card_ICCID,YearFlow from cmccyearflow where SetmealType='1' and YearFlow>0 and UpdateTime>'2021-07-31 00:00:00'";
                var lists = conn.Query<CMCCYEAR>(sql).ToList();
                foreach (var item in lists)
                {
                    decimal yearflow = 0;
                    string sqlyearflow = "select YearFlow from cmccyearflow where UpdateTime<'2021-07-30 21:00:00' and Card_ICCID='"+item.Card_ICCID+"'";
                    yearflow = conn.Query<CMCCYEAR>(sqlyearflow).Select(s => s.YearFlow).FirstOrDefault();
                    yearflow = yearflow + item.YearFlow;
                    string updatecmmccflow = "update cmccyearflow set YearFlow="+yearflow+ " where UpdateTime<'2021-07-30 21:00:00' and Card_ICCID='" + item.Card_ICCID + "'";
                    conn.Execute(updatecmmccflow);
                }
            }
                return t;
        }

        ///<summary>
        ///查看单卡的近六个月流量使用信息
        /// </summary>
        public static CmccYearFlows GetCmccCardMonthFlowInfo(GetYearFlowPara para)
        {
            CmccYearFlows flows = new CmccYearFlows();
            string ss = " limit " + (para.PagNumber - 1) * para.Num + "," + para.Num;
            string SetmealName = string.Empty;
            string sqlyear1 = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string striccid = string.Empty;
                    string sqlyear = string.Empty;
                    if (para.Card_ICCID != null)//单卡查询
                    {
                        sqlyear = "select DISTINCT t1.Card_ICCID as ICCID,ROUND(t1.MonthFlow/1024,2) as MonthFlow,t1.Months,t1.Years,t1.OperatorName, t1.Card_ID,t2.PackageDescribe as SetmealName " +
                                "from cmccyearflow t1 left join setmeal t2 on t1.SetmealID=t2.SetmealID where t1.Card_ICCID='"+para.Card_ICCID+ "' and MonthFlow is not NULL ORDER BY Months desc";
                        var listcard = conn.Query<cmccyearflow>(sqlyear).ToList();
                        sqlyear = sqlyear + ss;
                        flows.cmccyearflows = conn.Query<cmccyearflow>(sqlyear).ToList();
                        
                        flows.Total = listcard.Count();
                        flows.flg = "1";
                        flows.Msg = "成功!";
                    }
                    if (para.ListCard!=null && para.ListCard.Count>0)//批量查询
                    {                        
                        foreach (var item in para.ListCard)
                        {
                            striccid += "'" + item.ICCID + "',";
                        }
                        striccid = striccid.Substring(0, striccid.Length - 1);
                        if (para.Card_CompanyID == "1556265186243")//奇迹物联
                        {
                            sqlyear = "select DISTINCT t1.Card_ICCID as ICCID,ROUND(t1.MonthFlow/1024,2) as MonthFlow,t1.Months,t1.Years,t1.OperatorName, t1.Card_ID,t2.PackageDescribe as SetmealName ,t3.Card_Remarks,t3.CustomerCompany " +
                              "from cmccyearflow t1 left join setmeal t2 on t1.SetmealID=t2.SetmealID left join card t3 on t1.Card_ICCID=t3.Card_ICCID  where t1.Card_ICCID in (" + striccid + ") and MonthFlow is not NULL";
                        }
                        else
                        {
                            sqlyear = "select DISTINCT t1.Card_ICCID as ICCID,ROUND(t1.MonthFlow/1024,2) as MonthFlow,t1.Months,t1.Years,t1.OperatorName, t1.Card_ID,t2.PackageDescribe as SetmealName ,t3.Card_Remarks,t4.CompanyName " +
                                "from cmccyearflow t1 left join setmeal t2 on t1.SetmealID=t2.SetmealID left join card_copy1 t3 on t1.Card_ICCID=t3.Card_ICCID  left join company t4 on t1.Card_CompanyID=t4.CompanyID  where t1.Card_ICCID in (" + striccid + ") and MonthFlow is not NULL";
                        }
                        var listcard = conn.Query<cmccyearflow>(sqlyear).ToList();
                        flows.Total = listcard.Count();
                        if (para.Num != 0 && para.PagNumber != 0)
                        {
                             sqlyear1 = sqlyear + ss;
                            flows.cmccyearflows = conn.Query<cmccyearflow>(sqlyear1).ToList();
                        }
                        if (!string.IsNullOrWhiteSpace(para.Months))
                        {
                            flows.cmccyearflows = conn.Query<cmccyearflow>(sqlyear).ToList().Where(t => t.Months == para.Months).ToList();
                            flows.Total = flows.cmccyearflows.Count();
                            if (para.Num != 0 && para.PagNumber != 0)
                            {
                                flows.cmccyearflows = flows.cmccyearflows.Skip((para.PagNumber - 1) * para.Num).Take(para.Num).ToList();
                            }
                        }
                        if (string.IsNullOrWhiteSpace(para.Months))
                        {
                            flows.cmccyearflows = conn.Query<cmccyearflow>(sqlyear).ToList();
                            flows.Total = flows.cmccyearflows.Count();
                            if (para.Num != 0 && para.PagNumber != 0)
                            {
                                flows.cmccyearflows = flows.cmccyearflows.Skip((para.PagNumber - 1) * para.Num).Take(para.Num).ToList();
                            }
                        }
                        flows.flg = "1";
                        flows.Msg = "成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                flows.flg = "-1";
                flows.Msg = "失败!" + ex;
            }
            return flows;
        }

        ///<summary>
        ///物联网卡月使用量流量数据批量导出
        /// </summary>
        public static string MonthFlowExcel(GetYearFlowPara para)
        {
            CmccYearFlows flows = new CmccYearFlows();
            flows = GetCmccCardMonthFlowInfo(para);
            List<cmccyearflow> listcard = new List<cmccyearflow>();
            if (flows.flg == "1" && flows.cmccyearflows.Count > 0)
            {
                listcard = flows.cmccyearflows;
            }
            var FilePath = "";
            if (listcard.Count <= 65535)
            {
                //创建工作簿对象
                IWorkbook workbook = new HSSFWorkbook();
                //创建工作表
                ISheet sheet = workbook.CreateSheet("onesheet");
                IRow row0 = sheet.CreateRow(0);
                row0.CreateCell(0).SetCellValue("物联卡号");
                row0.CreateCell(1).SetCellValue("ICCID");
                row0.CreateCell(2).SetCellValue("月份");
                row0.CreateCell(3).SetCellValue("月使用流量MB");
                row0.CreateCell(4).SetCellValue("套餐名称");
                row0.CreateCell(5).SetCellValue("所属公司");
                row0.CreateCell(6).SetCellValue("备注");
                int count = listcard.Count + 1;
                for (int r = 1; r < count; r++)
                {
                    //创建行row
                    IRow row = sheet.CreateRow(r);
                    row.CreateCell(0).SetCellValue(listcard[r - 1].Card_ID);
                    row.CreateCell(1).SetCellValue(listcard[r - 1].ICCID);
                    row.CreateCell(2).SetCellValue(listcard[r - 1].Months);
                    row.CreateCell(3).SetCellValue(listcard[r - 1].MonthFlow.ToString());
                    row.CreateCell(4).SetCellValue(listcard[r - 1].SetmealName);
                    row.CreateCell(5).SetCellValue(listcard[r - 1].CustomerCompany);
                    row.CreateCell(6).SetCellValue(listcard[r - 1].Card_Remarks);
                }
                //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
                //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
                //自动生成文件名称
                string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
                FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                //FilePath=@"D:Files/" + FileName;
                using (FileStream url = File.OpenWrite(FilePath))
                {
                    //导出Excel文件
                    workbook.Write(url);
                };
            }
            else
            {
                //创建工作簿对象
                IWorkbook workbook = new HSSFWorkbook();
                int sheetnum = (listcard.Count / 65535) + 1;
                for (int i = 0; i < sheetnum; i++)
                {
                    //创建工作表
                    if (i == 0)
                    {
                        ISheet sheet = workbook.CreateSheet("sheet1");
                        IRow row0 = sheet.CreateRow(0);
                        row0.CreateCell(0).SetCellValue("物联卡号");
                        row0.CreateCell(1).SetCellValue("ICCID");
                        row0.CreateCell(2).SetCellValue("月份");
                        row0.CreateCell(3).SetCellValue("月使用流量MB");
                        row0.CreateCell(4).SetCellValue("套餐名称");
                        row0.CreateCell(5).SetCellValue("所属公司");
                        row0.CreateCell(6).SetCellValue("备注");
                        for (int r = 1; r < 65535; r++)
                        {
                            //创建行row
                            IRow row = sheet.CreateRow(r);
                            row.CreateCell(0).SetCellValue(listcard[r - 1].Card_ID);
                            row.CreateCell(1).SetCellValue(listcard[r - 1].ICCID);
                            row.CreateCell(2).SetCellValue(listcard[r - 1].Months);
                            row.CreateCell(3).SetCellValue(listcard[r - 1].MonthFlow.ToString());
                            row.CreateCell(4).SetCellValue(listcard[r - 1].SetmealName);
                            row.CreateCell(5).SetCellValue(listcard[r - 1].CustomerCompany);
                            row.CreateCell(6).SetCellValue(listcard[r - 1].Card_Remarks);
                        }
                    }
                    if (i > 0)
                    {
                        int j = i + 1;
                        string name = "sheet" + j.ToString();
                        ISheet sheets = workbook.CreateSheet(name);
                        IRow row0 = sheets.CreateRow(1);
                        row0.CreateCell(0).SetCellValue("物联卡号");
                        row0.CreateCell(1).SetCellValue("ICCID");
                        row0.CreateCell(2).SetCellValue("月份");
                        row0.CreateCell(3).SetCellValue("月使用流量MB");
                        row0.CreateCell(4).SetCellValue("套餐名称");
                        row0.CreateCell(5).SetCellValue("所属公司");
                        row0.CreateCell(6).SetCellValue("备注");
                        listcard = listcard.ToList().Skip(65534).ToList();
                        //for (int r = 1; r <cards.Count()-65535; r++)
                        for (int r = 1; r < listcard.Count() - 1; r++)
                        {
                            //创建行row
                            IRow row = sheets.CreateRow(r);
                            row.CreateCell(0).SetCellValue(listcard[r - 1].Card_ID);
                            row.CreateCell(1).SetCellValue(listcard[r - 1].ICCID);
                            row.CreateCell(2).SetCellValue(listcard[r - 1].Months);
                            row.CreateCell(3).SetCellValue(listcard[r - 1].MonthFlow.ToString());
                            row.CreateCell(4).SetCellValue(listcard[r - 1].SetmealName);
                            row.CreateCell(5).SetCellValue(listcard[r - 1].CustomerCompany);
                            row.CreateCell(6).SetCellValue(listcard[r - 1].Card_Remarks);
                        }
                    }
                }
                //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
                //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
                //自动生成文件名称
                string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
                FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
                // FilePath = @"D:Files/" + FileName;
                using (FileStream url = File.OpenWrite(FilePath))
                {
                    //导出Excel文件
                    workbook.Write(url);
                };
            }
            return FilePath;
        }

        ///<summary>
        ///查看僵尸卡数据信息
        /// </summary>
        public static DieCardDto GetDieCardInfo (string Company_ID,string Card_ICCID)
        {
            DieCardDto dto = new DieCardDto();
            List<diecard> diecards = new List<diecard>();
            try
            {
                string sql = string.Empty;
                string Card_Remarks = string.Empty;
                string SetMealName = string.Empty;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (Company_ID == "1556265186243")//奇迹物联
                    {
                        sql = "select * from diecard";
                        if (!string.IsNullOrWhiteSpace(Card_ICCID))
                        {
                            sql = sql + " where Card_ICCID='" + Card_ICCID + "'";  
                        }
                        var listdiecard = conn.Query<diecard>(sql).ToList();
                        foreach (var item in listdiecard)
                        {
                            diecard cards = new diecard();
                            //卡备注信息和套餐编号
                            string sqlcard = "select Card_Remarks,SetMealID2 from card where Card_ICCID='" + item.Card_ICCID+"'";
                            var listcardinfo = conn.Query<Card>(sqlcard).FirstOrDefault();
                            if (listcardinfo != null)
                            {
                                Card_Remarks = listcardinfo.Card_Remarks;
                                string sqlsetmeal = "select PackageDescribe as SetMealName from setmeal where SetmealID='"+listcardinfo.SetmealID2+"'";
                                SetMealName = conn.Query<diecard>(sqlsetmeal).Select(t => t.SetMealName).FirstOrDefault();
                            }
                            cards.SetMealName = SetMealName;
                            cards.Card_Remarks = Card_Remarks;
                            cards.Card_ICCID = item.Card_ICCID;
                            cards.Card_ID = item.Card_ID;
                            cards.Card_EndTime = item.Card_EndTime;
                            cards.Card_ActivationDate = item.Card_ActivationDate;
                            cards.LdleTime = "大于等于3";
                            cards.OperatorName = "中国移动";
                            diecards.Add(cards);
                        }
                        dto.diecards = diecards;
                        dto.flg = "1";
                        dto.Msg = "成功!";
                    }
                    else
                    {
                        sql= "select * from  diecard where Card_CompanyID = '" + Company_ID + "'";
                        if (!string.IsNullOrWhiteSpace(Card_ICCID))
                        {
                            sql = sql + " and Card_ICCID='" + Card_ICCID + "'";
                        }
                        var listdiecard = conn.Query<diecard>(sql).ToList();
                        foreach (var item in listdiecard)
                        {
                            diecard cards = new diecard();
                            //卡备注信息和套餐编号
                            string sqlcard = "select Card_Remarks,SetMealID2 from card_copy1 where Card_ICCID='" + item.Card_ICCID + "'";
                            var listcardinfo = conn.Query<Card>(sqlcard).FirstOrDefault();
                            if (listcardinfo != null)
                            {
                                Card_Remarks = listcardinfo.Card_Remarks;
                                string sqlsetmeal = "select PackageDescribe as SetMealName from setmeal where SetmealID='" + listcardinfo.SetmealID2 + "'";
                                SetMealName = conn.Query<diecard>(sqlsetmeal).Select(t => t.SetMealName).FirstOrDefault();
                            }
                            cards.SetMealName = SetMealName;
                            cards.Card_Remarks = Card_Remarks;
                            cards.Card_ICCID = item.Card_ICCID;
                            cards.Card_ID = item.Card_ID;
                            cards.Card_EndTime= item.Card_EndTime;
                            cards.Card_ActivationDate = item.Card_ActivationDate;
                            cards.LdleTime = "大于等于3";
                            cards.OperatorName = "中国移动";
                            diecards.Add(cards);
                        }
                        dto.diecards = diecards;
                        dto.flg = "1";
                        dto.Msg = "成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                dto.diecards = null;
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }

        /// <summary>
        /// 用户登录弹出是否有僵尸卡信息
        /// </summary>
        /// <param name="Company_ID"></param>
        /// <returns></returns>
        public static DieCardCountInfo GetDieCardCountInfo(string Company_ID)
        {
            DieCardCountInfo info = new DieCardCountInfo();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    if (Company_ID == "1556265186243")
                    {
                        info.IsDieCard = true;
                    }
                    else
                    {
                        string sql = "select * from diecard where Card_CompanyID='"+Company_ID+"'";
                        var listdiecard = conn.Query<diecard>(sql).ToList();
                        if (listdiecard.Count > 0)
                        {
                            info.IsDieCard = true;
                        }
                        else
                        {
                            info.IsDieCard = false;
                        }
                    }
                    info.flg = "1";
                    info.Msg = "成功!";
                        
                }
                catch (Exception ex)
                {
                    info.flg = "-1";
                    info.Msg = "失败!"+ex;
                }
            }
            return info;
        }


        ///<summary>
        ///生产抛料对比
        /// </summary>
        public static PaoLiaoDto GetPaoLiaoInfo(ICCIDSS para)
        {
            PaoLiaoDto dto = new PaoLiaoDto();
            try
            {
                if (para.card1.Count > 0 && para.card2.Count > 0)
                {
                    dto.getpaoliaos = para.card1.Where(t => !para.card2.Any(x => t.ICCID == x.ICCID)).ToList();
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
                else
                {
                    dto.flg = "-1";
                    dto.Msg = "上传的excel中没有数据,请确认后重新上传数据!";
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
        ///获取运营商  //OperatorID  1：移动  2:电信 3:联通 4:全网通卡 5:漫游卡
        /// </summary>
        public static AccountDto GetAccountInfo(string OperatorID)
        {
            AccountDto dto = new AccountDto();
            try
            {
                string sql = string.Empty; 
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (OperatorID == "1")//移动
                    {
                        sql = "select Remark,accountID,Company_ID from accounts  where Platform=11 or Platform=10 ";
                        var listcmcc = conn.Query<accountsinfo>(sql).ToList();
                        dto.accountsinfos = listcmcc.Where(t => t.Company_ID == "1556265186243").ToList();
                    }
                    if (OperatorID == "2")//电信
                    {
                        sql = "select Remark,accountID,Company_ID from accounts where Company_ID='1556265186243'  and Platform=21";
                        dto.accountsinfos = conn.Query<accountsinfo>(sql).ToList();
                    }
                    if (OperatorID == "3")//联通
                    {
                        sql = "select Remark,accountID,Company_ID from accounts where Company_ID='1556265186243' and Platform=31";
                        dto.accountsinfos = conn.Query<accountsinfo>(sql).ToList();
                    }
                    //dto.accountsinfos = conn.Query<accountsinfo>(sql).ToList();
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
        ///区域管控查询
        /// </summary>
        public static Regions GetRegionInfo (string Company_ID,string Value)
        {
            Regions info = new Regions();
            try
            {
                string sql = "select Card_ID,Card_ICCID,Card_IMSI,CompanyName,Card_Remarks,RegionLimitStatus,Card_CompanyID from card_copy1 t1 left join company t2 on t1.Card_CompanyID=t2.CompanyID  where t1.RegionLimitStatus='1'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (Company_ID == "1556265186243")
                    {
                        if (!string.IsNullOrWhiteSpace(Value))
                        {
                            Value = Regex.Replace(Value, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                            sql = sql + " and Card_ID='" + Value + "' or Card_ICCID='" + Value + "' and Card_IMSI='" + Value + "'";
                            info.dtos = conn.Query<RegionDto>(sql).ToList();
                        }
                        else
                        {
                            info.dtos = conn.Query<RegionDto>(sql).ToList();
                        }
                        info.Msg = "成功!";
                        info.flg = "1";
                    }
                    else
                    {
                        sql = "select Card_ID,Card_ICCID,Card_IMSI,CompanyName,Card_Remarks,RegionLimitStatus,Card_CompanyID from card_copy1 t1 left join company t2 on t1.Card_CompanyID=t2.CompanyID  where t1.RegionLimitStatus='1' and t1.Card_CompanyID='" + Company_ID + "'";
                        if (!string.IsNullOrWhiteSpace(Value))
                        {
                            Value = Regex.Replace(Value, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                            sql = sql + "  and Card_ID='" + Value + "' or Card_ICCID='" + Value + "' and Card_IMSI='" + Value + "'";
                            info.dtos = conn.Query<RegionDto>(sql).ToList();
                        }
                        else
                        {
                            info.dtos = conn.Query<RegionDto>(sql).ToList();
                        }
                        info.Msg = "成功!";
                        info.flg = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                info.dtos = null;
                info.Msg = "失败:"+ex;
                info.flg = "-1";
            }
            return info;
        }

        ///<summary>
        ///修改标签
        /// </summary>
        public static Information UpdateRegionLable(UpdateRegionLablePara para)
        {
            Information info = new Information();
            string updatelable = string.Empty;
            string iccid = string.Empty;
            string sn = string.Empty;
            int i = 0;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    try
                    {
                        if (para.Company_ID == "1556265186243")//奇迹物联
                        {
                            if (para.OperatorsFlg == "1")//移动
                            {
                                foreach (var item in para.lables)
                                {
                                    //只保留字母、数字 和汉字
                                    iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    //过滤任何制表符和空格如\t\n空格等
                                    //para.lables[i].ICCID=Regex.Replace(para.lables[i].ICCID, @"\s", "");
                                    updatelable = "update card  set RegionLabel='" + para.lables[i].标签 + "' where Card_ICCID='" + iccid + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功"+para.lables.Count.ToString()+"条数据";
                            }
                            if (para.OperatorsFlg == "2")//电信
                            {
                                foreach (var item in para.lables)
                                {
                                    iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update ct_card  set RegionLabel='" + para.lables[i].标签 + "' where Card_ICCID='" + iccid + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                            if (para.OperatorsFlg == "3")//联通
                            {
                                foreach (var item in para.lables)
                                {
                                    iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update cucc_card  set RegionLabel='" + para.lables[i].标签 + "' where Card_ICCID='" + iccid + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                            if (para.OperatorsFlg == "4")//全网通
                            {
                                foreach (var item in para.lables)
                                {
                                    sn = Regex.Replace(para.lables[i].SN, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update three_card  set RegionLabel='" + para.lables[i].标签 + "' where SN='" +sn + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                            if (para.OperatorsFlg == "5")//漫游
                            {
                                foreach (var item in para.lables)
                                {
                                    iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update roamcard  set RegionLabel='" + para.lables[i].标签 + "' where Card_ICCID='" + iccid + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                        }
                        else //非奇迹
                        {
                            if (para.OperatorsFlg == "1")//移动
                            {
                                foreach (var item in para.lables)
                                {
                                    iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update card_copy1  set RegionLabel='" + para.lables[i].标签 + "' where Card_ICCID='" + iccid + "' and Card_CompanyID='"+para.Company_ID+"'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                            if (para.OperatorsFlg == "2")//电信
                            {
                                foreach (var item in para.lables)
                                {
                                    iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update ct_cardcopy  set RegionLabel='" + para.lables[i].标签 + "' where Card_ICCID='" +iccid + "' and Card_CompanyID='" + para.Company_ID + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                            if (para.OperatorsFlg == "3")//联通
                            {
                                foreach (var item in para.lables)
                                {
                                    iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update cucc_cardcopy  set RegionLabel='" + para.lables[i].标签 + "' where Card_ICCID='" + iccid + "' and Card_CompanyID='" + para.Company_ID + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                            if (para.OperatorsFlg == "4")//全网通
                            {
                                foreach (var item in para.lables)
                                {
                                    sn = Regex.Replace(para.lables[i].SN, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update three_cardcopy  set RegionLabel='" + para.lables[i].标签 + "' where SN='" + sn + "' and Card_CompanyID='" + para.Company_ID + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                            if (para.OperatorsFlg == "5")//漫游
                            {
                                foreach (var item in para.lables)
                                {
                                    iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                    updatelable = "update roamcard_copy  set RegionLabel='" + para.lables[i].标签 + "' where Card_ICCID='" + iccid + "' and Card_CompanyID='" + para.Company_ID + "'";
                                    conn.Execute(updatelable);
                                    i++;
                                }
                                info.flg = "1";
                                info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        info.flg = "-1";
                        info.Msg = "失败:"+ex;
                    }
                    
                }
                    
            }
            catch (Exception ex)
            {

            }
            return info;
        }

        ///<summary>
        ///根据卡ICCID查看卡的会话信息列表
        /// </summary>
        public static GetCardHuihuaDto GetCardHuiHuaInfo(string Card_ICCID)
        {
            GetCardHuihuaDto dto = new GetCardHuihuaDto();
            try
            {
                string sql = "select case when Card_WorkState='01' then '在线' else '离线' end  as Card_WorkState,Card_ID,Card_ICCID,CreateDate,ip from  tongxin where  Card_ICCID='"+Card_ICCID+"' ORDER BY UpdateTime DESC";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    dto.CardActionInfos = conn.Query<CardHuiHuanInfo>(sql).ToList();
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败!"+ex;
            }
            return dto;
        }

        ///<summary>
        ///修改使用场景
        /// </summary>
        public static Information UpdateCardScene(UpdateRegionLablePara para)
        {
            Information info = new Information();
            string updatelable = string.Empty;
            string updatescenecard = string.Empty;
            string iccid = string.Empty;
            string sn = string.Empty;
            int i = 0;
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    if (para.Company_ID == "1556265186243")//奇迹物联
                    {
                        if (para.OperatorsFlg == "1")//移动
                        {
                            foreach (var item in para.lables)
                            {
                                //只保留字母、数字 和汉字
                                iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                //过滤任何制表符和空格如\t\n空格等
                                //para.lables[i].ICCID=Regex.Replace(para.lables[i].ICCID, @"\s", "");
                                updatelable = "update card  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "'";
                                conn.Execute(updatelable);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                        if (para.OperatorsFlg == "2")//电信
                        {
                            foreach (var item in para.lables)
                            {
                                iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update ct_card  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "'";
                                conn.Execute(updatelable);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                        if (para.OperatorsFlg == "3")//联通
                        {
                            foreach (var item in para.lables)
                            {
                                iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update cucc_card  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "'";
                                conn.Execute(updatelable);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                        if (para.OperatorsFlg == "4")//全网通
                        {
                            foreach (var item in para.lables)
                            {
                                sn = Regex.Replace(para.lables[i].SN, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update three_card  set Scene='" + para.lables[i].使用场景 + "' where SN='" + sn + "'";
                                conn.Execute(updatelable);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                        if (para.OperatorsFlg == "5")//漫游
                        {
                            foreach (var item in para.lables)
                            {
                                iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update roamcard  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "'";
                                conn.Execute(updatelable);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                    }
                    else //非奇迹
                    {
                        if (para.OperatorsFlg == "1")//移动
                        {
                            foreach (var item in para.lables)
                            {
                                iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update card_copy1  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "' and Card_CompanyID='" + para.Company_ID + "'";
                                updatescenecard= "update card  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "'";
                                conn.Execute(updatelable);
                                conn.Execute(updatescenecard);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                        if (para.OperatorsFlg == "2")//电信
                        {
                            foreach (var item in para.lables)
                            {
                                iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update ct_cardcopy  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "' and Card_CompanyID='" + para.Company_ID + "'";
                                updatescenecard = "update ct_card  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "'";
                                conn.Execute(updatelable);
                                conn.Execute(updatescenecard);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                        if (para.OperatorsFlg == "3")//联通
                        {
                            foreach (var item in para.lables)
                            {
                                iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update cucc_cardcopy  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "' and Card_CompanyID='" + para.Company_ID + "'";
                                updatescenecard = "update cucc_card  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "'";
                                conn.Execute(updatelable);
                                conn.Execute(updatescenecard);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                        if (para.OperatorsFlg == "4")//全网通
                        {
                            foreach (var item in para.lables)
                            {
                                sn = Regex.Replace(para.lables[i].SN, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update three_cardcopy  set Scene='" + para.lables[i].使用场景 + "' where SN='" + sn + "' and Card_CompanyID='" + para.Company_ID + "'";
                                updatescenecard = "update three_card  set Scene='" + para.lables[i].使用场景 + "' where SN='" + sn + "'";
                                conn.Execute(updatelable);
                                conn.Execute(updatescenecard);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                        if (para.OperatorsFlg == "5")//漫游
                        {
                            foreach (var item in para.lables)
                            {
                                iccid = Regex.Replace(para.lables[i].ICCID, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
                                updatelable = "update roamcard_copy  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "' and Card_CompanyID='" + para.Company_ID + "'";
                                updatescenecard = "update roamcard  set Scene='" + para.lables[i].使用场景 + "' where Card_ICCID='" + iccid + "'";
                                conn.Execute(updatelable);
                                conn.Execute(updatescenecard);
                                i++;
                            }
                            info.flg = "1";
                            info.Msg = "生效成功" + para.lables.Count.ToString() + "条数据";
                        }
                    }
                }
                catch (Exception ex)
                {
                    info.flg = "-1";
                    info.Msg = "失败:" + ex;
                }

            }
            return info;
        }
    }
}
