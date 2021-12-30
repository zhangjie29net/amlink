using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    public class UploadUserstockFilesPara
    {
        public List<FilePath> filePaths { get; set; }
        public string OutCode { get; set; }
        public string EnterCode { get; set; }
    }

    public class FilePath
    {
        public string FileKey { get; set; }
    }
}