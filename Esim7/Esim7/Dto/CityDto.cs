using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
    /// <summary>
    /// 返回省市县
    /// </summary>
    public class CityDto: Information
    {
        public List<ProvinceList> ProvinceChildren { get; set; }
        
    }
    public class ProvinceList
    {
        public int value { get; set; }
        public string label { get; set; }
        public List<CityList> CityChildren { get; set; }
    }
    public class CityList
    {
        public int value { get; set; }
        public string label { get; set; }
        public List<AreaList> CityChildren { get; set; }
    }
    public class AreaList
    {
        public int value { get; set; }
        public string label { get; set; }
    }
  
}