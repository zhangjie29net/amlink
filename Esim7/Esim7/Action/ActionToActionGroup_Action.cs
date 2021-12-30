using Dapper;
using Esim7.Models;
using Esim7.UNity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Esim7.ReturnMessage;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Action
{
    public class ActionToActionGroup_Action
    {
        /// <summary>
        /// 添加模块 
        /// </summary>
        /// <param name="Json">  </param>
        /// <returns></returns>
        public    string AddAction(string Json)
        {
            string s = "";
            actiongroup com = JsonConvert.DeserializeObject<actiongroup>(Json);
            List<action> ListACtion = com.action;

            string actiongroupid = Unit.GetTimeStamp(DateTime.Now);
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    string sql = "insert action(action_ID,action_Name,actiongroupid)values(@action_ID,@action_Name," + actiongroupid + ")";
                    conn.Execute(sql, ListACtion);
                    s = "添加Action成功";
                }
                catch (Exception ex)
                {
                    s = ex.ToString();
                }
            }
            using (IDbConnection conn = DapperService.MySqlConnection())
            {

                try
                {
                    string sql = "insert actiongroup(actiongroupid,ActionGroupName,status)values('" + actiongroupid + "','" + com.ActionGroupName + "',1)";
                    conn.Execute(sql);

                    s += " 添加模块成功";

                }
                catch (Exception ex)
                {

                    s = ex.ToString();
                }





            }
            return s;
        }
        /// <summary>
        /// 将权限分配给用户
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="GroupactionId"></param>
        /// <returns></returns>
        public  string  AddUserAction(string UserID, string GroupactionId)
        {



            string s = "";

            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    string sql = "UPDATE cf_user set actiongroupid='" + GroupactionId + "' where UserID='" + UserID + "'";
                    conn.Execute(sql);
                    s = "1";
                }
                catch (Exception ex)
                {
                  
                    s = ex.ToString();
                }
            }

            return s;


        }
        /// <summary>
        /// 权限注销
        /// </summary>
        /// <param name="UserID"></param>
        public string  giveupAction(string UserID)
        {
            string s = ""
   ;            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    string sql = "UPDATE cf_user set actiongroupid='' where UserID='" + UserID + "'";
                    conn.Execute(sql);
                    s = "1";
                }
                catch (Exception ex)
                {
                    s = ex.ToString();

                }
            }
            return s;

        }        
        /// <summary>
        /// 判断权限
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public  string JudgeAction(string actionID, string UserID)
        {
            string s = "";
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    string sql = "select 1 from cf_user t1 left join actiongroup  t2 on t2.actiongroupid=t1.actiongroupid left join  action t3 on t2.actiongroupid=t3.actiongroupid where t3.action_ID='" + actionID + "'and t1.UserID='" + UserID + "'";
                    List<string> li
                        = new List<string>();

                    li = conn.Query<string>(sql).ToList();


                    foreach (string item in li)
                    {
                        if (item == "1")
                        {
                            s = "1";
                        }
                        else
                        {
                            s = "0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    s = ex.ToString();
                }
            }

            return s;















        }
      
        /// <summary>
        /// 查看所有 授权信息
        /// </summary>
        /// <returns></returns>
        public List<CheckActionGroup> CheckGroupid() {

            List<CheckActionGroup> li
                       = new List<CheckActionGroup>();

            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                try
                {
                    string sql = "select * from actiongroup where status =1";
                   

                    li = conn.Query<CheckActionGroup>(sql).ToList();


                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



            return li;












        }









    }
    
}
