using Esim7.Action;
using System.Web.Http;

namespace Esim7.Controllers
{  /// <summary>
/// 旧平台接口对接  移动
/// </summary>
    public class APIController : ApiController
    {
        #region 用户信息类

        /// <summary>
        /// CMIOT_API2001-在线信息实时查询
        /// </summary>
        /// <returns></returns>
        [Route("GetCMIOT_API12001")]
        [HttpGet]
        public IHttpActionResult GetMsaageer(string imsi)
        {

            return Json<object>(APIACTION.GetCMIOT_API12001(imsi));
        }
        /// <summary>
        ///  CMIOT_API2002-用户状态信息实时查询集团客户可根据所属物联卡的码号信息实时查询该卡的状态信息。  
        ///  返回参数说明：应答数据信息：00-正常；01-单向停机 02-停机；03-预销号 04-销号；05-过户 06-休眠；07-待激 99-号码不存在
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GeTCMIOT_API2002")]
        [HttpGet]
        public IHttpActionResult GeTCMIOT_API2002(string imsi)
        {
            return Json<object>(APIACTION.GetCMIOT_API2002(imsi));
        }
        /// <summary>
        /// 开关机信息实时查询 1. 应答数据信息  （在Result中）status  0关机 1 开机      2.应答公共信息中 status    0-正确，非0-失败
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2008")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2008(string imsi)
        {

            return Json<object>(APIACTION.GetCMIOT_API2008(imsi));



        }
        /// <summary>
        ///  CMIOT_API2103-物联卡GPRS服务开通查询
        /// 集团客户可以通过卡号（MSISDN/ICCID/IMSI，单卡）信息查询此卡的GPRS服务开通状态
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2103")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2103(string imsi)
        {


            return Json<object>(APIACTION.GetCMIOT_API2103(imsi));

        }
        /// <summary>
        ///  CMIOT_API2105-物联卡生命周期查询  集团客户根据卡号（imsi、msisdn、iccid三个中任意一个），查询物联卡当前生命周期，生命周期包括：00:正式期，01:测试期，02:沉默期，03:其他
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2105")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2105(string imsi)
        {


            return Json<object>(APIACTION.GetCMIOT_API2105(imsi));

        }






        /// <summary>
        ///  CMIOT_API2107-单个用户已开通服务查询 集团客户可以通过卡号（仅MSISDN）查询物联卡当前的服务开通状态
        ///  
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2107")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2107(string imsi)
        {

            return Json<object>(APIACTION.GetCMIOT_API2107(imsi));

        }




        /// <summary>     
        /// CMIOT_API2110-物联卡开户日期查询
        /// 集团客户可以通过API来实现对单个询物联卡基础信息的查询，包括ICCID、MSISDN、IMSI、入网日期（开户日期）。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2110")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2110(string imsi)
        {
            return Json<object>(APIACTION.GetCMIOT_API2110(imsi));
        }

        #endregion





        #region 用量类

        /// <summary>
        ///  CMIOT_API2005-用户当月GPRS查询   集团客户可查询所属物联卡当月截止到前一天24点为止的GPRS使用量（单位：KB）。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        /// 
        [Route("GetCMIOT_API2005")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2005(string imsi)
        {


            return Json<object>(APIACTION.Get_CMIOT_API2005(imsi));


        }





        #endregion

        [Route("GetCMIOT_API2101")]
        [HttpGet]
        /// <summary>
        /// 返回集团GPRS在线物联卡数量查询
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetCMIOT_API2101()
        {



            return Json<object>(APIACTION.GetCMIOT_API2101());


        }


        /// <summary>
        /// CMIOT_API2020-套餐内GPRS流量使用情况实时查询 (集团客户)  集团客户可查询所属物联卡当月套餐内GPRS流量使用情况
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2020")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2020(string imsi)
        {







            return Json<object>(APIACTION.GetGetCMIOT_API2020(imsi));


        }


        /// <summary>
        /// 物联卡单日GPRS使用量查询  CMIOT_API2300
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2300")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2300(string imsi, string date)
        {

            return Json<object>(APIACTION.GetCMIOT_API2300(imsi, date));

        }
        /// <summary>
        /// 测试用
        /// </summary>
        /// <param name="Card_CompanyID"></param>
        /// <param name="num"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public IHttpActionResult GetListCMIOT_API2300(string Card_CompanyID, string num, string date)
        {



            return Json<object>(APIACTION.GetCMIOT_API2300s(Card_CompanyID, num, date));


        }

        /// <summary>
        /// 以公司ID获取我连卡单日GPRS用量 单位K
        /// </summary>
        /// <param name="Card_CompanyID">公司ID</param>
        /// <param name="num">从0开始每一页显示20条</param>
        /// <param name="date">必须为昨天之前的7天内的某一天，格式为yyyymmdd</param>
        /// <returns></returns>
        [Route("GetListCMIOT_API2300")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2300Test(string Card_CompanyID, string num, string date)
        {

            return Json<object>(APIACTION.GetCMIOT_API2300ss(Card_CompanyID, num, date));

        }


        /// <summary>
        ///查询公司未注销的卡片的数量 离线 在线状态数量查询
        /// </summary>
        /// <param name="CompanyID">公司ID</param>
        /// <returns></returns>
        [Route("GetOnlineandOFFNumber")]
        [HttpGet]

        public IHttpActionResult QuerryOnlineAndOff(string CompanyID)
        {

            return Json<object>(APIACTION.GetOnlineandOFFNumber(CompanyID));
        }
        /// <summary>
        ///  CMIOT_API2037-物联卡资费套餐查询 集团客户可以根据物联卡码号信息查询该卡的套餐信息。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2037")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2037(string imsi)
        {
            return Json<object>(APIACTION.Get_CMIOT_API2037(imsi));

        }

        
        /// <summary>
        /// 查询单张卡余额
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCMIOT_API2011")]
        [HttpGet]
        public IHttpActionResult GetCMIOT_API2011(string imsi)
        {



            return Json<object>(APIACTION.Get_CMIOT_API2011(imsi));



        }
        /// <summary>
        ///查询周期  ICCID
        /// </summary>
        /// <param name="ICCID"></param>
        /// <returns></returns>
        [Route("JudgeGetCMIOT_API2105")]
        [HttpGet]
        public IHttpActionResult JudgeGetCMIOT_API2105(string ICCID)
        {

            return Json<object>(APIACTION.JudgeGetCMIOT_API2105(ICCID));
        }
    }
}
