using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Esim7.Action.Action_haringGetExcel;

namespace Esim7.Controllers
{           /// <summary>
/// 客户卡数据导入
/// </summary>
    public class Card_ForCustomController : ApiController
    {

             /// <summary>
             /// 客户数据导入
             /// </summary>
             /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Card_Forcustom()
        {
            return Json<object>(Action.Action_haringGetExcel.ImportExcel_forJson());
        }

        ///<summary>
        ///修改客户卡开户日期和续费日期
        /// </summary>
        [HttpPost]
        [Route("UpdateCustomerTime")]
        public Information UpdateCustomerTime(Root root)
        {
            Information info = new Information();
            info = Action.Action_haringGetExcel.UpdateCustomerTime(root);
            return info;
        }
        ///<summary>
        ///客户数据导入 针对开始时间结束时间多乱杂情况  目前支持移动卡
        ///</summary>
        public Information UpdateUserCardData(Model_Charing_GetExcel para)
        {
            Information info = new Information();
            info = Action.Action_haringGetExcel.UploadUserCardData(para);
            return info;
        }

    }
}
