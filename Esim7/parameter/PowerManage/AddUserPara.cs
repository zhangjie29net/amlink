using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.PowerManage
{
    /// <summary>
    /// 权限管理 添加用户参数
    /// </summary>
    public class AddUserPara
    {
        /// <summary>
        /// 登录用户公司编号
        /// </summary>
        public string Company_ID{ get; set; }
        /// <summary>
        /// 用户登录名称
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        ///<summary>
        ///公司地址
        /// </summary>
        public string CompanyAddress { get; set; }
        ///<summary>
        ///联系电话
        /// </summary>
        public string CompanyPhone { get; set; }
        ///<summary>
        ///备注信息
        /// </summary>
        public string CompanyRemark { get; set; }
    }
}