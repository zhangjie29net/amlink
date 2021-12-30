using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.LargeFlow.LargeFlowModel
{
    /// <summary>
    /// 大流量卡API返回值接收实体类
    /// </summary>
    public class LargeFlowAPIModel
    {
    }
    public class LargeFlowDetailDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
        /// <summary>
        /// 请求成功
        /// </summary>
        public string message { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imei { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string card_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string carrier_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string access_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_need_verified { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_real_authentication { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_tested_active { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_force_stop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int traffic_test { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int traffic_test_used { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int traffic_test_left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string test_begin_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string test_end_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int traffic_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double traffic_use { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bind_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string power_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string online_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string activation_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Current_productsItem> current_products { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> next_products { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subscribe_histories { get; set; }
    }
    public class Current_productsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string product_number { get; set; }
        /// <summary>
        /// 联通 360G/360天
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string service_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string capacity_unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int product_capacity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cycle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cycle_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string effective_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string expiration_date { get; set; }
    }



    public class LargerFlowCardstatusDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataItem> data { get; set; }
        /// <summary>
        /// 请求成功
        /// </summary>
        public string message { get; set; }
    }
    public class DataItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
    }
}