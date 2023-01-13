using System.ComponentModel.DataAnnotations;

namespace Baeun_Project.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Required, MaxLength(200)]
        public string Surname { get; set; }
        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, MaxLength(200)]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Passwords does not match!")]
        public string PasswordConfirm { get; set; }
    }
}
