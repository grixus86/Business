using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string Password { get; set; }

        public string Roles { get; set; }
    }
}
