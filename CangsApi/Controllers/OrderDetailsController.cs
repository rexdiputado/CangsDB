using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class OrderDetailsController : Controller
    {
        // GET: OrderDetails
        public ActionResult All()
        {
            ViewBag.Title = "orderDetails";
            var dbase = new Models.CangsODEntities7();
            var allAL = dbase
                       .OrderDetails
                       .Select(ordet => new { ordet.transID,
                                              ordet.ordetQuantity,
                                              ordet.ordetPrice,
                                              ordet.ordetSubtotal,
                                              ordet.orderID,
                                              ordet.itemID
                                             }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult addOrderDetails()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities7();
            Models.OrderDetail order_detail = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OrderDetail>(tae);

            ctx.OrderDetails.Add(order_detail);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(order_detail.transID.ToString());
        }
    }
}