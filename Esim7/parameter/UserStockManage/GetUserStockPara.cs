using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    /// <summary>
    /// 查找用户库存信息参数
    /// </summary>
    public class GetUserStockPara
    {
        //当前用户登录编码
        public string Company_ID { get; set; }
        //物料编码
        public string MaterielCode { get; set; }
        //卡类型
        public string CardTypeName { get; set; }
        //套餐名称
        public string SetmealName { get; set; }
        //开始入库时间
        //public DateTime? StatrEnterTime { get; set; }
        //结束入库时间
        public DateTime? EndEnterTime { get; set; }

        public string[] StatrEnterTime { get; set; }
    }
   
}