using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.PowerManageModel
{
    public class cf_userInfo
    {
        /// <summary>
        /// 0:添加的用户名称已存在 1:添加成功 -1:添加失败
        /// </summary>
        public string flg { get; set; }
        public string MSg { get; set; }
        public int RowNum { get; set; }
        public List<cf_user> cf_Users { get; set; }
    }
    public class cf_user
    {
        public int id { get; set; }
        public string UserID { get; set; }
        public string LoginName { get; set; }
        public string Loginpassword { get; set; }      
        /// <summary>
        ///标志 是否注销 0，否          1，是
        /// </summary>
        public int status { get; set; }
        public string Company_ID { get; set; }
        public string permission { get; set; }        
        ///<summary>
        ///用户有的菜单"1,2,3,4"
        /// </summary>
        public string User_Menu { get; set; }
        ///<summary>
        ///用户上级用户Id
        /// </summary>
        public int User_Pid { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAdress { get; set; }
        public string CompanyPhone { get; set; }
        public string Companyremarks { get; set; }
    }

    ///<summary>
    ///给子用户分配卡信息
    /// </summary>
}