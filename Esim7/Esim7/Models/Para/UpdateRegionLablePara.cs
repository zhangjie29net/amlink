using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.Para
{
    /// <summary>
    /// 批量修改标签/使用场景入参
    /// </summary>
    public class UpdateRegionLablePara
    {
        /// <summary>
        /// OperatorsFlg 运营商标识 1:移动 2：电信 3：联通 4全网通 5：漫游
        /// </summary>
        public string OperatorsFlg { get; set; }
        public string Company_ID { get; set; }
        public List<RegionLable> lables { get; set; }
    }
    public class RegionLable
    {
        public string SN { get; set; }
        public string ICCID { get; set; }
        public string 标签 { get; set; }
        public string 使用场景 { get; set; }
    }
}