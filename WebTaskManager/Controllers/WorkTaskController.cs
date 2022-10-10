using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebTaskManager.Models;

namespace WebTaskManager.Controllers
{
    public class WorkTaskController : Controller
    {
        private AppConnectionContext _context;
        public WorkTaskController(AppConnectionContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult CreateTask()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(WorkTaskModel model)
        {
            if (ModelState.IsValid)
            {
                var task = await _context.WorkTasks.FirstOrDefaultAsync(t => t.Name == model.Name);
                if (task == null)
                {
                    task = new WorkTask() { Name = model.Name, Body = model.Body, Status = Status.Created, DateCreated = DateTime.Now };

                    _context.WorkTasks.Add(task);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("CreateTask");
                }
            }
            ModelState.AddModelError("", "Ошибка создания новой задачи");

            return View(model);
        }
    }
}
