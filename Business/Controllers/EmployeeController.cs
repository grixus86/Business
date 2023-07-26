using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using Business.Models;

namespace Business.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString;

        // Constructor của EmployeeController
        public EmployeeController()
        {
            _connectionString = "Server=DESKTOP-RJGQ1SN;Database=Employee;Trusted_Connection=True;";
        }

        // Phương thức hiển thị danh sách nhân viên
        public IActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT Id, FirstName, LastName, Email FROM Staff";
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

                            var employee = new EmployeeModel
                            {
                                Id = employeeID,
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email
                            };

                            employeeList.Add(employee);
                        }
                    }
                }
            }

            // Truyền danh sách nhân viên vào View thông qua Model
            return View(employeeList);
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeModel Staff)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection("Server=DESKTOP-RJGQ1SN;Database=Employee;Trusted_Connection=True;"))
                {
                    connection.Open();

                    string query = "INSERT INTO Staff (FirstName, LastName, Email) OUTPUT INSERTED.Id VALUES (@FirstName, @LastName, @Email);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", Staff.FirstName);
                        command.Parameters.AddWithValue("@LastName", Staff.LastName);
                        command.Parameters.AddWithValue("@Email", Staff.Email);

                        int newId = (int)command.ExecuteScalar();
                        // Dùng giá trị 'newId' trong code của bạn nếu cần thiết.
                    }
                }

                return RedirectToAction("Index");
            }

            return View(Staff);
        }
    }
}