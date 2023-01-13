using Baeun_Project.DAL;
using Baeun_Project.Models;
using Baeun_Project.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Baeun_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment env;
        public AboutController(AppDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Abouts.FirstOrDefaultAsync());
        }
        public async Task<IActionResult> Edit(int? id)
        {
            await db.Abouts.FindAsync(id);
            return View(await db.Abouts.FirstOrDefaultAsync(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(About about)
        {
            if (!ModelState.IsValid) return View();
            if (about.ImageFile != null)
            {
                about.Image = await about.ImageFile.Upload(env.WebRootPath, @"img/slider");
            }
            db.Abouts.Update(about);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "About");
        }
    }
}
