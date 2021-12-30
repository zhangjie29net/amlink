using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.User
{
    /// <summary>
    /// 用户修改个人信息参数
    /// </summary>
    public class UserUpdateInfo
    {
        ///<summary>
        ///当前登录用户的CompanyID
        /// </summary>
        public string Company_ID { get; set; }

        public string CompanyName { get; set; }

        public string CompanyPhone { get; set; }
        public string CompanyAdress { get; set; }
        public string loginname { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string fileName { get; set; }
    }
}
