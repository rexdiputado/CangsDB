using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class AccessRightsController : Controller
    {
        //GET ALL ACCESS RIGHTS
        public ActionResult All()
        {
            ViewBag.Title = "AccessRights";
            var dbase = new Models.CangsODEntities6();
            var allAL = dbase.AccessRights
                       .Select(accRight => new { accRight.rightNum,
                                                 accRight.employeeID,
                                                 accRight.levelNum
                                               }).ToList();


            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        //GET RIGHTS
        public ActionResult getRights(int id)
        {
            var ctx = new Models.CangsODEntities6();
            var rights = ctx
                       .AccessRights.Where(r => r.employeeID == id)
                       .Select(accRight => new { accRight.levelNum}).ToList();

            return Json(rights, JsonRequestBehavior.AllowGet);
        }

      
        //POST METHOD: EDIT
        [System.Web.Mvc.HttpPost]
        public ActionResult editRights(Models.AccessRight accright)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            accright = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AccessRight>(tae);

            var edit = ctx.AccessRights.Where(e => e.employeeID == accright.employeeID && e.levelNum == accright.levelNum).FirstOrDefault();

            if (edit != null){
                    ctx.AccessRights.Remove(edit);
                    ctx.SaveChanges();
                    Response.StatusCode = 200;

                return Content("Removed");
            } else {
                ctx.AccessRights.Add(accright);
                ctx.SaveChanges();
                Response.StatusCode = 200;
                return Content("Added");
            }
            

        }

        //POST METHOD: ADD
        [System.Web.Mvc.HttpPost]
        public ActionResult addAccessRights()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            Models.AccessRight acc_right= Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AccessRight>(tae);

            ctx.AccessRights.Add(acc_right);
            ctx.SaveChanges();

            Response.StatusCode = 200; 

            return Content(acc_right.rightNum.ToString());
        }

        
         public ActionResult deleteAccessRights(int id = 0)
         {
             var tae = Request.Form[0];
             var ctx = new Models.CangsODEntities6();
             Models.AccessRight accright= ctx.AccessRights.Find(id);

             if (accright == null)
             {
                 return HttpNotFound();
             }

             return View(accright);
         }

        //PUT METHOD: DELETE
        [System.Web.Mvc.HttpPost]
         public ActionResult deleteAccessRights(Models.AccessRight accright)
         {
             var tae = Request.Form[0];
             var ctx = new Models.CangsODEntities6();
             accright= Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AccessRight>(tae);



             if (ModelState.IsValid)
             {

                 ctx.Entry(accright).State = EntityState.Modified;
                 ctx.AccessRights.Remove(accright);
                 ctx.SaveChanges();
             }

             Response.StatusCode = 200;

             return Content(accright.rightNum.ToString());

         }

        /* public ActionResult editRights(int id = 0)
       {
           var tae = Request.Form[0];
           var ctx = new Models.CangsODEntities4();
           Models.AccessRight acc = ctx.AccessRights.Find(id);

           if (acc == null)
           {
               return HttpNotFound();
           }

           return View(acc);
       }*/
    }
}