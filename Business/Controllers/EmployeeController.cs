using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using Business.Models;

namespace Business.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString = "Server=DESKTOP-RJGQ1SN;Database=Employee;Trusted_Connection=True;";

        private List<string> GetExistingDepartments()
        {
            List<string> departments = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Name FROM Department";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departments.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return departments;
        }

        public IActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, FirstName, LastName, Email, Department FROM Staff";
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
                                Department = reader.GetString(4)
                            };
                            employeeList.Add(employee);
                        }
                    }
                }
            }
            return View(employeeList);
        }

        public IActionResult AddEmployee()
        {
            ViewBag.Departments = GetExistingDepartments();
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeModel employee)
        {
            ViewBag.Departments = GetExistingDepartments();
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Staff (FirstName, LastName, Email, Department) OUTPUT INSERTED.Id VALUES (@FirstName, @LastName, @Email, @Department);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@Department", employee.Department);
                        int newId = (int)command.ExecuteScalar();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            ViewBag.Departments = GetExistingDepartments();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, FirstName, LastName, Email, Department FROM Staff WHERE Id = @Id";
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
                                Department = reader.GetString(4)
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
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Staff SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Department = @Department WHERE Id = @Id;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", employee.Id);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@Department", employee.Department);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(employee);
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
            return RedirectToAction("Index");
        }
    }
}
