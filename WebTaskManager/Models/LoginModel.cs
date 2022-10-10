using System.ComponentModel.DataAnnotations;

namespace WebTaskManager.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
