
using Dapper;
using Esim7.Dto;
using Esim7.Models;
using Esim7.ReturnMessage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Esim7.Action
{
    /// <summary>
    ///控制面板信息获取
    /// </summary>
    public class Action_GetEchars
    {
        /// <summary>
        ///获取卡数量
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<Chars_CardNum> GetCardNUM(string CompanyID)
        {
            List<Chars_CardNum> liii = new List<Chars_CardNum>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                if (CompanyID == "1556265186243")
                {
                    //string sql2 = "";
                    //  sql2 = @"select    COUNT(1) AS NUM   , t6.CardTypeID ,t6.CardTypeName from card  t1  
                    //          inner join setmeal t5 on t5.SetMealID=t1.SetMealID2 left join cardtype t6 on t6.CardTypeID=t5.CardTypeID
                    //          WHERE  t1.status=1 GROUP BY t6.CardTypeID UNION all  select COUNT(1) as NUM ,  '1'as CardTypeID , 'Totel' as CardTypeName from card  where status=1";
                    //liii = conn.Query<Chars_CardNum>(sql2).AsList();
                    string nbcmccnum = "select count(*) as num from card t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + CompanyID + "' and t2.CardTypeID = '123'";//NB移动卡数量
                    string nbctnum = "select count(*) as num from ct_card t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + CompanyID + "' and t2.CardTypeID = '123'";//NB移电信数量
                    string nbcuccnum = "select count(*) as num from cucc_card t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + CompanyID + "' and t2.CardTypeID = '123'";//NB联通卡数量
                    string nbthreenum = "select count(*) as num from three_card t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='123'";//NB全网通卡数量
                    string shujucmcc = "select count(*) as num from card t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";// 移动卡2G/4G卡数量
                    string shujuct = "select count(*) as num from ct_card t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";// 电信卡2G/4G卡数量
                    string shujucucc = "select count(*) as num from cucc_card t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";// 联通卡2G/4G卡数量
                    string shujuthree = "select count(*) as num from three_card t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";// 全网通卡2G/4G卡数量
                    string cmccnum = "select count(*) as num from card  where Card_CompanyID='" + CompanyID + "'";// 移动卡数量
                    string ctnum = "select count(*) as num from ct_card  where Card_CompanyID='" + CompanyID + "'";// 电信卡数量
                    string cuccnum = "select count(*) as num from cucc_card  where Card_CompanyID='" + CompanyID + "'";// 联通卡数量
                    string threecardnum = "select count(*) as num from three_card  where Card_CompanyID='" + CompanyID + "'";// 全网通卡数量
                    int cmcc = 0;
                    int cucc = 0;
                    int ct = 0;
                    int three = 0;
                    int nbcmcc = 0;
                    int nbct = 0;
                    int nbcucc = 0;
                    int nbthree = 0;
                    int sjcmcc = 0;
                    int sjcucc = 0;
                    int sjct = 0;
                    int sjthree = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        Chars_CardNum chars = new Chars_CardNum();
                        if (i == 0)
                        {
                            chars.CardTypeID = "123";
                            chars.CardTypeName = "NB";
                            nbcmcc = conn.Query<countnum>(nbcmccnum).Select(t => t.num).FirstOrDefault();
                            nbcucc = conn.Query<countnum>(nbctnum).Select(t => t.num).FirstOrDefault();
                            nbct = conn.Query<countnum>(nbctnum).Select(t => t.num).FirstOrDefault();
                            nbthree = conn.Query<countnum>(nbthreenum).Select(t => t.num).FirstOrDefault();
                            chars.NUM = nbcucc + nbcmcc + nbct + nbthree;
                        }
                        if (i == 1)
                        {
                            chars.CardTypeID = "2147483647";
                            chars.CardTypeName = "2G/4G";
                            sjcmcc = conn.Query<countnum>(shujucmcc).Select(t => t.num).FirstOrDefault();
                            sjct = conn.Query<countnum>(shujuct).Select(t => t.num).FirstOrDefault();
                            sjcucc = conn.Query<countnum>(shujucucc).Select(t => t.num).FirstOrDefault();
                            sjthree = conn.Query<countnum>(shujuthree).Select(t => t.num).FirstOrDefault();
                            chars.NUM = sjcmcc + sjcucc + sjct + sjthree;
                        }
                        if (i == 2)
                        {
                            cmcc = conn.Query<countnum>(cmccnum).Select(t => t.num).FirstOrDefault();
                            ct = conn.Query<countnum>(ctnum).Select(t => t.num).FirstOrDefault();
                            cucc = conn.Query<countnum>(cuccnum).Select(t => t.num).FirstOrDefault();
                            three = conn.Query<countnum>(threecardnum).Select(t => t.num).FirstOrDefault();
                            chars.CardTypeID = "1";
                            chars.CardTypeName = "Totel";
                            chars.NUM = cmcc + ct + cucc + three;
                        }
                        liii.Add(chars);
                    }
                }
                else
                {
                    //string sql3 = string.Empty;
                    //sql3 = @"select COUNT(1) AS NUM,'123' AS CardTypeId,'NB' as CardTypeName from card t1 inner join card_copy1 t7 on t7.Card_ICCID = t1.Card_ICCID
                    //       left join accounts t6 on t6.AccountID = t1.accountsID left join setmeal t2 on t2.SetmealID = t1.setmealId2 left
                    //       join cardtype t3 on t3.CardTypeID = t2.CardTypeID left join card_xingtai t4 on t4.CardXTID = t2.CardXTID left
                    //       join operator t5 on t5.OperatorID = t2.OperatorID where t1.status = 1 and t7.Card_CompanyID = '" + CompanyID + "' AND CardTypeName = 'NB'";
                    //sql3 += @"UNION ALL select COUNT(1) AS NUM,'2147483647' AS CardTypeId,'2G/4G' as CardTypeName from card t1 inner
                    //       join card_copy1 t7 on t7.Card_ICCID = t1.Card_ICCID left join accounts t6 on t6.AccountID = t1.accountsID left
                    //       join setmeal t2 on t2.SetmealID = t1.setmealId2 left join cardtype t3 on t3.CardTypeID = t2.CardTypeID left
                    //       join card_xingtai t4 on t4.CardXTID = t2.CardXTID left join operator t5 on t5.OperatorID = t2.OperatorID
                    //       where t1.status = 1 and t7.Card_CompanyID =  '" + CompanyID + "' AND CardTypeName = '2G/4G'";
                    //sql3+=@"UNION ALL select COUNT(1) AS NUM,'1' AS CardTypeId,'Totel' as CardTypeName from card t1 inner join card_copy1 t7 on t7.Card_ICCID = t1.Card_ICCID
                    //       left join accounts t6 on t6.AccountID = t1.accountsID left join setmeal t2 on t2.SetmealID = t1.setmealId2 left
                    //       join cardtype t3 on t3.CardTypeID = t2.CardTypeID left join card_xingtai t4 on t4.CardXTID = t2.CardXTID left join operator t5 on t5.OperatorID = t2.OperatorID
                    //       where t1.status = 1 and t7.Card_CompanyID = '"+CompanyID+"'";
                    //liii = conn.Query<Chars_CardNum>(sql3).AsList();
                    string nbcmccnum = "select count(*) as num from card_copy1 t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + CompanyID + "' and t2.CardTypeID = '123'";//NB移动卡数量
                    string nbctnum = "select count(*) as num from ct_cardcopy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + CompanyID + "' and t2.CardTypeID = '123'";//NB移电信数量
                    string nbcuccnum = "select count(*) as num from cucc_cardcopy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + CompanyID + "' and t2.CardTypeID = '123'";//NB联通卡数量
                    string nbthreenum = "select count(*) as num from three_cardcopy t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='123'";//NB全网通卡数量
                    string shujucmcc = "select count(*) as num from card_copy1 t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";// 移动卡2G/4G卡数量
                    string shujuct = "select count(*) as num from ct_cardcopy t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";// 电信卡2G/4G卡数量
                    string shujucucc = "select count(*) as num from cucc_cardcopy t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";// 联通卡2G/4G卡数量
                    string shujuthree = "select count(*) as num from three_cardcopy t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";// 全网通卡2G/4G卡数量
                    string cmccnum = "select count(*) as num from card_copy1  where Card_CompanyID='" + CompanyID + "'";// 移动卡数量
                    string ctnum = "select count(*) as num from ct_cardcopy  where Card_CompanyID='" + CompanyID + "'";// 电信卡数量
                    string cuccnum = "select count(*) as num from cucc_cardcopy  where Card_CompanyID='" + CompanyID + "'";// 联通卡数量
                    string threecardnum = "select count(*) as num from three_cardcopy  where Card_CompanyID='" + CompanyID + "'";// 全网通卡数量
                    int cmcc = 0;
                    int cucc = 0;
                    int ct = 0;
                    int three = 0;
                    int nbcmcc = 0;
                    int nbct = 0;
                    int nbcucc = 0;
                    int nbthree = 0;
                    int sjcmcc = 0;
                    int sjcucc = 0;
                    int sjct = 0;
                    int sjthree = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        Chars_CardNum chars = new Chars_CardNum();
                        if (i == 0)
                        {
                            chars.CardTypeID = "123";
                            chars.CardTypeName = "NB";
                            nbcmcc = conn.Query<countnum>(nbcmccnum).Select(t => t.num).FirstOrDefault();
                            nbcucc = conn.Query<countnum>(nbcuccnum).Select(t => t.num).FirstOrDefault();
                            nbct = conn.Query<countnum>(nbctnum).Select(t => t.num).FirstOrDefault();
                            //nbthree = conn.Query<countnum>(nbthreenum).Select(t => t.num).FirstOrDefault();
                            chars.NUM = nbcucc + nbcmcc + nbct + nbthree;
                        }
                        if (i == 1)
                        {
                            chars.CardTypeID = "2147483647";
                            chars.CardTypeName = "2G/4G";
                            sjcmcc = conn.Query<countnum>(shujucmcc).Select(t => t.num).FirstOrDefault();
                            sjct = conn.Query<countnum>(shujuct).Select(t => t.num).FirstOrDefault();
                            sjcucc = conn.Query<countnum>(shujucucc).Select(t => t.num).FirstOrDefault();
                            //sjthree = conn.Query<countnum>(shujuthree).Select(t => t.num).FirstOrDefault();
                            chars.NUM = sjcmcc + sjcucc + sjct + sjthree;
                        }
                        if (i == 2)
                        {
                            cmcc = conn.Query<countnum>(cmccnum).Select(t => t.num).FirstOrDefault();
                            ct = conn.Query<countnum>(ctnum).Select(t => t.num).FirstOrDefault();
                            cucc = conn.Query<countnum>(cuccnum).Select(t => t.num).FirstOrDefault();
                            //three = conn.Query<countnum>(threecardnum).Select(t => t.num).FirstOrDefault();
                            chars.CardTypeID = "1";
                            chars.CardTypeName = "Totel";
                            chars.NUM = cmcc + ct + cucc + three;
                        }
                        liii.Add(chars);
                    }
                }
            }
            return liii;
        }

        /// <summary>
        ///获取卡数量
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static Return_Chars_CardNum GetCardNUM1(string CompanyID)
        {
            Return_Chars_CardNum liii = new Return_Chars_CardNum();
            List<Chars_CardNum> chars = new List<Chars_CardNum>();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string CardTotalSql = string.Empty;
                    string CardNbTotalSql = string.Empty;
                    int NBnum = 0;
                    int CardTotalNum = 0;
                    int OtherCardNum = 0;
                    CardTotalSql = "select CompanyTolCardNum as NUM from company where CompanyID='" + CompanyID + "'";
                    CardNbTotalSql = "SELECT CompanyTolNBCardNum as NUM from company where CompanyID='" + CompanyID + "'";
                    CardTotalNum = conn.Query<Chars_CardNum>(CardTotalSql).Select(t => t.NUM).FirstOrDefault();
                    NBnum = conn.Query<Chars_CardNum>(CardNbTotalSql).Select(t => t.NUM).FirstOrDefault();
                    OtherCardNum = CardTotalNum - NBnum;
                    for (int i = 0; i < 3; i++)
                    {
                        Chars_CardNum chars_Card = new Chars_CardNum();
                        if (i == 0)
                        {
                            chars_Card.CardTypeID = "123";
                            chars_Card.CardTypeName = "NB";
                            chars_Card.NUM = NBnum;
                        }
                        if (i == 1)
                        {
                            chars_Card.CardTypeID = "2147483647";
                            chars_Card.CardTypeName = "2G/4G";
                            chars_Card.NUM = OtherCardNum;
                        }
                        if (i == 2)
                        {
                            chars_Card.CardTypeID = "1";
                            chars_Card.CardTypeName = "Totel";
                            chars_Card.NUM = CardTotalNum;
                        }
                        chars.Add(chars_Card);
                        liii.Cards = chars;
                        liii.Message = "Success";
                        liii.status = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                liii.Message = "未能找到相应的卡数据";
                liii.status = "1";
            }
            return liii;
        }


        /// <summary>
        /// 获取服务在线和离线数量 服务数据
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<Chars_Card_CommunicationState> GetCommunicationStates(string CompanyID)
        {

            List<Chars_Card_CommunicationState> li = new List<Chars_Card_CommunicationState>();

            using (IDbConnection conn = DapperService.MySqlConnection())

            {
                string sql2 = @"select  CompanyOfflineCardNum, CompanyOnelineCardNum from company  where CompanyID=@CompanyID";
                li = conn.Query<Chars_Card_CommunicationState>(sql2, new { CompanyID = CompanyID }).AsList();
                if (li.Count == 0)
                {
                    Chars_Card_CommunicationState l = new Chars_Card_CommunicationState();

                    l.CompanyOfflineCardNum = "0";
                    l.CompanyOnelineCardNum = "0";
                    li.Add(l);
                }
                return li;
            }
        }

        /// <summary>
        /// 获取 物联卡账户状态 正常停机 等
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<Chars_Card_GetAccountState> getAccountStates(string CompanyID)
        {
            List<Chars_Card_GetAccountState> li = new List<Chars_Card_GetAccountState>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = @"select t1.CardAcountID ,t1.Normal,t1.Shutdown,
                                SUM(t1.UnidirectionalShutdown +t1.Presalesnumber+t1.Cancel+t1.Transfer+t1.Dormancy+t1.TobeActivated+t1.NoExist+t1.other) as  Other,t1.CompanyID
                                from cardacount_backstage_num  t1 where t1.companyid=@CompanyID GROUP BY t1.CardAcountID,t1.Normal,t1.Shutdown,Other,t1.CompanyID";
                li = conn.Query<Chars_Card_GetAccountState>(sql2, new { CompanyID = CompanyID }).AsList();
                if (li.Count == 0)
                {
                    Chars_Card_GetAccountState r = new Chars_Card_GetAccountState();
                    r.CompanyID = "公司ID不存在或未能找到";
                    r.CardAcountID = "未能找到请联系管理员";
                    r.Normal = "0";
                    r.Shutdown = "0";
                    r.Other = "0";
                    li.Add(r);
                }
            }
            return li;
        }
        public static List<Chars_Charing_CardNum> Get_Charing_CardNums(string CompanyID)
        {
            List<Chars_Charing_CardNum> li = new List<Chars_Charing_CardNum>();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = @"select  CompanyCharingNormalCardNum,CompanyCharingSilentCardNum,CompanyCharingTestingCardNum from  company where  companyid=@CompanyID";
                li = conn.Query<Chars_Charing_CardNum>(sql2, new { CompanyID = CompanyID }).AsList();
                if (li.Count == 0)
                {
                    Chars_Charing_CardNum r = new Chars_Charing_CardNum();
                    r.CompanyCharingNormalCardNum = "0";
                    r.CompanyCharingSilentCardNum = "0";
                    r.CompanyCharingTestingCardNum = "0";
                    li.Add(r);
                }
            }
            return li;
        }
        public class Huoyue
        {
            public float Number { get; set; }
            public float TotalNumber { get; set; }
            public string Result { get; set; }
            public string status { get; set; }
            public string Message { get; set; }
        }

        /// <summary>
        ///   获取活跃度
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<Huoyue> GetHuoYueDu(string CompanyID)
        {
            List<Huoyue> li = new List<Huoyue>();
            if (CompanyID == "1556265186243")
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"SELECT   COUNT(1)  as Number,(select COUNT(1) from  card   ) as TotalNumber from card  where Card_Monthlyusageflow<>null or Card_Monthlyusageflow<>'0'";
                    li = conn.Query<Huoyue>(sql2, new { CompanyID = CompanyID }).AsList();
                    if (li.Count == 1)
                    {
                        foreach (Huoyue item in li)
                        {
                            item.Result = (item.Number / item.TotalNumber * 100).ToString();
                            item.status = "0";
                            item.Message = "Success";
                        }
                    }
                    else
                    {
                        Huoyue h = new Huoyue();
                        h.status = "1";
                        h.Message = "信息获取失败";
                        h.Number = 0;
                        h.TotalNumber = 1;
                        h.Result = "0";
                        li.Add(h);
                    }
                }
            }
            else
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //string sql2 = @"SELECT   COUNT(1)  as Number,(select COUNT(1) from  card where CopyID='"+CompanyID+ @"'   ) as TotalNumber 
                    //from card  where Card_Monthlyusageflow<>null or Card_Monthlyusageflow<>'0' and copyID='"+CompanyID+"' ";
                    string sql2 = @"SELECT   COUNT(1)  as Number,(select COUNT(1) from  card_copy1 where Card_CompanyID='" + CompanyID + @"'   ) as TotalNumber 
                                    from card_copy1  where Card_Monthlyusageflow<>'' and Card_Monthlyusageflow<>'0' and Card_CompanyID='" + CompanyID + "' ";
                    li = conn.Query<Huoyue>(sql2).AsList();
                    if (li.Count == 1)
                    {
                        foreach (Huoyue item in li)
                        {
                            item.Result = (item.Number / item.TotalNumber * 100).ToString();
                            item.status = "0";
                            item.Message = "Success";
                        }
                    }
                    else
                    {
                        Huoyue h = new Huoyue();
                        h.status = "1";
                        h.Message = "信息获取失败";
                        h.Number = 0;
                        h.TotalNumber = 1;
                        h.Result = "0";
                        li.Add(h);
                    }
                }
            }
            return li;
        }
        public class MoonUsed
        {
            public string Used { get; set; }
            public string status { get; set; }
            public string Message { get; set; }
        }
        /// <summary>
        /// 流量使用情况
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<MoonUsed> GetFlowUsed(string CompanyID) {
            List<MoonUsed> li = new List<MoonUsed>();
            if (CompanyID == "1556265186243")
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"SELECT  SUM(Card_Monthlyusageflow)/1024  as Used from card";
                    li = conn.Query<MoonUsed>(sql2).AsList();
                    if (li.Count == 1)
                    {
                        foreach (MoonUsed item in li)
                        {
                            item.status = "0";
                            item.Message = "Success";
                        }
                    }
                    else
                    {
                        MoonUsed h = new MoonUsed();
                        h.status = "1";
                        h.Message = "信息获取失败";
                        h.Used = "0";
                        li.Add(h);
                    }
                }
            }
            else
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"SELECT   SUM(Card_Monthlyusageflow)/1024  as Used  
                                    from card_copy1  where Card_Monthlyusageflow<>null or Card_Monthlyusageflow<>'0' and Card_CompanyID='" + CompanyID + "' ";
                    li = conn.Query<MoonUsed>(sql2).AsList();
                    if (li.Count == 1)
                    {
                        foreach (MoonUsed item in li)
                        {
                            item.status = "0";
                            item.Message = "Success";
                        }
                    }
                    else
                    {
                        MoonUsed h = new MoonUsed();
                        h.status = "1";
                        h.Message = "信息获取失败";
                        h.Used = "0";
                        li.Add(h);
                    }
                }
            }
            return li;
        }
        public class Flows
        {
            public string CompanyID { get; set; }
            public string Flow { get; set; }
            public string Date { get; set; }
        }


        public class Company_flow
        {
            public List<Flows> flows { get; set; }
            public string status { get; set; }
            public string Mess { get; set; }
        }

        /// <summary>
        /// 获取每个公司每天的流量使用情况
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static Company_flow GetCompanyFlow_Day(string CompanyID)
        {
            Company_flow company_Flow = new Company_flow();
            company_Flow.flows = new List<Flows>();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"SELECT DATE_FORMAT(date,'%Y-%m-%d') as Date,CompanyID, Flow FROM company_flow WHERE DATE_FORMAT(   Date, '%Y%m' ) = DATE_FORMAT( CURDATE( ) ,'%Y%m' )  and CompanyID='" + CompanyID + "'";
                    company_Flow.flows = conn.Query<Flows>(sql2).ToList();
                }
                company_Flow.Mess = "Succcess";
                company_Flow.status = "0";
            }
            catch (Exception ex)
            {
                company_Flow.Mess = "出现错误请联系管理员" + ex.ToString();
                company_Flow.status = "1";
            }
            return company_Flow;
        }

        /// <summary>
        /// 获取每个公司每天的流量使用情况
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static Company_flow GetCompanyFlow_Day2(string CompanyID)
        {
            Company_flow company_Flow = new Company_flow();
            company_Flow.flows = new List<Flows>();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql2 = @"call  Flow2(@CompanyID)";
                    company_Flow.flows = conn.Query<Flows>(sql2, new { CompanyID = CompanyID }).ToList();
                }
                company_Flow.Mess = "Succcess";
                company_Flow.status = "0";
            }
            catch (Exception ex)
            {
                company_Flow.Mess = "出现错误请联系管理员" + ex.ToString();
                company_Flow.status = "1";
            }
            return company_Flow;
        }




        ///<summary>
        ///获取联通电信卡数量 卡NB数量 (太慢废弃)
        /// </summary>
        public static CardTotalNumberTypeDto GetCardNumTypeInfo(string CompanyID)
        {
            CardTotalNumberTypeDto dto = new CardTotalNumberTypeDto();
            //List<CardOperatorTypeNum> CMCCNUMTYPE = new List<CardOperatorTypeNum>();
            //List<CardOperatorTypeNum> CUCCNUMTYPE = new List<CardOperatorTypeNum>();
            //List<CardOperatorTypeNum> CTNUMTYPE = new List<CardOperatorTypeNum>();
            //List<CardOperatorTypeNum> THREECARDNUMTYPE = new List<CardOperatorTypeNum>();
            List<CardOperatorDto> source = new List<CardOperatorDto>();

            //List<CardOperatorTypeNum> operatorTypeNums = new List<CardOperatorTypeNum>();
            int NUM = 0;

            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (CompanyID == "1556265186243")//奇迹物联
                    {
                        //移动
                        string cmcccountSql = "select count(*) as NUM from card";
                        string cmcccardnbsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID from card t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID  where t2.CardTypeID='123'";
                        string cmcccardgsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID from card t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID  where t2.CardTypeID='2147483647'";
                        //联通
                        string cucccountSql = "select count(*) as NUM from cucc_card";
                        string cucccardnbsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID from cucc_card t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID  where t2.CardTypeID='123'";
                        string cucccardgsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID from cucc_card t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID  where t2.CardTypeID='2147483647'";
                        //电信
                        string ctcountSql = "select count(*) as NUM from ct_card";
                        string ctcardnbsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID from ct_card t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID  where t2.CardTypeID='123'";
                        string ctcardgsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID from ct_card t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID  where t2.CardTypeID='2147483647'";
                        //string cmcccard
                        CardOperatorDto card = new CardOperatorDto();
                        for (int i = 0; i < 3; i++)//移动
                        {
                            CardOperatorTypeNum cmcc = new CardOperatorTypeNum();
                            if (i == 0)
                            {
                                cmcc.CardTypeID = "123";
                                cmcc.CardTypeName = "NB";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cmcccardnbsql).Count();
                                card.NB = conn.Query<CardOperatorTypeNum>(cmcccardnbsql).Count();
                            }
                            if (i == 1)
                            {
                                cmcc.CardTypeID = "2147483647";
                                cmcc.CardTypeName = "2G/4G";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cmcccardgsql).Count();
                                card.NoNB = conn.Query<CardOperatorTypeNum>(cmcccardgsql).Count();
                            }
                            if (i == 2)
                            {
                                cmcc.CardTypeID = "1";
                                cmcc.CardTypeName = "Total";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cmcccountSql).Select(t => t.NUM).FirstOrDefault();
                                card.OperatorName = "中国移动";
                            }
                            //CMCCNUMTYPE.Add(cmcc); 
                        }
                        source.Add(card);
                        CardOperatorDto cucccard = new CardOperatorDto();
                        for (int i = 0; i < 3; i++)//移动
                        {
                            CardOperatorTypeNum cmcc = new CardOperatorTypeNum();
                            if (i == 0)
                            {
                                cmcc.CardTypeID = "123";
                                cmcc.CardTypeName = "NB";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cucccardnbsql).Count();
                                cucccard.NB = conn.Query<CardOperatorTypeNum>(cucccardnbsql).Count();
                            }
                            if (i == 1)
                            {
                                cmcc.CardTypeID = "2147483647";
                                cmcc.CardTypeName = "2G/4G";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cucccardgsql).Count();
                                cucccard.NoNB = conn.Query<CardOperatorTypeNum>(cucccardgsql).Count();
                            }
                            if (i == 2)
                            {
                                cmcc.CardTypeID = "1";
                                cmcc.CardTypeName = "Total";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cucccountSql).Select(t => t.NUM).FirstOrDefault();
                                cucccard.OperatorName = "中国联通";
                            }
                            //CUCCNUMTYPE.Add(cmcc);
                            //source.Add(cmcc);
                        }
                        source.Add(cucccard);
                        CardOperatorDto ctcard = new CardOperatorDto();
                        for (int i = 0; i < 3; i++)//移动
                        {
                            CardOperatorTypeNum cmcc = new CardOperatorTypeNum();
                            if (i == 0)
                            {
                                cmcc.CardTypeID = "123";
                                cmcc.CardTypeName = "NB";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(ctcardnbsql).Count();
                                ctcard.NB = conn.Query<CardOperatorTypeNum>(ctcardnbsql).Count();
                            }
                            if (i == 1)
                            {
                                cmcc.CardTypeID = "2147483647";
                                cmcc.CardTypeName = "2G/4G";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(ctcardgsql).Count();
                                ctcard.NoNB = conn.Query<CardOperatorTypeNum>(ctcardgsql).Count();
                            }
                            if (i == 2)
                            {
                                cmcc.CardTypeID = "1";
                                cmcc.CardTypeName = "Total";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(ctcountSql).Select(t => t.NUM).FirstOrDefault();
                                ctcard.OperatorName = "中国电信";
                            }
                        }
                        source.Add(ctcard);
                        dto.source = source;
                        dto.flg = "1";
                        dto.Msg = "成功!";
                    }
                    if (CompanyID != "1556265186243")//非奇迹物联的卡统计
                    {
                        //移动
                        string cmcccountSql = "select count(*) as NUM from card_copy1 where Card_CompanyID='" + CompanyID + "'";
                        string cmcccardnbsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID,t1.Card_CompanyID from card_copy1 t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID  where t1.Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='123'";
                        string cmcccardgsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID,t1.Card_CompanyID from card_copy1 t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID   where t1.Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";
                        //联通
                        string cucccountSql = "select count(*) as NUM from cucc_cardcopy where Card_CompanyID='" + CompanyID + "'";
                        string cucccardnbsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID,t1.Card_CompanyID from cucc_cardcopy t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID   where t1.Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='123'";
                        string cucccardgsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID,t1.Card_CompanyID from cucc_cardcopy t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID   where t1.Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";
                        //电信
                        string ctcountSql = "select count(*) as NUM from ct_cardcopy where Card_CompanyID='" + CompanyID + "'";
                        string ctcardnbsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID,t1.Card_CompanyID from ct_cardcopy t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID   where t1.Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='123'";
                        string ctcardgsql = "select t1.SetMealID2,t2.SetMealID,t2.CardTypeID,t1.Card_CompanyID from ct_cardcopy t1 left join setmeal t2 on t1.SetMealID2=t2.SetMealID left join cardtype t3 on t2.CardTypeID=t3.CardTypeID   where t1.Card_CompanyID='" + CompanyID + "' and t2.CardTypeID='2147483647'";
                        CardOperatorDto card = new CardOperatorDto();
                        for (int i = 0; i < 3; i++)//移动
                        {
                            CardOperatorTypeNum cmcc = new CardOperatorTypeNum();
                            if (i == 0)
                            {
                                cmcc.CardTypeID = "123";
                                cmcc.CardTypeName = "NB";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cmcccardnbsql).Count();
                                card.NB = cmcc.NUM;
                            }
                            if (i == 1)
                            {
                                cmcc.CardTypeID = "2147483647";
                                cmcc.CardTypeName = "2G/4G";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cmcccardgsql).Count();
                                card.NoNB = cmcc.NUM;
                            }
                            if (i == 2)
                            {
                                cmcc.CardTypeID = "1";
                                cmcc.CardTypeName = "Total";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cmcccountSql).Select(t => t.NUM).FirstOrDefault();
                                card.OperatorName = "中国移动";
                            }
                        }
                        source.Add(card);
                        CardOperatorDto cucccard = new CardOperatorDto();
                        for (int i = 0; i < 3; i++)//移动
                        {
                            CardOperatorTypeNum cmcc = new CardOperatorTypeNum();
                            if (i == 0)
                            {
                                cmcc.CardTypeID = "123";
                                cmcc.CardTypeName = "NB";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cucccardnbsql).Count();
                                cucccard.NB = cmcc.NUM;
                            }
                            if (i == 1)
                            {
                                cmcc.CardTypeID = "2147483647";
                                cmcc.CardTypeName = "2G/4G";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cucccardgsql).Count();
                                cucccard.NoNB = cmcc.NUM;
                            }
                            if (i == 2)
                            {
                                cmcc.CardTypeID = "1";
                                cmcc.CardTypeName = "Total";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(cucccountSql).Select(t => t.NUM).FirstOrDefault();
                                cucccard.OperatorName = "中国联通";
                            }
                        }
                        source.Add(cucccard);
                        CardOperatorDto ctcard = new CardOperatorDto();
                        for (int i = 0; i < 3; i++)//移动
                        {
                            CardOperatorTypeNum cmcc = new CardOperatorTypeNum();
                            if (i == 0)
                            {
                                cmcc.CardTypeID = "123";
                                cmcc.CardTypeName = "NB";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(ctcardnbsql).Count();
                                ctcard.NB = cmcc.NUM;
                            }
                            if (i == 1)
                            {
                                cmcc.CardTypeID = "2147483647";
                                cmcc.CardTypeName = "2G/4G";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(ctcardgsql).Count();
                                ctcard.NoNB = cmcc.NUM;
                            }
                            if (i == 2)
                            {
                                cmcc.CardTypeID = "1";
                                cmcc.CardTypeName = "Total";
                                cmcc.NUM = conn.Query<CardOperatorTypeNum>(ctcountSql).Select(t => t.NUM).FirstOrDefault();
                                ctcard.OperatorName = "中国电信";
                            }
                        }
                        source.Add(ctcard);
                        dto.source = source;
                        dto.flg = "1";
                        dto.Msg = "成功!";
                    }

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
        ///获取联通电信卡数量 卡NB数量 
        /// </summary>
        public static CardTotalNumberTypeDto GetCardNumTypeInfo1(string CompanyID)
        {
            CardTotalNumberTypeDto dto = new CardTotalNumberTypeDto();
            List<CardOperatorDto> source = new List<CardOperatorDto>();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql = "select * from countcardtypenum where CompanyID='" + CompanyID + "'";
                    source = conn.Query<CardOperatorDto>(sql).ToList();
                    dto.source = source;
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
        ///获取移动电信联通月流量使用数
        /// </summary>
        public static CardMonthFlowDto GetCardMonthFlowUsed(string CompanyID)
        {
            CardMonthFlowDto dto = new CardMonthFlowDto();
            List<CardMonthFlowInfo> cardMonthFlows = new List<CardMonthFlowInfo>();
            CardMonthFlowInfo info = new CardMonthFlowInfo();
            try
            {
                string cmccused = string.Empty;
                string cuccused = string.Empty;
                string ctused = string.Empty;
                decimal totalflow = 0;
                if (CompanyID == "1556265186243")
                {
                    string sqlcmcc = "select sum(Card_Monthlyusageflow) as CMCCFlow from card";
                    string sqlcucc = "select sum(Card_Monthlyusageflow) as CUCCFlow from cucc_card";
                    string sqlct = "select sum(Card_Monthlyusageflow) as CTFlow from ct_card";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        cmccused = conn.Query<CardMonthFlowInfo>(sqlcmcc).Select(t => t.CMCCFlow).FirstOrDefault();
                        cuccused = conn.Query<CardMonthFlowInfo>(sqlcucc).Select(t => t.CUCCFlow).FirstOrDefault();
                        ctused = conn.Query<CardMonthFlowInfo>(sqlct).Select(t => t.CTFlow).FirstOrDefault();
                        totalflow = Convert.ToDecimal(cmccused) + Convert.ToDecimal(cuccused) + Convert.ToDecimal(ctused);
                        info.CMCCFlow = cmccused;
                        info.CUCCFlow = cuccused;
                        info.CTFlow = ctused;
                        info.TotalFlow = totalflow.ToString();
                        cardMonthFlows.Add(info);
                    }
                    dto.cardMonthFlows = cardMonthFlows;
                    dto.flg = "1";
                    dto.Msg = "成功";
                }
                else
                {
                    string sqlcmcc = "select sum(Card_Monthlyusageflow) as CMCCUsed from card_copy1 where Card_CompanyID='" + CompanyID + "'";
                    string sqlcucc = "select sum(Card_Monthlyusageflow) as CUCCUsed from cucc_cardcopy where Card_CompanyID='" + CompanyID + "'";
                    string sqlct = "select sum(Card_Monthlyusageflow) as CTUsed from ct_cardcopy where Card_CompanyID='" + CompanyID + "'";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        cmccused = conn.Query<CardMonthFlowInfo>(sqlcmcc).Select(t => t.CMCCFlow).FirstOrDefault();
                        cuccused = conn.Query<CardMonthFlowInfo>(sqlcucc).Select(t => t.CUCCFlow).FirstOrDefault();
                        ctused = conn.Query<CardMonthFlowInfo>(sqlct).Select(t => t.CTFlow).FirstOrDefault();
                        totalflow = Convert.ToDecimal(cmccused) + Convert.ToDecimal(cuccused) + Convert.ToDecimal(ctused);
                        info.CMCCFlow = cmccused;
                        info.CUCCFlow = cuccused;
                        info.CTFlow = ctused;
                        info.TotalFlow = totalflow.ToString();
                        cardMonthFlows.Add(info);
                    }
                    dto.cardMonthFlows = cardMonthFlows;
                    dto.flg = "1";
                    dto.Msg = "成功";
                }
            }
            catch
            {
                dto.flg = "-1";
                dto.Msg = "出现错误请联系管理员!";
            }
            return dto;
        }

        ///<summary>
        ///获取移动电信联通卡的数量和其他卡的数量
        /// </summary>
        public static CardNumberDto GetCardNumberInfo(string CompanyID)
        {
            CardNumberDto dto = new CardNumberDto();
            List<CardNumberInfo> cardNumbers = new List<CardNumberInfo>();
            CardNumberInfo info = new CardNumberInfo();
            try
            {
                int cmccnum = 0;
                int cuccnum = 0;
                int ctnum = 0;
                int othernum = 0;
                if (CompanyID == "1556265186243")
                {
                    string sqlcmcc = "select count(*) as CMCCNumber from card";
                    string sqlcucc = "select count(*) as CUCCNumber from cucc_card";
                    string sqlct = "select count(*) as CTNumber from ct_card";
                    string sqlother = "select count(*) as OtherNumber from three_card";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        cmccnum = conn.Query<CardNumberInfo>(sqlcmcc).Select(t => t.CMCCNumber).FirstOrDefault();
                        cuccnum = conn.Query<CardNumberInfo>(sqlcucc).Select(t => t.CUCCNumber).FirstOrDefault();
                        ctnum = conn.Query<CardNumberInfo>(sqlct).Select(t => t.CTNumber).FirstOrDefault();
                        othernum = conn.Query<CardNumberInfo>(sqlother).Select(t => t.OtherNumber).FirstOrDefault();
                        info.CMCCNumber = cmccnum;
                        info.CUCCNumber = cuccnum;
                        info.CTNumber = ctnum;
                        info.OtherNumber = othernum;
                        cardNumbers.Add(info);
                    }
                    dto.cardNumbers = cardNumbers;
                    dto.flg = "1";
                    dto.Msg = "成功";
                }
                else
                {
                    string sqlcmcc = "select count(*) as CMCCNumber from card_copy1  where Card_CompanyID='" + CompanyID + "' ";
                    string sqlcucc = "select count(*) as CUCCNumber from cucc_cardcopy  where Card_CompanyID='" + CompanyID + "'";
                    string sqlct = "select count(*) as CTNumber from ct_cardcopy where Card_CompanyID='" + CompanyID + "'";
                    string sqlother = "select count(*) as OtherNumber from three_cardcopy where Card_CompanyID='" + CompanyID + "'";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        cmccnum = conn.Query<CardNumberInfo>(sqlcmcc).Select(t => t.CMCCNumber).FirstOrDefault();
                        cuccnum = conn.Query<CardNumberInfo>(sqlcucc).Select(t => t.CUCCNumber).FirstOrDefault();
                        ctnum = conn.Query<CardNumberInfo>(sqlct).Select(t => t.CTNumber).FirstOrDefault();
                        othernum = conn.Query<CardNumberInfo>(sqlother).Select(t => t.OtherNumber).FirstOrDefault();
                        info.CMCCNumber = cmccnum;
                        info.CUCCNumber = cuccnum;
                        info.CTNumber = ctnum;
                        info.OtherNumber = othernum;
                        cardNumbers.Add(info);
                    }
                    dto.cardNumbers = cardNumbers;
                    dto.flg = "1";
                    dto.Msg = "成功";
                }
            }
            catch
            {
                dto.flg = "-1";
                dto.Msg = "出现错误请联系管理员";
            }
            return dto;
        }

        ///<summary>
        ///统计卡总数 NB卡数量
        /// </summary>
        public static string countcard()
        {
            string sql = "select * from company";
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                var list = conn.Query<Company>(sql).ToList();
                int cmcccardnum = 0;
                int cucccardnum = 0;
                int ctcardnum = 0;
                int threecardnum = 0;
                int cmccnbcardnum = 0;
                int cuccnbcardnum = 0;
                int ctnbcardnum = 0;
                int threenbcardnum = 0;
                string sqlcmcccount = string.Empty;
                string sqlcucccount = string.Empty;
                string sqlctcount = string.Empty;
                string sqlthreecount = string.Empty;
                string sqlcmccnbcount = string.Empty;
                string sqlcuccnbcount = string.Empty;
                string sqlctnbcount = string.Empty;
                string sqlthreenbcount = string.Empty;
                int CompanyTolCardNum = 0;
                int CompanyTolNBCardNum = 0;
                foreach (var item in list)
                {
                    if (item.CompanyID == "1556265186243")
                    {
                        sqlcmcccount = "select count(*) as num from card";
                        sqlcucccount = "select count(*) as num from cucc_card";
                        sqlctcount = "select count(*) as num from ct_card";
                        sqlthreecount = "select count(*) as num from three_card";
                        sqlcmccnbcount = "select count(*) as num from card t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + item.CompanyID + "' and t2.CardTypeID = '123'";
                        sqlcuccnbcount = "select count(*) as num from cucc_card t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + item.CompanyID + "' and t2.CardTypeID = '123'";
                        sqlctnbcount = "select count(*) as num from ct_card t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + item.CompanyID + "' and t2.CardTypeID='123'";
                        sqlthreenbcount = "select count(*) as num from three_card t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + item.CompanyID + "' and t2.CardTypeID='123'";
                    }
                    else
                    {
                        sqlcmcccount = "select count(*) as num from card_copy1 where Card_CompanyID='" + item.CompanyID + "'";
                        sqlcucccount = "select count(*) as num from cucc_cardcopy where Card_CompanyID='" + item.CompanyID + "' ";
                        sqlctcount = "select count(*) as num from ct_cardcopy where Card_CompanyID='" + item.CompanyID + "'";
                        sqlthreecount = "select count(*) as num from three_cardcopy where Card_CompanyID='" + item.CompanyID + "'";
                        sqlcmccnbcount = "select count(*) as num from card_copy1 t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + item.CompanyID + "' and t2.CardTypeID = '123'";
                        sqlcuccnbcount = "select count(*) as num from cucc_cardcopy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID  where Card_CompanyID = '" + item.CompanyID + "' and t2.CardTypeID = '123'";
                        sqlctnbcount = "select count(*) as num from ct_cardcopy t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + item.CompanyID + "' and t2.CardTypeID='123'";
                        sqlthreenbcount = "select count(*) as num from three_cardcopy t1 left join  setmeal t2 on t1.SetMealID2=t2.SetmealID  where Card_CompanyID='" + item.CompanyID + "' and t2.CardTypeID='123'";
                    }
                    cmcccardnum = conn.Query<countnum>(sqlcmcccount).Select(t => t.num).FirstOrDefault();
                    cucccardnum = conn.Query<countnum>(sqlcucccount).Select(t => t.num).FirstOrDefault();
                    ctcardnum = conn.Query<countnum>(sqlctcount).Select(t => t.num).FirstOrDefault();
                    threecardnum = conn.Query<countnum>(sqlthreecount).Select(t => t.num).FirstOrDefault();
                    cmccnbcardnum = conn.Query<countnum>(sqlcmccnbcount).Select(t => t.num).FirstOrDefault();
                    cuccnbcardnum = conn.Query<countnum>(sqlcuccnbcount).Select(t => t.num).FirstOrDefault();
                    ctnbcardnum = conn.Query<countnum>(sqlctnbcount).Select(t => t.num).FirstOrDefault();
                    threenbcardnum = conn.Query<countnum>(sqlthreenbcount).Select(t => t.num).FirstOrDefault();
                    CompanyTolCardNum = cmcccardnum + cucccardnum + ctcardnum + threecardnum;
                    CompanyTolNBCardNum = cmccnbcardnum + cuccnbcardnum + ctnbcardnum + threenbcardnum;
                    string sqlupdate = "update  company set CompanyTolCardNum=" + CompanyTolCardNum + ",CompanyTolNBCardNum=" + CompanyTolNBCardNum + " where CompanyID='" + item.CompanyID + "'";
                    conn.Execute(sqlupdate);
                }
            }
            return null;
        }

        ///<summary>
        ///获取移动卡的近七天流量使用数据
        /// </summary>
        public static CardFlowDays GetcountcardFlow(string CompanyID)
        {
            CardFlowDays info = new CardFlowDays();
            List<FlowDay> data = new List<FlowDay>();
            FlowDay day = new FlowDay();
            List<decimal> flow = new List<decimal>();
            List<string> date = new List<string>();
            decimal flows = 0;
            string dates = string.Empty;
            try
            {
                string sqlflow = "select * from company_flow where CompanyID='" + CompanyID + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var lists = conn.Query<company_flow>(sqlflow).OrderBy(t => t.Date).ToList();
                    foreach (var item in lists)
                    {
                        flows = Convert.ToDecimal(item.Flow);
                        dates = item.Date.ToShortDateString();
                        flow.Add(flows);
                        date.Add(dates);
                        day.date = date;
                        day.flow = flow;
                    }
                    data.Add(day);
                    info.data = data;
                    info.flg = "1";
                    info.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                info.data = null;
                info.flg = "-1";
                info.Msg = "失败:" + ex;
            }
            return info;
        }


        #region 新接口
        ///<summary>
        ///获取卡数量 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        public static Return_Chars_CardNum GetCardNUM2(string CompanyID, string OperatorsFlg)
        {
            Return_Chars_CardNum cardNum = new Return_Chars_CardNum();
            List<Chars_CardNum> Cards = new List<Chars_CardNum>();
            try
            {
                if (OperatorsFlg == "1")//移动
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string CardTotalSql = string.Empty;
                        string CardNbTotalSql = string.Empty;
                        string CardNoNbTotalsql = string.Empty;
                        int NBnum = 0;
                        int NoNbNum = 0;
                        int CardTotalNum = 0;
                        if (CompanyID == "1556265186243")//奇迹物联   select NB, NoNB from countcardtypenum where OperatorName = '中国移动' and CompanyID = '1556265186243'
                        {
                            CardNbTotalSql = "select NB as NUM from countcardtypenum where OperatorName = '中国移动' and CompanyID = '1556265186243' and status=1";//nb卡的数量
                            CardNoNbTotalsql = "select NoNB as NUM from countcardtypenum where OperatorName = '中国移动' and CompanyID = '1556265186243' and status=1";//2g/4g卡数量
                            CardTotalSql = "select count(*) as NUM from card where status=1";
                        }
                        else
                        {
                            CardNbTotalSql = "select NB as NUM from countcardtypenum where OperatorName = '中国移动' and CompanyID = '" + CompanyID + "' and status=1";//nb卡的数量
                            CardNoNbTotalsql = "select NoNB as NUM from countcardtypenum where OperatorName = '中国移动' and CompanyID = '" + CompanyID + "' and status=1";//2g/4g卡数量
                            CardTotalSql = "select count(*) as NUM from card_copy1  where status=1 and Card_CompanyID='" + CompanyID + "'";
                        }
                        CardTotalNum = conn.Query<Chars_CardNum>(CardTotalSql).Select(t => t.NUM).FirstOrDefault();
                        NBnum = conn.Query<Chars_CardNum>(CardNbTotalSql).Select(t => t.NUM).FirstOrDefault();
                        NoNbNum = conn.Query<Chars_CardNum>(CardNoNbTotalsql).Select(t => t.NUM).FirstOrDefault();
                        for (int i = 0; i < 3; i++)
                        {
                            Chars_CardNum chars_Card = new Chars_CardNum();
                            if (i == 0)
                            {
                                chars_Card.CardTypeID = "123";
                                chars_Card.CardTypeName = "NB";
                                chars_Card.NUM = NBnum;
                            }
                            if (i == 1)
                            {
                                chars_Card.CardTypeID = "2147483647";
                                chars_Card.CardTypeName = "2G/4G";
                                chars_Card.NUM = NoNbNum;
                            }
                            if (i == 2)
                            {
                                chars_Card.CardTypeID = "1";
                                chars_Card.CardTypeName = "Totel";
                                chars_Card.NUM = CardTotalNum;
                            }
                            Cards.Add(chars_Card);
                            cardNum.Cards = Cards;
                            cardNum.Message = "Success";
                            cardNum.status = "0";
                        }
                    }
                }
                if (OperatorsFlg == "2")//电信
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string CardTotalSql = string.Empty;
                        string CardNbTotalSql = string.Empty;
                        string CardNoNbTotalsql = string.Empty;
                        int NBnum = 0;
                        int NoNbNum = 0;
                        int CardTotalNum = 0;
                        if (CompanyID == "1556265186243")//奇迹物联   select NB, NoNB from countcardtypenum where OperatorName = '中国移动' and CompanyID = '1556265186243'
                        {
                            CardNbTotalSql = "select NB as NUM from countcardtypenum where OperatorName = '中国电信' and CompanyID = '1556265186243' and status=1";//nb卡的数量
                            CardNoNbTotalsql = "select NoNB as NUM from countcardtypenum where OperatorName = '中国电信' and CompanyID = '1556265186243' and status=1 ";//2g/4g卡数量
                            CardTotalSql = "select count(*) as NUM from ct_card  where status=1";
                        }
                        else
                        {
                            CardNbTotalSql = "select NB as NUM from countcardtypenum where OperatorName = '中国电信' and CompanyID = '" + CompanyID + "' and status=1";//nb卡的数量
                            CardNoNbTotalsql = "select NoNB as NUM from countcardtypenum where OperatorName = '中国电信' and CompanyID = '" + CompanyID + "'  and status=1";//2g/4g卡数量
                            CardTotalSql = "select count(*) as NUM from ct_cardcopy  where status=1 and Card_CompanyID='" + CompanyID + "'";
                        }
                        CardTotalNum = conn.Query<Chars_CardNum>(CardTotalSql).Select(t => t.NUM).FirstOrDefault();
                        NBnum = conn.Query<Chars_CardNum>(CardNbTotalSql).Select(t => t.NUM).FirstOrDefault();
                        NoNbNum = conn.Query<Chars_CardNum>(CardNoNbTotalsql).Select(t => t.NUM).FirstOrDefault();
                        for (int i = 0; i < 3; i++)
                        {
                            Chars_CardNum chars_Card = new Chars_CardNum();
                            if (i == 0)
                            {
                                chars_Card.CardTypeID = "123";
                                chars_Card.CardTypeName = "NB";
                                chars_Card.NUM = NBnum;
                            }
                            if (i == 1)
                            {
                                chars_Card.CardTypeID = "2147483647";
                                chars_Card.CardTypeName = "2G/4G";
                                chars_Card.NUM = NoNbNum;
                            }
                            if (i == 2)
                            {
                                chars_Card.CardTypeID = "1";
                                chars_Card.CardTypeName = "Totel";
                                chars_Card.NUM = CardTotalNum;
                            }
                            Cards.Add(chars_Card);
                            cardNum.Cards = Cards;
                            cardNum.Message = "Success";
                            cardNum.status = "0";
                        }
                    }
                }
                if (OperatorsFlg == "3")//联通
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string CardTotalSql = string.Empty;
                        string CardNbTotalSql = string.Empty;
                        string CardNoNbTotalsql = string.Empty;
                        int NBnum = 0;
                        int NoNbNum = 0;
                        int CardTotalNum = 0;
                        if (CompanyID == "1556265186243")//奇迹物联   select NB, NoNB from countcardtypenum where OperatorName = '中国移动' and CompanyID = '1556265186243'
                        {
                            CardNbTotalSql = "select NB as NUM from countcardtypenum where OperatorName = '中国联通' and CompanyID = '1556265186243' and status=1";//nb卡的数量
                            CardNoNbTotalsql = "select NoNB as NUM from countcardtypenum where OperatorName = '中国联通' and CompanyID = '1556265186243' and status=1 ";//2g/4g卡数量
                            CardTotalSql = "select count(*) as NUM from cucc_card where status=1";
                        }
                        else
                        {
                            CardNbTotalSql = "select NB as NUM from countcardtypenum where OperatorName = '中国电信' and CompanyID = '" + CompanyID + "' and status=1";//nb卡的数量
                            CardNoNbTotalsql = "select NoNB as NUM from countcardtypenum where OperatorName = '中国电信' and CompanyID = '" + CompanyID + "'  and status=1";//2g/4g卡数量
                            CardTotalSql = "select count(*) as NUM from cucc_cardcopy  where status=1 and Card_CompanyID='" + CompanyID + "'";
                        }
                        CardTotalNum = conn.Query<Chars_CardNum>(CardTotalSql).Select(t => t.NUM).FirstOrDefault();
                        NBnum = conn.Query<Chars_CardNum>(CardNbTotalSql).Select(t => t.NUM).FirstOrDefault();
                        NoNbNum = conn.Query<Chars_CardNum>(CardNoNbTotalsql).Select(t => t.NUM).FirstOrDefault();
                        for (int i = 0; i < 3; i++)
                        {
                            Chars_CardNum chars_Card = new Chars_CardNum();
                            if (i == 0)
                            {
                                chars_Card.CardTypeID = "123";
                                chars_Card.CardTypeName = "NB";
                                chars_Card.NUM = NBnum;
                            }
                            if (i == 1)
                            {
                                chars_Card.CardTypeID = "2147483647";
                                chars_Card.CardTypeName = "2G/4G";
                                chars_Card.NUM = NoNbNum;
                            }
                            if (i == 2)
                            {
                                chars_Card.CardTypeID = "1";
                                chars_Card.CardTypeName = "Totel";
                                chars_Card.NUM = CardTotalNum;
                            }
                            Cards.Add(chars_Card);
                            cardNum.Cards = Cards;
                            cardNum.Message = "Success";
                            cardNum.status = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cardNum.Cards = null;
                cardNum.Message = "error:" + ex;
                cardNum.status = "-1";
            }
            return cardNum;
        }

        /// <summary>
        /// 获取物联卡账户状态 正常 停机 其他 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<Chars_Card_GetAccountState> getAccountStates1(string CompanyID, string OperatorsFlg)
        {
            List<Chars_Card_GetAccountState> li = new List<Chars_Card_GetAccountState>();
            string sqlNormal = string.Empty;//正常的卡统计
            string sqlShutdown = string.Empty;//停机卡
            string sqlOther = string.Empty;//其他
            string sqlsumnum = string.Empty;//总数
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                if (OperatorsFlg == "1")//移动
                {
                    if (CompanyID == "1556265186243")//奇迹物联
                    {
                        sqlNormal = "select sum(Card_State= '00' or Card_State = '2') as Normal from card where  status=1";
                        sqlShutdown = "select sum(Card_State= '01' or Card_State='02' or Card_State='4') as Shutdown from card where  status=1";
                        sqlsumnum = "select count(id) as NUM from card where  status=1";
                        Chars_Card_GetAccountState card = new Chars_Card_GetAccountState();
                        card.Normal = conn.Query<Chars_Card_GetAccountState>(sqlNormal).Select(t => t.Normal).FirstOrDefault();
                        card.Shutdown = conn.Query<Chars_Card_GetAccountState>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();
                        int n = Convert.ToInt32(card.Normal);
                        int s = Convert.ToInt32(card.Shutdown);
                        int sum = 0;
                        int other = 0;
                        sum = conn.Query<Chars_CardNum>(sqlsumnum).Select(t => t.NUM).FirstOrDefault();
                        other = sum - n - s;
                        card.Other = other.ToString();
                        card.CompanyID = CompanyID;
                        li.Add(card);
                    }
                    else
                    {
                        sqlNormal = "select sum(Card_State= '00' or Card_State = '2') as Normal from card_copy1 where Card_CompanyID='" + CompanyID + "' and status=1";
                        sqlShutdown = "select sum(Card_State= '01' or Card_State='02' or Card_State='4') as Shutdown from card_copy1 where Card_CompanyID='" + CompanyID + "' and status=1";
                        sqlsumnum = "select count(id) as NUM from card_copy1 where Card_CompanyID='" + CompanyID + "' and status=1";
                        Chars_Card_GetAccountState card = new Chars_Card_GetAccountState();
                        card.Normal = conn.Query<Chars_Card_GetAccountState>(sqlNormal).Select(t => t.Normal).FirstOrDefault();
                        card.Shutdown = conn.Query<Chars_Card_GetAccountState>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();
                        int n = Convert.ToInt32(card.Normal);
                        int s = Convert.ToInt32(card.Shutdown);
                        int sum = 0;
                        int other = 0;
                        sum = conn.Query<Chars_CardNum>(sqlsumnum).Select(t => t.NUM).FirstOrDefault();
                        other = sum - n - s;
                        card.Other = other.ToString();
                        card.CompanyID = CompanyID;
                        li.Add(card);
                    }
                }
                if (OperatorsFlg == "2")//电信
                {
                    if (CompanyID == "1556265186243")//奇迹物联
                    {
                        sqlNormal = "select sum(Card_State= '00') as Normal from ct_card where  status=1";
                        sqlShutdown = "select sum(Card_State='02') as Shutdown from ct_card where  status=1";
                        sqlsumnum = "select count(id) as NUM from ct_card where  status=1";
                        Chars_Card_GetAccountState card = new Chars_Card_GetAccountState();
                        card.Normal = conn.Query<Chars_Card_GetAccountState>(sqlNormal).Select(t => t.Normal).FirstOrDefault();
                        card.Shutdown = conn.Query<Chars_Card_GetAccountState>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();
                        int n = Convert.ToInt32(card.Normal);
                        int s = Convert.ToInt32(card.Shutdown);
                        int sum = 0;
                        int other = 0;
                        sum = conn.Query<Chars_CardNum>(sqlsumnum).Select(t => t.NUM).FirstOrDefault();
                        other = sum - n - s;
                        card.Other = other.ToString();
                        card.CompanyID = CompanyID;
                        li.Add(card);
                    }
                    else
                    {
                        sqlNormal = "select sum(Card_State= '00' or Card_State = '2') as Normal from ct_cardcopy where Card_CompanyID='" + CompanyID + "' and status=1";
                        sqlShutdown = "select sum(Card_State= '01' or Card_State='02' or Card_State='4') as Shutdown from ct_cardcopy where Card_CompanyID='" + CompanyID + "' and status=1";
                        sqlsumnum = "select count(id) as NUM from ct_cardcopy where Card_CompanyID='" + CompanyID + "' and status=1";
                        Chars_Card_GetAccountState card = new Chars_Card_GetAccountState();
                        card.Normal = conn.Query<Chars_Card_GetAccountState>(sqlNormal).Select(t => t.Normal).FirstOrDefault();
                        card.Shutdown = conn.Query<Chars_Card_GetAccountState>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();
                        int n = Convert.ToInt32(card.Normal);
                        int s = Convert.ToInt32(card.Shutdown);
                        int sum = 0;
                        int other = 0;
                        sum = conn.Query<Chars_CardNum>(sqlsumnum).Select(t => t.NUM).FirstOrDefault();
                        other = sum - n - s;
                        card.Other = other.ToString();
                        card.CompanyID = CompanyID;
                        li.Add(card);
                    }
                }
                if (OperatorsFlg == "3")//联通
                {
                    if (CompanyID == "1556265186243")//奇迹物联
                    {

                    }
                    else
                    {

                    }
                }
            }
            return li;
        }

        /// <summary>
        /// 流量使用情况 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<MoonUsed> GetFlowUsed1(string CompanyID, string OperatorsFlg)
        {
            List<MoonUsed> li = new List<MoonUsed>();
            if (OperatorsFlg == "1")//移动
            {
                if (CompanyID == "1556265186243")
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sql2 = @"SELECT  SUM(Card_Monthlyusageflow)/1024  as Used from card";
                        li = conn.Query<MoonUsed>(sql2).AsList();
                        if (li.Count == 1)
                        {
                            foreach (MoonUsed item in li)
                            {
                                item.status = "0";
                                item.Message = "Success";
                            }
                        }
                        else
                        {
                            MoonUsed h = new MoonUsed();
                            h.status = "1";
                            h.Message = "信息获取失败";
                            h.Used = "0";
                            li.Add(h);
                        }
                    }
                }
                else
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sql2 = @"SELECT   SUM(Card_Monthlyusageflow)/1024  as Used  
                                    from card_copy1  where Card_Monthlyusageflow<>null or Card_Monthlyusageflow<>'0' and Card_CompanyID='" + CompanyID + "' ";
                        li = conn.Query<MoonUsed>(sql2).AsList();
                        if (li.Count == 1)
                        {
                            foreach (MoonUsed item in li)
                            {
                                item.status = "0";
                                item.Message = "Success";
                            }
                        }
                        else
                        {
                            MoonUsed h = new MoonUsed();
                            h.status = "1";
                            h.Message = "信息获取失败";
                            h.Used = "0";
                            li.Add(h);
                        }
                    }
                }
            }
            if (OperatorsFlg == "2")//电信
            {
                if (CompanyID == "1556265186243")
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sql2 = @"SELECT  SUM(Card_Monthlyusageflow)/1024  as Used from ct_card";
                        li = conn.Query<MoonUsed>(sql2).AsList();
                        if (li.Count == 1)
                        {
                            foreach (MoonUsed item in li)
                            {
                                item.status = "0";
                                item.Message = "Success";
                            }
                        }
                        else
                        {
                            MoonUsed h = new MoonUsed();
                            h.status = "1";
                            h.Message = "信息获取失败";
                            h.Used = "0";
                            li.Add(h);
                        }
                    }
                }
                else
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sql2 = @"SELECT   SUM(Card_Monthlyusageflow)/1024  as Used  
                                    from ct_cardcopy  where Card_Monthlyusageflow<>null or Card_Monthlyusageflow<>'0' and Card_CompanyID='" + CompanyID + "' ";
                        li = conn.Query<MoonUsed>(sql2).AsList();
                        if (li.Count == 1)
                        {
                            foreach (MoonUsed item in li)
                            {
                                item.status = "0";
                                item.Message = "Success";
                            }
                        }
                        else
                        {
                            MoonUsed h = new MoonUsed();
                            h.status = "1";
                            h.Message = "信息获取失败";
                            h.Used = "0";
                            li.Add(h);
                        }
                    }
                }
            }
            if (OperatorsFlg == "3")//联通
            {

            }

            return li;
        }

        /// <summary>
        ///   获取活跃度 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public static List<Huoyue> GetHuoYueDu1(string CompanyID, string OperatorsFlg)
        {
            //月活跃度计算规则 激活的卡中在线占比
            List<Huoyue> li = new List<Huoyue>();
            if (OperatorsFlg == "1")//移动
            {
                try
                {
                    if (CompanyID == "1556265186243")
                    {
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            string sqltotalnum = "select SUM(Card_State='00' or Card_State='2') as TotalNumber from card";
                            string sqlnum = "select SUM(Card_WorkState='01') as Number from card";
                            float total = conn.Query<Huoyue>(sqltotalnum).Select(t => t.TotalNumber).FirstOrDefault();
                            float num = conn.Query<Huoyue>(sqlnum).Select(t => t.Number).FirstOrDefault();
                            Huoyue huoyue = new Huoyue();
                            huoyue.Number = num;
                            huoyue.TotalNumber = total;
                            huoyue.Result = (num / total * 100).ToString();
                            huoyue.Message = "Success";
                            huoyue.status = "0";
                            li.Add(huoyue);
                        }
                    }
                    else
                    {
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            string sqltotalnum = "select SUM(Card_State='00' or Card_State='2') as TotalNumber from card_copy1 where Card_CompanyID='" + CompanyID + "'";
                            string sqlnum = "select SUM(Card_WorkState='01') as Number from  card_copy1 where Card_CompanyID='" + CompanyID + "'";
                            float total = conn.Query<Huoyue>(sqltotalnum).Select(t => t.TotalNumber).FirstOrDefault();
                            float num = conn.Query<Huoyue>(sqlnum).Select(t => t.Number).FirstOrDefault();
                            Huoyue huoyue = new Huoyue();
                            huoyue.Number = num;
                            huoyue.TotalNumber = total;
                            huoyue.Result = (num / total * 100).ToString();
                            huoyue.Message = "Success";
                            huoyue.status = "0";
                            li.Add(huoyue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Huoyue h = new Huoyue();
                    h.status = "1";
                    h.Message = "信息获取失败:" + ex;
                    h.Number = 0;
                    h.TotalNumber = 1;
                    h.Result = "0";
                    li.Add(h);
                }
            }

            return li;
        }


        /////<summary>
        /////主界面数据  OperatorsFlg 1:移动 2:电信 3:联通
        ///// </summary>
        //public static IndexDto GetIndexInfo(string CompanyID, string OperatorsFlg)
        //{
        //    IndexDto dto = new IndexDto();
        //    try
        //    {
        //        IndexData DtoList = new IndexData();
        //        IndexData CtList = new IndexData();
        //        IndexData CuccList = new IndexData();
        //        string sqlcardtotal = string.Empty;//移动卡总数
        //        string sqlcardcttotal = string.Empty;//电信卡总数
        //        string sqlcardcucctotal = string.Empty;//联通卡总数
        //        string sqlcardnb = string.Empty;//nb总数
        //        string sqlmonthflow = string.Empty;//月使用流量总数单位MB
        //        string sqlActiveNum = string.Empty;//月活跃度
        //        string sqlOnline = string.Empty;//物联网卡在线数量
        //        string sqlOffline = string.Empty;//物联网卡离线数量
        //        string sqlNormal = string.Empty;//物联网卡用户状态正常 已激活 再用
        //        string sqlShutdown = string.Empty;//物联网卡用户状态停机
        //        string sqlOther = string.Empty;//物联网卡用户状态其他
        //        string sqlNormalUse = string.Empty;//物联网卡计费状态正常
        //        string sqlSilentPeriod = string.Empty;//物联网卡计费状态沉默期
        //        string sqlTestPeriod = string.Empty;//物联网卡计费状态测试期
        //        using (IDbConnection conn = DapperService.MySqlConnection())
        //        {
        //            if (CompanyID == "1556265186243")//奇迹
        //            {
        //                if (OperatorsFlg == "1")//移动
        //                {
        //                    sqlcardtotal = "select count(id) as CMCCCardTotal from card where status=1";//移动卡总数
        //                    sqlcardcttotal = "select count(id) as CTCardTotal from ct_card where  status=1";//电信卡总数
        //                    sqlcardcucctotal = "select count(id) as CUCCCardTotal from cucc_card where  status=1";//联通卡总数
        //                    sqlcardnb = "select NB as NBNum from countcardtypenum where OperatorName = '中国移动' and CompanyID = '1556265186243'";
        //                    sqlmonthflow = "select Flow as MonthFlowTotal from company_flow where CompanyID='" + CompanyID + "' ORDER BY Date desc";
        //                    sqlNormal = "select sum(Card_State= '00' or Card_State = '2') as Normal from card where  status=1";//状态正常 已激活
        //                    sqlShutdown = "select sum(Card_State= '01' or Card_State='02' or Card_State='4') as Shutdown from card where  status=1";//用户状态停机
        //                    sqlActiveNum = "select count(id) as ActiveNum from card where  Card_Monthlyusageflow is not null and Card_Monthlyusageflow !='' and Card_Monthlyusageflow!='0' and status=1";//月活跃有流量产生的卡
        //                    sqlOnline = "select SUM(Card_WorkState='01') as Online from card where status=1";//物联卡在线数量
        //                    sqlOffline = "select SUM(Card_WorkState='00') as Offline from card where status=1";//物联卡离线数量
        //                    sqlNormalUse = "";//就是物联网卡状态正常已激活再用
        //                    sqlSilentPeriod = "select sum(Card_State= '1' or Card_State = '7') as SilentPeriod from card where  status=1";
        //                    sqlTestPeriod = "select sum(Card_State= '6') as TestPeriod from card where  status=1";
        //                    dto.CMCCCardTotal = conn.Query<IndexDto>(sqlcardtotal).Select(t => t.CMCCCardTotal).FirstOrDefault();//移动卡总数
        //                    dto.CUCCCardTotal = conn.Query<IndexDto>(sqlcardcucctotal).Select(t => t.CUCCCardTotal).FirstOrDefault();//联通卡总数
        //                    dto.CTCardTotal = conn.Query<IndexDto>(sqlcardcttotal).Select(t => t.CTCardTotal).FirstOrDefault();//电信卡总数
        //                    IndexData data = new IndexData();
        //                    data.NBNum = conn.Query<IndexData>(sqlcardnb).Select(t => t.NBNum).FirstOrDefault();//nb卡总数
        //                    data.NoNBNum = dto.CMCCCardTotal - data.NBNum;//2G、4G卡数量
        //                    data.MonthFlowTotal = conn.Query<IndexData>(sqlmonthflow).Select(t => t.MonthFlowTotal).FirstOrDefault();//月流量总数
        //                    data.MonthFlowTotal = Math.Round(data.MonthFlowTotal / 1024, 2);//月流量总数MB
        //                    data.Normal = conn.Query<IndexData>(sqlNormal).Select(t => t.Normal).FirstOrDefault();//物联网卡状态已激活 再用 正常状态
        //                    decimal activeflow = conn.Query<IndexData>(sqlActiveNum).Select(t => t.ActiveNum).FirstOrDefault();//本月使用了流量的卡
        //                    if (data.Normal != 0)
        //                    {
        //                        data.ActiveNum = Math.Round((activeflow / data.Normal) * 100, 1);//月活跃度
        //                    }
        //                    data.Online = conn.Query<IndexData>(sqlOnline).Select(t => t.Online).FirstOrDefault();//物联网卡在线的数量
        //                    data.Offline = conn.Query<IndexData>(sqlOffline).Select(t => t.Offline).FirstOrDefault();//物联网卡离线的数量
        //                    data.Shutdown = conn.Query<IndexData>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();//物联网卡停机数量
        //                    data.Other = dto.CMCCCardTotal - data.Normal - data.Shutdown;//用户状态其他状态(总卡数量减去激活的和停机的卡数量)
        //                    data.NormalUse = data.Normal;//卡生命周期正常的数量
        //                    data.SilentPeriod = conn.Query<IndexData>(sqlSilentPeriod).Select(t => t.SilentPeriod).FirstOrDefault();//卡生命周期沉默期的数量
        //                    data.TestPeriod = conn.Query<IndexData>(sqlTestPeriod).Select(t => t.TestPeriod).FirstOrDefault();//卡生命周期测试期的数量
        //                    //CmccList.Add(data);
        //                    dto.DtoList = data;
        //                    dto.flg = "1";
        //                    dto.Msg = "成功!";
        //                }
        //                if (OperatorsFlg == "2")//电信
        //                {
        //                    sqlcardtotal = "select count(id) as CMCCCardTotal from card where status=1";//移动卡总数
        //                    sqlcardcttotal = "select count(id) as CTCardTotal from ct_card where  status=1";//电信卡总数
        //                    sqlcardcucctotal = "select count(id) as CUCCCardTotal from cucc_card where  status=1";//联通卡总数
        //                    sqlcardnb = "select NB as NBNum from countcardtypenum where OperatorName = '中国电信' and CompanyID = '1556265186243'";
        //                    //sqlmonthflow = "select Flow as MonthFlowTotal from company_flow where CompanyID='" + CompanyID + "' ORDER BY Date desc";
        //                    sqlNormal = "select sum(Card_State= '00') as Normal from ct_card where  status=1";//状态正常 已激活
        //                    sqlShutdown = "select sum(Card_State='02') as Shutdown from ct_card where  status=1";//用户状态停机
        //                    sqlActiveNum = "select count(id) as ActiveNum from ct_card where  Card_Monthlyusageflow is not null and Card_Monthlyusageflow !='' and Card_Monthlyusageflow!='0' and status=1";//月活跃有流量产生的卡
        //                    sqlOnline = "select SUM(Card_WorkState='01') as Online from ct_card where status=1";//物联卡在线数量
        //                    sqlOffline = "select SUM(Card_WorkState='00') as Offline from ct_card where status=1";//物联卡离线数量
        //                    sqlNormalUse = "";//就是物联网卡状态正常已激活再用
        //                    //sqlSilentPeriod = "select sum(Card_State= '1' or Card_State = '7') as SilentPeriod from card where  status=1";电信没有沉默期
        //                    sqlTestPeriod = "select sum(Card_State= '7' or Card_State= '10') as TestPeriod from ct_card where  status=1";
        //                    dto.CMCCCardTotal = conn.Query<IndexDto>(sqlcardtotal).Select(t => t.CMCCCardTotal).FirstOrDefault();//移动卡总数
        //                    dto.CUCCCardTotal = conn.Query<IndexDto>(sqlcardcucctotal).Select(t => t.CUCCCardTotal).FirstOrDefault();//联通卡总数
        //                    dto.CTCardTotal = conn.Query<IndexDto>(sqlcardcttotal).Select(t => t.CTCardTotal).FirstOrDefault();//电信卡总数
        //                    IndexData data = new IndexData();
        //                    data.NBNum = conn.Query<IndexData>(sqlcardnb).Select(t => t.NBNum).FirstOrDefault();//nb卡总数
        //                    data.NoNBNum = dto.CTCardTotal - data.NBNum;//2G、4G卡数量
        //                    //data.MonthFlowTotal = conn.Query<IndexData>(sqlmonthflow).Select(t => t.MonthFlowTotal).FirstOrDefault();//月流量总数
        //                    //data.MonthFlowTotal = data.MonthFlowTotal / 1024;//月流量总数MB
        //                    data.Normal = conn.Query<IndexData>(sqlNormal).Select(t => t.Normal).FirstOrDefault();//物联网卡状态已激活 再用 正常状态
        //                    decimal activeflow = conn.Query<IndexData>(sqlActiveNum).Select(t => t.ActiveNum).FirstOrDefault();//本月使用了流量的卡
        //                    if (data.Normal != 0)
        //                    {
        //                        data.ActiveNum = Math.Round((activeflow / data.Normal) * 100, 1);//月活跃度
        //                    }
        //                    data.Online = conn.Query<IndexData>(sqlOnline).Select(t => t.Online).FirstOrDefault();//物联网卡在线的数量
        //                    data.Offline = conn.Query<IndexData>(sqlOffline).Select(t => t.Offline).FirstOrDefault();//物联网卡离线的数量
        //                    data.Shutdown = conn.Query<IndexData>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();//物联网卡停机数量
        //                    data.Other = dto.CTCardTotal - data.Normal - data.Shutdown;//用户状态其他状态(总卡数量减去激活的和停机的卡数量)
        //                    data.NormalUse = data.Normal;//卡生命周期正常的数量
        //                    //data.SilentPeriod = conn.Query<IndexData>(sqlSilentPeriod).Select(t => t.SilentPeriod).FirstOrDefault();//卡生命周期沉默期的数量
        //                    data.TestPeriod = conn.Query<IndexData>(sqlTestPeriod).Select(t => t.TestPeriod).FirstOrDefault();//卡生命周期测试期的数量
        //                    //CtList.Add(data);
        //                    dto.DtoList = data;
        //                    dto.flg = "1";
        //                    dto.Msg = "成功!";
        //                }
        //                if (OperatorsFlg == "3")//联通
        //                {
        //                    sqlcardtotal = "select count(id) as CMCCCardTotal from card where status=1";//移动卡总数
        //                    sqlcardcttotal = "select count(id) as CTCardTotal from ct_card where  status=1";//电信卡总数
        //                    sqlcardcucctotal = "select count(id) as CUCCCardTotal from cucc_card where  status=1";//联通卡总数
        //                    sqlcardnb = "select NB as NBNum from countcardtypenum where OperatorName = '中国联通' and CompanyID = '1556265186243'";
        //                    //sqlmonthflow = "select Flow as MonthFlowTotal from company_flow where CompanyID='" + CompanyID + "' ORDER BY Date desc";
        //                    sqlNormal = "select sum(Card_State= '2') as Normal from cucc_card where  status=1";//状态正常 已激活
        //                    sqlShutdown = "select sum(Card_State='02') as Shutdown from cucc_card where  status=1";//用户状态停机
        //                    sqlActiveNum = "select count(id) as ActiveNum from cucc_card where  Card_Monthlyusageflow is not null and Card_Monthlyusageflow !='' and Card_Monthlyusageflow!='0' and status=1";//月活跃有流量产生的卡
        //                    sqlOnline = "select SUM(Card_WorkState='01') as Online from cucc_card where status=1";//物联卡在线数量
        //                    sqlOffline = "select SUM(Card_WorkState='00') as Offline from cucc_card where status=1";//物联卡离线数量
        //                    sqlNormalUse = "";//就是物联网卡状态正常已激活再用
        //                    sqlSilentPeriod = "select sum(Card_State = '07') as SilentPeriod from cucc_card where  status=1";//沉默期
        //                    sqlTestPeriod = "select sum(Card_State= '0') as TestPeriod from cucc_card where  status=1";//测试期
        //                    dto.CMCCCardTotal = conn.Query<IndexDto>(sqlcardtotal).Select(t => t.CMCCCardTotal).FirstOrDefault();//移动卡总数
        //                    dto.CUCCCardTotal = conn.Query<IndexDto>(sqlcardcucctotal).Select(t => t.CUCCCardTotal).FirstOrDefault();//联通卡总数
        //                    dto.CTCardTotal = conn.Query<IndexDto>(sqlcardcttotal).Select(t => t.CTCardTotal).FirstOrDefault();//电信卡总数
        //                    IndexData data = new IndexData();
        //                    data.NBNum = conn.Query<IndexData>(sqlcardnb).Select(t => t.NBNum).FirstOrDefault();//nb卡总数
        //                    data.NoNBNum = dto.CUCCCardTotal - data.NBNum;//2G、4G卡数量
        //                    //data.MonthFlowTotal = conn.Query<IndexData>(sqlmonthflow).Select(t => t.MonthFlowTotal).FirstOrDefault();//月流量总数
        //                    //data.MonthFlowTotal = data.MonthFlowTotal / 1048576;//月流量总数MB 联通的流量返回单位为B
        //                    data.Normal = conn.Query<IndexData>(sqlNormal).Select(t => t.Normal).FirstOrDefault();//物联网卡状态已激活 再用 正常状态
        //                    decimal activeflow = conn.Query<IndexData>(sqlActiveNum).Select(t => t.ActiveNum).FirstOrDefault();//本月使用了流量的卡
        //                    //data.ActiveNum = (activeflow / data.Normal) * 100;//月活跃度
        //                    if (data.Normal != 0)
        //                    {
        //                        data.ActiveNum = Math.Round((activeflow / data.Normal) * 100, 1);//月活跃度
        //                    }
        //                    data.Online = conn.Query<IndexData>(sqlOnline).Select(t => t.Online).FirstOrDefault();//物联网卡在线的数量
        //                    data.Offline = conn.Query<IndexData>(sqlOffline).Select(t => t.Offline).FirstOrDefault();//物联网卡离线的数量
        //                    data.Shutdown = conn.Query<IndexData>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();//物联网卡停机数量
        //                    data.Other = dto.CUCCCardTotal - data.Normal - data.Shutdown;//用户状态其他状态(总卡数量减去激活的和停机的卡数量)
        //                    data.NormalUse = data.Normal;//卡生命周期正常的数量
        //                    data.SilentPeriod = conn.Query<IndexData>(sqlSilentPeriod).Select(t => t.SilentPeriod).FirstOrDefault();//卡生命周期沉默期的数量
        //                    data.TestPeriod = conn.Query<IndexData>(sqlTestPeriod).Select(t => t.TestPeriod).FirstOrDefault();//卡生命周期测试期的数量
        //                    //CuccList.Add(data);
        //                    dto.DtoList = data;
        //                    dto.flg = "1";
        //                    dto.Msg = "成功!";
        //                }
        //            }
        //            else
        //            {
        //                if (OperatorsFlg == "1")//移动
        //                {
        //                    sqlcardtotal = "select count(id) as CMCCCardTotal from card_copy1 where status=1 and Card_CompanyID = '" + CompanyID + "'";//移动卡总数
        //                    sqlcardcttotal = "select count(id) as CTCardTotal from ct_cardcopy where  status=1 and Card_CompanyID = '" + CompanyID + "'";//电信卡总数
        //                    sqlcardcucctotal = "select count(id) as CUCCCardTotal from cucc_cardcopy where  status=1 and Card_CompanyID = '" + CompanyID + "'";//联通卡总数
        //                    sqlcardnb = "select NB as NBNum from countcardtypenum where OperatorName = '中国移动' and CompanyID = '" + CompanyID + "'";
        //                    sqlmonthflow = "select Flow as MonthFlowTotal from company_flow where CompanyID='" + CompanyID + "' ORDER BY Date desc";
        //                    sqlNormal = "select sum(Card_State= '00' or Card_State = '2') as Normal from card_copy1 where  status=1  and Card_CompanyID = '" + CompanyID + "'";//状态正常 已激活
        //                    sqlShutdown = "select sum(Card_State= '01' or Card_State='02' or Card_State='4') as Shutdown from card_copy1 where  status=1  and Card_CompanyID = '" + CompanyID + "'";//用户状态停机
        //                    sqlActiveNum = "select count(id) as ActiveNum from card_copy1 where  Card_Monthlyusageflow is not null and Card_Monthlyusageflow !='' and Card_Monthlyusageflow!='0' and status=1  and Card_CompanyID = '" + CompanyID + "'";//月活跃有流量产生的卡
        //                    sqlOnline = "select SUM(Card_WorkState='01') as Online from card_copy1 where status=1  and Card_CompanyID = '" + CompanyID + "'";//物联卡在线数量
        //                    sqlOffline = "select SUM(Card_WorkState='00') as Offline from card_copy1 where status=1 and Card_CompanyID = '" + CompanyID + "'";//物联卡离线数量
        //                    sqlNormalUse = "";//就是物联网卡状态正常已激活再用
        //                    sqlSilentPeriod = "select sum(Card_State= '1' or Card_State = '7') as SilentPeriod from card_copy1 where  status=1 and Card_CompanyID = '" + CompanyID + "'";
        //                    sqlTestPeriod = "select sum(Card_State= '6') as TestPeriod from card_copy1 where  status=1 and Card_CompanyID = '" + CompanyID + "'";
        //                    dto.CMCCCardTotal = conn.Query<IndexDto>(sqlcardtotal).Select(t => t.CMCCCardTotal).FirstOrDefault();//移动卡总数
        //                    dto.CUCCCardTotal = conn.Query<IndexDto>(sqlcardcucctotal).Select(t => t.CUCCCardTotal).FirstOrDefault();//联通卡总数
        //                    dto.CTCardTotal = conn.Query<IndexDto>(sqlcardcttotal).Select(t => t.CTCardTotal).FirstOrDefault();//电信卡总数
        //                    IndexData data = new IndexData();
        //                    data.NBNum = conn.Query<IndexData>(sqlcardnb).Select(t => t.NBNum).FirstOrDefault();//nb卡总数
        //                    data.NoNBNum = dto.CMCCCardTotal - data.NBNum;//2G、4G卡数量
        //                    data.MonthFlowTotal = conn.Query<IndexData>(sqlmonthflow).Select(t => t.MonthFlowTotal).FirstOrDefault();//月流量总数
        //                    data.MonthFlowTotal = Math.Round(data.MonthFlowTotal / 1024, 2);//月流量总数MB
        //                    data.Normal = conn.Query<IndexData>(sqlNormal).Select(t => t.Normal).FirstOrDefault();//物联网卡状态已激活 再用 正常状态
        //                    decimal activeflow = conn.Query<IndexData>(sqlActiveNum).Select(t => t.ActiveNum).FirstOrDefault();//本月使用了流量的卡
        //                    if (data.Normal != 0)
        //                    {
        //                        data.ActiveNum = Math.Round((activeflow / data.Normal) * 100, 1);//月活跃度
        //                    }
        //                    data.Online = conn.Query<IndexData>(sqlOnline).Select(t => t.Online).FirstOrDefault();//物联网卡在线的数量
        //                    data.Offline = conn.Query<IndexData>(sqlOffline).Select(t => t.Offline).FirstOrDefault();//物联网卡离线的数量
        //                    data.Shutdown = conn.Query<IndexData>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();//物联网卡停机数量
        //                    data.Other = dto.CMCCCardTotal - data.Normal - data.Shutdown;//用户状态其他状态(卡总数减去激活状态的减去停机状态的)
        //                    data.NormalUse = data.Normal;//卡生命周期正常的数量
        //                    data.SilentPeriod = conn.Query<IndexData>(sqlSilentPeriod).Select(t => t.SilentPeriod).FirstOrDefault();//卡生命周期沉默期的数量
        //                    data.TestPeriod = conn.Query<IndexData>(sqlTestPeriod).Select(t => t.TestPeriod).FirstOrDefault();//卡生命周期测试期的数量
        //                    //CmccList.Add(data);
        //                    dto.DtoList = data;
        //                    dto.flg = "1";
        //                    dto.Msg = "成功!";
        //                }
        //                if (OperatorsFlg == "2")//电信
        //                {
        //                    sqlcardtotal = "select count(id) as CMCCCardTotal from card_copy1 where status=1 and Card_CompanyID = '" + CompanyID + "'";//移动卡总数
        //                    sqlcardcttotal = "select count(id) as CTCardTotal from ct_cardcopy where  status=1 and Card_CompanyID = '" + CompanyID + "'";//电信卡总数
        //                    sqlcardcucctotal = "select count(id) as CUCCCardTotal from cucc_cardcopy where  status=1 and Card_CompanyID = '" + CompanyID + "'";//联通卡总数
        //                    sqlcardnb = "select NB as NBNum from countcardtypenum where OperatorName = '中国电信' and CompanyID = '" + CompanyID + "'";
        //                    //sqlmonthflow = "select Flow as MonthFlowTotal from company_flow where CompanyID='" + CompanyID + "' ORDER BY Date desc";
        //                    sqlNormal = "select sum(Card_State= '00') as Normal from ct_cardcopy where  status=1 and Card_CompanyID = '" + CompanyID + "'";//状态正常 已激活
        //                    sqlShutdown = "select sum(Card_State='02') as Shutdown from ct_cardcopy where  status=1 and Card_CompanyID = '" + CompanyID + "'";//用户状态停机
        //                    sqlActiveNum = "select count(id) as ActiveNum from ct_cardcopy where  Card_Monthlyusageflow is not null and Card_Monthlyusageflow !='' and Card_Monthlyusageflow!='0' and status=1 and Card_CompanyID = '" + CompanyID + "'";//月活跃有流量产生的卡
        //                    sqlOnline = "select SUM(Card_WorkState='01') as Online from ct_cardcopy where status=1 and Card_CompanyID = '" + CompanyID + "'";//物联卡在线数量
        //                    sqlOffline = "select SUM(Card_WorkState='00') as Offline from ct_cardcopy where status=1 and Card_CompanyID = '" + CompanyID + "'";//物联卡离线数量
        //                    sqlNormalUse = "";//就是物联网卡状态正常已激活再用
        //                    //sqlSilentPeriod = "select sum(Card_State= '1' or Card_State = '7') as SilentPeriod from card where  status=1";电信没有沉默期
        //                    sqlTestPeriod = "select sum(Card_State= '7' or Card_State= '10') as TestPeriod from ct_cardcopy where  status=1 and Card_CompanyID = '" + CompanyID + "'";
        //                    dto.CMCCCardTotal = conn.Query<IndexDto>(sqlcardtotal).Select(t => t.CMCCCardTotal).FirstOrDefault();
        //                    dto.CUCCCardTotal = conn.Query<IndexDto>(sqlcardcucctotal).Select(t => t.CUCCCardTotal).FirstOrDefault();//联通卡总数
        //                    dto.CTCardTotal = conn.Query<IndexDto>(sqlcardcttotal).Select(t => t.CTCardTotal).FirstOrDefault();//电信卡总数
        //                    IndexData data = new IndexData();
        //                    data.NBNum = conn.Query<IndexData>(sqlcardnb).Select(t => t.NBNum).FirstOrDefault();//nb卡总数
        //                    data.NoNBNum = dto.CTCardTotal - data.NBNum;//2G、4G卡数量
        //                    //data.MonthFlowTotal = conn.Query<IndexData>(sqlmonthflow).Select(t => t.MonthFlowTotal).FirstOrDefault();//月流量总数
        //                    //data.MonthFlowTotal = data.MonthFlowTotal / 1024;//月流量总数MB
        //                    data.Normal = conn.Query<IndexData>(sqlNormal).Select(t => t.Normal).FirstOrDefault();//物联网卡状态已激活 再用 正常状态
        //                    decimal activeflow = conn.Query<IndexData>(sqlActiveNum).Select(t => t.ActiveNum).FirstOrDefault();//本月使用了流量的卡
        //                    if (data.Normal != 0)
        //                    {
        //                        data.ActiveNum = Math.Round((activeflow / data.Normal) * 100, 1);//月活跃度
        //                    }
        //                    data.Online = conn.Query<IndexData>(sqlOnline).Select(t => t.Online).FirstOrDefault();//物联网卡在线的数量
        //                    data.Offline = conn.Query<IndexData>(sqlOffline).Select(t => t.Offline).FirstOrDefault();//物联网卡离线的数量
        //                    data.Shutdown = conn.Query<IndexData>(sqlShutdown).Select(t => t.Shutdown).FirstOrDefault();//物联网卡停机数量
        //                    data.Other = dto.CTCardTotal - data.Normal - data.Shutdown;//用户状态其他状态(总卡数量减去激活的和停机的卡数量)
        //                    data.NormalUse = data.Normal;//卡生命周期正常的数量
        //                    //data.SilentPeriod = conn.Query<IndexData>(sqlSilentPeriod).Select(t => t.SilentPeriod).FirstOrDefault();//卡生命周期沉默期的数量
        //                    data.TestPeriod = conn.Query<IndexData>(sqlTestPeriod).Select(t => t.TestPeriod).FirstOrDefault();//卡生命周期测试期的数量
        //                    //CtList.Add(data);
        //                    dto.DtoList = data;
        //                    dto.flg = "1";
        //                    dto.Msg = "成功!";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.flg = "-1";
        //        dto.Msg = "错误:" + ex;
        //    }
        //    return dto;
        //}


        ///<summary>
        ///主界面数据  OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        public static IndexDto GetIndexInfo(string CompanyID, string OperatorsFlg)
        {
            IndexDto info = new IndexDto();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    IndexData DtoList = new IndexData();
                    string sqlcardcmcc = "select CMCC_CardTotal as CMCCCardTotal from indexdatacount where CompanyID='" + CompanyID + "'";
                    string sqlcardcucc = "select CUCC_CardTotal as CUCCCardTotal from indexdatacount where CompanyID='" + CompanyID + "'";
                    string sqlcardct = "select CT_CardTotal as CTCardTotal from indexdatacount where CompanyID='" + CompanyID + "'";
                    string sqlthreecard = "select Three_CardTotal as ThreeCardTotal from indexdatacount where CompanyID='" + CompanyID + "'";
                    info.CMCCCardTotal = conn.Query<IndexDto>(sqlcardcmcc).Select(t => t.CMCCCardTotal).FirstOrDefault();
                    info.CUCCCardTotal = conn.Query<IndexDto>(sqlcardcucc).Select(t => t.CUCCCardTotal).FirstOrDefault();
                    info.CTCardTotal = conn.Query<IndexDto>(sqlcardct).Select(t => t.CTCardTotal).FirstOrDefault();
                    info.ThreeCardTotal = conn.Query<IndexDto>(sqlthreecard).Select(t => t.ThreeCardTotal).FirstOrDefault();
                    string sql = string.Empty;
                    if (OperatorsFlg == "1")//移动
                    {
                        sql = "select CMCC_NBNum as NBNum,CMCC_NoNBNum as NoNBNum,CMCC_MonthFlowTotal as MonthFlowTotal,CMCC_ActiveNum as ActiveNum,CMCC_Online as Online,CMCC_Offline as Offline," +
                            "CMCC_Normal as Normal,CMCC_Shutdown as Shutdown,CMCC_Other as Other,CMCC_NormalUse as NormalUse,CMCC_SilentPeriod as SilentPeriod,CMCC_TestPeriod as TestPeriod " +
                            "from indexdatacount where CompanyID='" + CompanyID + "'";
                        var list = conn.Query<IndexData>(sql).FirstOrDefault();
                        if (list != null)
                        {
                            DtoList.ActiveNum = list.ActiveNum;
                            DtoList.MonthFlowTotal = list.MonthFlowTotal;
                            DtoList.NBNum = list.NBNum;
                            DtoList.NoNBNum = list.NoNBNum;
                            DtoList.Normal = list.Normal;
                            DtoList.NormalUse = list.NormalUse;
                            DtoList.Offline = list.Offline;
                            DtoList.Online = list.Online;
                            DtoList.Other = list.Other;
                            DtoList.Shutdown = list.Shutdown;
                            DtoList.SilentPeriod = list.SilentPeriod;
                            DtoList.TestPeriod = list.TestPeriod;
                            info.DtoList = DtoList;
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                    
                    }
                    if (OperatorsFlg == "2")//电信
                    {
                        sql = "select CT_NBNum as NBNum,CT_NoNBNum as NoNBNum,CT_MonthFlowTotal as MonthFlowTotal,CT_ActiveNum as ActiveNum,CT_Online as Online,CT_Offline as Offline," +
                            "CT_Normal as Normal,CT_Shutdown as Shutdown,CT_Other as Other,CT_NormalUse as NormalUse,CT_SilentPeriod as SilentPeriod,CT_TestPeriod as TestPeriod " +
                            "from indexdatacount where CompanyID='" + CompanyID + "'";
                        var list = conn.Query<IndexData>(sql).FirstOrDefault();
                        if (list != null)
                        {
                            DtoList.ActiveNum = list.ActiveNum;
                            DtoList.MonthFlowTotal = list.MonthFlowTotal;
                            DtoList.NBNum = list.NBNum;
                            DtoList.NoNBNum = list.NoNBNum;
                            DtoList.Normal = list.Normal;
                            DtoList.NormalUse = list.NormalUse;
                            DtoList.Offline = list.Offline;
                            DtoList.Online = list.Online;
                            DtoList.Other = list.Other;
                            DtoList.Shutdown = list.Shutdown;
                            DtoList.SilentPeriod = list.SilentPeriod;
                            DtoList.TestPeriod = list.TestPeriod;
                            info.DtoList = DtoList;
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                    }
                    if (OperatorsFlg == "3")//联通
                    {
                        sql = "select CUCC_NBNum as NBNum,CUCC_NoNBNum as NoNBNum,CUCC_MonthFlowTotal as MonthFlowTotal,CUCC_ActiveNum as ActiveNum,CUCC_Online as Online,CUCC_Offline as Offline," +
                            "CUCC_Normal as Normal,CUCC_Shutdown as Shutdown,CUCC_Other as Other,CUCC_NormalUse as NormalUse,CUCC_SilentPeriod as SilentPeriod,CUCC_TestPeriod as TestPeriod " +
                            "from indexdatacount where CompanyID='" + CompanyID + "'";
                        var list = conn.Query<IndexData>(sql).FirstOrDefault();
                        if (list != null)
                        {
                            DtoList.ActiveNum = list.ActiveNum;
                            DtoList.MonthFlowTotal = list.MonthFlowTotal;
                            DtoList.NBNum = list.NBNum;
                            DtoList.NoNBNum = list.NoNBNum;
                            DtoList.Normal = list.Normal;
                            DtoList.NormalUse = list.NormalUse;
                            DtoList.Offline = list.Offline;
                            DtoList.Online = list.Online;
                            DtoList.Other = list.Other;
                            DtoList.Shutdown = list.Shutdown;
                            DtoList.SilentPeriod = list.SilentPeriod;
                            DtoList.TestPeriod = list.TestPeriod;
                            info.DtoList = DtoList;
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                    }
                    if (OperatorsFlg == "4")//全网通
                    {
                        sql = "select Three_NBNum as NBNum,Three_NoNBNum as NoNBNum,Three_MonthFlowTotal as MonthFlowTotal from indexdatacount where CompanyID='" + CompanyID + "'";
                        var list = conn.Query<IndexData>(sql).FirstOrDefault();
                        if (list != null)
                        {
                            DtoList.ActiveNum =0;
                            DtoList.MonthFlowTotal = list.MonthFlowTotal;
                            DtoList.NBNum = list.NBNum;
                            DtoList.NoNBNum = list.NoNBNum;
                            DtoList.Normal = 0;
                            DtoList.NormalUse = 0;
                            DtoList.Offline = 0;
                            DtoList.Online = 0;
                            DtoList.Other =0;
                            DtoList.Shutdown = 0;
                            DtoList.SilentPeriod =0;
                            DtoList.TestPeriod = 0;
                            info.DtoList = DtoList;
                            info.flg = "1";
                            info.Msg = "成功!";
                        }
                    }
                }
                 
            }
            catch(Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:"+ex;
            }
            return info;
        }


        ///<summary>
        ///主界面数据返回用户总卡使用的近七天流量 按运营商
        /// </summary>
        public static CardFlowDays GetIndexFlow(string CompanyID, string OperatorsFlg)
        {
            CardFlowDays dto = new CardFlowDays();
            List<FlowDay> data = new List<FlowDay>();
            List<FlowDays> CtData = new List<FlowDays>();
            List<FlowDays> CuccData = new List<FlowDays>();
            FlowDay days = new FlowDay();
            List<decimal> flow = new List<decimal>();
            List<string> date = new List<string>();
            decimal flows = 0;
            string dates = string.Empty;
            try
            {
                if (OperatorsFlg == "1")//移动
                {
                    string sqlflow = "select * from company_flow where CompanyID='" + CompanyID + "'";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        var lists = conn.Query<company_flow>(sqlflow).OrderBy(t => t.Date).ToList();
                        foreach (var item in lists)
                        {
                            flows = Convert.ToDecimal(item.Flow);
                            dates = item.Date.ToShortDateString();
                            flow.Add(flows);
                            date.Add(dates);
                            days.date = date;
                            days.flow = flow;
                        }
                        data.Add(days);
                        dto.data = data;
                        dto.flg = "1";
                        dto.Msg = "成功!";
                    }
                }
                if (OperatorsFlg == "2")//电信
                {
                    for (int i = 7; i > 0; i--)
                    {
                        DateTime time = DateTime.Now.AddDays(-i);
                        date.Add(time.Year.ToString() + "/" + time.Month.ToString() + "/" + time.Day.ToString());
                        flow.Add(0);
                        days.date = date;
                        days.flow = flow;
                    }
                    data.Add(days);
                    dto.data = data;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
                if (OperatorsFlg == "3")//联通
                {
                    for (int i = 7; i > 0; i--)
                    {
                        DateTime time = DateTime.Now.AddDays(-i);
                        date.Add(time.Year.ToString() + "/" + time.Month.ToString() + "/" + time.Day.ToString());
                        flow.Add(0);
                        days.date = date;
                        days.flow = flow;
                    }
                    data.Add(days);
                    dto.data = data;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
                if (OperatorsFlg == "4")//全网通
                {
                    string sqlflow = "select * from company_threeflow where CompanyID='" + CompanyID + "'";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        var lists = conn.Query<company_flow>(sqlflow).OrderBy(t => t.Date).ToList();
                        foreach (var item in lists)
                        {
                            flows = Convert.ToDecimal(item.Flow);
                            dates = item.Date.ToShortDateString();
                            flow.Add(flows);
                            date.Add(dates);
                            days.date = date;
                            days.flow = flow;
                        }
                        data.Add(days);
                        dto.data = data;
                        dto.flg = "1";
                        dto.Msg = "成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                dto.data = null;
                dto.flg = "-1";
                dto.Msg = "失败:" + ex;
            }
            return dto;
        }

        ///<summary>
        ///返回十张本月流量使用最多的卡号和流量值 OperatorsFlg 1:移动 2:电信 3:联通 4:全网通
        /// </summary>
        public static MaxFlowDto GetMonthMaxFlow(string CompanyID, string OperatorsFlg)
        {
            MaxFlowDto dto = new MaxFlowDto();
            List<FlowDto> flowDtos = new List<FlowDto>();
            List<string> key = new List<string>();
            List<string> value = new List<string>();
            string sql = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (CompanyID == "1556265186243")//奇迹
                    {
                        if (OperatorsFlg == "1")//移动
                        {
                            sql = "select Card_ICCID as  Card_ID,Card_Monthlyusageflow from card where status=1 order by CONVERT (Card_Monthlyusageflow, DECIMAL) desc limit 5";
                            var cardlist = conn.Query<Card>(sql).ToList();
                            
                            foreach (var item in cardlist)
                            {
                                FlowDto flow = new FlowDto();
                                flow.key=item.Card_ID;
                                if (!string.IsNullOrWhiteSpace(item.Card_Monthlyusageflow))
                                {
                                    decimal flows = Convert.ToDecimal(item.Card_Monthlyusageflow);
                                    flows = flows / 1024;
                                    flows = Math.Round(flows,2);
                                    flow.value = flows.ToString();
                                }
                                else
                                {
                                    flow.value ="0";
                                }
                                flowDtos.Add(flow);
                            }
                            dto.flowDtos = flowDtos;
                            dto.flg = "1";
                            dto.Msg = "成功!";
                        }
                        if (OperatorsFlg == "2")//电信
                        {
                            sql = "select Card_ICCID as Card_ID,Card_Monthlyusageflow from ct_card where status=1  order by CONVERT (Card_Monthlyusageflow, DECIMAL) desc limit 5";
                            var cardlist = conn.Query<Card>(sql).ToList();
                            foreach (var item in cardlist)
                            {
                                FlowDto flow = new FlowDto();
                                flow.key = item.Card_ID;
                                if (!string.IsNullOrWhiteSpace(item.Card_Monthlyusageflow))
                                {
                                    decimal flows = Convert.ToDecimal(item.Card_Monthlyusageflow);
                                    flows = flows / 1024;
                                    flows = Math.Round(flows, 2);
                                    flow.value = flows.ToString();
                                }
                                else
                                {
                                    flow.value = "0";
                                }
                                flowDtos.Add(flow);
                            }
                            dto.flowDtos = flowDtos;
                            dto.flg = "1";
                            dto.Msg = "成功!";
                        }
                        if (OperatorsFlg == "3")//联通
                        {
                            sql = "select Card_ICCID as Card_ID,Card_Monthlyusageflow from cucc_card where status=1  order by CONVERT (Card_Monthlyusageflow, DECIMAL) desc limit 5";
                            var cardlist = conn.Query<Card>(sql).ToList();
                            foreach (var item in cardlist)
                            {
                                FlowDto flow = new FlowDto();
                                flow.key = item.Card_ICCID;
                                if (string.IsNullOrWhiteSpace(item.Card_Monthlyusageflow))
                                {
                                    item.Card_Monthlyusageflow = "0";
                                }
                                flow.value = item.Card_Monthlyusageflow;
                                flowDtos.Add(flow);
                            }
                            dto.flowDtos = flowDtos;
                            dto.flg = "1";
                            dto.Msg = "成功!";
                        }
                        if (OperatorsFlg == "4")//全网通
                        {
                            sql = "select SN as Card_ID,Card_totalflow as Card_Monthlyusageflow from three_card where status=1  order by CONVERT (Card_totalflow, DECIMAL) desc limit 5";
                            var cardlist = conn.Query<Card>(sql).ToList();
                            foreach (var item in cardlist)
                            {
                                FlowDto flow = new FlowDto();
                                flow.key = item.Card_ID;
                                decimal flows = Convert.ToDecimal(item.Card_Monthlyusageflow);
                                flows = Math.Round(flows, 2);
                                flow.value = flows.ToString();
                                flowDtos.Add(flow);
                            }
                            dto.flowDtos = flowDtos;
                            dto.flg = "1";
                            dto.Msg = "成功!";
                        }
                    }
                    else
                    {
                        if (OperatorsFlg == "1")//移动
                        {
                            sql = "select Card_ICCID as Card_ID,Card_Monthlyusageflow from card_copy1  where Card_CompanyID='" + CompanyID + "'  and status=1  order by CONVERT (Card_Monthlyusageflow, DECIMAL) desc limit 5";
                            var cardlist = conn.Query<Card>(sql).ToList();
                            foreach (var item in cardlist)
                            {
                                FlowDto flow = new FlowDto();
                                flow.key = item.Card_ID;
                                if (!string.IsNullOrWhiteSpace(item.Card_Monthlyusageflow))
                                {
                                    decimal flows = Convert.ToDecimal(item.Card_Monthlyusageflow);
                                    flows = flows / 1024;
                                    flows = Math.Round(flows, 2);
                                    flow.value = flows.ToString();
                                }
                                else
                                {
                                    flow.value = "0";
                                }
                                flowDtos.Add(flow);
                            }
                            dto.flowDtos = flowDtos;
                            dto.flg = "1";
                            dto.Msg = "成功!";
                        }
                        if (OperatorsFlg == "2")//电信
                        {
                            sql = "select Card_ICCID as Card_ID,Card_Monthlyusageflow from ct_cardcopy  where Card_CompanyID='" + CompanyID + "'   and status=1 order by CONVERT (Card_Monthlyusageflow, DECIMAL) desc limit 5";
                            var cardlist = conn.Query<Card>(sql).ToList();
                            foreach (var item in cardlist)
                            {
                                FlowDto flow = new FlowDto();
                                flow.key = item.Card_ID;
                                if (!string.IsNullOrWhiteSpace(item.Card_Monthlyusageflow))
                                {
                                    decimal flows = Convert.ToDecimal(item.Card_Monthlyusageflow);
                                    flows = flows / 1024;
                                    flows = Math.Round(flows, 2);
                                    flow.value = flows.ToString();
                                }
                                else
                                {
                                    flow.value = "0";
                                }
                                flowDtos.Add(flow);
                            }
                            dto.flowDtos = flowDtos;
                            dto.flg = "1";
                            dto.Msg = "成功!";
                        }
                        if (OperatorsFlg == "3")//联通
                        {
                            sql = "select Card_ICCID as Card_ID,Card_Monthlyusageflow from cucc_cardcopy where Card_CompanyID='" + CompanyID + "'  and status=1 order by CONVERT (Card_Monthlyusageflow, DECIMAL) desc limit 5";
                            var cardlist = conn.Query<Card>(sql).ToList();
                            foreach (var item in cardlist)
                            {
                                FlowDto flow = new FlowDto();
                                flow.key = item.Card_ICCID;
                                if (string.IsNullOrWhiteSpace(item.Card_Monthlyusageflow))
                                {
                                    item.Card_Monthlyusageflow = "0";
                                }
                                flow.value = item.Card_Monthlyusageflow;
                                flowDtos.Add(flow);
                            }
                            dto.flowDtos = flowDtos;
                            dto.flg = "1";
                            dto.Msg = "成功!";
                        }
                        if (OperatorsFlg == "4")//全网通
                        {
                            sql = "select SN as Card_ID,Card_totalflow as Card_Monthlyusageflow from three_cardcopy where Card_CompanyID='" + CompanyID + "'  and status=1 order by CONVERT (Card_totalflow, DECIMAL) desc limit 5";
                            var cardlist = conn.Query<Card>(sql).ToList();
                            foreach (var item in cardlist)
                            {
                                FlowDto flow = new FlowDto();
                                flow.key = item.Card_ID;
                                decimal flows = Convert.ToDecimal(item.Card_Monthlyusageflow);
                                flows = Math.Round(flows, 2);
                                flow.value = flows.ToString();
                                flowDtos.Add(flow);
                            }
                            dto.flowDtos = flowDtos;
                            dto.flg = "1";
                            dto.Msg = "成功!";
                        }
                    }
                }  
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败:" + ex;
            }
            return dto;
        }

        #endregion


        ///<summary>
        ///在线问题反馈
        /// </summary>
        public static Information SubmitFeedback(feedback para)
        {
            Information info = new Information();
            try
            {
                string sqladd = "insert into feedback (Company_ID,PhoneNum,Content,AddTime) values('" + para.Company_ID + "','" + para.PhoneNum + "','" + para.Content + "','" + DateTime.Now + "')";
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
                info.Msg = "失败!";
            }
            return info;
        }

        ///<summary>
        ///查看问题反馈信息
        /// </summary>
        public static FeedBackDto GetFeedBackInfo(string Company_ID)
        {
            FeedBackDto dto = new FeedBackDto();
            List<feedback> feedbacks = new List<feedback>();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sql = string.Empty;
                    if (Company_ID == "1556265186243")
                    {
                        sql = "select t1.id,t1.PhoneNum,t1.Content,t1.AddTime as AddTimes,t2.CompanyName,t1.Company_ID from feedback t1 left join company t2 on t1.Company_ID=t2.CompanyID";
                        var list = conn.Query<feedback>(sql).OrderByDescending(t => t.AddTimes).ToList();
                        foreach (var item in list)
                        {
                            feedback deed = new feedback();
                            deed.AddTime= item.AddTimes.ToString("yyyy-MM-dd hh:mm:ss");
                            deed.Company_ID = item.Company_ID;
                            deed.CompanyName = item.CompanyName;
                            deed.Content = item.Content;
                            deed.PhoneNum = item.PhoneNum;
                            feedbacks.Add(deed);
                        }
                        dto.feedbacks = feedbacks;
                    }
                    else
                    {
                        //string sqlss = "select * from feedback";
                        sql = "select t1.id,t1.PhoneNum,t1.Content,t1.AddTime as AddTimes,t2.CompanyName,t1.Company_ID from feedback t1 left join company t2 on t1.Company_ID=t2.CompanyID where  t1.Company_ID='" + Company_ID+"'";
                        var list = conn.Query<feedback>(sql).OrderByDescending(t => t.AddTimes).ToList();
                        foreach (var item in list)
                        {
                            feedback deed = new feedback();
                            deed.AddTime = item.AddTimes.ToString("yyyy-MM-dd hh:mm:ss");
                            deed.Company_ID = item.Company_ID;
                            deed.CompanyName = item.CompanyName;
                            deed.Content = item.Content;
                            deed.PhoneNum = item.PhoneNum;
                            feedbacks.Add(deed);
                        }
                        dto.feedbacks = feedbacks;
                    }                    
                    dto.flg = "1";
                    dto.Msg = "成功";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败!";
            }
            return dto;
        }
    }
}
