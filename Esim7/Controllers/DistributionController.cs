using Esim7.Action;
using Esim7.Dto;
using Esim7.Models;
using Esim7.parameter.DistributionManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{
    /// <summary>
    /// 分润模块
    /// </summary>
    public class DistributionController : ApiController
    {
        DistributionAction r = new DistributionAction();
        ///<summary>
        ///设置分润提成比例
        /// </summary>
        [HttpPost]
        [Route("AddDistribution")]
        public Information AddDistribution(distributionratio para)
        {
            Information info = new Information();
            info = r.AddDistribution(para);
            return info;
        }

        /// <summary>
        /// 获取子级用户
        /// </summary>
        /// <param name="Company_ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetChildrenCompanyInfo")]
        public GetChildrenCompanyDto GetChildrenCompanyInfo(string Company_ID)
        {
            GetChildrenCompanyDto dto = new GetChildrenCompanyDto();
            dto = r.GetChildrenCompanyInfo(Company_ID);
            return dto;
        }

        ///<summary>
        ///编辑子级用户的提成比例
        ///</summary>
        [HttpPost]
        [Route("UpdateChildrenDistribution")]
        public Information UpdateChildrenDistribution(distributionratio para)
        {
            Information info = new Information();
            info = r.UpdateChildrenDistribution(para);
            return info;
        }

        ///<summary>
        ///查看子级用户提成比例 获取组合信息卡的价格
        /// </summary>
        [HttpPost]
        [Route("GetChildrenDistributionInfo")]
        public GetChildrenDistributionDto GetChildrenDistributionInfo(DistributionPara para)
        {
            GetChildrenDistributionDto dto = new GetChildrenDistributionDto();
            dto = r.GetChildrenDistributionInfo(para);
            return dto;

        }

        ///<summary>
        ///子级用户创建订单
        /// </summary>
        [HttpPost]
        [Route("AddOrder")]
        public GetOrder AddOrder(saleorder para)
        {
            GetOrder info = new GetOrder();
            info = r.AddOrder(para);
            return info;
        }

        ///<summary>
        ///返回子级可选择的套餐卡类型运营商信息
        /// </summary>
        [HttpGet]
        [Route("GetAvailableSetmealDto")]
        public AvailableSetmeal GetAvailableSetmealDto(string Company_ID)
        {
            AvailableSetmeal info = new AvailableSetmeal();
            info = r.GetAvailableSetmealDto(Company_ID);
            return info;
        }

        ///<summary>
        ///返回套餐信息
        /// </summary>
        [HttpGet]
        [Route("GetSetmealInfo")]
        public SetMealInfoDto GetSetmealInfo(string Company_ID)
        {
            SetMealInfoDto dto = new SetMealInfoDto();
            dto = r.GetSetmealInfo(Company_ID);
            return dto;
        }

        /// <summary>
        /// 获取省市县
        /// </summary>
        [HttpGet]
        [Route("GetCityInfo")]
        public CityDto GetCityInfo()
        {
            CityDto dto = new CityDto();
            dto = r.GetCityInfo();
            return dto;
        }

        ///<summary>
        ///查看个人信息  余额和订单信息
        /// </summary>
        [HttpPost]
        [Route("GetPersonalInfo")]
        public PersonalInfoDto GetPersonalInfo(GetPersonalInfoPara para)
        {
            PersonalInfoDto dto = new PersonalInfoDto();
            dto = r.GetPersonalInfo(para);
            return dto;
        }

        ///<summary>
        ///提现
        /// </summary>
        [HttpPost]
        [Route("Withdrawal")]
        public Information Withdrawal(WithdrawalPara para)
        {
            Information info = new Information();
            info = r.Withdrawal(para);
            return info;
        }
    }
}
