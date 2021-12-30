using Esim7.Action_JiHe_CardMess;
using Esim7.Action_KuCun;
using Esim7.Action_OneLink_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{
    public class TESTController : ApiController
    {




        public object Get() {


            return Action_Stock.GetHTTP();

        }

        public object Get1() {




            return Action_Onelink_New_CardMeaasge.GetFunctionalOpenQuery2("1440233541200");



        }


        public object Get2(string value) {





            return Action_NewSimlink_Card_Message.GetFunctionalOpenQuery(value);





        }

        public object Get3(string value) {

            return Action_Onelink_New_CardMeaasge.GetFunctionalOpen(value );


        }
    }
}
