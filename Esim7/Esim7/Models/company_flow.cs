using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class company_flow
    {
        public int id { get; set; }
        public string CompanyID { get; set; }
        public string Flow { get; set; }
        public DateTime Date { get; set; }
    }
}