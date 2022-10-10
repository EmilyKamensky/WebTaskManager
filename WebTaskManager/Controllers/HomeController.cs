using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTaskManager.Models;

namespace WebTaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AppConnectionContext _context;

        public HomeController(ILogger<HomeController> logger, AppConnectionContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetTask()
        {
            var idUser = await _context.Users.FirstOrDefaultAsync(x => x.Name == User.Identity.Name);

            return View(await _context.WorkTasks.Where(x => x.ExecutorId == idUser.Id).ToListAsync());
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> StartExecute(int? id)
        {
            if (id != null)
            {
                var task = await _context.WorkTasks.FirstOrDefaultAsync(t => t.Id == id);
                if (task != null)
                {
                    task.Status = Status.Taken;

                    _context.WorkTasks.Update(task);
                    await _context.SaveChangesAsync();

                }
            }
            return RedirectToAction("GetTask");
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> StopExecute(int? id)
        {
            if (id != null)
            {
                var task = await _context.WorkTasks.FirstOrDefaultAsync(t => t.Id == id);
                if (task != null)
                {
                    task.Status = Status.Completed;

                    _context.WorkTasks.Update(task);
                    await _context.SaveChangesAsync();

                }
            }
            return RedirectToAction("GetTask");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AssignTask()
        {
            ViewBag.WorkTasks = await _context.WorkTasks.ToListAsync();
            ViewBag.Users = await _context.Users.ToListAsync();

            return View();
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AppointTask(int? id, int? userid)
        {
            if (id != null && userid != null)
            {
                var task = await _context.WorkTasks.FirstOrDefaultAsync(t => t.Id == id);
                if (task != null)
                {
                    task.ExecutorId = userid;
                    task.Status = Status.Appointed;

                    _context.WorkTasks.Update(task);
                    await _context.SaveChangesAsync();

                }
            }
            return RedirectToAction("AssignTask");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ConfirmTask(int? id)
        {
            if (id != null)
            {
                var task = await _context.WorkTasks.FirstOrDefaultAsync(t => t.Id == id);
                if (task != null)
                {
                    task.Status = Status.Confirmed;

                    _context.WorkTasks.Update(task);
                    await _context.SaveChangesAsync();

                }
            }
            return RedirectToAction("AssignTask");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
