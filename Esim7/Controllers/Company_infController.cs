using Esim7.Action;
using Esim7.Models;
using Esim7.ReturnMessage;
using Esim7.UNity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{[RoutePrefix("Company")]
    public class Company_infController : ApiController
    {
        [HttpGet]
        [Route("QueryCompany")]
        public IHttpActionResult SelectAll(string CompanyID)
        {
            Return_Company c = new Return_Company();
            try
            {
                c.conpany = CompanyAction.Query(CompanyID);
                c.flg = "1";
                c.MSg = "Success";
            }
            catch (Exception)
            {
                c.flg = "0";
                c.MSg = "error";
            }
            //return  Json(   CompanyAction.Query());
            return Json<Return_Company>(c);
        }
        /// <summary>
        /// {"CompanyName":"csfa","Companyremarks":"fwe","CompanyPhone":"18831313675","CompanyAdress":"wegergre","Username":"aqfe"}
        /// CompanyName: 公司名称，Companyremarks：公司备注， "CompanyPhone 公司电话，CompanyAdress公司地址，Username 用户名称
        /// </summary>
        /// <param name="Json">{"CompanyName":"csfa","Companyremarks":"fwe","CompanyPhone":"18831313675","CompanyAdress":"wegergre","Username":"aqfe"}</param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("AddCompany")]
        //public IHttpActionResult Compamy_add(string Json)
        //{
        //    Return_Company c = new Return_Company();
        //    Company company = new Company();
        //    Company com = JsonConvert.DeserializeObject<Company>(Json);
        //    try
        //    {
        //        c.conpany = CompanyAction.Add(com);
        //        foreach (Company item in c.conpany)
        //        {
        //            if (item.CompanyID != null)
        //            {
        //                c.flg = "1";
        //                c.MSg = "添加成功 账户名称为：" + com.Username + "初始密码为123456";
        //            }
        //            else
        //            {
        //                c.flg = "0";
        //                c.MSg = "添加失败";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        c.flg = "0";
        //        c.MSg = "error" + ex;
        //    }
        //    return Json<Return_Company>(c);
        //}

        /// <summary>
        /// 添加公司2 Get方式
        /// </summary>
        /// <param name="CompanyName">公司名称</param>
        /// <param name="Companyremarks">备注</param>
        /// <param name="CompanyPhone">联系方式</param>
        /// <param name="CompanyAdress">地址</param>
        /// <param name="Username">登录名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("AddCompany2")]
        public IHttpActionResult company_add2(Company para) {
            Return_Company c = new Return_Company();
            Company company = new Company();
            //Company com = new Company();
            //com.CompanyAdress = CompanyAdress;
            //com.CompanyName = CompanyName;
            //com.Companyremarks = Companyremarks;
            //com.CompanyPhone = CompanyPhone;
            //com.Username = Username;
            //com.Password = Password;
            //com.Company_ID = Company_ID;
            try
            {
                c.conpany = CompanyAction.Add(para);
                foreach (Company item in c.conpany)
                {
                    if (item.CompanyID != null)
                    {
                        c.flg = "1";
                        c.MSg = "添加成功 账户名称为：" + para.Username ;
                    }
                    else
                    {
                        c.flg = "0";
                        c.MSg = "添加失败:"+item.CompanyName;
                    }
                }
            }
            catch (Exception ex)
            {
                c.flg = "0";
                c.MSg = "error" + ex;
            }
            return Json<Return_Company>(c);
        }

        /// <summary>
        /// 注销公司
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteCompany")]
        public Information Delete(string CompanyID)
        {
            Information info = new Information();
            info=CompanyAction.Delete(CompanyID);
            return info;
        }

        //[HttpPost]
        //[Route("UpdateCompany")]
        //public IHttpActionResult Update(dynamic dynamic)
        //{
        //    Company company = new Company();
        //    company.CompanyAdress = dynamic["CompanyAdress"];
        //    company.CompanyName = dynamic["CompanyName"];
        //    company.CompanyPhone = dynamic["CompanyPhone"];
        //    company.Companyremarks = dynamic["Companyremarks"];
        //    return Json<Return_Company>(CompanyAction.Update(company));
        //}


        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCompany")]
        public Information Update(Company para)
        {
            Information info = new Information();
            info = CompanyAction.Update(para);
            return info;
        }
        

        ///<summary>
        ///获取子用户信息
        /// </summary>
        [HttpGet]
        [Route("GetAllCompany")]
        public GetCompanyDto GetCompanyInfo(string Company_ID, string CompanyName)
        {
            GetCompanyDto c = new GetCompanyDto();
            c= CompanyAction.GetAllcompany1(Company_ID,CompanyName);
            return c;
        }

        /// <summary>
        ///  获取分组（客户）数据
        /// </summary>
        /// <param name="Card_CompanyID"></param>
        /// <param name="PagNumber"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        [Route("GetCompanyNumber")]
        [HttpGet]
        public IHttpActionResult GetmessageForCompany2(string Card_CompanyID, int PagNumber, int Num)
        {
            Return_CardMessage3 r = new Return_CardMessage3();
            try
            {
                r.Cards = CompanyAction.GetCardsForCompany(Card_CompanyID, PagNumber, Num);
                r.flg = "1";
                r.Msg = "Success";
            }
            catch (Exception ex)
            {
                r.flg = "0";
                r.Msg = "error" + ex;
            }
            return Json<Return_CardMessage3>(r);
        }

        /// <summary>
        /// 给用户分配移动NB卡基站定位接口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertCompanyLocationInfo")]
        public  Information InsertCompanyLocationInfo(cardlocations para)
        {
            Information info = new Information();
            info = CompanyAction.InsertCompanyLocationInfo(para);
            return info;
        }

        ///<summary>
        ///用户查询基站定位API接口信息
        /// </summary>
        [HttpGet]
        [Route("GetCmccLocationInfo")]
        public cardlocation GetCmccLocationInfo(string Company_ID, string CompanyName)
        {
            cardlocation info = new cardlocation();
            info = CompanyAction.GetCmccLocationInfo(Company_ID, CompanyName);
            return info;
        }
    }
}
