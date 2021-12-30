using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.PowerManage
{
    /// <summary>
    /// 权限管理添加菜单参数
    /// </summary>
    public class AddMenuPara
    {
        public string MenuName { get; set; }
        public string MenuURL { get; set; }
        public int Menu_FatherID { get; set; }       
    }

    ///<summary>
    ///权限管理编辑菜单参数
    /// </summary>
    public class EditMenuPara
    {
        public int id { get; set; }
        public string MenuName { get; set; }
        public string MenuURL { get; set; }
        public int Menu_FatherID { get; set; }
    }
}