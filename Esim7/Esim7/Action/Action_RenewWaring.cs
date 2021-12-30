using Dapper;
using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Esim7.UNity;
using static Esim7.Models.Model_RenewWaring;
using System.Text.RegularExpressions;

namespace Esim7.Action
{/// <summary>
/// 续费预警
/// </summary>
    public class Action_RenewWaring
    {
        public static Waring GetWaring()
        {
            Waring w = new Waring();
            List<Message_ForHoutai> Gonghai = new List<Message_ForHoutai>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = @"select t1.Card_ICCID as ICCID,     DATE_FORMAT( t1.Card_EndTime ,'%Y-%m-%d %H:%i:%s') as Endtime,t1.OutofstockID as OutofID, t4.batchID as productID  ,t6.CardXTName as CardXingtai ,t5.CardTypeName as CardTypeName,t7.operatorsName as operatorsName
                                from card t1 left join outofstock t2 on t1.OutofstockID=t2.OutofstockID left join taocan  t3 on t3.SetmealID=t2.SetmealID left join product t4 on t2.BatchID=t4.BatchID
                                left  join cardtype t5 on t5.CardTypeID=t3.CardTypeID left join card_xingtai t6 on t3.CardxingtaiID=t6.CardXTID left join operators t7 on t7.operatorsID=t3.OperatorsID
                                where t1.isout<>1 and  (TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW()))<=60";
                w.Gonghai = conn.Query<Message_ForHoutai>(sql2).ToList();
            }
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = @"select t1.Card_ICCID as ICCID,   DATE_FORMAT( t8.Card_EndTime ,'%Y-%m-%d %H:%i:%s')  as Endtime,t8.OutofstockID as OutofID, t4.batchID as productID  ,t6.CardXTName as CardXingtai ,t5.CardTypeName as CardTypeName,t7.operatorsName as operatorsName,
                                DATE_FORMAT( t1.Card_EndTime ,'%Y-%m-%d %H:%i:%s')       as CustomEndDate,t9.CompanyID as CustomID,t9.CompanyName as CustomName from card_copy1 t1 left  join card t8 on t8.Card_ICCID=t1.Card_ICCID 
                                left join outofstock t2 on t8.OutofstockID=t2.OutofstockID left join taocan  t3 on t3.SetmealID=t2.SetmealID left join product t4 on t2.BatchID=t4.BatchID
                                left  join cardtype t5 on t5.CardTypeID=t3.CardTypeID left join card_xingtai t6 on t3.CardxingtaiID=t6.CardXTID left join operators t7 on t7.operatorsID=t3.OperatorsID
                                left join company t9 on  t9.CompanyID=t1.Card_CompanyID where  ((TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW()))<=60|| (TO_DAYS(NOW()) -TO_DAYS(t1.Card_EndTime))<=60)";
                w.Custom = conn.Query<Message_ForHoutai>(sql2).ToList();
            }
            return w;
        }

        /// <summary>
        /// 客户续费预警  客户登录时显示
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<Message_ForHoutai> Waring_ForCustom(string CompanyID)
        {
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = @"select  count(1) as Number ,t9.CompanyPhone  as Phone,
                             t9.CompanyID as CompanyID,t9.CompanyName as CompanyName ,(select count(1)from card_copy1 where Card_CompanyID='" + CompanyID + @"')as Total
                             from card_copy1 t1 left join card t8 on t8.Card_ICCID=t1.Card_ICCID left join outofstock t2 on t8.OutofstockID=t2.OutofstockID
                             left join taocan  t3 on t3.SetmealID=t2.SetmealID left  join cardtype t5 on t5.CardTypeID=t3.CardTypeID
                             left join card_xingtai t6 on t3.CardxingtaiID=t6.CardXTID left join company t9 on  t9.CompanyID=t1.Card_CompanyID
                             where TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60  and t1.Card_CompanyID='" + CompanyID + "'";
                return conn.Query<Message_ForHoutai>(sql2).ToList();
            }           
        }
        #region   大总结
        /// <summary>
        /// 客户续费预警  客户登录时显示
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static object Waring_total1(string CompanyID)
        {
            //客户看到的东西
            if (CompanyID != "1556265186243")
            {
                Waring_Total_Custom v = new Waring_Total_Custom();
                v.Type = "2";
                ///获取     客户端的截止日期
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"select  count(1) as Number ,t9.CompanyPhone  as Phone,
 t9.CompanyID as CompanyID,t9.CompanyName as CompanyName ,(select count(1)from card_copy1 where Card_CompanyID='" + CompanyID + @"')as Total
from card_copy1 t1 
left join company t9 on  t9.CompanyID=t1.Card_CompanyID
where     TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60          
and t1.Card_CompanyID='" + CompanyID + "'";
                    Message_ForCustom message_ForCustom = new Message_ForCustom();
                    message_ForCustom.List_card_Custom = new List<Card_ForWaring_Custom3>();
                    foreach (Message_ForHoutai_return item in conn.Query<Message_ForHoutai_return>(sql2).ToList())
                    {
                        message_ForCustom.CompanyID = item.CompanyID;
                        message_ForCustom.CompanyName = item.CompanyName;
                        message_ForCustom.Number = item.Number;
                        message_ForCustom.Phone = item.Phone;
                        message_ForCustom.Total = item.Total;
                        using (IDbConnection conns = DapperService.MySqlConnection())

                        {
                            string sql22 = @"select    t1.Card_ICCID  as ICCID ,t1.Card_ID,t9.CompanyID,t9.CompanyName  as CustomName,date_format(t1.Card_EndTime  , '%Y-%m-%d')  as  Custom_EndTime
from card_copy1 t1 
left join company t9 on  t9.CompanyID=t1.Card_CompanyID
where     TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60  and t1.Card_CompanyID='" + CompanyID + "'";
                           message_ForCustom.List_card_Custom= conns.Query<Card_ForWaring_Custom3>(sql22).ToList();
                        }
                        v.Custom = new List<Message_ForCustom>();
                        v.Custom.Add(message_ForCustom);                             
                    }
                }
                return v;
            }
            //后台看的东西
            else if (CompanyID == "1556265186243")
            {
                waringTotal_Houtai v = new waringTotal_Houtai();
                v.Type = "1";
                List<Message_ForHoutai_return> Message_ForHoutai = new List<Message_ForHoutai_return>();
                   List<Message_ForHoutai> Message_Cards_Houtai = new List<Message_ForHoutai>();
                //获取  真实数据需要续费的             
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"select   COUNT(1) as Total, t1.Card_EndTime, IFNULL(t10.CompanyID,'1556265186243') as CompanyID,(select COUNT(1) from card ) as Number
  ,IFNULL(t10.CompanyName , '奇迹物联') as CompanyName    ,t10.CompanyPhone as Phone
from card t1
left join company t10  on t10.CompanyID=t1.Card_CompanyID
where   t1.status=1 and  TO_DAYS(t1.Card_EndTime)-TO_DAYS(NOW())<=60 ";    // GROUP BY  t1.Card_EndTime
                    foreach (Message_ForCustom item in conn.Query<Message_ForCustom>(sql2).ToList())
                    {
                        Message_ForHoutai_return r = new Message_ForHoutai_return();
                        r.CompanyID = item.CompanyID;
                        r.CompanyName = item.CompanyName;
                        r.Number = item.Number;
                        r.Phone = item.Phone;
                        r.Total = item.Total;
                        r.List_card_Custom = new List<Message_ForHoutai>();
                        //获取 数据card信息
                        using (IDbConnection conns = DapperService.MySqlConnection())
                        {
                            string sql2s = @"select  t1.Card_ICCID as ICCID,t1.Card_ID,t1.Card_IMSI,  date_format(t1.Card_EndTime, '%Y-%m-%d') as RealEndTime,t10.CompanyName,IFNULl(t12.CompanyName,'公海') as CustomName,IFNULl(date_format(t11.Card_EndTime , '%Y-%m-%d'),'未分配') as Custom_EndTime,t5.CardTypeName,t7.operatorsName,t4.batchID AS ProductID,
t3.SetmealName ,FROM_UNIXTIME(t4.batchID/1000,'%Y-%m-%d') as ProductDate,t6.CardXTName as CardXingtai
from card t1
left join outofstock t2 on t1.OutofstockID=t2.OutofstockID
left join taocan  t3 on t3.SetmealID=t2.SetmealID
left join product t4 on t2.BatchID=t4.BatchID

left  join cardtype t5 on t5.CardTypeID=t3.CardTypeID
left join card_xingtai t6 on t3.CardxingtaiID=t6.CardXTID

left join operators t7 on t7.operatorsID=t3.OperatorsID
 
left join company t10  on t10.CompanyID=t1.Card_CompanyID
left join card_copy1 t11 on t11.Card_ICCID=t1.Card_ICCID
left join company t12 on t12.CompanyID=t11.Card_CompanyID
where     TO_DAYS(t1.Card_EndTime)-TO_DAYS(NOW())<=60 ";
                            r.List_card_Custom = conns.Query<Message_ForHoutai>(sql2s).ToList();                          
                        }
                        v.Message_ForHoutai = new List<Message_ForHoutai_return>();
                        v.Message_ForHoutai.Add(r);
                    } 
                }

                //前台需要续费的
                v.Message_Custom = new List<Message_ForCustom2>();
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"select COUNT(1) as Total, t1.Card_EndTime, IFNULL(t10.CompanyID, '1556265186243') as CompanyID,t3.Total as Number
                     ,IFNULL(t10.CompanyName, '奇迹物联') as CompanyName   ,t10.CompanyPhone as Phone
                    from card_copy1 t1
                    left
                    join company t10  on t10.CompanyID = t1.Card_CompanyID
                    left
                    join (select COUNT(1) as Total, t1.Card_EndTime, IFNULL(t10.CompanyID, '1556265186243') as CompanyID
                      ,IFNULL(t10.CompanyName, '奇迹物联') as CompanyName
                    from card_copy1 t1
                    left
                    join company t10  on t10.CompanyID = t1.Card_CompanyID
                    GROUP  BY t1.Card_CompanyID)
                    t3 on t3.CompanyID = t1.Card_CompanyID
                    where TO_DAYS(t1.Card_EndTime)-TO_DAYS(NOW()) <= 60 GROUP BY t1.Card_CompanyID";

                   List< Message_ForCustom > middle
                = conn.Query<Message_ForCustom>(sql2).ToList();                   
                    foreach (Message_ForCustom item in middle)
                    {
                        Message_ForCustom2 message_ForCustom2_Middel = new Message_ForCustom2();
                        message_ForCustom2_Middel.CompanyID = item.CompanyID;
                        message_ForCustom2_Middel.CompanyName = item.CompanyName;
                        message_ForCustom2_Middel.Number = item.Number;
                        message_ForCustom2_Middel.Phone = item.Phone;
                        message_ForCustom2_Middel.Total = item.Total;
                        message_ForCustom2_Middel.List_card_Custom = new List<Card_ForWaring_Custom>();
                        using (IDbConnection conns = DapperService.MySqlConnection())
                        {   
                            string sql22 = @"select    t1.Card_ICCID  as ICCID ,t1.Card_ID,t9.CompanyID,t9.CompanyName  as CustomName,date_format(t1.Card_EndTime, '%Y-%m-%d') as Custom_EndTime
from card_copy1 t1 
left join company t9 on  t9.CompanyID=t1.Card_CompanyID
where     TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60  and t1.Card_CompanyID='" + item.CompanyID + "'" ;

                            //   List<Card_ForWaring_Custom> List_card_Custom22 = conn.Query<Card_ForWaring_Custom>(sql22).ToList();
                            foreach (Card_ForWaring_Custom items in conn.Query<Card_ForWaring_Custom>(sql22).ToList())
                            {

                                Card_ForWaring_Custom v2 = new Card_ForWaring_Custom();
                                v2.Custom_EndTime = items.Custom_EndTime;
                                v2.Card_ID = items.Card_ID;
                                v2.CompanyID = items.CompanyID;
                                v2.CustomName = items.CustomName;
                                v2.ICCID = items.ICCID;
                                message_ForCustom2_Middel.List_card_Custom.Add(v2);
                            }
                            v.Message_Custom.Add(message_ForCustom2_Middel);                           
                        }
                    }
                }

                return v;
            }
            else
            {
                return "error";
            }
        }
        #endregion

        /// <summary>
        /// 客户续费预警  客户登录时显示
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static object Waring_total(string CompanyID)
        {
            object s = new object();
            CustomMessage c = new CustomMessage();
            NewModel newModel = new NewModel();
            List<ForHoutai> forHoutais = new List<ForHoutai>();
            List<Custom> custom = new List<Custom>();
            string sql = "";
            string sql1 = string.Empty;
            //是客户查看
            if (CompanyID != "1556265186243")
            {
                c.Type = 2;
                ///获取     客户端的截止日期
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    sql = @"select  count(1) as Number ,t9.CompanyPhone  as Phone,
                    t9.CompanyID as CompanyID,t9.CompanyName as CompanyName ,(select count(1)from card_copy1 where Card_CompanyID='" + CompanyID + @"' and status=1)as Total
                    from card_copy1 t1 left join company t9 on  t9.CompanyID=t1.Card_CompanyID where  TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60 and t1.Card_CompanyID='" + CompanyID + "' and t1.status=1";
                    List<ForHoutai> Customs = conn.Query<ForHoutai>(sql).ToList();
                    foreach (ForHoutai item in Customs)
                    {
                        ForHoutai customs = new ForHoutai();
                        if (!string.IsNullOrWhiteSpace(item.CompanyID))
                        {
                            customs.CompanyID = item.CompanyID;
                            customs.CompanyName = item.CompanyName;
                            customs.Number = item.Number;
                            customs.Phone = item.Phone;
                            customs.Total = item.Total;
                            forHoutais.Add(customs);
                        }
                        else
                        {
                            string sqlcompany = "select CompanyID,CompanyName,CompanyPhone as Phone from company where CompanyID='"+CompanyID+"'";
                            var itemcom = conn.Query<ForHoutai>(sqlcompany).FirstOrDefault();
                            if (itemcom != null)
                            {
                                customs.CompanyID = itemcom.CompanyID;
                                customs.CompanyName = itemcom.CompanyName;
                                customs.Number = item.Number;
                                customs.Phone = itemcom.Phone;
                                customs.Total = item.Total;
                                forHoutais.Add(customs);
                            }
                        }
                    }
                    c.Custom = forHoutais;
                    s = c;
                }
            }
            else if (CompanyID == "1556265186243")//后台查看
            {
                newModel.Type = 1;

                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //sql1 = @"select   COUNT(1) as Number, t1.Card_EndTime, IFNULL(t10.CompanyID,'1556265186243') as CompanyID,(select COUNT(1) from card ) as Total
                    //,IFNULL(t10.CompanyName , '奇迹物联') as CompanyName    ,t10.CompanyPhone as Phone from card t1
                    //left join company t10  on t10.CompanyID=t1.Card_CompanyID
                    //where   t1.status=1 and  TO_DAYS(t1.Card_EndTime)-TO_DAYS(NOW())<=60 ";
                    //List<ForHoutai> houtais = conn.Query<ForHoutai>(sql1).ToList();
                    //foreach (ForHoutai item in houtais)
                    //{
                    //    ForHoutai forHoutai = new ForHoutai();
                    //    forHoutai.CompanyID = item.CompanyID;
                    //    forHoutai.CompanyName = item.CompanyName;
                    //    forHoutai.Number = item.Number;
                    //    forHoutai.Phone = item.Phone;
                    //    forHoutai.Total = item.Total;
                    //    forHoutais.Add(forHoutai);
                    //}
                    sql1 = "SELECT t2.CompanyID,t2.CompanyName,t2.CompanyTolCardNum as Total,sum(TO_DAYS(t1.Card_EndTime)-TO_DAYS(NOW()) <= 60) as Number ,t2.CompanyPhone as Phone from card t1 left join company t2 on t1.Card_CompanyID = t2.CompanyID  where t2.CompanyID = '1556265186243'";
                    newModel.Message_ForHoutai = conn.Query<ForHoutai>(sql1).ToList(); ;
                    //sql = @"select COUNT(1) as Number, t1.Card_EndTime, IFNULL(t10.CompanyID, '1556265186243') as CompanyID,t3.Total as Total
                    // ,IFNULL(t10.CompanyName, '奇迹物联') as CompanyName   ,t10.CompanyPhone as Phone
                    //from card_copy1 t1
                    //left
                    //join company t10  on t10.CompanyID = t1.Card_CompanyID
                    //left
                    //join (select COUNT(1) as Total, t1.Card_EndTime, IFNULL(t10.CompanyID, '1556265186243') as CompanyID
                    //  ,IFNULL(t10.CompanyName, '奇迹物联') as CompanyName
                    //from card_copy1 t1
                    //left
                    //join company t10  on t10.CompanyID = t1.Card_CompanyID
                    //GROUP  BY t1.Card_CompanyID)
                    //t3 on t3.CompanyID = t1.Card_CompanyID
                    //where TO_DAYS(t1.Card_EndTime)-TO_DAYS(NOW()) <= 60 GROUP BY t1.Card_CompanyID";
                    //List<Custom> middle = conn.Query<Custom>(sql).ToList();
                    //foreach (Custom item in middle)
                    //{
                    //    Custom customs = new Custom();
                    //    customs.CompanyID = item.CompanyID;
                    //    customs.CompanyName = item.CompanyName;
                    //    customs.Number = item.Number;
                    //    customs.Phone = item.Phone;
                    //    customs.Total = item.Total;
                    //    custom.Add(customs);
                    //}
                    //newModel.Message_Custom = custom;
                    sql = "select t2.Card_CompanyID as CompanyID,t1.CompanyName,t1.CompanyPhone as Phone,t1.CompanyTolCardNum as Total,sum(TO_DAYS(Card_EndTime)-TO_DAYS(NOW()) <= 60  and t2.`status`=1) as Number from company t1 left join card_copy1 t2 on t1.CompanyID=t2.Card_CompanyID where TO_DAYS(t2.Card_EndTime)-TO_DAYS(NOW()) <= 60 GROUP BY t2.Card_CompanyID";
                    newModel.Message_Custom = conn.Query<Custom>(sql).ToList();
                    //foreach (var item in lists)
                    //{
                    //    string sqls = "select count(1) as Total,sum(TO_DAYS(Card_EndTime)-TO_DAYS(NOW()) <= 60) as Number from card_copy1 where Card_CompanyID='"+item.CompanyID+"'";
                    //    var listss = conn.Query<Custom>(sqls).FirstOrDefault();
                    //    Custom customs = new Custom();
                    //    customs.CompanyID = item.CompanyID;
                    //    customs.CompanyName = item.CompanyName;
                    //    customs.Number = listss.Number;
                    //    customs.Phone = item.Phone;
                    //    customs.Total = listss.Total;
                    //    custom.Add(customs);
                    //}
                    //newModel.Message_Custom = custom;
                    s = newModel;
                }
            }
            return s;
        }

        ///<summary>
        ///续费预警点击数量查看信息
        /// </summary>
        public static object Waring_totalDetail(string CompanyID)
        {
            object s = new object();
            List<WaringDetail> waringDetails = new List<WaringDetail>();                  
            string sql = string.Empty;           
            //是查看客户的预警信息
            if (CompanyID != "1556265186243")
            {                
                //获取客户端的截止日期
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    sql = @"select    t1.Card_ICCID  as ICCID ,t1.Card_ID,t9.CompanyID,t9.CompanyName  as CustomName,date_format(t1.Card_EndTime, '%Y-%m-%d') as Custom_EndTime
                            from card_copy1 t1 left join company t9 on  t9.CompanyID=t1.Card_CompanyID
                            where TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60  and t1.Card_CompanyID='" + CompanyID + "' and t1.status=1";
                    List<WaringDetail> Customs = conn.Query<WaringDetail>(sql).ToList();
                    foreach (WaringDetail item in Customs)
                    {
                        WaringDetail customs = new WaringDetail();
                        customs.CompanyID = item.CompanyID;
                        customs.Card_ID = item.Card_ID;
                        customs.CustomName = item.CustomName;
                        customs.Custom_EndTime = item.Custom_EndTime;
                        customs.ICCID = item.ICCID;
                        waringDetails.Add(customs);
                    }
                    s = waringDetails;
                }
            }
            else if (CompanyID == "1556265186243")//后台查看
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    sql = @"select    t1.Card_ICCID  as ICCID ,t1.Card_ID,t9.CompanyID,t9.CompanyName  as CustomName,date_format(t1.Card_EndTime, '%Y-%m-%d') as Custom_EndTime from card t1 
                          left join company t9 on  t9.CompanyID=t1.Card_CompanyID where TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60  and t1.Card_CompanyID='"+CompanyID+"'";
                    List<WaringDetail> Customs = conn.Query<WaringDetail>(sql).ToList();
                    foreach (WaringDetail item in Customs)
                    {
                        WaringDetail customs = new WaringDetail();
                        customs.CompanyID = item.CompanyID;
                        customs.Card_ID = item.Card_ID;
                        customs.CustomName = item.CustomName;
                        customs.Custom_EndTime = item.Custom_EndTime;
                        customs.ICCID = item.ICCID;
                        waringDetails.Add(customs);
                    }
                    s = waringDetails;                   
                }
            }
            return s;
        }

        /// <summary>
        ///     客户续费预警  客户登录时显示   点击总数时显示
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List_Card_ForWaring Card_ForWarings(string CompanyID)
        {
            List_Card_ForWaring v = new List_Card_ForWaring();
            List<Card_ForWaring_Custom> List_card_Custom = new List<Card_ForWaring_Custom>();
            if (CompanyID != "1556265186243")
            {
                v.Type = "2";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @" select   t1.Card_ICCID as ICCID ,t1.Card_ID as Card_ID,t9.CompanyName ,t9.CompanyID,DATE_FORMAT( t1.Card_EndTime ,'%Y-%m-%d') as Card_EndTime
                                    from card_copy1 t1 left join card t8 on t8.Card_ICCID=t1.Card_ICCID left join outofstock t2 on t8.OutofstockID=t2.OutofstockID
                                    left join taocan  t3 on t3.SetmealID=t2.SetmealID left  join cardtype t5 on t5.CardTypeID=t3.CardTypeID
                                    left join card_xingtai t6 on t3.CardxingtaiID=t6.CardXTID left join company t9 on  t9.CompanyID=t1.Card_CompanyID
                                    where  TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60  and t1.Card_CompanyID='" + CompanyID + "'";
                    v.List_card_Custom = conn.Query<Card_ForWaring_Custom>(sql2).ToList();
                }
            }
            else if (CompanyID == "1556265186243")
            {
                v.Type = "1";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @" select   t1.Card_ICCID as ICCID ,t1.Card_ID as Card_ID,t9.CompanyName ,t9.CompanyID,DATE_FORMAT( t1.Card_EndTime ,'%Y-%m-%d') as Card_EndTime
                                    from card_copy1 t1 left join card t8 on t8.Card_ICCID=t1.Card_ICCID left join outofstock t2 on t8.OutofstockID=t2.OutofstockID
                                    left join taocan  t3 on t3.SetmealID=t2.SetmealID left  join cardtype t5 on t5.CardTypeID=t3.CardTypeID
                                    left join card_xingtai t6 on t3.CardxingtaiID=t6.CardXTID left join company t9 on  t9.CompanyID=t1.Card_CompanyID
                                    where     TO_DAYS(t1.Card_EndTime) -TO_DAYS(NOW())<=60  and t1.Card_CompanyID='" + CompanyID + "'";
                    v.List_card_Custom = conn.Query<Card_ForWaring_Custom>(sql2).ToList();
                }
            }
            return v;
        }
       /// <summary>
       /// 流量预警
       /// </summary>
       /// <returns></returns>
        public static Model_FlowWarming Flow_Waring()
        {
            Model_FlowWarming w = new Model_FlowWarming();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"select   t1.Card_ICCID as ICCID,t2.Flow  as TotalFlow , t1.Card_Monthlyusageflow  as UsedFlow from card  t1 
                                    inner  join   setmeal t2  on t2.SetmealID=t1.SetMealID2 and !ISNULL(t1.Card_Monthlyusageflow) and t2.Flow<>0 and t1.Card_Monthlyusageflow<>''";
                    var s = conn.Query<Model_JiHe.Model_JiHe_Card_Message.FlowWarning>(sql2).ToList();
                    w.CardMessage = from x in s
                                        //上阀值0.8
                                    let usedrate = float.Parse(x.UsedFlow) / float.Parse(x.TotalFlow) / 1024
                                    let UsedFlow = float.Parse(x.UsedFlow) / 1024
                                    orderby x.ICCID
                                    where (float.Parse(x.UsedFlow) / 1024) / float.Parse(x.TotalFlow) > 0.8
                                    select new { x.ICCID, x.TotalFlow, UsedFlow, usedrate };
                }
                w.status = "0";
                w.Message = "OK";
            }
            catch (System.Exception)
            {
                w.status = "1";
                w.Message = "Error";
            }
            return w;
        }


        /// <summary>
        /// 流量预警增加参数
        /// </summary>
        /// <returns></returns>
        public static Model_FlowWarming Flow_Waring1(string CompanyID)
        {
            Model_FlowWarming w = new Model_FlowWarming();
            string sql2 = "";
            string RelationCode = string.Empty;
            string WaringNum = string.Empty;
            string striccids = string.Empty;
            int num = 0;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (CompanyID == "1556265186243")//奇迹物联
                    {
                        sql2 = @"select   t1.Card_ICCID as ICCID,t2.Flow  as TotalFlow , t1.Card_Monthlyusageflow  as UsedFlow from card  t1 
                                    inner  join   setmeal t2  on t2.SetmealID=t1.SetMealID2 and !ISNULL(t1.Card_Monthlyusageflow) and t2.Flow<>0 and t1.Card_Monthlyusageflow<>''";
                        var s = conn.Query<Model_JiHe.Model_JiHe_Card_Message.FlowWarning>(sql2).ToList();
                        w.CardMessage = from x in s
                                            //上阀值0.8
                                        let usedrate = float.Parse(x.UsedFlow) / float.Parse(x.TotalFlow) / 1024
                                        let UsedFlow = float.Parse(x.UsedFlow) / 1024
                                        orderby x.ICCID
                                        where (float.Parse(x.UsedFlow) / 1024) / float.Parse(x.TotalFlow) > 0.8
                                        select new { x.ICCID, x.TotalFlow, UsedFlow, usedrate };
                    }
                    if (CompanyID != "1556265186243")
                    {
                        string sql = "select * from setrules where CompanyID='"+CompanyID+ "' and SetType='1'";
                        WaringNum = conn.Query<GetSetRulesCardInfo>(sql).Select(t => t.WaringNum).FirstOrDefault();
                        var ss = conn.Query<GetSetRulesCardInfo>(sql).ToList();
                        if (ss.Count > 0)//设置了提醒的
                        {
                            foreach (var item in ss)
                            {
                                string sqls = "select * from card_setrules where RelationCode='" + item.RelationCode + "'";
                                
                                var iccids = conn.Query<SetRulesCardInfos>(sqls).ToList();
                                foreach (var items in iccids)
                                {
                                    striccids += "'" + items.ICCID + "',";
                                }
                            }
                            striccids = striccids.Substring(0, striccids.Length - 1);
                            sql2 = @"select t1.Card_ICCID as ICCID,t1.Card_Monthlyusageflow as UsedFlow,(t2.Flow) as TotalFlow from card_copy1 t1 left join setmeal t2 on t1.SetmealID2 = t2.SetmealID
                                    where t1.Card_CompanyID = '" + CompanyID + "'  and !ISNULL(t1.Card_Monthlyusageflow) and t2.Flow<>0 and t1.Card_Monthlyusageflow<>'' and t1.Card_ICCID in(" + striccids+")";
                            var s = conn.Query<Model_JiHe.Model_JiHe_Card_Message.FlowWarning>(sql2).ToList();
                            if (s.Count>0)
                            {
                                w.CardMessage = from x in s
                                                    //上阀值为用户设置的值 
                                                let usedrate = float.Parse(x.UsedFlow) / float.Parse(x.TotalFlow) / 1024
                                                let UsedFlow = float.Parse(x.UsedFlow) / 1024
                                                orderby x.ICCID
                                                where (float.Parse(x.UsedFlow) / 1024) / float.Parse(x.TotalFlow) > float.Parse(WaringNum) / 100//用户传入的阀值
                                                select new { x.ICCID, x.TotalFlow, UsedFlow, usedrate };
                            }
                           
                        }
                        else if(ss.Count==0) //未设置提醒
                        {
                            sql2 = @"select t1.Card_ICCID as ICCID,t1.Card_Monthlyusageflow as UsedFlow,(t2.Flow) as TotalFlow from card_copy1 t1 left join setmeal t2 on t1.SetmealID2 = t2.SetmealID
                                    where t1.Card_CompanyID = '" + CompanyID + "'  and !ISNULL(t1.Card_Monthlyusageflow) and t2.Flow<>0 and t1.Card_Monthlyusageflow<>''";
                                    var s = conn.Query<Model_JiHe.Model_JiHe_Card_Message.FlowWarning>(sql2).ToList();
                                     w.CardMessage = from x in s
                                                //上阀值0.8 默认
                                            let usedrate = float.Parse(x.UsedFlow) / float.Parse(x.TotalFlow) / 1024
                                            let UsedFlow = float.Parse(x.UsedFlow) / 1024
                                            orderby x.ICCID
                                            where (float.Parse(x.UsedFlow) / 1024) / float.Parse(x.TotalFlow) > 0.8
                                            select new { x.ICCID, x.TotalFlow, UsedFlow, usedrate };
                            num = w.CardMessage.Count();
                        }
                        
                        //sql2 = @"select t1.Card_ICCID,t1.Card_Monthlyusageflow as UsedFlow,(t2.Flow * 1024) as TotalFlow from card_copy1 t1 left join setmeal t2 on t1.SetmealID2 = t2.SetmealID
                        // where convert(t1.Card_Monthlyusageflow/1024, SIGNED) > (convert(t2.Flow, SIGNED) * 0.8) and t1.Card_CompanyID = '"+CompanyID+"'";
                   
                    }       
                }
                w.status = "0";
                w.Message = "OK" + num.ToString(); ;
            }
            catch (System.Exception)
            {
                w.status = "1";
                w.Message = "Error";
            }
            return w;
        }

        ///<summary>
        ///设置流量资费提醒规则
        /// </summary>
        public static Information SetRules(SetRulesPara para)
        {
            Information info = new Information();
            DateTime time = DateTime.Now;
            string RelationCode = Unit.GetTimeStamp(time);
            string strsql = string.Empty;
            string RuleType = string.Empty;
            string RuleName = string.Empty;
            string Status ="正常";
            if (para.SetType == "1")
            {
                 RuleType = "实时用量监控类-流量套餐用量";
                 RuleName = "超流量预警"+para.WaringNum+"%";
            }
            if (para.SetType == "2")
            {
                RuleType = "续费日期实时监控";
                RuleName = "资费到期前"+para.WaringNum+"天提示";
            }
            if (para.SetType == "3")
            {
                RuleType = "机卡分离预警";
                RuleName = "机卡分离短信提示";
            }
            try
            {
                string sql = "insert into setrules (CompanyID,Phone,SetType,OperatorsType,RelationCode,AddTime,WaringNum,RuleType,RuleName,Status,Remark) " +
                    "values('"+para.CompanyID+"','"+para.Phone+"','"+para.SetType+"','"+para.OperatorsType+"','"+RelationCode+"','"+time+"','"+para.WaringNum+"','"+RuleType+"','"+RuleName+"','"+Status+"','"+para.Remark+"')";
                StringBuilder sqladdcardsetrules = new StringBuilder("insert into card_setrules (RelationCode,SetType,OperatorsType,ICCID,SN,AddTime) values");
                foreach (var item in para.cards)
                {
                    if (para.OperatorsType!="4")//ICCID
                    {
                        strsql += "('"+RelationCode+"','"+para.SetType+"','"+para.OperatorsType+"','"+item.ICCID+"','"+item.SN+"','"+time+"'),"; 
                    }
                    if (para.OperatorsType == "4")//SN
                    {
                        strsql += "('" + RelationCode + "','" + para.SetType + "','" + para.OperatorsType + "','" + item.ICCID + "','" + item.SN + "','" + time + "'),";
                    }
                }
                sqladdcardsetrules.Append(strsql);
                string sqlcardrules = sqladdcardsetrules.ToString().Substring(0, sqladdcardsetrules.Length - 1);
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sql);
                    if (para.SetType != "3")
                    {
                        conn.Execute(sqlcardrules);
                    }
                    info.Msg = "成功!";
                    info.flg = "1";
                }
            }
            catch (Exception ex)
            {
                string sqldel = "delete from setrules where RelationCode='"+RelationCode+"'";
                string sqlcardrulesdel = "delete from card_setrules where RelationCode='" + RelationCode + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sqldel);
                    conn.Execute(sqlcardrulesdel);
                }
                info.flg = "-1";
                info.Msg = "出现错误:"+ex;
            }
            return info;
        }

        ///<summary>
        ///查看设置流量资费信息和资费流量触发日志
        /// </summary>
        public static SetRulesInfoDto GetSetRulesInfos(SetRulesInfoPara para)
        {
            SetRulesInfoDto info = new SetRulesInfoDto();
            List<SetRules> rules = new List<SetRules>();
            List<SendLog> sendLogs = new List<SendLog>();
            try
            {
                string sqlsetrules = string.Empty;
                if (para.CompanyID == "1556265186243")
                {
                    sqlsetrules = "select * from setrules";
                    if (!string.IsNullOrWhiteSpace(para.RuleName))
                    {
                        sqlsetrules = "select * from setrules where RuleName like '%"+para.RuleName+"%'";
                    }
                }
                if (para.CompanyID != "1556265186243")
                {
                    sqlsetrules = "select * from setrules where CompanyID='" + para.CompanyID + "'";
                    if (!string.IsNullOrWhiteSpace(para.RuleName))
                    {
                        sqlsetrules = "select * from setrules where CompanyID='" + para.CompanyID + "' and RuleName like '%" + para.RuleName + "%'";
                    }
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var listreules = conn.Query<SetRules>(sqlsetrules).ToList().OrderByDescending(t=>t.AddTime).ToList();                   
                    foreach (var item in listreules)
                    {
                        SetRules rs = new SetRules();
                        rs.AddTime = item.AddTime;
                        rs.CompanyID = item.CompanyID;
                        rs.RelationCode = item.RelationCode;
                        rs.RuleName = item.RuleName;
                        rs.RuleType = item.RuleType;
                        rs.SetType = item.SetType;
                        rs.Status = item.Status;
                        rs.Remark = item.Remark;
                        rs.WaringNum = item.WaringNum;
                        rs.Phone = item.Phone;
                        string sqllog = "select  RelationCode,AddTime as SendTime  from timetskslog where RelationCode='" + item.RelationCode+"'";
                        var listlog = conn.Query<SendLog>(sqllog).ToList();
                        foreach (var items in listlog)
                        {
                            List<SendCardInfos> cardInfos = new List<SendCardInfos>();
                            SendLog log = new SendLog();
                            log.id = item.id;
                            log.RuleName = item.RuleName;
                            log.RuleType = item.RuleType;
                            log.SendTime = items.SendTime;
                            log.ReminderMode = items.ReminderMode;
                            
                            string sqlcardinfo = "select * from card_setrules where RelationCode='"+item.RelationCode+"'";
                            var cardinfos = conn.Query<SendCardInfos>(sqlcardinfo).ToList();
                            string cardid = string.Empty;
                            decimal flow = 0;
                            foreach (var itemss in cardinfos)
                            {
                                SendCardInfos sendCard = new SendCardInfos();
                                string sqlcardid = "select Card_ID,Card_Monthlyusageflow from card_copy1 where Card_ICCID='" + itemss.ICCID+"'";
                                var cardinfo = conn.Query<Card>(sqlcardid).FirstOrDefault();
                                if (cardinfo != null)
                                {
                                    cardid = cardinfo.Card_ID;
                                    if (!string.IsNullOrWhiteSpace(cardinfo.Card_Monthlyusageflow))
                                    {
                                         flow = Convert.ToDecimal(cardinfo.Card_Monthlyusageflow) / 1024;
                                    }
                                }
                                sendCard.ICCID = itemss.ICCID;
                                sendCard.Flow = flow.ToString();
                                sendCard.Card_ID = cardid;
                                cardInfos.Add(sendCard);
                            }
                            log.cardInfos = cardInfos;
                            sendLogs.Add(log);
                            rs.sendLogs = sendLogs;
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                        rules.Add(rs);
                        info.rules = rules;
                        
                    }
                    if (listreules.Count < 1)
                    {
                        info.flg = "-2";
                        info.Msg = "暂无数据!";
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
        ///查看设置了提示规则信息的详情(卡列表)
        /// </summary>
        public static RulesDetailInfos GerSetRulesDetail(string RelationCode)
        {
            RulesDetailInfos info = new RulesDetailInfos();
            try
            {
                //string sqlsetrules = "select ICCID,SN,OperatorsType,SetType from card_setrules where RelationCode='" + RelationCode+"'";
                string sqlsetrules = "select ICCID, SN,(case when OperatorsType = '1' then '移动' when OperatorsType = '2' then '电信' when OperatorsType = '3' then '联通' else '全网通' end) OperatorsType," +
                    " (case when SetType = '1'  then '流量'  when SetType = '2' then '资费' else '机卡分离' end)  SetType from card_setrules where RelationCode = '"+RelationCode+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    info.setRules = conn.Query<SetRulesDetailInfo>(sqlsetrules).ToList();
                    info.Msg = "成功" ;
                    info.flg = "1";
                }
            }
            catch (Exception ex)
            {
                info.setRules = null;
                info.Msg = "失败:"+ex;
                info.flg = "-1";
            }
            return info;
        }

        ///<summary>
        ///删除预警规则
        /// </summary>
        public static Information DeleteSetRules(string RelationCode)
        {
            Information info = new Information();
            try
            {
                string sqlrules = "delete from setrules where RelationCode='"+RelationCode+"'";
                string sqlcardrules = "delete from card_setrules where RelationCode='"+RelationCode+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sqlrules);
                    conn.Execute(sqlcardrules);
                    info.Msg = "成功!";
                    info.flg = "1";
                }
            }
            catch (Exception ex)
            {
                info.Msg = "失败!"+ex;
                info.flg = "-1";
            }
            return info;
        }



        ///<summary>
        ///编辑预警规则
        /// </summary>
        public static Information UpdateSetRules(UpdateRulesPara para)
        {
            Information info = new Information();
            try
            {                string waringnum = para.WaringNum;
                string RuleName = string.Empty;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sqlwaring = "select WaringNum,RuleName,SetType from setrules where RelationCode='" + para.RelationCode+"'";
                    var waringinfo = conn.Query<SetRules>(sqlwaring).FirstOrDefault();
                    if (waringinfo != null)
                    {
                        RuleName = waringinfo.RuleName;
                        if (waringinfo.SetType == "1")//编辑流量预警信息
                        {
                            if (waringnum != waringinfo.WaringNum)//设置的比例不相等
                            {
                                RuleName = RuleName.Substring(0, RuleName.Length - 3);
                                RuleName = RuleName + waringnum + "%";
                            }
                        }
                        if (waringinfo.SetType == "2")//编辑资费预警信息
                        {
                            if (waringnum != waringinfo.WaringNum)//设置的天数不相等
                            {
                                RuleName = RuleName.Substring(0, RuleName.Length - 5);
                                RuleName = RuleName + waringnum + "天提示";
                            }
                        } 
                    }
                    string sqlrules = "update setrules set WaringNum='" + para.WaringNum + "',Remark='" + para.Remark + "',Phone='" + para.Phone + "',RuleName='"+ RuleName + "' where RelationCode='" + para.RelationCode + "'";
                    conn.Execute(sqlrules);
                    info.Msg = "成功!";
                    info.flg = "1";
                }
            }
            catch (Exception ex)
            {
                info.Msg = "失败!" + ex;
                info.flg = "-1";
            }
            return info;
        }

        ///<summary>
        ///查看机卡分离数量
        /// </summary>
        public static GetFenLiNum GetFenLiInfo(string CompanyID)
        {
            GetFenLiNum info = new GetFenLiNum();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql = string.Empty;
                    if (CompanyID == "1556265186243")
                    {
                        sql = "select count(id) as Number  from card where status=1 and IsSeparate='1'";
                        info.Number = conn.Query<GetFenLiNum>(sql).Select(t => t.Number).FirstOrDefault();
                        info.Msg = "成功!";
                        info.flg = "1";
                    }
                    else
                    {
                        sql = "select count(id) as Number  from card_copy1 where status=1 and IsSeparate='1' and Card_CompanyID='"+CompanyID+"'";
                        info.Number = conn.Query<GetFenLiNum>(sql).Select(t => t.Number).FirstOrDefault();
                        info.Msg = "成功!";
                        info.flg = "1";
                    }
                }      
            }
            catch (Exception ex)
            {
                info.Number = 0;
                info.Msg = "失败!";
                info.flg = "=1";
            }
            return info;
        }
    }
}