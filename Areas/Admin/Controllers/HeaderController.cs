using Baeun_Project.DAL;
using Baeun_Project.Models;
using Baeun_Project.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Baeun_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HeaderController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment env;
        public HeaderController(AppDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Headers.ToListAsync());
        }
        public async Task<IActionResult> Info(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Header");
            return View(await db.Headers.FirstOrDefaultAsync(x => x.Id == id));
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Header header)
        {
            if (!ModelState.IsValid) return View();
            //Header duplicate = await db.Headers.FirstOrDefaultAsync(x => x.Id == header.Id);
            //if (duplicate != null) return View();
            if (!header.ImageFile.IsImage()) return View();
            if (!header.ImageFile.IsValidSize(100000)) return View();
            header.Image = await header.ImageFile.Upload(env.WebRootPath, @"img/slider");
            db.Headers.Add(header);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Header");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Header");
            return View(await db.Headers.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Header");
            Header HeaderDelete = await db.Headers.FindAsync(id);
            if (HeaderDelete == null) return RedirectToAction("Index", "Header");
            db.Headers.Remove(HeaderDelete);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Header");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Header");
            return View(await db.Headers.FirstOrDefaultAsync(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Header header)
        {
            if (!ModelState.IsValid) return View();
            if (header.ImageFile != null)
            {
                header.Image = await header.ImageFile.Upload(env.WebRootPath, @"img/slider");
            }
            db.Headers.Update(header);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Header");
        }
    }
}
