using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Business.Logic;
using NugetBusiness.Models;

namespace Business.Controllers
{
    public class DepartmentController : Controller
    {
        //khai bao bien chi doc 
        private readonly string _connectionString = "server=.\\SQLEXPRESS;Database=Business;Trusted_Connection=True;";
        DepartmentLogic _departmentLogic = new DepartmentLogic();


        public IActionResult Index()
        {
             var listDepartment = _departmentLogic.ShowListDepartment();
            return View(listDepartment);
        }


        public IActionResult AddDepartment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDepartment(DepartmentModel department)
        {
            if (ModelState.IsValid)
            {
                _departmentLogic.AddDepartment(department);
                return RedirectToAction("Index");
            }

            return View(department);
        }



        [HttpPost]
        public IActionResult DeleteDepartment(int id)
        {
            _departmentLogic.DeleteDepartment(id);
            return RedirectToAction("Index");
        }


    }
}
