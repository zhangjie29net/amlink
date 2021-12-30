using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class permissions
    {
        public int permissionsID { get; set; }
        public string permissionsname { get; set; }
        public int permissions_RoleID { get; set; }
        public int actiongroupId { get; set; }



    }
}