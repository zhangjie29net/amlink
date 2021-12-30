using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Dapper;
using Esim7.Dto;
using Esim7.Models;
using Esim7.Models.UserStockModel;
using Esim7.parameter.DistributionManage;
using Esim7.UNity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Esim7.Action
{
    /// <summary>
    /// 分润模块
    /// </summary>
    public class DistributionAction
    {
        ///<summary>
        ///设置分润提成比例(奇迹和子用户设置)
        /// </summary>
        public Information AddDistribution(distributionratio para)
        {
            Information info = new Information();
            DateTime time = DateTime.Now;
            string InstallNum = Unit.GetTimeStamp(DateTime.Now);
            string OperatorID = string.Empty;
            string CardXTID = string.Empty;
            string CardTypeID = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //根据setmealID获取卡形态运营商等信息
                    string sqlsetmeal = "select * from setmeal where SetmealID='"+para.SetmealID+"'";
                    var setmealinfo = conn.Query<setmeal>(sqlsetmeal).FirstOrDefault();
                    if (setmealinfo != null)
                    {
                        OperatorID = setmealinfo.OperatorID;
                        CardXTID = setmealinfo.CardXTID;
                        CardTypeID = setmealinfo.CardTypeID;
                    }
                    if (para.Company_ID == "1556265186243")
                    {
                        if (para.CustomerCompanyID != "all")//一个用户一个用户的设置
                        {
                            string sqladd = "insert into distributionratio(Company_ID,OperatorID,CardXTID,CardTypeID,SetmealID,CardPrice,SaleCardPrice,tcbl,sxfbl,CustomerCompanyID,AddTime,InstallNum)" +
                            "values('" + para.Company_ID + "','" + OperatorID + "','" + CardXTID + "','" + CardTypeID + "','" + para.SetmealID + "','" + para.CardPrice + "','" + para.CardPrice + "'," + para.tcbl + "," +
                            "" + para.sxfbl + ",'" + para.CustomerCompanyID + "','" + time + "','" + InstallNum + "')";
                            conn.Execute(sqladd);
                        }
                        if (para.CustomerCompanyID == "all")
                        {
                            StringBuilder sqladdstr = new StringBuilder("insert into distributionratio(Company_ID,OperatorID,CardXTID,CardTypeID,SetmealID,CardPrice,SaleCardPrice,tcbl,sxfbl,CustomerCompanyID,AddTime,InstallNum) values");
                            //获取登录用户的子用户信息
                            string sqlcustomer = "select * from cf_user where Company_ID='" + para.Company_ID + "'";
                            int id = conn.Query<GetInfoDto>(sqlcustomer).Select(t => t.id).FirstOrDefault();
                            string sqlcustomerlist = "select * from cf_user where User_Pid=" + id + " and status=1";
                            var list = conn.Query<distributionratio>(sqlcustomerlist).ToList();
                            foreach (var item in list)
                            {
                                sqladdstr.Append("('" + para.Company_ID + "','" + OperatorID + "','" + CardXTID + "','" + CardTypeID + "','" + para.SetmealID + "'," + para.CardPrice + "," + para.CardPrice + "," + para.tcbl + "," + para.sxfbl + ",'" + item.Company_ID + "','" + time + "','" + InstallNum + "'),");
                            }
                            conn.Execute(sqladdstr.ToString().Substring(0, sqladdstr.Length - 1));
                        }
                        info.flg = "1";
                        info.Msg = "提成比例设置成功!";
                    }
                    if (para.Company_ID != "1556265186243")
                    {
                        if (para.CustomerCompanyID != "all")//一个用户一个用户的设置
                        {
                            string sqladd = "insert into distributionratio(Company_ID,OperatorID,CardXTID,CardTypeID,SetmealID,CardPrice,SaleCardPrice,tcbl,sxfbl,CustomerCompanyID,AddTime,InstallNum)" +
                            "values('" + para.Company_ID + "','" + OperatorID + "','" + CardXTID + "','" + CardTypeID + "','" + para.SetmealID + "','" + para.CardPrice + "','" + para.CardPrice + "'," + para.tcbl + "," +
                            "" + para.sxfbl + ",'" + para.CustomerCompanyID + "','" + time + "','" + InstallNum + "')";
                            conn.Execute(sqladd);
                        }
                        if (para.CustomerCompanyID == "all")
                        {
                            StringBuilder sqladdstr = new StringBuilder("insert into distributionratio(Company_ID,OperatorID,CardXTID,CardTypeID,SetmealID,CardPrice,SaleCardPrice,tcbl,sxfbl,CustomerCompanyID,AddTime,InstallNum) values");
                            //获取登录用户的子用户信息
                            string sqlcustomer = "select * from cf_user where Company_ID='" + para.Company_ID + "'";
                            int id = conn.Query<GetInfoDto>(sqlcustomer).Select(t => t.id).FirstOrDefault();
                            string sqlcustomerlist = "select * from cf_user where User_Pid=" + id + " and status=1";
                            var list = conn.Query<distributionratio>(sqlcustomerlist).ToList();
                            foreach (var item in list)
                            {
                                sqladdstr.Append("('" + para.Company_ID + "','" + OperatorID + "','" + CardXTID + "','" + CardTypeID + "','" + para.SetmealID + "','" + para.CardPrice + "'," + para.CardPrice + "," + para.tcbl + "," + para.sxfbl + ",'" + item.Company_ID + "','" + time + "','" + InstallNum + "'),");
                            }
                            conn.Execute(sqladdstr.ToString().Substring(0, sqladdstr.Length - 1));
                        }
                        info.flg = "1";
                        info.Msg = "提成比例设置成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "提成比例设置失败:"+ex;
            }
            return info;
        }

        /// <summary>
        /// 获取子级用户
        /// </summary>
        /// <param name="Company_ID"></param>
        /// <returns></returns>
        public GetChildrenCompanyDto GetChildrenCompanyInfo (string Company_ID)
        {
            GetChildrenCompanyDto dto = new GetChildrenCompanyDto();
            List<ChildrenCompany> childrens = new List<ChildrenCompany>();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string getusidsql = "select id from cf_user where Company_ID='"+Company_ID+"' and status=1";
                    int Id = 0;
                    Id= conn.Query<distributionratio>(getusidsql).Select(t=>t.Id).FirstOrDefault();
                    string getusinfo = "select Company_ID from cf_user where User_Pid=" + Id + "";
                    var userinfos = conn.Query<distributionratio>(getusinfo).ToList();
                    foreach (var item in userinfos)
                    {
                        string Company_Name = "";
                        string sqlcomname = "select CompanyName from company where CompanyID='"+item.Company_ID+"'";
                        Company_Name = conn.Query<Company>(sqlcomname).Select(t => t.CompanyName).FirstOrDefault();
                        ChildrenCompany company = new ChildrenCompany();
                        company.CustomerCompanyID = item.Company_ID;
                        company.Company_Name = Company_Name;
                        childrens.Add(company);
                    }
                    ChildrenCompany children = new ChildrenCompany();
                    children.CustomerCompanyID = "all";
                    children.Company_Name = "全部";
                    childrens.Insert(0,children);
                    dto.childrens = childrens;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.childrens = null;
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }

        ///<summary>
        ///编辑子级用户的提成比例
        /// </summary>
        public Information UpdateChildrenDistribution(distributionratio para)
        {
            Information info = new Information();
            DateTime time = DateTime.Now;
            string CardXTID = string.Empty;
            string OperatorID = string.Empty;
            string CardTypeID = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (para.Company_ID == "1556265186243")//当前登录的用户公司编码 如果是奇迹物联直接修改信息
                    {
                        string updatedistribution = "update distributionratio set SaleCardPrice="+para.CardPrice+ ",CardPrice="+para.CardPrice+", tcbl=" + para.tcbl + ",sxfbl='" + para.sxfbl + "' where Id=" + para.Id + "";
                        conn.Execute(updatedistribution);
                        info.flg = "1";
                        info.Msg = "成功!";
                    }
                    if (para.Company_ID != "1556265186243")
                    {
                        string updatedistribution = "update distributionratio set SaleCardPrice=" + para.CardPrice + "  where Id=" + para.Id + "";
                        conn.Execute(updatedistribution);
                        info.flg = "1";
                        info.Msg = "成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:"+ex;
            }
            return info;
        }
        ///<summary>
        ///查看子级用户提成比例
        /// </summary>
        public GetChildrenDistributionDto GetChildrenDistributionInfo(DistributionPara para)
        {
            GetChildrenDistributionDto dto = new GetChildrenDistributionDto();
            try
            {
                string sql = string.Empty;
                if (para.Company_ID == "1556265186243")
                {
                     sql = @"select t1.Id,t1.tcbl,t1.AddTime,t1.CustomerCompanyID,t1.Company_ID,t1.SetmealID,t6.OperatorName,t1.InstallNum,
                               t1.CardXTID,t1.OperatorID,t1.CardTypeID,t1.sxfbl,t1.CardPrice,t2.PackageDescribe as SetmealName,t3.CardTypeName,t4.CardXTName,t5.CompanyName
                               from distributionratio t1 left join setmeal t2 on t1.SetmealID=t2.SetmealID left join cardtype t3 on t1.CardTypeID=t3.CardTypeID left join 
                               card_xingtai t4 on t1.CardXTID=t4.CardXTID left join company t5 on t1.CustomerCompanyID=t5.CompanyID
                               left join operator t6 on t1.OperatorID=t6.OperatorID where t1.Company_ID='" + para.Company_ID + "'";
                }
                else
                {
                    if (para.ApiFlg == "1")//查看列表
                    {
                        sql = @"select t1.Id,t1.tcbl,t1.AddTime,t1.CustomerCompanyID as Company_ID,t1.SetmealID,t6.OperatorName,t1.InstallNum,
                               t1.CardXTID,t1.OperatorID,t1.CardTypeID,t1.sxfbl,t1.CardPrice,t2.PackageDescribe as SetmealName,t3.CardTypeName,t4.CardXTName,t5.CompanyName
                               from distributionratio t1 left join setmeal t2 on t1.SetmealID=t2.SetmealID left join cardtype t3 on t1.CardTypeID=t3.CardTypeID left join 
                               card_xingtai t4 on t1.CardXTID=t4.CardXTID left join company t5 on t1.CustomerCompanyID=t5.CompanyID
                               left join operator t6 on t1.OperatorID=t6.OperatorID where t1.Company_ID='" + para.Company_ID + "'";//t1.CustomerCompany='" + para.Company_ID + "' or 
                    }
                    if(string.IsNullOrWhiteSpace(para.ApiFlg))//在线购卡
                    {
                        sql = @"select t1.Id,t1.tcbl,t1.AddTime,t1.CustomerCompanyID as Company_ID,t1.SetmealID,t6.OperatorName,t1.InstallNum,
                               t1.CardXTID,t1.OperatorID,t1.CardTypeID,t1.sxfbl,t1.SaleCardPrice as CardPrice,t2.PackageDescribe as SetmealName,t3.CardTypeName,t4.CardXTName,t5.CompanyName
                               from distributionratio t1 left join setmeal t2 on t1.SetmealID=t2.SetmealID left join cardtype t3 on t1.CardTypeID=t3.CardTypeID left join 
                               card_xingtai t4 on t1.CardXTID=t4.CardXTID left join company t5 on t1.CustomerCompanyID=t5.CompanyID
                               left join operator t6 on t1.OperatorID=t6.OperatorID where t1.CustomerCompanyID='" + para.Company_ID + "'";
                    }
                    
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    dto.getChildrens = conn.Query<GetChildrenDistribution>(sql).OrderByDescending(t => t.AddTime).ToList();
                    if (!string.IsNullOrWhiteSpace(para.OperatorID))
                    {
                        dto.getChildrens = dto.getChildrens.Where(t => t.OperatorID == para.OperatorID).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.CardTypeID))
                    {
                        dto.getChildrens = dto.getChildrens.Where(t => t.CardTypeID == para.CardTypeID).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.CardXTID))
                    {
                        dto.getChildrens = dto.getChildrens.Where(t => t.CardXTID == para.CardXTID).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(para.SetmealID))
                    {
                        dto.getChildrens = dto.getChildrens.Where(t => t.SetmealID == para.SetmealID).ToList();
                    }
                    dto.Msg = "查询成功!";
                    dto.flg = "1";
                }
            }
            catch (Exception ex)
            {
                dto.getChildrens = null;
                dto.Msg = "查询失败:"+ex;
                dto.flg = "-1";
            }
            return dto;
        }

        ///<summary>
        ///子级用户创建订单
        /// </summary>
        public GetOrder AddOrder(saleorder para)
        {
            string Company_ID = string.Empty;
            string Pid = "select * from cf_user where Company_ID='"+para.Company_ID+"'";//根据登录的用户公司编码找到父级公司给Company_ID赋值
            int User_Pid = 0;
            GetOrder info = new GetOrder();
            DateTime time = DateTime.Now;
            DateTime RenewalDateAddTime = DateTime.Now.AddMonths(para.MonthNum);
            string CityStr = string.Empty;
            string Address = string.Empty;
            string OrderNum = Unit.GetTimeStamp(DateTime.Now);
            decimal CardPrice = 0;//卡价格奇迹设置
            decimal SaleCardPrice =0;//卡销售价格 二级用户设置
            decimal tcbl = 0;//提成比例
            decimal sxfbl = 0;//手续费比例
            decimal Commission = 0;//回扣
            decimal Pricedifference = 0;//差价
            decimal TotalPricedifference = 0;//总差价
            decimal Totalsxfprice = 0;//总的手续费
            string Describe = string.Empty;//返佣金或者提现描述
            decimal TotalPrice = para.CardPrice * para.CardNum * para.MonthNum; 
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (para.CityStr.Length > 0)
                    {
                        for (var i = 0; i < para.CityStr.Length; i++)
                        {
                            //CityStr += para.CityStr[i].ToString() + ",";
                            int classid = Convert.ToInt32(para.CityStr[i]);
                            string citynamesql = "select class_name as label from db_yhm_city where class_id=" + classid + "";
                            CityStr += conn.Query<AreaList>(citynamesql).Select(t => t.label).FirstOrDefault();
                        }
                    }
                    Address = CityStr + para.Address;

                    User_Pid = conn.Query<GetInfoDto>(Pid).Select(t => t.User_Pid).FirstOrDefault();
                    string Pcompanyinfo = "select * from cf_user where id=" + User_Pid + "";
                    Company_ID = conn.Query<saleorder>(Pcompanyinfo).Select(t => t.Company_ID).FirstOrDefault();
                    //计算父级的回扣
                    string sqlcardprice = "select CardPrice from distributionratio where CustomerCompanyID='" + Company_ID + "' and SetmealID='" + para.SetmealID + "'";//奇迹给二级用户设置的卡价格
                    string sqlcustomerprice = "select SaleCardPrice as CardPrice  from distributionratio where CustomerCompanyID='" + para.Company_ID + "' and SetmealID='" + para.SetmealID + "'";//二级用户给三级用户设置的卡价格
                    string sqlbl = "select * from distributionratio where CustomerCompanyID='" + Company_ID + "' and SetmealID='" + para.SetmealID + "'";//奇迹给二级用户设置的提成比例和手续费比例
                    CardPrice = conn.Query<saleorder>(sqlcardprice).Select(t => t.CardPrice).FirstOrDefault();
                    SaleCardPrice = conn.Query<saleorder>(sqlcustomerprice).Select(t => t.CardPrice).FirstOrDefault();
                    var lists = conn.Query<GetInfoDto>(sqlbl).FirstOrDefault();
                    if (lists != null)
                    {
                        tcbl = lists.tcbl / 100;
                        sxfbl = lists.sxfbl / 100;
                    }
                    if (CardPrice == SaleCardPrice)//如果二级用户按照原价卖出
                    {
                        Commission = TotalPrice * tcbl;
                    }
                    if (CardPrice < SaleCardPrice)//二级用户设置价格高于奇迹设置价格
                    {
                        Pricedifference = SaleCardPrice - CardPrice;
                        TotalPricedifference = Pricedifference * para.CardNum * para.MonthNum;//总利润价格
                        Totalsxfprice = TotalPricedifference * sxfbl;
                        Commission = TotalPricedifference - Totalsxfprice;
                    }
                    if (CardPrice > SaleCardPrice)//二级用户设置价格低于奇迹设置价格
                    {
                        info.flg = "-1";
                        info.Msg = "服务器崩溃请稍后重试";
                        return info;
                    }

                    Describe = "返佣金+" + Math.Round(Commission, 2) + "元";
                    string addordersql = "insert into saleorder(Company_ID,OperatorName,CardXTName,CardTypeName,SetmealName,CardPrice,CustomerCompany,TotalPrice,CardNum,MonthNum,PurchaseDate,RenewalDate,AddTime,OrderNum,Commission,Remark,Address)" +
                    "values('" + Company_ID + "','" + para.OperatorName + "','" + para.CardXTName + "','" + para.CardTypeName + "','" + para.SetmealName + "'," + para.CardPrice + ",'" + para.Company_ID + "'," +
                    "" + TotalPrice + "," + para.CardNum + "," + para.MonthNum + ",'" + time + "','" + RenewalDateAddTime + "','" + time + "','" + OrderNum + "'," + Commission + ",'" + Describe + "','" + Address + "')";
                    string APP_PRIVATE_KEY = "MIIEogIBAAKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQABAoIBAG0Cc/Z9IgU5cYYoEhid+wd6zxGMlmxiJGr0M+U9l7P7y00BsfdFQ5BJPzx1m14xLKWYeaZPVXb0AnnCd4LUvHe7f6rLQ5Wbjg/xOioHTTBYhvRGpIIokY7rlUhNwNANR9J+gxoE7OPeZaWDdEbCWXMQlxi2yH8zH3QA8PBrfcLtF0i7mfLA+/oo4hw4HpsqebUjNYGh5KR6O63yf7vJS99wFZP5Dmk5zoO8AAxeuRkEAoiGFtsa8S4xQw4GRp/3nAhUK2Ib7y0qZSlB1oUd4j6QR7IwbNDA7/8uJv4lU6OQ7QZMm02JX4zieQmkv3XtTDQ2V6TJ8EQ3NvtpyzYa9AECgYEA70SrOl0J7TygrJWVMm1hiW/uO58cZ+d+Ab/2DGOQs/pTL4I2aT0fW95Hc/IoTTRoi09VeQUAvfmhpmOwwkfZSUToB6ojV6Mg8BCntmyY7ASXWSUK95Lgfcj1Vr964PlAMBsSoM8jjgk0zvgeb8P1a6qyU/IPV1i4IAh9PbVoyEECgYEA4mTkKnkq/+iHiQsLXsyJNbNX90PbktbwCW3sa9GNLwZEhZFkq/N/tvB9+qVRYE1S/TXBHIhKaj34T0ZlYOu78ZQQM6LgM7nRStoLrlVSG/srgQlHm9LgOgynzZkXBUD+uU347cJ5aM4Ew8FAMEd9OWEeIPHrZDK4VvFXqOBYq/ECgYBn0CCG4yVSdJK2LvSb+49tRU5VOhTmFC+87KACAhUfscX0AAhBow5/GrNf4DqSPOH7R8GrD3uh8bSsb+aadPgW7TnLUYuiE5pP7roF0ZqMFPXh7MuUXXrfuJiSOeRDxoGOHcD4Wsdvchkij88M6TYLr/VNrOHxIQJKi8RjSNmcwQKBgANKxkqb0nVAM2BZycOKI+ClB/1vfizndTwd3hc/R9dMNwjeMWGSu+O0IZDYgJNu7GsEMhexH6vl1MuKUYUUSHpd1dJ6Zto5tIJrI0pYsUX45AwPT3xDl8EgV/xUYpJP/KRDLwB+GHferxENqVpKX9bKw75k5jBh0G5rOgQZpxBxAoGATTXDJ02LLE+cjJEF814V7AnMaH6/xMA9DJ/fNKaajyIviDTbB2+0ZM7bWoLdCuQ/k4K+tlMpT7foY6GE+sZHHKcdUe3llWVKR4kpGKrreP8/BZPTL3NjBMngvn68T5cq453/f7IaVDcXJyAJV/sM5wDqWfaY5KuUL1YTTallxzk=";
                    string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQAB";
                    IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021002118612482", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "UTF-8", false);
                    AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
                    request.BizContent = "{" +
                    "\"out_trade_no\":\""+OrderNum+ "\"," +
                    "\"total_amount\":"+TotalPrice+ "," +
                    "\"product_code\":\"FAST_INSTANT_TRADE_PAY\"," +
                    "\"subject\":\"simcard\"" +
                    "  }";
                    request.SetReturnUrl("http://www.iot-simlink.com/#/payment");//支付成功后返回的地址
                    AlipayTradePagePayResponse response = client.pageExecute(request, null, "post");
                    conn.Execute(addordersql);
                    info.Body = response.Body;
                    info.flg = "1";
                    info.Msg = "成功!";
                    //info.TotalPrice = TotalPrice;
                    //info.OrderNum = OrderNum;
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:"+ex;
            }
            return info;
        }

      
        

        ///<summary>
        ///返回子级可选择的套餐卡类型运营商信息
        /// </summary>
        public AvailableSetmeal GetAvailableSetmealDto(string Company_ID)
        {
            AvailableSetmeal dto = new AvailableSetmeal();
            List<OperatorList> operatorLists = new List<OperatorList>();
            List<CardXTList> cardXTLists = new List<CardXTList>();
            List<CardTypeList> cardTypeLists = new List<CardTypeList>();
            List<SetmealList> setmealLists = new List<SetmealList>();
            string CardXTName = string.Empty;
            string CardTypeName = string.Empty;
            string SetmalName = string.Empty;
            string OperatorName = string.Empty;
            try
            {
                string sql =string.Empty;
                string sqlcardxt = string.Empty;
                string sqlcardtype = string.Empty;
                string sqlcardsetmeal = string.Empty;
                string sqloperator = string.Empty;
                if (Company_ID == "1556265186243")//奇迹物联
                {
                    sql = "select * from distributionratio where Company_ID='" + Company_ID + "'";
                    sqlcardxt = "select DISTINCT CardXTID from distributionratio where Company_ID = '" + Company_ID + "'";
                    sqlcardtype = "select DISTINCT CardTypeID from distributionratio where Company_ID = '" + Company_ID + "'";
                    sqlcardsetmeal = "select DISTINCT SetmealID from distributionratio where Company_ID = '" + Company_ID + "'";
                    sqloperator = "select DISTINCT OperatorID from distributionratio where Company_ID = '" + Company_ID + "'";
                }
                else
                {
                    sql = "select * from distributionratio where CustomerCompanyID='" + Company_ID + "'";
                    sqlcardxt = "select DISTINCT CardXTID from distributionratio where CustomerCompanyID = '" + Company_ID + "'";
                    sqlcardtype = "select DISTINCT CardTypeID from distributionratio where CustomerCompanyID = '" + Company_ID + "'";
                    sqlcardsetmeal = "select DISTINCT SetmealID from distributionratio where CustomerCompanyID = '" + Company_ID + "'";
                    sqloperator = "select DISTINCT OperatorID from distributionratio where CustomerCompanyID = '" + Company_ID + "'";
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var list = conn.Query<distributionratio>(sqloperator).ToList();
                    foreach (var item in list)
                    {
                        OperatorList operatorList = new OperatorList();
                        string sqloperatorname = "select OperatorName from operator where OperatorID='"+item.OperatorID+"'";
                        OperatorName = conn.Query<OperatorList>(sqloperatorname).Select(t => t.OperatorName).FirstOrDefault();
                        operatorList.OperatorName = OperatorName;
                        operatorList.OperatorID = item.OperatorID;
                        operatorLists.Add(operatorList);
                    }
                    var listcardxt = conn.Query<distributionratio>(sqlcardxt).ToList();
                    foreach (var itemxt in listcardxt)
                    {
                        CardXTList cardXTList = new CardXTList();
                        cardXTList.CardXTID = itemxt.CardXTID;
                        string sqlxtname = "select CardXTName from card_xingtai where CardXTID='" + itemxt.CardXTID + "'";
                        CardXTName = conn.Query<GetChildrenDistribution>(sqlxtname).Select(t => t.CardXTName).FirstOrDefault();
                        cardXTList.CardXTName = CardXTName;
                        cardXTLists.Add(cardXTList);
                    }
                    var listcardtype = conn.Query<distributionratio>(sqlcardtype).ToList();
                    foreach (var itemtype in listcardtype)
                    {
                        CardTypeList cardTypeList = new CardTypeList();
                        string sqltypename = "select CardTypeName from cardtype where CardTypeID='" + itemtype.CardTypeID + "'";
                        CardTypeName = conn.Query<CardTypeList>(sqltypename).Select(t => t.CardTypeName).FirstOrDefault();
                        cardTypeList.CardTypeID = itemtype.CardTypeID;
                        cardTypeList.CardTypeName = CardTypeName;
                        cardTypeLists.Add(cardTypeList);
                    }
                    var listcardsetmeal= conn.Query<distributionratio>(sqlcardsetmeal).ToList();
                    foreach (var itemsetmeal in listcardsetmeal)
                    {
                        SetmealList setmealList = new SetmealList();
                        string sqlsetmalname = "select PackageDescribe as SetmealName from setmeal where SetmealID='" + itemsetmeal.SetmealID + "'";
                        SetmalName = conn.Query<SetmealList>(sqlsetmalname).Select(t => t.SetmealName).FirstOrDefault();
                        setmealList.SetmealName = SetmalName;
                        setmealList.SetmealID = itemsetmeal.SetmealID;
                        setmealLists.Add(setmealList);
                    }
                    dto.setmealLists = setmealLists;
                    dto.cardTypeLists = cardTypeLists;
                    dto.cardXTLists = cardXTLists;
                    dto.operatorLists = operatorLists;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }


        ///<summary>
        ///返回套餐信息
        /// </summary>
        public SetMealInfoDto GetSetmealInfo(string Company_ID)
        {
            SetMealInfoDto dto = new SetMealInfoDto();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (Company_ID == "1556265186243")
                    {
                        string sqlsetmeal = "select SetmealID,PackageDescribe as SetmealName from setmeal where Company_ID='" + Company_ID + "'";
                        dto.SetmealLists = conn.Query<SetmealList>(sqlsetmeal).ToList();
                        dto.flg = "1";
                        dto.Msg = "成功!";
                    }
                    else
                    {
                        string sqlsetmeal = "select t1.SetmealID,t2.PackageDescribe as SetmealName from distributionratio t1 left join setmeal t2 on t1.SetmealID=t2.SetmealID where t1.CustomerCompanyID='" + Company_ID + "'";
                        dto.SetmealLists = conn.Query<SetmealList>(sqlsetmeal).ToList();
                        dto.flg = "1";
                        dto.Msg = "成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                dto.SetmealLists = null;
                dto.flg = "-1";
                dto.Msg = "出现错误:"+ex;
            }
            return dto;
        }

        /// <summary>
        /// 获取省市县  废弃
        /// </summary>
        public CityDto GetCityInfo1()
        {
            CityDto Dto = new CityDto();
            List<ProvinceList> ProvinceChildren = new List<ProvinceList>();
            try
            {
                string strprovince = "select * from db_yhm_city where class_type=1";//省数据
                string strcity = "select * from db_yhm_city where class_type=2";//市数据
                string strares = "select * from db_yhm_city where class_type=3";//区数据
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    
                    var provincelist = conn.Query<db_yhm_city>(strprovince).ToList();
                    foreach (var item in provincelist)
                    {
                        ProvinceList province = new ProvinceList();
                        province.label = item.class_name;
                        province.value = item.class_id;
                        ProvinceChildren.Add(province);
                        var citylist = conn.Query<db_yhm_city>(strcity).Where(t=>t.class_parent_id==item.class_id).ToList();
                        List<CityList> CityChildren = new List<CityList>();
                        foreach (var cityitem in citylist)
                        {
                            
                            CityList city = new CityList();
                            city.value = cityitem.class_id;
                            city.label = cityitem.class_name;
                            CityChildren.Add(city);
                            province.CityChildren = CityChildren;
                            var areaList = conn.Query<db_yhm_city>(strares).Where(t=>t.class_parent_id==cityitem.class_id).ToList();
                            List<AreaList> AreaChildren = new List<AreaList>();
                            foreach (var areaitem in areaList)
                            { 
                                AreaList area = new AreaList();
                                area.value = areaitem.class_id;
                                area.label = areaitem.class_name;
                                AreaChildren.Add(area);
                                city.CityChildren = AreaChildren;
                            }
                        }
                    }
                    Dto.ProvinceChildren = ProvinceChildren;
                   
                    Dto.flg = "1";
                    Dto.Msg = "成功";
                }
            }
            catch (Exception ex)
            {
                Dto.ProvinceChildren = null;
                Dto.flg = "-1";
                Dto.Msg = "错误:"+ex;
            }
            return Dto;
        }



        /// <summary>
        /// 获取省市县 优化后的
        /// </summary>
        /// <returns></returns>
        public CityDto GetCityInfo()
        {
            CityDto Dto = new CityDto();
            List<ProvinceList> ProvinceChildren = new List<ProvinceList>();
            try
            {
                string strprovince = "select class_id,class_parent_id,class_name,class_type,class_name as label,class_id as value from db_yhm_city where class_type=1";//省数据
                string strcity = "select class_id,class_parent_id,class_name,class_type,class_name as label,class_id as value from db_yhm_city where class_type=2";//市数据
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var provincelist = conn.Query<db_yhm_city>(strprovince).ToList();
                    foreach (var item in provincelist)
                    {
                        ProvinceList province = new ProvinceList();
                        province.label = item.class_name;
                        province.value = item.class_id;
                        ProvinceChildren.Add(province);
                        var citylist = conn.Query<db_yhm_city>(strcity).Where(t => t.class_parent_id == item.class_id).ToList();//获取市区
                        List<CityList> CityChildren = new List<CityList>();
                        foreach (var cityitem in citylist)
                        {
                            CityList city = new CityList();
                            city.value = cityitem.class_id;
                            city.label = cityitem.class_name;
                            CityChildren.Add(city);
                            province.CityChildren = CityChildren;
                            string strares = "select class_id, class_parent_id, class_name, class_type, class_name as label,class_id as value from db_yhm_city where class_type = 3 and class_parent_id = "+cityitem.class_id+"";
                            List<AreaList> AreaChildren = conn.Query<AreaList>(strares).ToList();
                            city.CityChildren = AreaChildren;
                        }
                    }
                    Dto.ProvinceChildren = ProvinceChildren;
                    Dto.flg = "1";
                    Dto.Msg = "成功";
                }
            }
            catch (Exception ex)
            {
                Dto.ProvinceChildren = null;
                Dto.flg = "-1";
                Dto.Msg = "错误:" + ex;
            }
            return Dto;
        }







        ///<summary>
        ///查看个人信息  余额和订单信息
        /// </summary>
        public PersonalInfoDto GetPersonalInfo(GetPersonalInfoPara para)
        {
            PersonalInfoDto dto = new PersonalInfoDto();
            
            try
            {
                string sql = "select * from saleorder where Company_ID='"+para.Company_ID+"'";
                string sqlAccountBalance = "select AccountBalance as Commission from cf_user where Company_ID='" + para.Company_ID + "'";
                decimal Balance = 0;
                string CustomerCompanyName = string.Empty;
                List<OrderInfo> info = new List<OrderInfo>();
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    Balance = conn.Query<OrderInfo>(sqlAccountBalance).Select(t => t.Commission).FirstOrDefault();
                    dto.Balance = Balance;
                    var list = conn.Query<OrderInfo>(sql).OrderByDescending(t=>t.AddTime).ToList();
                    foreach (var item in list)
                    {
                        OrderInfo order = new OrderInfo();
                        order.CardNum = item.CardNum;
                        order.CardPrice = item.CardPrice;
                        order.CardTypeName = item.CardTypeName;
                        order.CardXTName = item.CardXTName;
                        string CustomerCompanysql = "select CompanyName as  CustomerCompany from company where CompanyID='"+item.CustomerCompany+"'";
                        order.CustomerCompany= conn.Query<OrderInfo>(CustomerCompanysql).Select(t => t.CustomerCompany).FirstOrDefault();
                        order.Remark = item.Remark;
                        order.MonthNum = item.MonthNum;
                        order.OperatorName = item.OperatorName;
                        //order.OrderType = item.OrderType;
                        order.SetmealName = item.SetmealName;
                        order.TotalPrice = item.TotalPrice;
                        order.OrderNum = item.OrderNum;
                        order.AddTime = item.AddTime;
                        //order.OrderType = item.OrderType;
                        string sqltype = "select Status as OrderStatus from payinfo where OrderNum='" + item.OrderNum+"'";
                        order.OrderStatus = conn.Query<OrderInfo>(sqltype).Select(t => t.OrderStatus).FirstOrDefault();//0未支付  1已支付
                        info.Add(order);
                    }                    
                    dto.orderInfos = info;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }

        ///<summary>
        ///提现
        /// </summary>
        public Information Withdrawal(WithdrawalPara para)
        {
            Information info = new Information();
            DateTime time = DateTime.Now;
            string Remark = string.Empty;
            string ordernum = Unit.GetTimeStamp(DateTime.Now);
            try
            {
                //获取用户余额
                string sqlusermoney = "select AccountBalance as Money from cf_user where Company_ID='" + para.Company_ID+"'";
                string sqlcompanyname = "select CompanyName from company where CompanyID='"+para.Company_ID+"'";
                decimal AccountBalance = 0;
                string companyname = string.Empty;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    AccountBalance = conn.Query<WithdrawalPara>(sqlusermoney).Select(t => t.Money).FirstOrDefault();
                    companyname = conn.Query<Company>(sqlcompanyname).Select(t => t.CompanyName).FirstOrDefault();
                    if (para.Money > AccountBalance)
                    {
                        info.flg = "-1";
                        info.Msg = "提现金额不能大于账户余额！";
                        return info;
                    }
                    else
                    {
                        Remark = "提现" + para.Money + "元";
                        //提现订单
                        string adduserwithdrawal = "insert into saleorder(Company_ID,Extract,Remark,OrderNum,AddTime,CustomerCompany)values('" + para.Company_ID+"','"+para.Money+"','"+Remark+"','"+ordernum+"','"+time+"','"+para.Company_ID+"')";
                        //支付审核
                        string addpaysql = "insert into payinfo(Company_ID,PayCompany,Phone,Remark,OrderNum,BankCardNum,BankName,Addtime,PayUserName)" +
                            " values('"+para.Company_ID+"','"+companyname+"','"+para.Phone+"','"+Remark+"','"+ordernum+"','"+para.BankCardNum+"','"+para.BankName+"','"+time+"','"+para.UserName+"')";
                        conn.Execute(adduserwithdrawal);
                        conn.Execute(addpaysql);
                        info.flg = "1";
                        info.Msg = "提交成功!";
                    }
                    
                }
                //string addorderinfosql = "inse";
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:"+ex;
            }
            return info;
        }
    }
}