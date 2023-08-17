using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using Business.Models;

namespace Business.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString;

        public EmployeeController()
        {
            _connectionString = "Server=.\\SQLEXPRESS;Database=Employee;Trusted_Connection=True;";
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
                            int employeeID = reader.GetInt32(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            string email = reader.GetString(3);
                            string department = reader.GetString(4);

                            var employee = new EmployeeModel
                            {
                                Id = employeeID,
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email,
                                Department = department
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
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeModel employee)
        {
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
    }
}