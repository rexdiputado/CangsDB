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
            var dbase = new Models.CangsODEntities14();
            var allAL = dbase.Items.Where(c => c.isDeleted == 0 && c.itemQuantityStored != 0).OrderBy(i => i.category)
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


        public ActionResult resetPurchaseCountYear()
        {
            var form = Request.Form[0];
            var dbase = new Models.CangsODEntities14();
            var item = new Models.Item();
            var reset = dbase.Items.Select(p => new {
                p.itemID,
                p.itemName,
                p.itemQuantityStored,
                p.itemPrice,
                p.picture,
                p.itemDescription,
                p.category,
                p.purchaseCountYear
            }).ToArray();

            for (int i = 0; i < reset.Length; i++)
            {
                Models.Item count = dbase.Items.Find(reset[i].itemID);
                count.purchaseCountYear = 0;
                dbase.SaveChanges();

            }

            return Content("True");

        }


        public ActionResult resetPurchaseCountQuarter()
        {
            var form = Request.Form[0];
            var dbase = new Models.CangsODEntities14();
            var item = new Models.Item();
            var reset = dbase.Items.Select(p => new {
                 p.itemID,
                p.itemName,
                p.itemQuantityStored,
                p.itemPrice,
                p.picture,
                p.itemDescription,
                p.category,
                p.purchaseCountQuarter
            }).ToArray();

            for (int i = 0; i < reset.Length; i++)
            {
                Models.Item count = dbase.Items.Find(reset[i].itemID);
                count.purchaseCountQuarter = 0;
                dbase.SaveChanges();

            }

            return Content("True");

        }


        public ActionResult resetPurchaseCountMonth()
        {
            var form = Request.Form[0];
            var dbase = new Models.CangsODEntities14();
            var item = new Models.Item();
            var reset = dbase.Items.Select(p => new {
                                                        p.itemID,
                                                        p.itemName,
                                                        p.itemQuantityStored,
                                                        p.itemPrice,
                                                        p.picture,
                                                        p.itemDescription,
                                                        p.category,
                                                        p.purchaseCountMonth
                                                     }).ToArray();

            for(int i = 0; i < reset.Length; i++)
            {
                Models.Item  count = dbase.Items.Find(reset[i].itemID);
                count.purchaseCountMonth = 0;
                dbase.SaveChanges();
                
            }

            return Content("True");

        }
        // [System.Web.Mvc.HttpPost]
        public ActionResult returnCategory()
        {
            var category = Request.Form[0];
            var dbase = new Models.CangsODEntities14();
            Models.Item retcat = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Item>(category);
            var cat = dbase.Items.Where(c => c.category == retcat.category)
                      .Select(c => new
                      {
                          c.itemID,
                          c.itemName,
                          c.itemDescription,
                          c.itemPrice,
                          c.itemQuantityStored,
                          c.picture
                      }).ToList();

            return Json(cat, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult updateItemQty(int id, int id2)
        {
            var dbase = new Models.CangsODEntities14();
            var find = dbase.Items.Where(i => i.itemID == id).FirstOrDefault();
            var qty = dbase.OrderDetails.Where(i => i.orderID == id2 && i.itemID == id).FirstOrDefault();

            if ((find != null && qty != null))
            {
                find.itemQuantityStored -= Int32.Parse(qty.ordetQuantity);
                dbase.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("failed");
            }

        }

        public ActionResult updatePurchaseCount(int id, int id2)
        {
            var dbase = new Models.CangsODEntities14();
            var find = dbase.Items.Where(i => i.itemID == id).FirstOrDefault();
            var qty = dbase.OrderDetails.Where(i => i.orderID == id2 && i.itemID == id).FirstOrDefault();

         
               
                if ((find !=null && qty != null ))
                {
                    find.purchaseCountAllTime += Int32.Parse(qty.ordetQuantity);
                    find.purchaseCountMonth += Int32.Parse(qty.ordetQuantity);
                    find.purchaseCountQuarter += Int32.Parse(qty.ordetQuantity);
                    find.purchaseCountYear += Int32.Parse(qty.ordetQuantity);
                    dbase.SaveChanges();

                    return Content("success");
                }
                else
                {
                    return Content("fail");
                }
            //var qty = this.getOrderQty();
           

        }

        public ActionResult returnItemName(int id)
        {
            var dbase = new Models.CangsODEntities14();
            var itemname = dbase.Items.Where(i => i.itemID == id)
                           .Select(i => new { i.itemName, i.itemDescription }).ToList();

            return Json(itemname, JsonRequestBehavior.AllowGet);
        }

        public ActionResult returnItem(int id)
        {
            var dbase = new Models.CangsODEntities14();
            var itemid = dbase.Items.Where(i => i.itemID == id)
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

            return Json(itemid, JsonRequestBehavior.AllowGet);
        }

        //ITEM SORTING BASED ON PURCHASE COUNT
        public ActionResult itemStatistics()
        {
            var ctx = new Models.CangsODEntities14();
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
            var ctx = new Models.CangsODEntities14();
            int limit = 20;

            var descending = ctx.Items.Where(i => i.isDeleted == 0 && i.purchaseCountYear != 0)
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
            var ctx = new Models.CangsODEntities14();
            int limit = 20;

            var descending = ctx.Items.Where(i => i.isDeleted == 0 && i.purchaseCountQuarter != 0)
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
            var ctx = new Models.CangsODEntities14();
            int limit = 20;

            var descending = ctx.Items.Where(i => i.isDeleted == 0 && i.purchaseCountMonth != 0)
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
            var ctx = new Models.CangsODEntities14();
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
            var ctx = new Models.CangsODEntities14();
            
            Models.Item item = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Item>(tae);
        

            ctx.Items.Add(item);
            ctx.SaveChanges();
            Response.StatusCode = 200;

           return Content(item.itemID.ToString());
           
        }

        public ActionResult restoreItem()
        {
            var ctx = new Models.CangsODEntities14();
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
            var ctx = new Models.CangsODEntities14();
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
            var ctx = new Models.CangsODEntities14();
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
            var ctx = new Models.CangsODEntities14();
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