using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Business.Models;
using Microsoft.AspNetCore.Authentication;

namespace Business.Controllers
{
    public class AccountController : Controller
    {

        private readonly string _connectionString = "server=DESKTOP-RJGQ1SN;Database=Employee;Trusted_Connection=True;";


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        private bool AuthenticateUser(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Staff WHERE Email = @Email AND PassWord = @PassWord";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PassWord", password);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        [HttpPost]
        public IActionResult Login(EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                bool isAuthenticated = AuthenticateUser(employee.Email, employee.Password);
                if (isAuthenticated)
                {
                    return RedirectToAction("Index", "Employee");
                }
                ModelState.AddModelError("", "Đăng nhập không thành công, vui lòng kiểm tra lại Email và mật khẩu.");
            }
            return View(employee);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}  
