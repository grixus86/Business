using System.Data;
using System.Data.SqlClient;
using NugetBusiness.Models;


namespace Business.Logic
{
    public class RoleLogic
    {
        private readonly string _connectionString;
        public RoleLogic(string _conectionString)
        {
            _conectionString = _connectionString;
        }

        public void GetExistingRoles(List<RoleModel> roles)
        {
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
        }

    }

}
