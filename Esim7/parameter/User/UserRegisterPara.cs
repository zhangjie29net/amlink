using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.User
{
    public class UserRegisterPara
    {
        public string CompanyName { get; set; }
        //public string Companyremarks { get; set; }
        public string CompanyPhone { get; set; }
        //public string CompanyAdress { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string Job { get; set; }
        public string VerificationCode { get; set; }
    }
}