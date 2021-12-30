using Esim7.Action_KuCun;
using Esim7.Models;
using System.Web.Http;
using static Esim7.Action.Action_haringGetExcel;

namespace Esim7.Controllers
{     /// <summary>
///库存管理模块
/// </summary>
    public class StockController : ApiController

    #region  配置模块 添加
    {   /// <summary>
        /// 添加套餐     {"Operator":"12","Code":"123","PartNumber":"wef","PackageDescribe":"sgfer","Remark":"cewcw","Flow":"","CardXTID":"","CardTypeID":""}
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("AddSetMeal")]
        public IHttpActionResult Add_SetMeal()
        {
            return Json(Action_Stock.Add_SetMeal());
        }
        /// <summary>
        /// 添加运营商  移动电信   {"OperatorName":"中国联通"}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost, Route("AddOperator")]
        public IHttpActionResult Add_Operator()
        {
            return Json(Action_Stock.Add_Operator());
        }
        /// <summary>
        /// 添加 库房  {"StorageRoomName":"中国联通"}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost, Route("AddStorageroom")]
        public IHttpActionResult Add_storageroom()
        {
            return Json(Action_Stock.Add_storageroom());
        }
        /// <summary>
        /// 添加卡类型    {"CardTypeName":"NB"}
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("AddCardType")]
        public IHttpActionResult Add_CardType()
        {
            return Json(Action_Stock.Add_CardType());

        }
        /// <summary>
        /// 添加卡形态        {"CardXTName":"NB"}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost, Route("AddCardXingTai")]
        public IHttpActionResult Add_CardXingTai()
        {
            return Json(Action_Stock.Add_CardXingTai());
        }
        #endregion
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="Type">operator 运营商信息 storageroom 库房信息 cardtype 卡类型信息 cardxingtai 卡形态信息</param>
        /// <returns></returns>
        [HttpGet, Route("GetStockBasicMessage")]
        public IHttpActionResult GetMessage(string Type)
        {
            return Json(Action_Stock.GetStockMessage(Type));
        }
        /// <summary>
        /// 获取套餐信息  
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet, Route("GetSetMeal")]
        public IHttpActionResult GetSetMeal(string Company_ID)
        {
            return Json(Action_Stock.GetSelmeal(Company_ID));
        }

        ///<summary>
        ///修改套餐
        /// </summary>
        [HttpPost]
        [Route("UpateSetmeal")]
        public Information UpateSetmeal(UpdateSetmealPara para)
        {
            Information info = new Information();
            info = Action_Stock.UpateSetmeal(para);
            return info;
        }

        ///<summary>
        ///删除套餐
        /// </summary>
        [HttpGet]
        [Route("DeleSetmeal")]
        public Information DeleSetmeal(string SetMealID)
        {
            Information info = new Information();
            info = Action_Stock.DeleSetmeal(SetMealID);
            return info;
        }

        ///<summary>
        ///获取各种卡的套餐
        /// </summary>
        [HttpGet]
        [Route("GetSelmealType")]
        public SelmealTypeDto GetSelmealType(string Company_ID)
        {
            SelmealTypeDto dto = new SelmealTypeDto();
            dto = Action_Stock.GetSelmealType(Company_ID);
            return dto;
        }

        /// <summary>
        /// 入库操作  POST 
        /// Json 示例 
        /// {"operatorID":"操作人员 如 张三",
        /// "SetmealID2":"获取套餐的内码对应于获取套餐信息字段SetmealID",
        /// "testDate":"测试期 月",
        /// "silentDate":"沉默期 月",
        /// "OpeningDate":"开卡年限 年",
        /// "OperatorsID":"惠州移动 西安移动 可不填写",
        /// "Remarks":"备注",
        /// "StorageRoomID":"库房 仓库内码",
        /// "AccountID":"API配置信息生成的内码 用于数据更新和查找  用于区分运营商",
        /// "Platform":"平台内编码   用于区某个运营商的分新旧平台"
        /// "ICCIDS":[{"ICCID":""}]}
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("WareHousing")]
        public IHttpActionResult WareHousing()
        {
            return Json(Action_Stock.WareHousing());

        }
        /// <summary>
        /// 出库操作       {"ProductNumber":"库存剩余","ProductNumbers":"入库时总数量","OutNumber":"出库数量","remark":"备注","BatchID ":"库存单号","Operator":"操作人员","purpose":"用途","AccountID":"","SetmealID":"套餐ID"}
        /// </summary>
        /// <returns></returns> 
        [HttpPost, Route("OutofStock")]
        public IHttpActionResult OutofStock()
        {

            return Json(Action_Stock.OutOfStock2());

        }
        /// <summary>
        /// 添加API {"APPID":"","ECID":"","PASSWORD":"","accountName":"","TOKEN ":"","TRANSID":"","Remark":"","URL":"","Platform":""}
        /// </summary>
        /// <returns></returns> 
        [HttpPost, Route("AddAccount")]
        public IHttpActionResult AddAccount()
        {
            return Json(Action_Stock.AddAccount());

        }
        /// <summary>
        /// 更新运营商API {"APPID":"","ECID":"","PASSWORD":"","accountName":"","TOKEN ":"","TRANSID":"","Remark":"","URL":"","Platform":"","accountID":""}
        /// </summary>
        /// <returns></returns> 
        [HttpPost, Route("UpdateAccount")]
        public IHttpActionResult UpdateAccount()
        {
            return Json(Action_Stock.UpdateAccount());

        }
        /// <summary>
        /// 获取 库存信息
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetProduct()
        {

            return Json(Action_Stock.GetProuud());

        }
        /// <summary>
        ///获取 出库日志信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("Get_OutofStock_log")]
        public IHttpActionResult Get_OutofStock_log()
        {

            return Json(Action_Stock.Get_OutofStock_log());

        }

        /// <summary>
        ///   获取出库对应的ICCID号
        /// </summary>
        /// <param name="OutofstockID">入库单号</param>
        /// <returns></returns>
        /// 
        [HttpGet, Route("Get_OutOfStock_ICCID")]
        public IHttpActionResult Get_OutOfStock_ICCID(string OutofstockID)
        {


            return Json(Action_Stock.Get_OutOfStock_ICCID(OutofstockID));

        }
        /// <summary>
        /// 修改套餐   针对于公海数据   json：  {"SetMealID":"套餐ID"，"ICCID":[{"card":""}]}
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("Update_package")]
        public IHttpActionResult Update_package()
        {
            return Json(Action_Stock.Update_package());
        }

        ///<summary>
        ///奇迹修改卡的续费起止时间
        /// </summary>
        [HttpPost]
        [Route("UpdateCardRenewTime")]
        public  Information UpdateCardRenewTime(Root para)
        {
            Information info = new Information();
            info = Action_Stock.UpdateCardRenewTime(para);
            return info;
        }
    }
}
