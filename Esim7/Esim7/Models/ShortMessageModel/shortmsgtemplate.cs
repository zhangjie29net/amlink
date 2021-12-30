using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.ShortMessageModel
{
    /// <summary>
    /// 短信模板表映射类
    /// </summary>
    public class shortmsgtemplate
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateContent { get; set; }
        public string TemplateNum { get; set; }
        public string Remark { get; set; }
        public DateTime AddTime { get; set; }
        public int IsDelete { get; set; }
    }
}