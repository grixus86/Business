using System.Data.SqlClient;
using NugetBusiness.Models;
namespace Business.Logic
{
    public class DepartmentLogic 
    {
        private readonly string _connectionString;
        public DepartmentLogic(string _conectionString)
        {
            _conectionString = _connectionString;
        }

        public void GetExistingDepartments(List<DepartmentModel> departments)
        {
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
        }

        public void ShowListDepartment()
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
        }


        public void AddDepartment(DepartmentModel department)
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
        }


        public void DeleteDepartment(int id)
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
        }


        public void GetExitsDepartment()
        {

        }
    }


}
