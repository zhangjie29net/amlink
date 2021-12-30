using Esim7.Action;
using Esim7.Models;
using Esim7.Models.UserUploadCardModel;
using Esim7.ReturnMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Esim7.Model_Stock.Model_Stock_Config;

namespace Esim7.Controllers
{
    ///<summary>
    ///用户添加API信息上传非奇迹卡
    /// </summary>
    public class UserUploadCardController : ApiController
    {
        UserUploadCardAction u = new UserUploadCardAction();
        ///<summary>
        ///用户查看API信息
        /// </summary>
        [HttpGet]
        [Route("GetUserAPI")]
        public UserApiDto GetUserAPI(string Company_ID)
        {
            UserApiDto dto = new UserApiDto();
            dto = u.GetUserAPI(Company_ID);
            return dto;
        }

        ///<summary>
        ///用户添加运营商平台API
        /// </summary>
        [HttpPost]
        [Route("UserAddAPI")]
        public Information UserAddAPI(Account para)
        {
            Information info = new Information();
            info = u.UserAddAPI(para);
            return info;
        }

        ///<summary>
        ///用户编辑API
        /// </summary>
        [HttpPost]
        [Route("UserEditAPI")]
        public Information UserEditAPI(Account para)
        {
            Information info = new Information();
            info = u.UserEditAPI(para);
            return info;
        }

        ///<summary>
        ///用户添加套餐
        /// </summary>
        [HttpPost]
        [Route("UserAddsetmeal")]
        public Information UserAddsetmeal(Package para)
        {
            Information info = new Information();
            info = u.UserAddsetmeal(para);
            return info;
        }
        ///<summary>
        ///用户查看套餐
        /// </summary>
        [HttpGet]
        [Route("GetUserSetmeal")]
        public GetUserSetmealDto GetUserSetmeal(string Company_ID, string CardSetmealType)
        {
            GetUserSetmealDto dto = new GetUserSetmealDto();
            dto = u.GetUserSetmeal(Company_ID, CardSetmealType);
            return dto;
        }


        ///<summary>
        ///用户上传卡数据到copy1或库存中或者奇迹上传卡数据到card表
        /// </summary>
        [HttpPost]
        [Route("UploadCard")]
        public Information UploadCard(UserUploadCardDto para)
        {
            Information info = new Information();
            info = u.UploadCard(para);
            return info;
        }

        ///<summary>
        ///用户添加反馈信息
        /// </summary>
        [HttpPost]
        [Route("Addfeedback")]
        public Information Addfeedback(feedbackInfo para)
        {
            Information info = new Information();
            info = u.Addfeedback(para);
            return info;
        }
    }
}
