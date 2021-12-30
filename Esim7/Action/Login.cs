using Dapper;
using Esim7.Models;
using Esim7.parameter.User;
using Esim7.UNity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Esim7.Action
{
    public class Login
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static User Judge(String loginname, string password)
        {
            
            List<User> li = new List<User>();
            List<User> li1 = new List<User>();
            User user = new User();
            string s = Unit.MD5_64(password);
            string Num = Unit.GetTimeStamp(DateTime.Now);
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = "select * from cf_user where LoginName=@LoginName and Loginpassword=@Loginpassword and status=1";
                string sql3 = "select LoginName from cf_user where LoginName=@LoginName and Loginpassword=@Loginpassword and status=-1 ";
                string sqlcfuser = "select LoginName as loginname from cf_user where LoginName='" + loginname+"'";
                string cfuser = conn.Query<User>(sqlcfuser).Select(t => t.loginname).FirstOrDefault();
                if (cfuser != null)
                {
                    loginname = cfuser;
                }
                else
                {
                    string sqlcompany = "select CompanyID as Company_ID from company where CompanyPhone='" + loginname + "'";
                    string companyid = conn.Query<User>(sqlcompany).Select(t => t.Company_ID).FirstOrDefault();
                    if (companyid != null)
                    {
                        string sqlname = "select LoginName as loginname from cf_user where Company_ID ='" + companyid + "'";
                        string name = conn.Query<User>(sqlname).Select(t => t.loginname).FirstOrDefault();
                        if (name != null)
                        {
                            loginname = name;
                        }
                    }
                }

                li = conn.Query<User>(sql2, new { LoginName = loginname, Loginpassword = s }).AsList();
                li1 = conn.Query<User>(sql3, new { LoginName = loginname, Loginpassword = s }).AsList();
                foreach (User item in conn.Query<User>(sql2, new { LoginName = loginname, Loginpassword = s }).AsList())
                {
                    if (li.Count == 1)
                    {
                        string companysql = "select * from company where CompanyID='" + item.Company_ID + "'";
                        user.CompanyAdress = conn.Query<User>(companysql).Select(t => t.CompanyAdress).FirstOrDefault();
                        user.CompanyName = conn.Query<User>(companysql).Select(t => t.CompanyName).FirstOrDefault();
                        user.CompanyPhone = conn.Query<User>(companysql).Select(t => t.CompanyPhone).FirstOrDefault();
                        user.userid = item.userid;
                        user.Company_ID = item.Company_ID;
                        user.loginname = loginname;
                        user.MSg = "success";
                        user.flg = "1";
                        user.status = 1;
                        user.fileName = item.fileName;
                        user.AccountBalance = item.AccountBalance;
                        if (item.Company_ID == "1556265186243")
                        {
                            user.permission = "1";
                        }
                        else
                        {
                            user.permission = "2";
                            //判断是否有用户登录记录数据
                            string userloginsql = "select * from userlogincount where CompanyID='"+item.Company_ID+"'";
                            var logincount = conn.Query<userlogincount>(userloginsql).FirstOrDefault();
                            if (logincount == null)
                            {
                                //创建记录数据
                                string adduserlogincount = "insert into userlogincount (CompanyID,CompanyName,WeekCount,MonthCount,TotalCount,StartTime,WeekCountTime,MonthCountTime)" +
                                " values ('" + user.Company_ID + "','" + user.CompanyName + "','1','1','1','" + DateTime.Now + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                                conn.Execute(adduserlogincount);

                            }
                            else//已经有记录则修改原记录数据
                            {
                                //获取数据 
                                string updatelogincount = string.Empty;
                                int WeekCount = logincount.WeekCount + 1;
                                int MonthCount = logincount.MonthCount + 1;
                                int TotalCount = logincount.TotalCount + 1;
                                DateTime weektime = logincount.WeekCountTime;
                                DateTime monthtime = logincount.MonthCountTime;
                                DateTime dt = DateTime.Now;
                                DateTime nowMon = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));
                                TimeSpan ts = weektime - nowMon;
                                if (ts.Days >= 0 && ts.Days < 7)//本周
                                {
                                    updatelogincount = "update userlogincount set WeekCount=" + WeekCount + " where CompanyID='" + user.Company_ID + "'";
                                    conn.Execute(updatelogincount);
                                }
                                else//非本周
                                {
                                    WeekCount = 1;
                                    updatelogincount = "update userlogincount set WeekCount=" + WeekCount + " ,WeekCountTime='" + DateTime.Now + "' where CompanyID='" + user.Company_ID + "'";
                                    conn.Execute(updatelogincount);
                                }
                                DateTime nowMonth = dt.AddDays(1 - dt.Day);
                                DateTime endMonth = nowMonth.AddMonths(1).AddDays(-1);
                                TimeSpan ts1 = monthtime - nowMonth;
                                TimeSpan ts2 = endMonth - monthtime;
                                if(ts1.Days >= 0 && ts2.Days >= 0)//本月
                                {
                                    updatelogincount = "update userlogincount set MonthCount=" + MonthCount + " where CompanyID='" + user.Company_ID + "'";
                                    conn.Execute(updatelogincount);
                                }
                                else//非本月
                                {
                                    MonthCount = 1;
                                    updatelogincount = "update userlogincount set MonthCount=" + MonthCount + " ,MonthCountTime='" + DateTime.Now + "' where CompanyID='" + user.Company_ID + "'";
                                    conn.Execute(updatelogincount);
                                }
                                updatelogincount = "update userlogincount set TotalCount=" + TotalCount + "  where CompanyID='" + user.Company_ID + "'";
                                conn.Execute(updatelogincount);
                            }
                        }
                        li.Clear();
                    }
                }
                if (li1.Count == 1)
                {
                    user.MSg = "该用户已禁用!";
                    user.flg = "0";
                    user.status = -1;
                    user.permission = "-1";
                }
                return user;
            }
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static User Inner(string username, string password)
        {
            string s = Unit.MD5_64(password);

            string Num = Unit.GetTimeStamp(DateTime.Now);

            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = "select * from cf_user where LoginName=@LoginName";
                User user = new User();
                List<User> li = new List<User>();
                li.Add(conn.Query<User>(sql2, new { LoginName = username }).SingleOrDefault());
                foreach (User item in li)
                {
                    if (item != null)
                    {
                        user.loginname = "用户名已经存在！";
                        user.MSg = "已存在";
                        user.flg = "0";
                        li.Clear();
                        return user;
                    }
                }
                {
                    string sql = "INSERT INTO cf_user(UserID, LoginName, Loginpassword,status ) values(@UserID,@LoginName, @Loginpassword,@status)";
                    var result = conn.Execute(sql,
                                     new
                                     {
                                         UserID = Num,
                                         LoginName = username,
                                         Loginpassword = s,
                                         status = 1
                                     });
                    user.loginname = username;
                    user.userid = Num;
                    user.loginpwd = password;
                    user.status = 1;
                    user.MSg = "恭喜注册成功！";
                    user.flg = "1";
                    li.Clear();
                    return user;
                }
                //string sql = "select min(IMEI+0) as IMEI ,SUBSTRING(SN ,4) as SN from am900 where  oldimsi=''and EID='' and status=" + this.txt_status.Text.Trim();
                //    conn.Query<User>(sql).AsList()
                //    ;
                // }
            }
        }

        ///<summary>
        ///新用户注册
        /// </summary>
        public UserRegisterDto UserRegister(UserRegisterPara para)
        {
            UserRegisterDto info = new UserRegisterDto();
            try
            {
                string password = Unit.MD5_64("123456");
                string Company_ID = Unit.GetTimeStamp(DateTime.Now);
                string User_Menu = "1,2,3,4,6,7,8,9,11,18,19,20,21,22,23,24,30,31,32,46,50,51,52,55,34,36,37,38";//给注册用户一个默认的菜单
                string loginnamesql = "select LoginName as loginname from cf_user where LoginName='" + para.CompanyPhone + "'";
                string Userid = Unit.GetTimeStamp(DateTime.Now);
                string code = string.Empty;
                string adduser = "insert into cf_user(UserID,LoginName,Loginpassword,status,Company_ID,User_Menu,User_Pid,IsCostomer,Email,UserType,Job,UserName) " +
                    "values('" + Userid + "','" + para.CompanyPhone + "','" + password + "'," + 1 + ",'" + Company_ID + "','" + User_Menu + "'," + 27 + "," + false + ",'" + para.Email + "','" + 2 + "','"+para.Job+"','"+para.Username+"')";
                string addcompany = "insert into company(CompanyID,CompanyName,CompanyOpeningDate,status,CompanyPhone)" +
                    " values('"+Company_ID+"','"+para.CompanyName+"','"+DateTime.Now+"',"+1+",'"+para.CompanyPhone+"')";
                string shortcodesql = "select code from shortcode where phone='"+para.CompanyPhone+"'";
                string delshortcode = "delete from shortcode where phone='" + para.CompanyPhone + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //code = conn.Query<shortcode>(shortcodesql).Select(t => t.code).FirstOrDefault();
                    var codes = conn.Query<shortcode>(shortcodesql).Select(t => t.code).ToList();
                    var username = conn.Query<User>(loginnamesql).Select(t => t.loginname).FirstOrDefault();
                    if (username != null)
                    {
                        info.flg = "-1";
                        info.Msg = "该手机号已经注册过!";
                        return info;
                    }
                    //if (para.VerificationCode != code)
                    //{
                    //    info.flg = "-1";
                    //    info.Msg = "验证码错误!";
                    //    return info;
                    //}
                    if (!codes.Contains(para.VerificationCode))
                    {
                        info.flg = "-1";
                        info.Msg = "验证码错误!";
                        return info;
                    }
                    else
                    {
                        conn.Execute(adduser);
                        conn.Execute(addcompany);
                        conn.Execute(delshortcode);
                        info.flg = "1";
                        info.Msg = "添加成功!";
                        info.LoginName = para.CompanyPhone;
                    }
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
        ///下发短信
        /// </summary>
        public PhoneShortMessage shormessage(string phonenum)
        {
            PhoneShortMessage t = new PhoneShortMessage();
            string VerificationCode = "";
            Random rd = new Random();
            int num = rd.Next(100000, 999999);
            string Code = num.ToString();
            Information info = new Information();
            string account = "dh25452";
            string phones = phonenum;
            //string password = Unit.GetMD5_32("KbddA3Wx");
            string password = "198d76557f32f713f0ef825e0512499d";
            string content = "您好，您的手机验证码为:"+Code+"。";
            string sign = "【eSIM物联工场】";
            string URL = @"http://www.dh3t.com/json/sms/Submit";
            string parastr = "account=" + account + "&password" + password + "&phones" + phones + "&sign" + sign + "&content" + content;
          
            string data = $"account={account}&password={password}&phones={phones}&sign={sign}&content={content}";
            StringBuilder str = new StringBuilder();
                        str.Append("{");
                        str.Append("account:\"" + account + "\",");
                        str.Append("password:\"" + password + "\",");
                        str.Append("phones:\"" + phones + "\",");
                        str.Append("content:\"" + content + "\",");
                        str.Append("sign:\"" + sign + "\"");
                        str.Append("}");
                        string json = str.ToString();
                        byte[] bytes = Encoding.UTF8.GetBytes(json);            
                         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                         request.Method = "POST";
                         request.ContentType = "application/json";
                        request.ContentLength = bytes.Length;
                         Stream requestStream = request.GetRequestStream();
                         requestStream.Write(bytes, 0, bytes.Length);
                         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                         Stream responseStream = response.GetResponseStream();
                         if (responseStream != null)
                         {
                                 using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                                 {
                                         //result = reader.ReadToEnd();
                                         t = JsonConvert.DeserializeObject<PhoneShortMessage>(reader.ReadToEnd());
                                        if (t.result == "0")
                                        {
                                            VerificationCode = Code;
                                            using (IDbConnection conn = DapperService.MySqlConnection())
                                            {
                                                string addshortcode = "insert into shortcode(phone,code) values('" + phonenum + "','" + Code + "')";
                                                conn.Execute(addshortcode);
                                            }
                                            t.code = Code;
                                        }
                                         reader.Close();
                                 }
                         }
            return t;
        }


        ///<summary>
        ///下发短信测试新内容
        /// </summary>
        public PhoneShortMessage shormessagecontent(string phonenum)
        {
            PhoneShortMessage t = new PhoneShortMessage();
            string VerificationCode = "";
            Random rd = new Random();
            int num = rd.Next(100000, 999999);
            string Code = num.ToString();
            Information info = new Information();
            string account = "dh25452";
            string phones = phonenum;
            //string password = Unit.GetMD5_32("KbddA3Wx");
            string password = "198d76557f32f713f0ef825e0512499d";
            string content = "您好,您的物联网卡流量已经达到阈值请及时处理";
            string sign = "【eSIM物联工场】";
            string URL = @"http://www.dh3t.com/json/sms/Submit";
            string parastr = "account=" + account + "&password" + password + "&phones" + phones + "&sign" + sign + "&content" + content;

            string data = $"account={account}&password={password}&phones={phones}&sign={sign}&content={content}";
            StringBuilder str = new StringBuilder();
            str.Append("{");
            str.Append("account:\"" + account + "\",");
            str.Append("password:\"" + password + "\",");
            str.Append("phones:\"" + phones + "\",");
            str.Append("content:\"" + content + "\",");
            str.Append("sign:\"" + sign + "\"");
            str.Append("}");
            string json = str.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    //result = reader.ReadToEnd();
                    t = JsonConvert.DeserializeObject<PhoneShortMessage>(reader.ReadToEnd());
                    if (t.result == "0")
                    {
                        VerificationCode = Code;
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            string addshortcode = "insert into shortcode(phone,code) values('" + phonenum + "','" + Code + "')";
                            conn.Execute(addshortcode);
                        }
                        t.code = Code;
                    }
                    reader.Close();
                }
            }
            return t;
        }


     




        ///<summary></summary>

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static User update(User u)
        {
            string password = u.loginpwd;
            string username = u.loginname;
            string s = Unit.MD5_64(password);
            string Num = Unit.GetTimeStamp(DateTime.Now);
            using (IDbConnection
            conn = DapperService.MySqlConnection())
            {
                string sql2 = "select * from cf_user where LoginName=@LoginName  and status=1";
                User user = new User();
                List<User> li = new List<User>();
                //foreach (User item in li)
                //{
                li = conn.Query<User>(sql2, new { LoginName = username, Loginpassword = s }).AsList();
                //foreach (User item in li)
                {
                    if (li.Count == 1)
                    {
                        string UserID = "";
                        foreach (User item in li)
                        {
                            UserID = item.userid;
                        }
                        user.loginname = u.loginname;
                        string sql = "update cf_user set loginpassword=@password where userid= " + UserID;
                        var result = conn.Execute(sql, new { password = s });
                        user.MSg = "success";
                        user.status = 1;
                        user.flg = result.ToString();
                        li.Clear(); 
                    }
                    else
                    {
                        user.MSg = "该用户未注册或密码错误请重试！";
                        user.status = 100;
                    }
                }
                return user;
            }
        }

        /// <summary>
        /// 查询 User 没有注销的用户
        /// </summary>
        /// <returns></returns>
        public static List<User> Getusers()
        {
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = "select UserID,LoginName from cf_user where status=1";
                User user = new User();
                List<User> li = new List<User>();
                //foreach (User item in li)
                //{
                li = conn.Query<User>(sql2).AsList();
                return li;
            }
        }

        ///<summary>
        ///修改个人信息
        /// </summary>
        public Information UpdateInfo(UserUpdateInfo para)
        {
            Information info = new Information();
            #region  本地使用
            try
            {
                string filenamesql = "select fileName from cf_user where Company_ID='" + para.Company_ID + "'";
                string sqlupdateinfo = "update company set CompanyPhone='" + para.CompanyPhone + "',CompanyAdress='" + para.CompanyAdress + "',CompanyName='" + para.CompanyName + "' where CompanyID='" + para.Company_ID + "'";
                string sqlupdateuser = "update cf_user set LoginName='" + para.loginname + "',fileName='" + para.fileName + "' where Company_ID='" + para.Company_ID + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sqlupdateinfo);
                    conn.Execute(sqlupdateuser);
                    info.flg = "1";
                    info.Msg = "修改成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "修改失败:" + ex;
            }
            #endregion
            #region  发布使用
            //try
            //{
            //    string fileName = string.Empty;
            //    string dir = @"C:\IIS\UploadUserLogoFiles\";
            //    string filenamesql = "select fileName from cf_user where Company_ID='" + para.Company_ID + "'";
            //    string sqlupdateinfo = "update company set CompanyPhone='" + para.CompanyPhone + "',CompanyAdress='" + para.CompanyAdress + "',CompanyName='" + para.CompanyName + "' where CompanyID='" + para.Company_ID + "'";
            //    string sqlupdateuser = "update cf_user set LoginName='" + para.loginname + "',fileName='" + para.fileName + "' where Company_ID='" + para.Company_ID + "'";
            //    using (IDbConnection conn = DapperService.MySqlConnection())
            //    {
            //        conn.Execute(sqlupdateinfo);
            //        conn.Execute(sqlupdateuser);
            //        fileName = conn.Query<User>(filenamesql).Select(t => t.fileName).FirstOrDefault();
            //        if (fileName != "default.png")
            //        {
            //            File.Delete(dir + fileName);//删除之前的logo文件 保留默认文件
            //        }                    
            //        info.flg = "1";
            //        info.Msg = "修改成功!";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    info.flg = "-1";
            //    info.Msg = "修改失败:" + ex;
            //}
            #endregion
            return info;
        }

        ///<summary>
        ///重置密码
        /// </summary>
        public Information ResetPassword(User para)
        {
            Information info = new Information();
            string password = Unit.MD5_64("123456");
            try
            {
                //string sql = "select from cf_user where UserID='"+para.userid+ "' and Company_ID='"+para.Company_ID+ "' and LoginName='"+para.loginname+"'";
                string sql = "select t2.Company_ID,t1.CompanyName, t2.UserID,t2.LoginName from company t1 left join cf_user t2 on t1.CompanyID=t2.Company_ID where t1.CompanyName='" + para.CompanyName+"' and t2.LoginName='"+para.loginname+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var listinfo = conn.Query<User>(sql).FirstOrDefault();
                    if (listinfo != null)
                    {
                        string sqlupdate = "update cf_user set Loginpassword='"+password+ "' where  UserID='"+listinfo.userid+ "' and  Company_ID='"+listinfo.Company_ID+"'";
                        conn.Execute(sqlupdate);
                        info.flg = "1";
                        info.Msg = "成功!";

                    }
                    else
                    {
                        info.flg = "100";
                        info.Msg = "用户不存在或者输入错误!";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return info;
        }

        #region simlink平台企业认证
        ///<summary>
        ///企业认证   上传认证信息文件
        /// </summary>
        public Information UploadAuthenticationInfo(UploadAuthenticationPara para)
        {
            Information info = new Information();
            try
            {
                //生成企业验证码
                
                string AuthenticationCode = Unit.GetTimeStamp(DateTime.Now);
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string updateuser = "update cf_user set AuthenticationCode='"+AuthenticationCode+ "' where Company_ID='"+para.Company_ID+"'";
                    string addinfo = "insert into authentication(AuthenticationCode,Company_ID,CompanyName,LicenseCode,LicenseName,IDCode,IDpositiveName,IDbackName,AddTime) " +
                        "values('" + AuthenticationCode + "','" + para.Company_ID + "','"+para.CompanyName+"','"+para.LicenseCode+"','"+para.LicenseName+"','"+para.IDCode+"','"+para.IDpositiveName+"','"+para.IDbackName+"','"+DateTime.Now+"')";
                    conn.Execute(addinfo);
                    conn.Execute(updateuser);
                    info.flg = "1";
                    info.Msg = "提交成功以转至客服审核";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败"+ex;
            }
            return info;
        }


        ///<summary>
        ///查看企业认证信息
        /// </summary>
        public AuthenticationDtos GetAuthenticationInfo(string Company_ID)
        {
            //仅提交用户和平台管理员可见
            AuthenticationDtos dtos = new AuthenticationDtos();
            List<AuthenticationInfo> listinfo = new List<AuthenticationInfo>();
            string sql = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (Company_ID == "1556265186243")//平台管理员
                    {
                        sql = "select * from authentication";
                        var lists = conn.Query<AuthenticationInfo>(sql).ToList();
                        foreach (var item in lists)
                        {
                            AuthenticationInfo dto = new AuthenticationInfo();
                            dto.AddTime = item.AddTime;
                            dto.AuthenticationCode = item.AuthenticationCode;
                            dto.CompanyName = item.CompanyName;
                            dto.Company_ID = item.Company_ID;
                            dto.IDbackName = "http://101.200.35.208:9090/UploadAuthenticationFiles/IDbackFiles/" + item.IDbackName;
                            dto.IDCode = item.IDCode;
                            dto.IDpositiveName = "http://101.200.35.208:9090/UploadAuthenticationFiles/IDpositiveFiles/" + item.IDpositiveName;
                            dto.LicenseCode = item.LicenseCode;
                            dto.LicenseName = "http://101.200.35.208:9090/UploadAuthenticationFiles/LicenseFiles/" + item.LicenseName;
                            dto.Status = item.Status;
                            listinfo.Add(dto);
                        }                       
                        dtos.dtos = listinfo;
                        dtos.flg = "1";
                        dtos.Msg = "成功!";
                    }
                    else
                    {
                        sql = "select * from authentication where Company_ID='"+ Company_ID + "'";
                        var lists = conn.Query<AuthenticationInfo>(sql).ToList();
                        foreach (var item in lists)
                        {
                            AuthenticationInfo dto = new AuthenticationInfo();
                            dto.AddTime = item.AddTime;
                            dto.AuthenticationCode = item.AuthenticationCode;
                            dto.CompanyName = item.CompanyName;
                            dto.Company_ID = item.Company_ID;
                            dto.IDbackName = "http://101.200.35.208:9090/UploadAuthenticationFiles/IDbackFiles" + item.IDbackName;
                            dto.IDCode = item.IDCode;
                            dto.IDpositiveName = "http://101.200.35.208:9090/UploadAuthenticationFiles/IDpositiveFiles" + item.IDpositiveName;
                            dto.LicenseCode = item.LicenseCode;
                            dto.LicenseName = "http://101.200.35.208:9090/UploadAuthenticationFiles/LicenseFiles" + item.LicenseName;
                            dto.Status = item.Status;
                            listinfo.Add(dto);
                        }
                        dtos.dtos = listinfo;
                        dtos.flg = "1";
                        dtos.Msg = "成功!";
                    }
                }                   
            }
            catch (Exception ex)
            {
                dtos.dtos = null;
                dtos.flg = "-1";
                dtos.Msg = "失败:"+ex;
            }
            return dtos;
        }
        #endregion
    }
}