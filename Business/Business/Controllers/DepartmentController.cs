using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Business.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly string _connectionString;

        public DepartmentController()
        {
            _connectionString = "Server=DESKTOP-RJGQ1SN;Database=Employee;Trusted_Connection=True;";
        }

        public IActionResult Index()
        {
            List<DepartmentModel> departmentList = new List<DepartmentModel>();

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
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);

                            var department = new DepartmentModel
                            {
                                Id = id,
                                Name = name


                            };

                            departmentList.Add(department);
                        }
                    }
                }
            }

            return View(departmentList);
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
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Department (Name) OUTPUT INSERTED.Id VALUES (@Name);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", department.Name);

                        int newId = (int)command.ExecuteScalar();
                    }
                }

                return RedirectToAction("Index");
            }

            return View(department);
        }

        [HttpPost]
        public IActionResult DeleteDepartment(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Department WHERE Id = @Id;";
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
