using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.UserStockModel
{
    /// <summary>
    ///用户库存上传文件表
    /// </summary>
    public class userstockfiles
    {
        public int Id { get; set; }
        public string FileKey { get; set;}
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string EnterCode { get; set; }
        public string OutCode { get; set; }
        public bool IsDelete { get; set; }
        public DateTime AddTime { get; set; }
    }
}