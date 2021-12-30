using Dapper;
using Esim7.Models;
using Esim7.Models.PowerManageModel;
using Esim7.ReturnMessage;
using Esim7.UNity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Esim7.Action
{
    public class CompanyAction
    {



        /// <summary>
        /// 添加公司 
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static List<Company> Add(Company com)
        {
            string UserName = com.Username;
            string User_Menu = "1,2,3,4,6,7,8,9,11,18,19,20,21,22,23,24,30,31,32,46,50,51,52,55,34,36,37,38";//给创建用户一个默认的菜单
            string CompanyID = Unit.GetTimeStamp(DateTime.Now);
            string companyname = com.CompanyName;
            List<Company> lii = new List<Company>();
            string remarks = com.Companyremarks;
            string Pawwword = com.Password;
            //string Company_ID = com.Company_ID;
            string Company_ID = com.CompanyID;
            bool f = false;
            int pid = 0;
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string SQL = "select id from cf_user where Company_ID='"+Company_ID+"'";
                pid = conn.Query<cf_user>(SQL).Select(t => t.id).FirstOrDefault();
                string sql2 = "select * from company where CompanyName=@CompanyName";
                Company user = new Company();
                List<Company> li = new List<Company>();
                //foreach (User item in li)
                //{
                li.Add(conn.Query<Company>(sql2, new { CompanyName = companyname }).SingleOrDefault());
                foreach (Company item in li)
                {
                    if (item != null || JudgeUser(UserName))
                    {
                        user.CompanyName = "公司已经存在或用户已经存在！";
                        li.Clear();
                        li.Add(user);
                        return li;
                    }
                }
                {
                    string sql = "INSERT INTO company (CompanyID,Companyname,CompanyOpeningDate,Companyremarks,status,CompanyPhone,CompanyAdress,LicenseCode,DutyParagraph,BankAccount,Bank,AddressPhone)" +
                        " VALUES(@companyID,@CompanyName,@CompanyOpeningDate,@CompanyRemarks,@status,@CompanyPhone,@CompanyAdress,@LicenseCode,@DutyParagraph,@BankAccount,@Bank,@AddressPhone)";
                    try
                    {
                        var result = conn.Execute(sql,
                                    new
                                    {
                                        companyID = CompanyID,
                                        CompanyName = com.CompanyName,
                                        CompanyOpeningDate = DateTime.Now,
                                        CompanyRemarks = com.Companyremarks,
                                        CompanyPhone = com.CompanyPhone,
                                        CompanyAdress = com.CompanyAdress,
                                        status = 1,
                                        LicenseCode=com.LicenseCode,
                                        Bank=com.Bank,
                                        BankAccount=com.BankAccount,
                                        DutyParagraph=com.DutyParagraph,
                                        AddressPhone=com.AddressPhone
                                    });
                        f = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    //com.flg = "1";
                    //com.MSg = "添加成功";
                    com.CompanyID = CompanyID;
                    lii.Add(com);
                }
            }
            if (f)
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string UserID = Unit.GetTimeStamp(DateTime.Now);
                    {
                        string sql = "INSERT INTO cf_user (UserID,LoginName,Loginpassword,status,Company_ID,User_Pid,User_Menu,Email) VALUES(@UserID,@LoginName,@Loginpassword,@status,@Company_ID,'" + pid + "','"+ User_Menu + "','"+com.Email+"')";
                        try
                        {
                            if (string.IsNullOrWhiteSpace(Pawwword))
                            {
                                Pawwword = "123456";
                            }
                            var result = conn.Execute(sql,
                                        new
                                        {
                                            UserID = UserID,
                                            LoginName = com.Username,
                                            Loginpassword = Unit.MD5_64(Pawwword),
                                            Company_ID = CompanyID,
                                            status = 1,
                                            User_Pid=pid
                                        });
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    //string sql = "select min(IMEI+0) as IMEI ,SUBSTRING(SN ,4) as SN from am900 where  oldimsi=''and EID='' and status=" + this.txt_status.Text.Trim();
                    //    conn.Query<User>(sql).AsList()
                    //    ;
                    // }
                }
            }
            return lii;
        }


        /// <summary>
        /// 无条件查询 所有企业
        /// </summary>
        /// <returns></returns>      
        public static List<Company> Query(string CompanyID)
        {
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                const string query = "select str_to_date(CompanyOpeningDate, '%Y-%m-%d %H')  as  CompanyOpeningDate , CompanyID ,CompanyName,Companyremarks ,CompanyPhone,CompanyAdress,status from company where status=1 and CompanyID=@CompanyID";
                // var info= conn.Query<Commpany>(query).ToList();
                return conn.Query<Company>(query, new { CompanyID = CompanyID }).ToList();
            }
        }


        /// <summary>
        ///  获取公司信息（总账户/二级用户）
        /// </summary>
        /// <param name="Company_ID"></param>
        /// <returns></returns>
        public static GetCompanyDto GetAllcompany1(string Company_ID,string CompanyName)
        {
            GetCompanyDto dto = new GetCompanyDto();
            string userids = string.Empty;
            try
            {
                string sqluserpid = "select id from cf_user where Company_ID='" + Company_ID + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    int userpid = conn.Query<Company>(sqluserpid).Select(s => s.id).FirstOrDefault();
                    string useridssql = "select Company_ID as CompanyID from cf_user where User_Pid=" + userpid + " and status=1";
                    var userlist = conn.Query<Company>(useridssql).ToList();
                    if (userlist.Count != 0)
                    {
                        foreach (var item in userlist)
                        {
                            userids += "'" + item.CompanyID + "'" + ",";
                        }
                        userids = userids.Substring(0, userids.Length - 1);
                        //string sql = "select * from company where CompanyID in(" + userids+ ")";
                        string sql = "select t1.CompanyID,t1.CompanyID as Company_ID,t1.CompanyName,t1.CompanyPhone,t1.CompanyAdress,t1.Companyremarks,t2.LoginName as Username,t2.Email, t1.CompanyTolCardNum as Number," +
                            "str_to_date(t1.CompanyOpeningDate, '%Y-%m-%d %H') as CompanyOpeningDate,t1.LicenseCode,t1.DutyParagraph,t1.Bank,t1.BankAccount,t1.AddressPhone, " +
                            "(case t1.status when '1' then '正常' else '禁用' end  ) as status from company t1 left join cf_user t2 on t1.CompanyID = t2.Company_ID" +
                            " where t1.CompanyID in(" + userids + ")";
                        if (string.IsNullOrWhiteSpace(CompanyName))
                        {
                            dto.conpany = conn.Query<Company>(sql).ToList().OrderByDescending(t => t.CompanyOpeningDate).ToList();
                            dto.flg = "1";
                            dto.MSg = "Success";
                        }
                        if (!string.IsNullOrWhiteSpace(CompanyName))
                        {
                            dto.conpany = conn.Query<Company>(sql).ToList().Where(t=>t.CompanyName.Contains(CompanyName)).ToList().OrderByDescending(t => t.CompanyOpeningDate).ToList();
                            dto.flg = "1";
                            dto.MSg = "Success";
                        }
                    }
                    if (userlist.Count == 0)
                    {
                        List<Company> companies = new List<Company>();
                        dto.conpany = companies;
                        dto.flg = "1";
                        dto.MSg = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                dto.flg = "0";
                dto.MSg = "error" + ex;
            }
            return dto;
        }


        /// <summary>
        ///  公司卡数据查询 每个公司 包括客户续费日期 和真实续费日期   20191008
        /// </summary>
        /// <param name="Card_CompanyID"></param>
        /// <param name="PagNumber"></param>
        /// <param name="Num"></param>
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
                    //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                    string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,t1.Card_IMSI,t1.Card_Type,
                                    t1.Card_State,t1.Card_WorkState,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate 
                                    ,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as Card_EndTime from card t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID
                                    left join  operators t3 on t3.operatorsID=t2.operatorsID left join outofstock t4 on  t4.OutofstockID=t1.OutofstockID
                                    left join taocan t5 on t5.SetMealID=t4.SetMealID left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID
                                    left join city t8 on t8.cityID=t2.cityID where t1.status=1" + s;
                    Card card = new Card();
                    //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
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
                //  item.Card_State = items.STATUS;
                //}

                #endregion

                int i = 0;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {

                    //string sql2 = @" SELECT   count(*) from card t1 LEFT JOIN accounts t2  on t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID
                    //                left join outofstock t4 on   t4.OutofstockID=t1.OutofstockID left join taocan t5 on t5.SetMealID=t4.SetMealID left join cardtype t6 on t6.CardTypeID=t1.cardType
                    //                left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID left join city t8 on t8.cityID=t2.cityID where t1.status=1   group by  t1.Card_ICCID";
                    //i = conn.Query<string>(sql2).ToList().Count;
                    string sql2 = @"select count(*) as total from card where status=1";
                    string t = string.Empty;
                    t = conn.Query<Card_API>(sql2).Select(a => a.total).FirstOrDefault();
                    i = Convert.ToInt32(t);
                }
                c.total = i.ToString();
                c.CardAndAPI = li;
            }
            else
            {
                s += " limit " + (PagNumber - 1) * Num + "," + Num;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //string sql2 = "select  Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                    string sql2 = @"SELECT  DISTINCT t1.Card_ICCID  ,t1.Card_IMEI,t1.Card_State,t1.Card_WorkState,t1.Card_Monthlyusageflow, t1.Card_ID,
                                    t1.Card_IMSI,t1.Card_Type,date_format(t1.Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,        
                                    date_format( t1.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,t1.Card_Remarks, t1.status, t1.Card_CompanyID
                                    ,t2.operatorsID ,t3.operatorsName ,t5.SetmealName ,t6.CardTypeName,t7.CardXTName,t8.cityName,date_format(t1.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate ,date_format(t9.Card_EndTime, '%Y-%m-%d %H:%i:%s') as RenewDate_custom ,
                                    date_format( t9.Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate_Custom from card t1 LEFT JOIN accounts t2  on
                                    t2.accountID=t1.accountsID left join  operators t3 on t3.operatorsID=t2.operatorsID left join outofstock t4 on  t4.OutofstockID=t1.OutofstockID
                                    left join taocan t5 on t5.SetMealID=t4.SetMealID left join cardtype t6 on t6.CardTypeID=t1.cardType left join card_xingtai t7 on t7.CardXTID=t5.CardxingtaiID
                                    left join city t8 on t8.cityID=t2.cityID inner join card_copy1 t9 on t9.Card_ICCID=t1.Card_ICCID where t1.status=1 and t9.Card_CompanyID='"+Card_CompanyID+"' " + s;

                    Card card = new Card();
                    //Card_Opendate   Card_Monthlyusageflow   Card_WorkState   Card_State
                    li = conn.Query<Card>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList();
                }
                s = " and t1.Card_CompanyID=@Card_CompanyID    group by  t1.Card_ICCID ";
                int i = 0;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //string sql2 = "select    Card_State,Card_WorkState,Card_Monthlyusageflow, Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;
                    string sql2 = @" SELECT   count(*) from card_copy1 t1 where t1.status=1   " + s;
                    i = conn.Query<string>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList().Count;
                }
                c.total = i.ToString();
                c.CardAndAPI = li;              
            }
            return c;
        }



        /// <summary>
        /// 公司注销
        /// </summary>
        /// <param name="CompanyName"></param>
        /// <returns></returns>
        public static Information Delete(string CompanyID)
        {
            Information r = new Information();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string delcompany = "UPDATE  company set status=0 where CompanyID='"+CompanyID+"'";
                string dekuser = "UPDATE  cf_user set status=-1 where Company_ID='" + CompanyID+ "'";
                try
                {
                    conn.Execute(delcompany);
                    conn.Execute(dekuser);
                    r.flg = "1";
                    r.Msg = "注销成功!";
                }
                catch (Exception ex)
                {
                    r.flg = "-1";
                    r.Msg = "注销失败:"+ex;
                }
                return r;
            }
        }

        ///// <summary>
        /////    公司信息修改
        ///// </summary>
        ///// <param name="com"></param>
        ///// <returns></returns>
        //public static Return_Company Update(Company com)
        //{
        //    Return_Company r = new Return_Company();
        //    Company company = new Company();
        //    using (IDbConnection conn = DapperService.MySqlConnection())
        //    {
        //        string sql = "UPDATE  company set CompanyPhone=@CompanyPhone,CompanyAdress=@CompanyAdress ,Companyremarks=@Companyremarks where CompanyName=@CompanyName  and status=1";
        //        var result = conn.Execute(sql, new { CompanyName = com.CompanyName, CompanyPhone = com.CompanyPhone, CompanyAdress = com.CompanyAdress, Companyremarks = com.Companyremarks });
        //        if (result == 1)
        //        {
        //            // company.CompanyName ="用户:"+ CompanyName+"  注销成功";
        //            r.flg = "1";
        //            r.MSg = "用户:" + com.CompanyName + " 信息修改成功";
        //        }
        //        else
        //        {
        //            r.flg = "0";
        //            r.MSg = "该用户不存在或已经注销";
        //        }
        //        return r;
        //    }
        //}

        /// <summary>
        ///    公司信息修改
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static Information Update(Company para)
        {
            Information r = new Information();
           
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string companyname = string.Empty;
                string updateompany = "UPDATE  company set CompanyPhone='"+para.CompanyPhone+"',CompanyAdress='"+para.CompanyAdress+"',Companyremarks='"+para.Companyremarks+ "',CompanyName='"+para.CompanyName+"'," +
                    "LicenseCode='"+para.LicenseCode+"',DutyParagraph='"+para.DutyParagraph+"',Bank='"+para.Bank+"',BankAccount='"+para.BankAccount+ "',AddressPhone='"+para.AddressPhone+"' where CompanyID='" + para.CompanyID+"'";
                string updateuser = "update cf_user set Email='"+para.Email+ "' where Company_ID='" + para.CompanyID+"'";
                try
                {
                    string sqlcomname = "select CompanyName from company where CompanyName='" + para.CompanyName + "' and status=1";
                    string sqlcomid = "select CompanyName from company where CompanyID='" + para.CompanyID + "' and status=1";
                    companyname = conn.Query<Company>(sqlcomid).Select(t => t.CompanyName).FirstOrDefault();
                    var listcom = conn.Query<Company>(sqlcomname).ToList();
                    if (companyname==para.CompanyName)
                    {
                        conn.Execute(updateompany);
                        conn.Execute(updateuser);
                        r.Msg = "修改信息成功!";
                        r.flg = "1";
                    }
                    else 
                    if(companyname != para.CompanyName)
                    {
                        if (listcom.Count > 0)
                        {
                            r.Msg = "修改信息失败,该公司名称已存在！";
                            r.flg = "-1";
                        }
                        if (listcom.Count == 0)
                        {
                            string sqlcom = "update contractorder set CompanyName='"+para.CompanyName+ "' where CustomCompanyID='" +para.CompanyID+ "'";
                            conn.Execute(updateompany);
                            conn.Execute(updateuser);
                            conn.Execute(sqlcom);
                            r.Msg = "修改信息成功!";
                            r.flg = "1";
                        }
                    }                   
                }
                catch (Exception ex)
                {
                    r.Msg = "修改信息失败:"+ex;
                    r.flg = "-1";
                }
                return r;
            }
        }

       

        private static bool JudgeUser(string username)
        {
            bool f = false;
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = "select * from cf_user where LoginName=@LoginName";
                User user = new User();
                List<User> li = new List<User>();
                //foreach (User item in li)
                //{
                try
                {
                    li.Add(conn.Query<User>(sql2, new { LoginName = username }).SingleOrDefault());
                }
                catch (Exception)
                {

                    throw;
                }
                foreach (User item in li)
                {
                    if (item != null)
                    {
                        user.loginname = "用户名已经存在！";
                        user.MSg = "已存在";
                        user.flg = "0";
                        f = true;
                    }
                }
                //foreach (User item in li)
                //{


                //    if (item != null)
                //    {
                //        user.loginname = "用户名已经存在！";
                //        user.MSg = "已存在";
                //        user.flg = "0";

                //        f = true;
                //    }


                //}
                li.Clear();
            }

            return f;
        }

        /// <summary>
        /// 给用户分配移动NB卡基站定位接口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static Information InsertCompanyLocationInfo(cardlocations para)
        {
            Information info = new Information();
            DateTime time = DateTime.Now.AddYears(1);
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string addinfo = "insert into cardlocation(CustomerComapnyID,OrderType,StartTime,EendTime,UseNum,TotalNum,AddTime)" +
                        " values('"+para.CustomerComapnyID+"','1','"+DateTime.Now+"','"+time+"','0',"+para.TotalNum+",'"+DateTime.Now+"')";
                    conn.Execute(addinfo);
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
        ///用户查询基站定位API接口信息
        /// </summary>
        public static cardlocation GetCmccLocationInfo(string Company_ID,string CompanyName)
        {
            cardlocation info = new cardlocation();
            //string CompanyName = "测试账户0512";
            //string CompanyName = string.Empty;
            string sqlinfo = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (Company_ID == "1556265186243")
                    {
                        sqlinfo = "select t1.StartTime,t1.EendTime,t1.UseNum,t1.TotalNum,t1.CustomerComapnyID,t2.CompanyName,(t1.TotalNum-t1.UseNum) as SurplusNum from cardlocation t1 left join company t2 on t1.CustomerComapnyID=t2.CompanyID";
                        info.Cmcclocations = conn.Query<cmcclocation>(sqlinfo).ToList();
                        if (!string.IsNullOrWhiteSpace(CompanyName))
                        {
                            sqlinfo =sqlinfo+ " where t2.CompanyName like '%"+CompanyName+"%'";
                            info.Cmcclocations = conn.Query<cmcclocation>(sqlinfo).ToList();
                        }
                        if (info.Cmcclocations.Count > 0)
                        {
                            info.flg = "1";
                            info.Msg = "成功";
                        }
                        else
                        {
                            info.flg = "-1";
                            info.Msg = "暂无数据!";
                        }
                    }
                    else
                    {
                        sqlinfo = "select t1.StartTime,t1.EendTime,t1.UseNum,t1.TotalNum,t1.CustomerComapnyID,t2.CompanyName,(t1.TotalNum-t1.UseNum) as SurplusNum from cardlocation t1 left join company t2 on t1.CustomerComapnyID=t2.CompanyID where t1.CustomerComapnyID='" + Company_ID+"'";
                        info.Cmcclocations = conn.Query<cmcclocation>(sqlinfo).ToList();
                        info.flg = "1";
                        info.Msg = "成功";
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