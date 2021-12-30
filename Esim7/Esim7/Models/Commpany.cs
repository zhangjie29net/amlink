using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class Company
    {
        ///<summary>
        ///当前登录用户的CompanyID
        /// </summary>
        //public string Company_ID { get; set; }       
        /// <summary>
        /// 添加的子用户的CompanyID
        /// </summary>
        public string CompanyID { get; set; }

        public string CompanyName { get; set; }
        public string Companyremarks { get; set; }
       
       
        //public int status { get; set; }

        public string CompanyPhone { get; set; }
        public string CompanyAdress { get; set; }
        public   string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// 用户创建时间  开户时间
        /// </summary>
        public DateTime  CompanyOpeningDate { get; set; }

        public string status { get; set; }
        public string Number { get; set; }
        public int id { get; set; }
        public string CompanyTolCardNum { get; set; }
        ///<summary>
        ///邮箱
        /// </summary>
        public string Email { get; set; }
        ///<summary>
        ///营业执照注册号
        /// </summary>
        public string LicenseCode { get; set; }
        ///<summary>
        ///税务登记号
        /// </summary>
        public string DutyParagraph { get; set; }
        ///<summary>
        ///开户银行
        /// </summary>
        public string Bank { get; set; }
        ///<summary>
        ///银行账号
        /// </summary>
        public string BankAccount { get; set; }
        ///<summary>
        ///地址电话
        /// </summary>
        public string AddressPhone { get; set; }
    }

    public class GetCompanyDto
    {
        public string MSg { get; set; }
        public string flg { get; set; }
        public List<Company> conpany { get; set; }
    }
    
}