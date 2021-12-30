using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    public class UpdateEnterStockPara
    {
        //当前用户编码
        public string Company_ID { get; set; }
        //入库单号
        public string EnterCode { get; set; }
        //出库单号
        public string OutCode { get; set; }
        //物料编码
        public string MaterielCode { get; set; }
        //套餐名称
        public string SetmealName { get; set; }
        //库存所在城市
        public string StockCity { get; set; }
        //库存具体位置
        public string StockAdderss { get; set; }
        //运营商名称
        public string OperatorName { get; set; }
        //卡形态名称
        public string CardXTName { get; set; }
        //卡类型名称
        public string CardTypeName { get; set; }
        //流量
        public string Flow { get; set; }
        //卡价格
        public decimal CardPrice { get; set; }
    }
}