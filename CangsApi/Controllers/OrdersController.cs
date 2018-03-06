using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class OrdersController : Controller
    {
        //GET ALL ORDERS
        public ActionResult All()
        {
            ViewBag.Title = "Order";
            var dbase = new Models.CangsODEntities14();
            //string[] statuses = { "Pending", "Verified", "Canceled", "Delivered" };
            var allAL = dbase
                       .Orders.Where(o => o.isDeleted == 0).OrderByDescending(s => s.orderStatus)
                       .Select(order => new { order.orderID,
                                              order.orderDate,
                                              order.orderTotal,
                                              order.orderStatus,
                                              order.orderRemarks,
                                              order.location,
                                              order.orderTime,
                                              order.packaging,
                                              order.customerID,
                                              order.cashTendered
                                            }).ToList();
            /*allAL.Sort((x, y) =>
            {
                return Array.IndexOf(statuses, x.orderStatus).CompareTo(Array.IndexOf(statuses, y.orderStatus));
            });*/


            return Json(allAL, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult returnQuantityStored()
        {
            var dbase = new Models.CangsODEntities14();
            var qty = dbase.Items.Select(q => new
                                        {
                                            q.itemID,
                                            q.itemQuantityStored
                                        }).ToList();

            return Json(qty, JsonRequestBehavior.AllowGet);
        }

       public ActionResult filterStatusPV()
        {
            var dbase = new Models.CangsODEntities14();
            var filter = dbase.Orders.Where(f => f.orderStatus == "pending"  || f.orderStatus == "verified").OrderByDescending(s => s.orderID).ThenBy(s => s.orderStatus)
                         .Select(o => new {
                             o.orderID,
                             o.orderDate,
                             o.orderTotal,
                             o.orderStatus,
                             o.orderRemarks,
                             o.location,
                             o.orderTime,
                             o.packaging,
                             o.customerID,
                             o.cashTendered
                         }).ToList();
          

            return Json(filter, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getHistory(int id)
        {
            var dbase = new Models.CangsODEntities14();
            int limit = 20;
            var getcustomer = dbase.Orders.Where(g => g.customerID == id).OrderByDescending(o => o.orderID).Take(limit)
                              .Select(c => new
                              {
                                  c.orderID,
                                  c.orderDate,
                                  c.orderTotal,
                                  c.orderStatus,
                                  c.orderRemarks,
                                  c.location,
                                  c.orderTime,
                                  c.packaging,
                                  c.customerID,
                                  c.cashTendered
                              }).ToList();

            return Json(getcustomer, JsonRequestBehavior.AllowGet);
        }

        public ActionResult filterStatusCD()
        {
            var dbase = new Models.CangsODEntities14();
            var filter = dbase.Orders.Where(f => f.orderStatus == "delivered" || f.orderStatus == "cancelled").OrderByDescending(s => s.orderStatus)
                         .Select(o => new {
                             o.orderID,
                             o.orderDate,
                             o.orderTotal,
                             o.orderStatus,
                             o.orderRemarks, 
                             o.location,
                             o.orderTime,
                             o.packaging,
                             o.customerID,
                             o.cashTendered
                         }).ToList();

            return Json(filter, JsonRequestBehavior.AllowGet);
        }

        public ActionResult returnDate()
        {
            var currentTime = DateTime.Now.ToString();
            //order.orderDate = DateTime.Now.ToString();

            return Json(currentTime, JsonRequestBehavior.AllowGet);

        }

        //POST METHOD: ADD
        [System.Web.Mvc.HttpPost]
        public ActionResult addOrder()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities14();
            Models.Order order = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Order>(tae);

           
                order.orderDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm tt");

                ctx.Orders.Add(order);
                ctx.SaveChanges();

                //Response.StatusCode = 200; //try catch if errpr return errpr stautis code

                return Content(order.orderID.ToString());
        }


        //PUT METHOD: DELETE
        [System.Web.Mvc.HttpPut]
        public ActionResult delete(int id)
        {
            var ctx = new Models.CangsODEntities14();
            var order = ctx.Orders.Where(c => c.orderID == id).FirstOrDefault();

            if (order != null)
            {

                order.isDeleted = 1;
                ctx.SaveChanges();

                Response.StatusCode = 200;
                return Content("Customer deleted.");
            }
            else
            {

                Response.StatusCode = 500;
                return Content("Customer not found.");
            }

        }

        //OVERRIDE
        public ActionResult editOrder(int id = 0)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities14();
            Models.Order order= ctx.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        //POST: edit
        [System.Web.Mvc.HttpPost]
        public ActionResult editOrder(Models.Order order)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities14();
            var ordet = new Models.OrderDetail();
           
            order = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Order>(tae);
            
                if (ModelState.IsValid)
                {
                    ctx.Entry(order).State = EntityState.Modified;
                    ctx.SaveChanges();
                }

            Response.StatusCode = 200;
            
            //return View(customer);
            return Content(order.orderID.ToString());

        }

       /* public ActionResult updatePurchaseCount(int id, int id2)
        {
            var dbase = new Models.CangsODEntities14();
            var find = dbase.Items.Where(i => i.itemID == id).FirstOrDefault();
            var qty = dbase.OrderDetails.Select(q => new
            {
                q.transID,
                q.ordetQuantity,
                q.ordetPrice,
                q.ordetSubtotal,
                q.orderID,
                q.itemID,
                q.itemName,
                q.itemDescription
            }).ToArray();

            for (int i = 0; i < qty.Length; i++)
            {
                Models.OrderDetail q = dbase.OrderDetails.Where(d => d.orderID == id2 && d.itemID == id).FirstOrDefault();
                find.itemQuantityStored += Int32.Parse(q.ordetQuantity);
                dbase.SaveChanges();
            }

            return Content("");

        }*/
    }
}