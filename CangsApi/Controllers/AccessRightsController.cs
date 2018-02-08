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
            var dbase = new Models.CangsODEntities13();
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
            var ctx = new Models.CangsODEntities13();
            var rights = ctx.AccessRights.Where(r => r.employeeID == id)
                        .Select(accRight => new { accRight.levelNum}).ToList();

            return Json(rights, JsonRequestBehavior.AllowGet);
        }

      
        //POST METHOD: EDIT
        [System.Web.Mvc.HttpPost]
        public ActionResult editRights(Models.AccessRight accright)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();
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
            var ctx = new Models.CangsODEntities13();
            Models.AccessRight acc_right= Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AccessRight>(tae);

            ctx.AccessRights.Add(acc_right);
            ctx.SaveChanges();

            Response.StatusCode = 200; 

            return Content(acc_right.rightNum.ToString());
        }

        /*[System.Web.Mvc.HttpPost]
        public ActionResult deleteRights()
        {
            var req = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
           Models.AccessRight right = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AccessRight>(req);

            var rights = ctx.AccessRights.Where(r => r.employeeID == right.employeeID)
                         .Select(accRight => new { accRight.levelNum }).FirstOrDefault();

           // ctx.Entry(rights).State = EntityState.Modified;
            ctx.AccessRights.Remove(right);
            ctx.SaveChanges();

            return Content("Deleted");
        }*/

        public ActionResult deleteAccessRights(int id)
        {
            //var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();
            var sel = ctx.AccessRights.Where(e => e.employeeID == id);

            var rights = ctx.AccessRights.Where(r => r.employeeID == id).Select(accRight => new { accRight.rightNum }).ToArray();


           for(int i = 0; i < rights.Length; i++)
            {
                Models.AccessRight acc = ctx.AccessRights.Find(rights[i].rightNum);
                ctx.AccessRights.Remove(acc);
                ctx.SaveChanges();
            }

            // var remove = ctx.AccessRights.Where(e => e.employeeID == id);



            return Content("");
        }

       //PUT METHOD: DELETE
       /*[System.Web.Mvc.HttpPost]
        public ActionResult deleteAccessRights(Models.AccessRight accright)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            accright= Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AccessRight>(tae);

           var rights = ctx.AccessRights.Where(r => r.employeeID == accright.employeeID)
                         .Select(accRight => new { accRight.levelNum }).FirstOrDefault();

            if (ModelState.IsValid)
            {

                ctx.Entry(accright).State = EntityState.Modified;
                ctx.AccessRights.Remove(accright);
                ctx.SaveChanges();
            }

            Response.StatusCode = 200;

            return Content("Deleted");

        }*/

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