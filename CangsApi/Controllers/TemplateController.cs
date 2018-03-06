using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult All()
        {
            ViewBag.Title = "template";
            var dbase = new Models.CangsODEntities14();
            var allAL = dbase
                       .Templates
                       .Select(temp => new
                       {
                           temp.templateID,
                           temp.customerID,
                           temp.templateName
                       }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        public ActionResult deleteTempDetails()
        {
            var req = Request.Form[0];
            var dbase = new Models.CangsODEntities14();
            Models.TemplateDetail temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.TemplateDetail>(req);
            var id = temp.templateID;
            var tempde = dbase.TemplateDetails.Where(i => i.templateID == id)
                      .Select(i => new {
                          i.tempDetailID,
                          i.temdeQuantity,
                          i.itemID,
                          i.templateID
                      }).ToList();

            if (ModelState.IsValid)
            {

                dbase.Entry(temp).State = EntityState.Modified;
                dbase.TemplateDetails.Remove(temp);
                dbase.SaveChanges();
            }

            return Content("deleted");
        }

        public ActionResult getTempDetails(int id)
        { 
            var dbase = new Models.CangsODEntities14();
            var tempde = dbase.TemplateDetails.Where(i => i.templateID == id)
                      .Select(i => new {
                          i.tempDetailID,
                          i.temdeQuantity,
                          i.itemID,
                          i.templateID
                      }).ToList();

            return Json(tempde, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult editTemplate()
        {
            
            var req = Request.Form[0];
            var dbase = new Models.CangsODEntities14();
            Models.Template temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Template>(req);
            var id = temp.templateID;
            var name = temp.templateName; 
            var delete = dbase.Templates.Where(i => i.templateID == id)
                      .Select(i => new {
                                          i.templateID,
                                          i.customerID,
                                          i.templateName
                                        }).ToList();

            if (ModelState.IsValid)
            {
                dbase.Entry(temp).State = EntityState.Modified;
                dbase.SaveChanges();
            }


            return Content("");
        }

        public ActionResult returnCustomerID(int id)
        {
            var dbase = new Models.CangsODEntities14();
            var returnID = dbase.Templates.Where(t => t.customerID == id)
                           .Select(t => new { t.customerID, t.templateID, t.templateName }).ToList();

            return Json(returnID, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult addTemplate()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities14();
            Models.Template template = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Template>(tae);

            ctx.Templates.Add(template);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(template.templateID.ToString());
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult deleteTemplate(Models.Template temp)
        {
            var template = Request.Form[0];
            var dbase = new Models.CangsODEntities14();
            temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Template>(template);
            var cat = dbase.Templates.Where(t => t.templateID == temp.templateID)
                      .Select(t => new
                      {
                          t.templateID,
                          t.templateName,
                          t.customerID
                      }).ToList();

            if (ModelState.IsValid)
            {

                dbase.Entry(temp).State = EntityState.Modified;
                dbase.Templates.Remove(temp);
                dbase.SaveChanges();
            }

            return Content(temp.templateID.ToString());
        }
    }
}