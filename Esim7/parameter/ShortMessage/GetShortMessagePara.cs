using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.ShortMessage
{
    /// <summary>
    /// 查看短信发送日志 入参
    /// </summary>
    public class GetShortMessagePara
    {
        public string Company_ID { get; set; }
        public string TaskName { get; set; }
        public string[] StatrEnterTime { get; set; }
    }
}