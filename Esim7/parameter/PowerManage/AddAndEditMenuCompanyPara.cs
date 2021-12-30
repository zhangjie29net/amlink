using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.PowerManage
{
    /// <summary>
    /// 添加编辑用户菜单和修改用户信息入参
    /// </summary>
    public class AddAndEditMenuCompanyPara
    {
        public string CompanyName { get; set; }
        public string LoginName { get; set; }
        public string CompanyAdress { get; set; }
        public string CompanyPhone { get; set; }
        public string Companyremarks { get; set; }
        public string CompanyMenu { get; set; }
        /// <summary>
        /// 要修改的用户的Company_ID
        /// </summary>
        public string Company_ID { get; set; }        
    }
}