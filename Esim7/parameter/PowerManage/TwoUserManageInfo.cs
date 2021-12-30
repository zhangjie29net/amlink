using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.PowerManage
{
    /// <summary>
    /// 二级用户管理子集用户信息
    /// </summary>
    public class TwoUserManageInfo
    {
        public List<Company> conpany { get; set; }
        public string flg { get; set; }
        public string Msg { get; set; }
    }
}