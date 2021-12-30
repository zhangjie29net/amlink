using Esim7.Action;
using Esim7.ReturnMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{
    public class ActionGroupController : ApiController
    {

        /// <summary>
        /// 添加授权模板
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(string Json)
        {
            ResultCurrency r = new ResultCurrency();
            try
            {
                ActionToActionGroup_Action ac = new ActionToActionGroup_Action();
                r.MSg = ac.AddAction(Json);
                r.flg = "1";
            }
            catch (Exception ex)
            {
                r.MSg = "接口失败";
                r.flg = "0";
            }
            return Json<ResultCurrency>(r);
        }

        //[HttpPost]
        //[Route("Judge")]
        //public IHttpActionResult Judge() {





        //}



    }
}
