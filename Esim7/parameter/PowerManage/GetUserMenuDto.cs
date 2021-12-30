using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.PowerManage
{
    /// <summary>
    /// 动态生成菜单
    /// </summary>
    public class GetUserMenuDto
    {
        public string Index { get; set; }
        public string MenuName { get; set; }
        public string icon { get; set; }

        public List<childrens> children { get; set; }
    }

    public class childrens
    {
        public string Menu { get; set; }
        public string MenuName { get; set; }
    }
}