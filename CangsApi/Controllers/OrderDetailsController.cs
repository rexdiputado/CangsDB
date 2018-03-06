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
            var dbase = new Models.CangsODEntities14();
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
            var ctx = new Models.CangsODEntities14();

            Models.OrderDetail order_detail = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OrderDetail>(tae);
            var id = order_detail.itemID;
            var id2 = order_detail.orderID;
            
            Response.StatusCode = 200; //try catch if errpr return errpr stautis code
            ctx.OrderDetails.Add(order_detail);
            ctx.SaveChanges();
            var itemQty = new ItemController().updateItemQty(id, id2);
            var purchaseCount = new ItemController().updatePurchaseCount(id, id2);
            //var qty = new OrdersController().editOrder(id, id2);


            /* var item = new Models.Item();
             item.purchaseCountAllTime += 1;
             item.purchaseCountQuarter += 1;
             item.purchaseCountMonth += 1;
             item.purchaseCountYear += 1;
             item.itemQuantityStored -= 1;
             ctx.SaveChanges();*/

            return Content(" ");
        }

        public ActionResult returnOrderDetails(int id)
        {
            var dbase = new Models.CangsODEntities14();
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