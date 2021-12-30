using Esim7.Models;
using Esim7.Models.UserStockModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    /// <summary>
    /// 查看出库信息
    /// </summary>
    public class OutStockDto: Information
    {
        public List<UserStock> outstock { get; set; }
    }
}