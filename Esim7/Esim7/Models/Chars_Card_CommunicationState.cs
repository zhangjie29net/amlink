using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{/// <summary>
///通讯状态
/// </summary>
    public class Chars_Card_CommunicationState
    {
        /// <summary>
        ///  服务数据在线数量
        /// </summary>
        public string CompanyOnelineCardNum { get; set; }
        /// <summary>
        ///数据服务离线数量
        /// </summary>
        public string CompanyOfflineCardNum { get; set; }
    }
}