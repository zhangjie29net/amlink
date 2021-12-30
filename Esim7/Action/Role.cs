using Dapper;
using Esim7.Models;
using Esim7.UNity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Esim7.Action
{
    public class Role
    {



     //   public static List<role> AddRole( role role  ) {


     //       string RoleID= Unit.GetTimeStamp(DateTime.Now);

     //       string RoleName = role.rolename;
     //       string createUserID = role.createUserId;
     //       string PermissionsID = role.permissionsid;

     //       using (IDbConnection
     //conn = DapperService.MySqlConnection())

     //       {





     //           string sql2 = "select * from cf_role where CompanyName=@CompanyName";


     //           Company user = new Company();




     //           List<Company> li = new List<Company>();
     //           //foreach (User item in li)
     //           //{
     //           li.Add(conn.Query<Company>(sql2, new { CompanyName = companyname }).SingleOrDefault());


     //           foreach (Company item in li)
     //           {


     //               if (item != null)
     //               {
     //                   user.CompanyName = "公司已经存在！";
     //                   user.CompanyID = "error";
     //                   user.CompanyOpeningDate = "error";
     //                   li.Clear();
     //                   li.Add(user);
     //                   return li;
     //               }

     //           }


     //           {
     //               string sql = "INSERT INTO company (CompanyID,Companyname,CompanyOpeningDate,Companyremarks,status) VALUES(@companyID,@CompanyName,@CompanyOpeningDate,@CompanyRemarks,@status)";
     //               try
     //               {
     //                   var result = conn.Execute(sql,
     //                               new
     //                               {
     //                                   companyID = CompanyID,
     //                                   CompanyName = com.CompanyName,
     //                                   CompanyOpeningDate = DateTime.Now,
     //                                   CompanyRemarks = com.Companyremarks,

     //                                   status = 1
     //                               });
     //                   f = true;
     //               }
     //               catch (Exception ex)
     //               {

     //                   throw ex;
     //               }



     //               //com.flg = "1";
     //               //com.MSg = "添加成功";
     //               com.CompanyID = CompanyID;

     //               lii.Add(com);

     //           }



     //       }



     //   }




        /// <summary>
        /// 添加公司 
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static List<Company> Add(Company com)
        {


            string CompanyID = Unit.GetTimeStamp(DateTime.Now);
            string companyname = com.CompanyName;

            List<Company> lii = new List<Company>();

            string remarks = com.Companyremarks;

            bool f = false;
            using (IDbConnection
           conn = DapperService.MySqlConnection())

            {





                string sql2 = "select * from company where CompanyName=@CompanyName";


                Company user = new Company();




                List<Company> li = new List<Company>();
                //foreach (User item in li)
                //{
                li.Add(conn.Query<Company>(sql2, new { CompanyName = companyname }).SingleOrDefault());


                foreach (Company item in li)
                {


                    if (item != null)
                    {
                        user.CompanyName = "公司已经存在！";
                        user.CompanyID = "error";
                      
                        li.Clear();
                        li.Add(user);
                        return li;
                    }

                }


                {
                    string sql = "INSERT INTO company (CompanyID,Companyname,CompanyOpeningDate,Companyremarks,status) VALUES(@companyID,@CompanyName,@CompanyOpeningDate,@CompanyRemarks,@status)";
                    try
                    {
                        var result = conn.Execute(sql,
                                    new
                                    {
                                        companyID = CompanyID,
                                        CompanyName = com.CompanyName,
                                        CompanyOpeningDate = DateTime.Now,
                                        CompanyRemarks = com.Companyremarks,

                                        status = 1
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
                using (IDbConnection
      conn = DapperService.MySqlConnection())

                {



                    string UserID = Unit.GetTimeStamp(DateTime.Now);




                    {
                        string sql = "INSERT INTO cf_user (UserID,LoginName,Loginpassword,status,Company_ID) VALUES(@UserID,@LoginName,@Loginpassword,@status,@Company_ID)";
                        try
                        {
                            var result = conn.Execute(sql,
                                        new
                                        {
                                            UserID = UserID,
                                            LoginName = com.CompanyName,
                                            Loginpassword = Unit.MD5_64("123456"),
                                            Company_ID = CompanyID,

                                            status = 1
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










    }
}