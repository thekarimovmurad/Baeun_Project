using Baeun_Project.DAL;
using Baeun_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Baeun_Project.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext db;
        public ProjectController(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index(int id)
        {
            Project project = await db.Projects.Include(x => x.ProjectImages).FirstOrDefaultAsync(x => x.Id == id);
            return View(project);
        }
    }
}
