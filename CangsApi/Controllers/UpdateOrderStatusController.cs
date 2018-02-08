using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class UpdateOrderStatusController : Controller
    {
        //GET ALL UPDATED ORDER STATUSES
        public ActionResult All()
        {
            ViewBag.Title = "UpdateOrderStatus";
            var dbase = new Models.CangsODEntities13();
            var allAL = dbase.UpdateOrderStatus1
                       .Select(orstat => new { orstat.orstatStatus,
                                               orstat.statusID,
                                               orstat.orstatRemarks,
                                               orstat.orstatDate,
                                               orstat.employeeID,
                                               orstat.orderID
                                              }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        //POST METHOD: ADD
        [System.Web.Mvc.HttpPost]
        public ActionResult addUpdateStatus()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();
            Models.UpdateOrderStatus update_stat = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.UpdateOrderStatus>(tae);

            ctx.UpdateOrderStatus1.Add(update_stat);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(update_stat.statusID.ToString());
        }
    }
}