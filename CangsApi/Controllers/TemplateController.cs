using System;
using System.Collections.Generic;
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
            var dbase = new Models.CangsODEntities13();
            var allAL = dbase
                       .Templates
                       .Select(temp => new { temp.templateID,
                                             temp.customerID
                                           }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        public ActionResult returnCustomerID(int id)
        {
            var dbase = new Models.CangsODEntities13();
            var returnID = dbase.Templates.Where(t => t.customerID == id)
                           .Select(t => new { t.customerID, t.templateID }).ToList();

            return Json(returnID, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult addTemplate()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();
            Models.Template template = Newtonsoft.Json.JsonConvert.DeserializeObject < Models.Template>(tae);

            ctx.Templates.Add(template);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(template.templateID.ToString());
        }
    }
}