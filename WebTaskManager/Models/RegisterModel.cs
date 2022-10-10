using System.ComponentModel.DataAnnotations;

namespace WebTaskManager.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
