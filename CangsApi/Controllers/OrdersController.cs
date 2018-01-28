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
            var dbase = new Models.CangsODEntities6();
            var allAL = dbase
                       .Orders.Where(o => o.isDeleted ==0)
                       .Select(order => new { order.orderID,
                                              order.orderDate,
                                              order.orderTotal,
                                              order.orderStatus,
                                              order.orderRemarks,
                                              order.location,
                                              order.orderTime,
                                              order.packaging,
                                              order.customerID
                                            }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        //POST METHOD: ADD
        [System.Web.Mvc.HttpPost]
        public ActionResult addOrder()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            Models.Order order = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Order>(tae);

            ctx.Orders.Add(order);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(order.orderID.ToString());
        }


        //PUT METHOD: DELETE
        [System.Web.Mvc.HttpPut]
        public ActionResult delete(int id)
        {
            var ctx = new Models.CangsODEntities6();
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
            var ctx = new Models.CangsODEntities6();
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
            var ctx = new Models.CangsODEntities6();
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
    }
}