using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class CustomerLogController : Controller
    {
        [System.Web.Mvc.HttpPost]
        public ActionResult customerLog()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();
            Models.CustomerLog cuslog = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.CustomerLog>(tae);

            ctx.CustomerLogs.Add(cuslog);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(cuslog.logID.ToString());
        }
    }
}