using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
    /// <summary>
    /// 返回子级用户信息
    /// </summary>
    public class GetChildrenCompanyDto: Information
    {
        public List<ChildrenCompany> childrens { get; set; }
    }
    public class ChildrenCompany
    {
        /// <summary>
        /// 子用户的公司编码
        /// </summary>
        public string CustomerCompanyID { get; set; }
        public string Company_Name { get; set; }
    }

    ///<summary>
    ///信息接收
    /// </summary>
    public class GetInfoDto
    {
        public int id { get; set; }
        public int User_Pid { get; set; }
        public decimal tcbl { get; set; }//提成比例
        public decimal sxfbl { get; set; }//手续费比例

    }
}