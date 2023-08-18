using System.Data.SqlClient;
using System.Web.Mvc;
using NugetBusiness.Models;

namespace Business.Logic
{
    public class StaffLogic
    {
        private readonly string _connectionString = "server=.\\SQLEXPRESS;Database=Business;Trusted_Connection=True;";

        public List<StaffModel> ShowListStaff()
        {
            List<StaffModel> staffList = new List<StaffModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Staff.Id, Staff.FirstName, Staff.LastName, Staff.Email, Staff.DepartmentId, Department.Name AS DepartmentName, Staff.RoleId, Roles.Name AS RoleName " +
                               "FROM Staff, Department, Roles " +
                               "WHERE Staff.DepartmentId = Department.Id AND Staff.RoleId = Roles.Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var staff = new StaffModel
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
                            staffList.Add(staff);
                        }
                    }
                }
            }
            return staffList;
        }

        public List<DepartmentModel> GetExistingDepartments()
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
                            var department = new DepartmentModel()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                            };
                            departmentList.Add(department);
                        }
                    }
                }
            }
            return departmentList;
        }


        public List<RoleModel> GetExistingRoles()
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

        public void AddStaff(StaffModel staff)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Staff (FirstName, LastName, Email, DepartmentId, Password, RoleId) VALUES (@FirstName, @LastName, @Email, @DepartmentId, @Password, @RoleId);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", staff.FirstName);
                    command.Parameters.AddWithValue("@LastName", staff.LastName);
                    command.Parameters.AddWithValue("@Email", staff.Email);
                    command.Parameters.AddWithValue("@DepartmentId", staff.DepartmentId);
                    command.Parameters.AddWithValue("@Password", staff.Password);
                    command.Parameters.AddWithValue("@RoleId", staff.RoleId);
                    command.ExecuteNonQuery();
                }
            }


        }



        public StaffModel GetStaffById(int id)
        {
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
                            var staff = new StaffModel
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                DepartmentId = reader.GetInt32(4),
                                RoleId = reader.GetInt32(5)
                            };
                            return staff;
                        }

                    }
                }
            }
            return null;
        }

        public void EditStaff(StaffModel staff)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Staff SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DepartmentId = @DepartmentId, RoleId = @RoleId WHERE Id = @Id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", staff.Id);
                    command.Parameters.AddWithValue("@FirstName", staff.FirstName);
                    command.Parameters.AddWithValue("@LastName", staff.LastName);
                    command.Parameters.AddWithValue("@Email", staff.Email);
                    command.Parameters.AddWithValue("@DepartmentId", staff.DepartmentId);
                    command.Parameters.AddWithValue("@RoleId", staff.RoleId);
                    command.ExecuteNonQuery();
                }
            }

        }

        public void DeleteStaff(int id) 
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
        }



    }
}
