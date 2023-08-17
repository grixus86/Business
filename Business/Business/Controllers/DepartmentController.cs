using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Business.Logic;
using NugetBusiness.Models;

namespace Business.Controllers
{
    public class DepartmentController : Controller
    {
        //khai bao bien chi doc 
        private readonly string _connectionString;
        private readonly DepartmentLogic _departmentLogic;

        //
        public DepartmentController()
        {
            _connectionString = "server=.\\SQLEXPRESS;Database=Employee;Trusted_Connection=True;";
        }

        public DepartmentController(DepartmentLogic departmentLogic)
        {
            _departmentLogic = departmentLogic;
        }

        private List<DepartmentModel> GetExistingDepartments()
        {
            List<DepartmentModel> departments = new List<DepartmentModel>();
            return departments;
        }

        public IActionResult ShowListDepartment(DepartmentModel departmentList)
        {
            _departmentLogic.ShowListDepartment();
            return View(departmentList);
        }



        [HttpPost]
        public IActionResult AddDepartment(DepartmentModel department)
        {
            if (ModelState.IsValid)
            {
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
