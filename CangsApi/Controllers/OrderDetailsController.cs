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
            var dbase = new Models.CangsODEntities13();
            var allAL = dbase
                       .OrderDetails
                       .Select(ordet => new { ordet.transID,
                                              ordet.ordetQuantity,
                                              ordet.ordetPrice,
                                              ordet.ordetSubtotal,
                                              ordet.orderID,
                                              ordet.itemID,
                                              ordet.itemName,
                                              ordet.itemDescription
                                             }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult addOrderDetails()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();

            var item = new Models.Item();
            item.purchaseCountAllTime += 1;
            item.purchaseCountQuarter += 1;
            item.purchaseCountMonth += 1;
            item.purchaseCountYear += 1;
            item.itemQuantityStored -= 1;

            Models.OrderDetail order_detail = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OrderDetail>(tae);
            ctx.OrderDetails.Add(order_detail);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code
            
            return Content(" ");
        }

        public ActionResult returnOrderDetails(int id)
        {
            var dbase = new Models.CangsODEntities13();
            var ret = dbase.OrderDetails.Where(r => r.orderID == id)
                     .Select( r => new { r.transID,
                                         r.ordetQuantity,
                                         r.ordetPrice,
                                         r.ordetSubtotal,
                                         r.orderID,
                                         r.itemID,
                                         r.itemName,
                                         r.itemDescription
                                       }).ToList();

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}