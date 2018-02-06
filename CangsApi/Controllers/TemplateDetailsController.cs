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
            var dbase = new Models.CangsODEntities10();
            var allAL = dbase.TemplateDetails.Select(temde => new { temde.tempDetailID, temde.temdeQuantity, temde.itemID, temde.templateID }).ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult addTemplateDetails()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities10();
            Models.TemplateDetail temp_det= Newtonsoft.Json.JsonConvert.DeserializeObject<Models.TemplateDetail>(tae);

            ctx.TemplateDetails.Add(temp_det);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(temp_det.tempDetailID.ToString());
        }
    }
}