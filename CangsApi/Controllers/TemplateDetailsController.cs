using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class TemplateDetailsController : Controller
    {
        // GET: TemplateDetails
        public ActionResult All()
        {
            ViewBag.Title = "TemplateDetails";
            var dbase = new Models.CangsODEntities13();
            var allAL = dbase.TemplateDetails.Select(temde => new { temde.tempDetailID, temde.temdeQuantity, temde.itemID, temde.templateID }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult addTemplateDetails()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities13();
            Models.TemplateDetail temp_det= Newtonsoft.Json.JsonConvert.DeserializeObject<Models.TemplateDetail>(tae);

            ctx.TemplateDetails.Add(temp_det);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(temp_det.tempDetailID.ToString());
        }

        public ActionResult returnTemplateID(int id)
        {
            var dbase = new Models.CangsODEntities13();
            var tempid = dbase.TemplateDetails.Where(t => t.templateID == id)
                          .Select(t => new { t.tempDetailID, t.temdeQuantity, t.Item, t.templateID }).ToList();

            return Json(tempid, JsonRequestBehavior.AllowGet);
        }
    }
}