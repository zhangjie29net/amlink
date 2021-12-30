using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 地区映射类
    /// </summary>
    public class db_yhm_city
    {
        public int class_id { get; set; }
        public int class_parent_id { get; set; }
        public string class_name { get; set; }
        public int class_type { get; set; }
    }
}