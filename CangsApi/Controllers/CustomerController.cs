using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace CangsApi.Controllers
{
    public class CustomerController : Controller
    {
        //GET ALL CUSTOMER
        public ActionResult All()
        {
            ViewBag.Title = "customer";
            var dbase = new Models.CangsODEntities6();
            var allAL = dbase.Customers.Where(c => c.isDeleted == 0)
                       .Select(cus => new { cus.customerID,
                                            cus.cusPassword,
                                            cus.number,
                                            cus.address,
                                            cus.cusLastName,
                                            cus.cusMiddleName,
                                            cus.cusFirstName,
                                            cus.verificationCode
                                          }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet); }

        //POST METHOD: ADD
        [System.Web.Mvc.HttpPost]
        public ActionResult addCustomer()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            Models.Customer customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Customer>(tae);

            ctx.Customers.Add(customer);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(customer.customerID.ToString());
         }

        //PUT METHOD: DELETE
        [System.Web.Mvc.HttpPut]
        public ActionResult delete(int id)
        {
            var ctx = new Models.CangsODEntities6();
            var customer = ctx.Customers.Where(c => c.customerID== id).FirstOrDefault();

            if (customer != null)
            {

                customer.isDeleted = 1;
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

      
        public ActionResult editCustomer(int id = 0)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            Models.Customer customer = ctx.Customers.Find(id);

            if(customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        //POST METHOD: EDIT
        [System.Web.Mvc.HttpPost]
        public ActionResult editCustomer(Models.Customer customer)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Customer>(tae);

            if(ModelState.IsValid)
            {
                ctx.Entry(customer).State = EntityState.Modified;
                ctx.SaveChanges();
            }
                
            Response.StatusCode = 200;

            //return View(customer);
            return Content(customer.customerID.ToString());

        }

        /* //Override
         public ActionResult deleteCustomer(int id = 0)
         {
             var tae = Request.Form[0];
             var ctx = new Models.CangsODEntities4();
             Models.Customer customer = ctx.Customers.Find(id);

             if (customer == null)
             {
                 return HttpNotFound();
             }

             return View(customer);
         }

         //POST: delete
         [System.Web.Mvc.HttpPost]
         public ActionResult deleteCustomer(Models.Customer customer)
         {
             var tae = Request.Form[0];
             var ctx = new Models.CangsODEntities4();
             customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Customer>(tae);

             if (ModelState.IsValid)
             {
                 ctx.Entry(customer).State = EntityState.Modified;
                 ctx.Customers.Remove(customer);
                 ctx.SaveChanges();
             }

             Response.StatusCode = 200;

             return Content(customer.customerID.ToString());

         }*/

    }
}