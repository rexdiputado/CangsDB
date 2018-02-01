using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class AccessLevelsController : Controller
    {
       //GET ALL ACCESS LEVELS
        public ActionResult Index()
        {
            ViewBag.Title = "AccessLevels";
            var dbase = new Models.CangsODEntities7();
            var allAL = dbase.AccessLevels
                        .Select(accLevels => new { accLevels.levelNum,
                                                  accLevels.levDescription })
                                                  .ToList();

            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        //POST METHOD: ADD
        [System.Web.Mvc.HttpPost]
        public ActionResult addAccessLevels()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities7();
            Models.AccessLevel acc_lev = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.AccessLevel>(tae);

            ctx.AccessLevels.Add(acc_lev);
            ctx.SaveChanges();

            Response.StatusCode = 200; //try catch if errpr return errpr stautis code

            return Content(acc_lev.levelNum.ToString());
        }

        
        /*public ActionResult getLevelNum(int num)
        {
            var ctx = new Models.CangsODEntities3();
            var lNum = ctx.AccessLevels.Where(a => a.levelNum == num).FirstOrDefault();

            if (lNum == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(lNum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLevDescription(string description)
        {
            var ctx = new Models.CangsODEntities3();
            var lDes = ctx.AccessLevels.Where(a => a.levDescription == description).FirstOrDefault();

            if (lDes == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(lDes, JsonRequestBehavior.AllowGet);
        }*/
    }
}
