using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.PowerManageModel
{
    public class fk_menuInfo
    {
        public string flg { get; set; }
        public string Msg { get; set; }
        public int RowCount { get; set; }
        public List<fk_menu> fk_Menus { get; set; }
    }
    /// <summary>
    /// 权限管理菜单表
    /// </summary>
    public class fk_menu
    {
        public int id { get; set; }
        public string MenuID { get; set; }
        public string lable { get; set; }
        public string MenuURL { get; set; }
        public string MenuName { get; set; }
        /// <summary>
        /// 父菜单
        /// </summary>
        public int Menu_FatherID { get; set; }
        ///<summary>
        ///级别  1:一级菜单 2:二级菜单
        /// </summary>
        public int Type { get; set; }
        public string Index { get; set; }
        public string icon { get; set; }

    }


    public class OneMenuInfo: Message
    {
        public List<OneMenu> oneMenus { get; set; }
    }
    ///<summary>
    ///显示顶级菜单信息
    /// </summary>
    public class OneMenu
    {
        ///<summary>
        ///菜单id
        /// </summary>
        public int Id{ get; set; }
        ///<summary>
        ///菜单名称
        /// </summary>
        public string MenuName { get; set; }
    }

    ///<summary>
    ///显示树状结构菜单信息
    /// </summary>
    public class TreelikeMenu:Message
    {
        public List<TreeMenuInfo> treeMenuInfos { get; set; }
        public int[] key { get; set; }
        /// <summary>
        /// 半选中状态
        /// </summary>
        public int[] key1 { get; set; }
    }   
    public class TreeMenuInfo
    {
        //public int Type { get; set; }
        public string lable { get; set; }
        public int id { get; set; }
        public bool disabled { get; set; }
        public List<nemuinfo> children { get; set; } 
    }

    public class nemuinfo
    {
        public int id { get; set; }
        public string lable { get; set; }
        public bool disabled { get; set; }
    }

}