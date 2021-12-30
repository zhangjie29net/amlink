using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 物联网卡计费状态
    /// </summary>
    public class Chars_Charing_CardNum
    {

        /// <summary>
        ///正常使用
        /// </summary>
        public string CompanyCharingNormalCardNum { get; set; }
       /// <summary>
       /// 沉默
       /// </summary>
        public string CompanyCharingSilentCardNum { get; set; }
        /// <summary>
        /// 测试期
        /// </summary>
        public string CompanyCharingTestingCardNum { get; set; }

    }
}