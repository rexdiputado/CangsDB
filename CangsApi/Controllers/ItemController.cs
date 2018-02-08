using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;


namespace CangsApi.Controllers
{
    public class ItemController : Controller
    {

       
        // GET ALL ITEMS
        public ActionResult All()
        {
            ViewBag.Title = "Item";
            var dbase = new Models.CangsODEntities13();
            var allAL = dbase.Items.Where(c => c.isDeleted == 0).OrderBy(i => i.category)
                       .Select(item => new { item.itemID,
                                             item.itemName,
                                             item.itemQuantityStored,
                                             item.itemPrice,
                                             item.purchaseCountAllTime,
                                             item.picture,
                                             item.itemDescription,
                                             item.category,
                                             item.purchaseCountQuarter,
                                             item.purchaseCountMonth,
                                             item.purchaseCountYear
                       }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        public ActionResult returnItemName(int id)
        {
            var dbase = new Models.CangsODEntities13();
            var itemname = dbase.Items.Where(i => i.itemID == id)
                           .Select(i => new { i.itemName, i.itemDescription }).ToList();

            return Json(itemname, JsonRequestBehavior.AllowGet);
        }

        //ITEM SORTING BASED ON PURCHASE COUNT
       public ActionResult itemStatistics()
        {
            var ctx = new Models.CangsODEntities13();
            int limit = 20;
         
            var descending = ctx.Items.Where(i => i.isDeleted == 0)
                             .OrderByDescending(x => x.purchaseCountAllTime)
                             .Take(limit).Select(d => new { d.itemID,
                                                            d.itemName,
                                                            d.itemQuantityStored,
                                                            d.itemPrice,
                                                            d.purchaseCountAllTime,
                                                            d.picture,
                                                            d.itemDescription,
                                                            d.category,
                             }).ToList();
            
            return Json(descending, JsonRequestBehavior.AllowGet);
        }

        public ActionResult itemStatisticsYear()
        {
            var ctx = new Models.CangsODEntities13();
            int limit = 20;

            var descending = ctx.Items.Where(i => i.isDeleted == 0)
                             .OrderByDescending(x => x.purchaseCountYear)
                             .Take(limit).Select(d => new {
                                 d.itemID,
                                 d.itemName,
                                 d.itemQuantityStored,
                                 d.itemPrice,
                                 d.picture,
                                 d.itemDescription,
                                 d.category,
                                 d.purchaseCountYear
                             }).ToList();

            return Json(descending, JsonRequestBehavior.AllowGet);
        }

        public ActionResult itemStatisticsQuarter()
        {
            var ctx = new Models.CangsODEntities13();
            int limit = 20;

            var descending = ctx.Items.Where(i => i.isDeleted == 0)
                             .OrderByDescending(x => x.purchaseCountQuarter)
                             .Take(limit).Select(d => new {
                                 d.itemID,
                                 d.itemName,
                                 d.itemQuantityStored,
                                 d.itemPrice,
                                 d.picture,
                                 d.itemDescription,
                                 d.category,
                                 d.purchaseCountQuarter
                             }).ToList();

            return Json(descending, JsonRequestBehavior.AllowGet);
        }

        public ActionResult itemStatisticsMonth()
        {
            var ctx = new Models.CangsODEntities13();
            int limit = 20;

            var descending = ctx.Items.Where(i => i.isDeleted == 0)
                             .OrderByDescending(x => x.purchaseCountMonth)
                             .Take(limit).Select(d => new {
                                 d.itemID,
                                 d.itemName,
                                 d.itemQuantityStored,
                                 d.itemPrice,
                                 d.picture,
                                 d.itemDescription,
                                 d.category,
                                 d.purchaseCountMonth
                             }).ToList();

            return Json(descending, JsonRequestBehavior.AllowGet);
        }

        public ActionResult sortCategory()
        {
            var ctx = new Models.CangsODEntities13();
            var category = ctx.Items.OrderBy(c => c.category)
                           .Select(cat => new
                           {   cat.itemID,
                               cat.itemName,
                               cat.itemQuantityStored,
                               cat.itemPrice,
                               cat.purchaseCountAllTime,
                               cat.picture,
                               cat.itemDescription,
                               cat.category,
                               cat.purchaseCountQuarter,
                               cat.purchaseCountMonth,
                               cat.purchaseCountYear
                           }).ToList();

            return Json(category, JsonRequestBehavior.AllowGet);
        }
      
        //POST METHOD: ADD
       [System.Web.Mvc.HttpPost]
        public ActionResult addItem()
        {
            var tae = Request.Form[0];  
            var ctx = new Models.CangsODEntities13();
            
            Models.Item item = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Item>(tae);
        

            ctx.Items.Add(item);
            ctx.SaveChanges();
            Response.StatusCode = 200;

           return Content(item.itemID.ToString());
           
        }

        public ActionResult restoreItem()
        {
            var ctx = new Models.CangsODEntities13();
            var restore = ctx.Items.Where(r => r.isDeleted == 1)
                          .Select(item => new
                          {
                              item.itemID,
                              item.itemName,
                              item.itemQuantityStored,
                              item.itemPrice,
                              item.purchaseCountAllTime,
                              item.picture,
                              item.itemDescription,
                              item.category,
                              item.purchaseCountQuarter,
                              item.purchaseCountMonth,
                              item.purchaseCountYear
                          }).ToList();

            return Json(restore, JsonRequestBehavior.AllowGet);
        }

        //PUT METHOD: DELETE
        [System.Web.Mvc.HttpPut]
        public ActionResult deleteItem(int id)
        {
            var ctx = new Models.CangsODEntities13();
            var item = ctx.Items.Where(i => i.itemID == id).FirstOrDefault();

            if (item != null)
            {

                item.isDeleted = 1;
                ctx.SaveChanges();

                Response.StatusCode = 200;
                return Content("Item deleted.");
            }
            else
            {

                Response.StatusCode = 500;
                return Content("Item not found.");
            }

        }

        public ActionResult editOrder(int id = 0)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();
            Models.Item item= ctx.Items.Find(id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        //POST METHOD: EDIT
        [System.Web.Mvc.HttpPost]
        public ActionResult editItem(Models.Item item)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();
            item = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Item>(tae);

            if (ModelState.IsValid)
            {
                ctx.Entry(item).State = EntityState.Modified;
                ctx.SaveChanges();
            }

            Response.StatusCode = 200;

            return Content(item.itemID.ToString());

        }

        //POST METHOD: UPLOAD IMAGE
        [System.Web.Http.HttpPost]
        public ActionResult uploadFile()
        {
            var save = "";
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = System.Web.HttpContext.Current.Request;

            Console.Write("" + httpRequest);
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = System.Web.HttpContext.Current.Server.MapPath("~/UploadFile/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    save = postedFile.FileName;
                    
                   
                }
                
            }
            return Json(save, JsonRequestBehavior.AllowGet);
        }

        /*  public void convertImage()
        {
            //var i = new Models.CangsODEntities4();
            //var img = i.Items.Select(p => new { p.picture }).FirstOrDefault();
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities4();

            Models.Item item = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Item>(tae);
            var img = ctx.Items.Select(i => new { item.picture }).ToList();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(img);
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            byte[] arr = Convert.FromBase64String(base64);

            string jsonBack = Encoding.UTF8.GetString(arr);
            var accountBack = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Item>(jsonBack);


        }*/

        /* //Override
        public ActionResult deleteItem(int id = 0)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            Models.Item item = ctx.Items.Find(id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }


        //POST: delete
        [System.Web.Mvc.HttpPost]
        public ActionResult deleteOrder(Models.Item item)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            item = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Item>(tae);

            if (ModelState.IsValid)
            {

                ctx.Entry(item).State = EntityState.Modified;
                ctx.Items.Remove(item);
                ctx.SaveChanges();
            }

            Response.StatusCode = 200;

            return Content(item.itemID.ToString());

        }*/
    }
}