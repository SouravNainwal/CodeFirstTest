using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models
{
    public class loginT
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="password did not match")]
        public string ConfirmPassword { get; set; }
    }
}
