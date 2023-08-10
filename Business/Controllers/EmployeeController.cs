﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using Business.Models;

namespace Business.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString = "server=.\\SQLEXPRESS;Database=Employee;Trusted_Connection=True;";



        public IActionResult Index() 
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Staff.Id, Staff.FirstName, Staff.LastName, Staff.Email, Staff.DepartmentId, Department.Name AS DepartmentName, Staff.RoleId, Roles.Name AS RoleName " +
                               "FROM Staff INNER JOIN Department ON Staff.DepartmentId = Department.Id INNER JOIN Roles ON Staff.RoleId = Roles.Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employee = new EmployeeModel
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                DepartmentId = reader.GetInt32(4),
                                DepartmentName = reader.GetString(5),
                                RoleId = reader.GetInt32(6),
                                RoleName = reader.GetString(7)
                            };
                            employeeList.Add(employee);
                        }
                    }
                }
            }
            return View(employeeList);
        }
        private List<DepartmentModel> GetExistingDepartments()
        {
            List<DepartmentModel> departments = new List<DepartmentModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name FROM Department";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var department = new DepartmentModel()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                            };
                            departments.Add(department);
                        }
                    }
                }
            }
            return departments;
        }


        private List<RoleModel> GetExistingRoles()
        {
            List<RoleModel> roles = new List<RoleModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name FROM Roles";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var role = new RoleModel()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                            };
                            roles.Add(role);
                        }
                    }
                }
            }
            return roles;
        }


        public IActionResult AddEmployee()
        {
            ViewBag.Departments = GetExistingDepartments();
            ViewBag.Roles = GetExistingRoles(); // Thêm danh sách roles vào ViewBag
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeModel employee)
        {
            ViewBag.Departments = GetExistingDepartments();
            ViewBag.Roles = GetExistingRoles();

            // Kiểm tra xem Email đã tồn tại trong database hay chưa
            if (IsEmailExists(employee.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại, vui lòng nhập lại.");

                // Nếu có lỗi, tạo toast message với loại "error" và nội dung lỗi
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Email đã tồn tại, vui lòng nhập lại.";
                return View(employee);
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Staff (FirstName, LastName, Email, DepartmentId, Password, RoleId) VALUES (@FirstName, @LastName, @Email, @DepartmentId, @Password, @RoleId);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
                        command.Parameters.AddWithValue("@Password", employee.Password);
                        command.Parameters.AddWithValue("@RoleId", employee.RoleId);
                        command.ExecuteNonQuery();
                    }
                }
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Thêm nhân viên thành công.";
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            ViewBag.Departments = GetExistingDepartments();
            ViewBag.Roles = GetExistingRoles();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, FirstName, LastName, Email, DepartmentId, RoleId FROM Staff WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var employee = new EmployeeModel
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                DepartmentId = reader.GetInt32(4),
                                RoleId = reader.GetInt32(5)
                            };
                            return View(employee);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditEmployee(EmployeeModel employee)
        {
            ViewBag.Departments = GetExistingDepartments();
            ViewBag.Roles = GetExistingRoles();

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Staff SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DepartmentId = @DepartmentId, RoleId = @RoleId WHERE Id = @Id;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", employee.Id);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
                        command.Parameters.AddWithValue("@RoleId", employee.RoleId);
                        command.ExecuteNonQuery();
                    }
                }
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Chỉnh sửa thành công!";
                return RedirectToAction("Index");
            }
            return View(employee);
        }


        private bool IsEmailExists(string email)
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
        public IActionResult DeleteEmployee(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Staff WHERE Id = @Id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Xóa nhân viên thành công.";
            return RedirectToAction("Index");
        }
    }
}