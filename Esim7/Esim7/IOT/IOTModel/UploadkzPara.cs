using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.IOT.IOTModel
{
    public class UploadkzPara
    {
        public List<kzdata> kzdatas { get; set; }
    }
    public class kzdata
    {
        public string deviceid { get; set; }
        public string factory_apikey { get; set; }
        public string sta_mac { get; set; }
        public string sap_mac { get; set; }
        public string device_model { get; set; }
    }
}