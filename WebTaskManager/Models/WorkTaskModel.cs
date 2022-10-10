using System.ComponentModel.DataAnnotations;

namespace WebTaskManager.Models
{
    public class WorkTaskModel
    {
        [Required(ErrorMessage = "Введите название задачи")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Опишите задачу")]
        public string Body { get; set; }
    }
}
