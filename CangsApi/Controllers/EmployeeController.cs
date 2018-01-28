using CangsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CangsApi.Controllers
{
    public class EmployeeController : Controller
    {
        // GET ALLA EMPLOYEE
        public ActionResult All ()
        {
            ViewBag.Title = "Employee";
            var dbase = new Models.CangsODEntities6();
            var allAL = dbase.Employees.Where(e => e.isDeleted == 0)
                       .Select(emp => new { emp.employeeID,
                                            emp.empPassword,
                                            emp.empType,
                                            emp.empLastName,
                                            emp.empMiddleName,
                                            emp.empFirstName
                                          }).ToList();


            return Json(allAL, JsonRequestBehavior.AllowGet);
        }

        //POST METHOD: ADD
        [System.Web.Mvc.HttpPost]
        public ActionResult addEmployee()
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            Models.Employee emp = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Employee>(tae);

            ctx.Employees.Add(emp);
            ctx.SaveChanges();

            Response.StatusCode = 200;

            return Content(emp.employeeID.ToString());
        }
   
        //GET ONE EMPLOYEE FOR RIGHTS
        public ActionResult getOneEmployee(int id)
        {
            var ctx = new Models.CangsODEntities6();
            var get = ctx.Employees.Where(e => e.employeeID == id)
                      .Select(emp => new { emp.empFirstName,
                                           emp.empLastName,
                                           emp.empMiddleName,
                                           emp.empPassword,
                                           emp.empType
                                          }).ToList();

            return Json(get, JsonRequestBehavior.AllowGet);
        }

        //PUT METHOD: DELETE
        [System.Web.Mvc.HttpPut]
        public ActionResult delete(int id)
        {
            var ctx = new Models.CangsODEntities6();
            var employee = ctx.Employees.Where(e => e.employeeID == id).FirstOrDefault();

            if (employee != null){

                employee.isDeleted = 1;
                ctx.SaveChanges();

                Response.StatusCode = 200;
                return Content("Employee deleted.");
            } else {

                Response.StatusCode = 500;
                return Content("Employee not found.");
            }
            
         } 

        public ActionResult editEmployee(int id = 0)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            Models.Employee employee = ctx.Employees.Find(id);

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        //POST METHOD: EDIT
        [System.Web.Mvc.HttpPost]
        public ActionResult editEmployee(Models.Employee employee)
        {
            var tae = Request.Form[0];
            var ctx = new Models.CangsODEntities6();
            employee = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Employee>(tae);

            if (ModelState.IsValid)
            {
                ctx.Entry(employee).State = EntityState.Modified;
                ctx.SaveChanges();
            }

            Response.StatusCode = 200;

            return Content(employee.employeeID.ToString());

        }
    }
}