using Esim7.Action;
using Esim7.Models;
using Esim7.ReturnMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{       /// <summary>
/// 权限管理模块
/// </summary>
    public class ActionController : ApiController
    {
        
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="Json">{ "ActionGroupName":"Test1","action":[{"action_ID":"123","action_Name":"999"},{"action_ID":"sdjfd","action_Name":"999wsfw"}]}</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAction(string Json) {
            ActionToActionGroup_Action ac = new ActionToActionGroup_Action();

          //  return Json<string>(ac.AddAction(Json));


            Return_UserAction u = new Return_UserAction();
            ActionToActionGroup_Action a = new ActionToActionGroup_Action();
            try
            {

                string s = ac.AddAction(Json);

              
                    u.AddUserActionMess =s;

                    u.flg = "1";
                    u.MSg = "Success";
               
                   
               


            }
            catch (Exception ex)
            {
                u.AddUserActionMess = "Error";
                u.flg = "2";
                u.MSg = "接口失败";


            }

            return Json<Return_UserAction>(u);



        }
        /// <summary>
        /// 将模块授权给用户
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="GroupactionId"></param>
        /// <returns></returns>
        [HttpPost]       
        public IHttpActionResult AddUserActions(string UserID, string GroupactionId) {


            Return_UserAction u = new Return_UserAction();
            ActionToActionGroup_Action a = new ActionToActionGroup_Action();
            try
            {
                u.AddUserActionMess = a.AddUserAction(UserID, GroupactionId);
                u.flg = "1";
                u.MSg = "模块分配成功";

            }
            catch (Exception ex)
            {
                u.AddUserActionMess = "Error";
                u.flg = "0";
                u.MSg = "失败";


            }

            return Json<Return_UserAction>(u);







        }
        /// <summary>
        /// 权限注销
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet]      
        public IHttpActionResult giveupAction(string UserID) {




            Return_UserAction u = new Return_UserAction();
            ActionToActionGroup_Action a = new ActionToActionGroup_Action();
            try
            {
                u.AddUserActionMess = a.giveupAction(UserID);
                u.flg = "1";
                u.MSg = "模块分配成功";

            }
            catch (Exception ex)
            {
                u.AddUserActionMess = "Error";
                u.flg = "0";
                u.MSg = "失败";


            }

            return Json<Return_UserAction>(u);












        }
        /// <summary>
        ///判断权限
        /// </summary>
        /// <param name="actionID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet]       
        public IHttpActionResult JudgeAction(string actionID, string UserID) {


            Return_UserAction u = new Return_UserAction();
            ActionToActionGroup_Action a = new ActionToActionGroup_Action();
            try
            {

                string s = a.JudgeAction(actionID, UserID);
               
                if (s=="1")
                {
                    u.AddUserActionMess = "该用户有此权限";

                    u.flg = "1";
                    u.MSg = "Success";
                }
                else
                {
                    u.AddUserActionMess = "该用户没有有此权限";

                    u.flg = "0";
                    u.MSg = "Success";
                }
               

            }
            catch (Exception ex)
            {
                u.AddUserActionMess = "Error";
                u.flg = "2";
                u.MSg = "接口失败";


            }

            return Json<Return_UserAction>(u);

        }
        /// <summary>
        /// 查看所有授权模块
        /// </summary>
        /// <returns></returns>
        [HttpGet]     
        public IHttpActionResult CheckGroupid() {
            Result_CheckGroupId L = new Result_CheckGroupId();
            Return_UserAction u = new Return_UserAction();
            ActionToActionGroup_Action a = new ActionToActionGroup_Action();
          
            try
            {

              L.lii = a.CheckGroupid();

                u.flg = "1";
                u.AddUserActionMess = "ok";
                u.MSg = "success";
                L.ReturnMess = u;
               
               
            }
            catch (Exception ex)
            {
                u.flg = "2";
                u.AddUserActionMess = "Error" + ex;
                u.MSg = "接口失败";
                L.ReturnMess = u;                
            }

            return Json<Result_CheckGroupId > (L);



        }
    }
}
