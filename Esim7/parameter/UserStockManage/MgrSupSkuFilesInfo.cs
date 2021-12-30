using Esim7.Models.UserStockModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    public class MgrSupSkuFilesInfo
    {
        public MgrSupSkuFilesInfo(userstockfiles entity, string rootPath)
        {
            this.IsExist = true;
            //var str = entity.FileGuid.ToString("N");
            this.FilePath = string.Format("{0}", rootPath);
        }
        public MgrSupSkuFilesInfo()
        {
            this.IsExist = false;
        }
        public bool IsExist { get; set; }
        public string FilePath { get; set; }
    }
}