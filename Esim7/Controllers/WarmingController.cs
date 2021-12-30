using Esim7.Models;
using System.Web.Http;

namespace Esim7.Controllers
{/// <summary>
///  预警和续费
/// </summary>
    public class WarmingController : ApiController
    {/// <summary>
     /// 获取全部的信息 公海和客户的无筛选条件    
     /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get_renew_card()
        {    
            return Json<object>(Action.Action_Waring.model_Warings());
        }
        /// <summary>
        /// 续费操作    { "Year":"1",  "Type":"1",  "CompanyID":"123","Cards":[{"ICCID":"898602B8261890502400"}]   }
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Renew_realDate1()
        {
            return Json<object>(Action.Action_Waring.ImportExcel2());
        }

        ///<summary>
        ///续费操作改版
        /// </summary>
        [HttpPost]
        [Route("Renew_realDate")]
        public OnlineRenewDto Renew_realDate(OnlineRenewPara para)
        {
            OnlineRenewDto dto = new OnlineRenewDto();
            dto = Action.Action_Waring.Renew_realDate(para);
            return dto; 

        }
    }
}
