using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace WebTaskManager.Models
{
    public class WorkTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public int? MasterId { get; set; }
        public int? ExecutorId { get; set; }
        public DateTime? DateCreated { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        [Display(Name = "Новая")]
        Created,
        [Display(Name = "Назначена на выполнение")]
        Appointed,
        [Display(Name = "Взята в работу")]
        Taken,
        [Display(Name = "Исполнитель сообщил о выполнении")]
        Completed,
        [Display(Name = "Выполнение подтверждено")]
        Confirmed
    }
}
