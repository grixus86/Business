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
        private readonly StaffLogic _staffLogic;


        public IActionResult Index()
        {

            _staffLogic.ShowListStaff();
            return View();
        }

        private List<DepartmentModel> GetExistingDepartments()
        {
            List<DepartmentModel> departments = new List<DepartmentModel>();
            return departments;
        }


        private List<RoleModel> GetExistingRoles()
        {
            List<RoleModel> roles = new List<RoleModel>();
            return roles;
        }


        public bool IsEmailExists(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Staff WHERE Email = @Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        [HttpPost]
        public IActionResult AddStaff(StaffModel staff)
        {

            // Kiểm tra xem Email đã tồn tại trong database hay chưa
            if (IsEmailExists(staff.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại, vui lòng nhập lại.");

                // Nếu có lỗi, tạo toast message với loại "error" và nội dung lỗi
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Email đã tồn tại, vui lòng nhập lại.";
                if (ModelState.IsValid)
                {

                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Thêm nhân viên thành công.";
                    return RedirectToAction("Index");
                }
            }
            return View(staff);
        }


        [HttpGet]
        public IActionResult EditStaff(int id)
        {
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult EditStaff(StaffModel staff)
        {

            if (ModelState.IsValid)
            {
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Chỉnh sửa thành công!";
                return RedirectToAction("Index");
            }
            return View(staff);
        }


        [HttpPost]
        public IActionResult DeleteStaff(int id)
        {
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Xóa nhân viên thành công.";
            return RedirectToAction("Index");
        }
    }
}



