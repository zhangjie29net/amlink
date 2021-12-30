using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 对比数据取不同上传参数
    /// </summary>
    public class duibijihepara
    {
        public List<UpdateICCIDS> iccids{ get; set; }
    }

    public class UpdateICCIDS
    {
        public string ICCID { get; set; }
    }
}