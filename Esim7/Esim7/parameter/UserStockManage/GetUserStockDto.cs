using Esim7.Models;
using Esim7.Models.UserStockModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    /// <summary>
    /// 获取用户库存信息
    /// </summary>
    public class GetUserStockDto
    {
        public string flg { get; set; }
        public string Msg { get; set; }
        public List<UserStock> userStocks { get; set; }
    }

    ///<summary>
    ///获取用户库存出入详细信息
    /// </summary>
    public class GetUserStockDetailDto
    {
        public string flg { get; set; }
        public string Msg { get; set; }
        public List<StockCardInfo> stockCards { get; set; }
    }

    ///<summary>
    ///入库出库卡信息
    /// </summary>
    public class StockCardInfo
    {
        public int Id { get; set; }
        public string Company_ID { get; set; }
        //操作人
        public string personnel { get; set; }
        //物料编码
        public string MaterielCode { get; set; }
        //物料名称
        public string MaterielName { get; set; }
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

        //沉默期
        public int silentDate { get; set; }
        //测试期
        public int TestDate { get; set; }
        //流量
        public string Flow { get; set; }
        public string Remark { get; set; }
        //出库用途
        public string OutPurpose { get; set; }
        //卡价格
        public decimal CardPrice { get; set; }
        //批次
        public string pici { get; set; }
        public string Card_ID { get; set; }
        public string Card_ICCID { get; set; }
    }

}