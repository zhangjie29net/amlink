using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class User
    {
        public string userid { get; set; }
        public string loginname { get; set; }
        public string loginpwd { get; set; }
        public string MSg { get; set; }
        /// <summary>
        ///标志 是否注销 0，否          1，是
        /// </summary>
        public int status { get; set; }
        public string flg { get; set; }
        public string Company_ID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyAdress { get; set; }
        public string permission { get; set; }
        public string fileName { get; set; }
        public string UserType{ get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal AccountBalance { get; set; }
        public int User_Pid { get; set; }
    }

    ///<summary>
    ///上传用户认证数据和信息
    /// </summary>
    public class UploadAuthenticationPara
    {
        public string Company_ID { get; set; }
        public string CompanyName { get; set; }
        /// <summary>
        /// 营业执照注册号
        /// </summary>
        public string LicenseCode { get; set;}
        /// <summary>
        /// 上传的营业执照名称
        /// </summary>
        public string LicenseName { get; set; }
        ///<summary>
        ///身份证号码
        /// </summary>
        public string IDCode { get; set; }
        ///<summary>
        ///身份证正面照文件名
        /// </summary>
        public string IDpositiveName { get; set; }
        ///<summary>
        ///身份证反面照文件名
        /// </summary>
        public string IDbackName { get; set; }
    }

    public class AuthenticationDtos : Information
    {
        public List<AuthenticationInfo> dtos { get; set; }
    }

    ///<summary>
    ///查看企业认证信息
    /// </summary>
    public class AuthenticationInfo
    {
        /// <summary>
        /// 认证码
        /// </summary>
        public string AuthenticationCode { get; set; }
        public string Company_ID { get; set; }
        public string CompanyName { get; set; }
        /// <summary>
        /// 营业执照编号
        /// </summary>
        public string LicenseCode { get; set; }
        /// <summary>
        /// 营业执照文件存放路径
        /// </summary>
        public string LicenseName { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCode { get; set; }
        /// <summary>
        /// 身份证正面照文件存放位置
        /// </summary>
        public string IDpositiveName { get; set; }
        /// <summary>
        /// 身份证反面照文件存放位置
        /// </summary>
        public string IDbackName { get; set; }
        public int Status { get; set; }
        public DateTime AddTime { get; set; }
    }
}