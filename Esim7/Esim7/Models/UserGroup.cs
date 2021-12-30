using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class UserGroup
    {

        public User User { get; set; }

        public int UserGroupId { get; set; }


        public string UserGroupName { get; set; }

        public int UserId { get; set; }
        public int RoleId { get; set; }

    }
}