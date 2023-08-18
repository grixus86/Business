using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using NugetBusiness.Models;
using Business.Logic;

namespace Business.Controllers
{
    public class StaffController : Controller
    {

        private readonly string _connectionString = "server=.\\SQLEXPRESS;Database=Business;Trusted_Connection=True;";
        StaffLogic _staffLogic = new StaffLogic();
        public IActionResult Index()
        {

            var listStaff = _staffLogic.ShowListStaff();
            return View(listStaff);
        }


        public List<DepartmentModel> GetExistingDepartments()
        {
            var listDepartment = _staffLogic.GetExistingDepartments();
            return listDepartment;
        }

        public List<RoleModel> GetExistingRoles()
        {
            var listRoles = _staffLogic.GetExistingRoles();
            return listRoles;
        }






        public IActionResult AddStaff()
        {
            ViewBag.Departments = GetExistingDepartments();
            ViewBag.Roles = GetExistingRoles(); // Thêm danh sách roles vào ViewBag
            return View();
        }



        [HttpPost]
        public IActionResult AddStaff(StaffModel staff)
        {

            // Kiểm tra xem Email đã tồn tại trong database hay chưa
            if (_staffLogic.IsEmailExists(staff.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại, vui lòng nhập lại.");
                // Nếu có lỗi, tạo toast message với loại "error" và nội dung lỗi
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Email đã tồn tại, vui lòng nhập lại.";
                return View(staff);
            }

            if (ModelState.IsValid)
            {
                _staffLogic.AddStaff(staff);
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Thêm nhân viên thành công.";
                return RedirectToAction("Index");
            }
            return View(staff);
        }


        [HttpGet]
        public IActionResult EditStaff(int id)
        {
            ViewBag.Departments = GetExistingDepartments();
            ViewBag.Roles = GetExistingRoles(); // Thêm danh sách roles vào ViewBag

            var getStaff = _staffLogic.GetStaffById(id);
            if (getStaff != null)
            {
                return View("EditStaff", getStaff);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult EditStaff(StaffModel staff)
        {

            if (ModelState.IsValid)
            {

                _staffLogic.EditStaff(staff);
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Chỉnh sửa thành công!";
                return RedirectToAction("Index");
            }
            return View(staff);
        }


        [HttpPost]
        public IActionResult DeleteStaff(int id)
        {
            _staffLogic.DeleteStaff(id);
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Xóa nhân viên thành công.";
            return RedirectToAction("Index");
        }
    }
}



