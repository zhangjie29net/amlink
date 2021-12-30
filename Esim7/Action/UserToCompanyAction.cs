using Dapper;
using Esim7.ReturnMessage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Esim7.Action
{
    public class UserToCompanyAction
    {

        /// <summary>
        /// 将员工添加到公司
        /// </summary>
        /// <param name="user"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public  static List<Return_UserToCompany> Adduser(string user,string CompanyID)
        {
            List<Return_UserToCompany> li = new List<Return_UserToCompany>();

           
         
               

                    Return_UserToCompany r = new Return_UserToCompany();

                    using (IDbConnection
     conn = DapperService.MySqlConnection())
                    {
                        string sql = "UPDATE cf_user set Company_ID=@Company_ID where UserID=@UserID and status=1";
                        var result = conn.Execute(sql, new { UserID=user, Company_ID=CompanyID });
                        if (result == 1)
                        {
                            // company.CompanyName ="用户:"+ CompanyName+"  注销成功";
                            r.flg = "1";
                            r.MSg = "success";
                            r.UserID = "用户ID为："+ user;
                            li.Add(r);
                        }
                        else
                        {
                            r.flg = "0";
                            r.MSg = "该用户不存在或已经注销";

                        }
                       
                
            }




            return li;









        }




//        /// <summary>
//        /// 查看员工
//        /// </summary>
//        /// <param name="user"></param>
//        /// <param name="CompanyID"></param>
//        /// <returns></returns>
//        public static List<Return_UserToCompany> delete(string CompanyID) {









//            List<Return_UserToCompany> li = new List<Return_UserToCompany>();





//            Return_UserToCompany r = new Return_UserToCompany();

//            using (IDbConnection
//conn = DapperService.MySqlConnection())
//            {
//                string sql = "select* from";
//                var result = conn.Execute(sql, new { UserID = user, Company_ID = CompanyID });
//                if (result == 1)
//                {
//                    // company.CompanyName ="用户:"+ CompanyName+"  注销成功";
//                    r.flg = "1";
//                    r.MSg = "success";
//                    r.UserID = "用户ID为：" + user;
//                    li.Add(r);
//                }
//                else
//                {
//                    r.flg = "0";
//                    r.MSg = "该用户不存在或已经注销";

//                }


//            }


//          //  conn.Query<Company>(query).ToList();

//            return li;
















//        }














    }
}