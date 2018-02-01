using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class UpdateItemController : Controller
    {
        //GET ALL UPDATE ITEMS
        public ActionResult All()
        {
            ViewBag.Title = "UpdateItem";
            var dbase = new Models.CangsODEntities7();
            var allAL = dbase.UpdateItems
                       .Select(log => new { log.updateID,
                                            log.logDate,
                                            log.logQuantity,
                                            log.logPrice,
                                            log.itemID,
                                            log.employeeID
                                           }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        //POST METHOD: ADD
        [System.Web.Mvc.HttpPost]
        public ActionResult addUpdateItem()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities7();
            Models.UpdateItem update_item = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.UpdateItem>(tae);

            ctx.UpdateItems.Add(update_item);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(update_item.updateID.ToString());
        }


        /*  public ActionResult getUpdateID(int id)
          {
              var ctx = new Models.CangsODEntities6();
              var uID = ctx.UpdateItems.Where(a => a.updateID == id).FirstOrDefault();

              if (uID == null)
                  return Json(null, JsonRequestBehavior.AllowGet);

              return Json(uID, JsonRequestBehavior.AllowGet);

          }

          public ActionResult getLogDate(DateTime ldate)
          {
              var ctx = new Models.CangsODEntities6();
              var date = ctx.UpdateItems.Where(a => a.logDate == ldate).FirstOrDefault();

              if (date == null)
                  return Json(null, JsonRequestBehavior.AllowGet);

              return Json(date, JsonRequestBehavior.AllowGet);
          }

          public ActionResult getLogPrice(int money)
          {
              var ctx = new Models.CangsODEntities6();
              var lPrice = ctx.UpdateItems.Where(a => a.logPrice == money).FirstOrDefault();


              if (lPrice == null)
                  return Json(null, JsonRequestBehavior.AllowGet);

              return Json(lPrice, JsonRequestBehavior.AllowGet);
          }

          public ActionResult getLogQuantity(int quantity)
          {
              var ctx = new Models.CangsODEntities6();
              var qty = ctx.UpdateItems.Where(a => a.logQuantity == quantity).FirstOrDefault();

              if (qty == null)
                  return Json(null, JsonRequestBehavior.AllowGet);

              return Json(qty, JsonRequestBehavior.AllowGet);
          }

          public ActionResult getItemID(int id)
          {
              var ctx = new Models.CangsODEntities6();
              var item = ctx.UpdateItems.Where(a => a.itemID == id).FirstOrDefault();

              if (item == null)
                  return Json(null, JsonRequestBehavior.AllowGet);

              return Json(item, JsonRequestBehavior.AllowGet);
          }

          public ActionResult getEmployeeID(int id)
          {
              var ctx = new Models.CangsODEntities6();
              var eID = ctx.UpdateItems.Where(a => a.employeeID == id).FirstOrDefault();

              if (eID == null)
                  return Json(null, JsonRequestBehavior.AllowGet);

              return Json(eID, JsonRequestBehavior.AllowGet);
          }*/


    }
}