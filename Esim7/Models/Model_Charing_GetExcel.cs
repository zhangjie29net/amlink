using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class Model_Charing_GetExcel
    {

       public  List<Mesage> Card { get; set; }

        public string  CompanyID{ get; set; }



    }






    public class Mesage {


        public string ICCID { get; set; }
        public string  ActivateDate { get; set; }
        public string  EndDate { get; set; }
    }
}