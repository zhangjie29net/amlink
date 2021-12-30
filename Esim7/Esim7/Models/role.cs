using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class role
    {
        public int roleID { get; set; }
        public string rolename { get; set; }
        public string  createUserId { get; set; }
        public string  permissionsid { get; set; }

        public permissions permissions { get; set; }

       
    }
}