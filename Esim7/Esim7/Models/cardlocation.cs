using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 添加移动物联网NB卡的基站定位
    /// </summary>
    public class cardlocations
    {
        /// <summary>
        /// 设置用户的公司内码(选择)
        /// </summary>
        public string CustomerComapnyID { get; set; }
        ///<summary>
        ///总次数
        /// </summary>
        public int TotalNum { get; set; }
    }

    /// <summary>
    /// 移动基站定位表实体类
    /// </summary>
    public class cmcclocation
    {
        public int id { get; set; }
        /// <summary>
        /// 设置用户的公司内码(选择)
        /// </summary>
        public string CustomerComapnyID { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EendTime { get; set; }
        /// <summary>
        /// 使用次数
        /// </summary>
        public int UseNum { get; set; }
        /// <summary>
        /// 总次数
        /// </summary>
        public int TotalNum { get; set; }
        ///<summary>
        ///剩余次数
        /// </summary>
        public int SurplusNum { get; set; }
    } 


    public class cardlocation :Information
    {
        public string flg { get; set; }
        public string Msg { get; set; }
        public List<cmcclocation> Cmcclocations { get; set; }
    }
    
}