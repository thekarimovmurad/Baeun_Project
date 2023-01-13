using Baeun_Project.DAL;
using Baeun_Project.Models;
using Baeun_Project.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baeun_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProjectController : Controller
    {
        //private readonly AppDbContext db;
        //private readonly IWebHostEnvironment env;
        //public ProjectController(AppDbContext _db, IWebHostEnvironment _env)
        //{
        //    db = _db;
        //    env = _env;
        //}
        //public async Task<IActionResult> Index()
        //{
        //    return View(await db.Projects.ToListAsync());
        //}
        //public IActionResult Add()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Add(Project project)
        //{
        //    if (!ModelState.IsValid) return View();
        //    Project duplicate = await db.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);
        //    if (duplicate != null) return View();
        //    project.Image = await project.ImageFile.Upload(env.WebRootPath, @"img/projects");
        //    await db.Projects.AddAsync(project);
        //    await db.SaveChangesAsync();
        //    if (project.ImageFiles.Count() != 0)
        //    {
        //        foreach (IFormFile item in project.ImageFiles)
        //        {
        //            ProjectImage pImg = new ProjectImage()
        //            {
        //                Image = await item.Upload(env.WebRootPath, @"img/projects"),
        //                ProjectId = project.Id,
        //            };
        //            db.ProjectImages.Add(pImg);
        //        }
        //        await db.SaveChangesAsync();
        //    }
        //    return RedirectToAction("Index", "Project");
        //}
        //public async Task<IActionResult> Info(int? id)
        //{
        //    if (id == null) return RedirectToAction("Index", "Project");
        //    return View(await db.Projects.Include(x => x.ProjectImages).FirstOrDefaultAsync(x => x.Id == id));
        //}
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null) return RedirectToAction("Index", "Project");
        //    return View(await db.Projects.Include(x => x.ProjectImages).FirstOrDefaultAsync(x => x.Id == id));
        //}

        //public async Task<IActionResult> DeleteConfirmed(int? id)
        //{
        //    if (id == null) return RedirectToAction("Index", "Project");
        //    Project ProjectToDelete = await db.Projects.FindAsync(id);
        //    if (ProjectToDelete == null) return RedirectToAction("Index", "Project");
        //    db.Projects.Remove(ProjectToDelete);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index", "Project");
        //}
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null) return RedirectToAction("Index", "Project");
        //    return View(await db.Projects.Include(x => x.ProjectImages).FirstOrDefaultAsync(x => x.Id == id));
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit(Project project)
        //{
        //    if (!ModelState.IsValid) return View();
        //    if (project.ImageFile != null)
        //    {
        //        project.Image = await project.ImageFile.Upload(env.WebRootPath, @"img/projects");
        //    }
        //    db.Projects.Update(project);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index", "Project");
        //}
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProjectController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Projects.Include(p => p.ProjectImages);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Projects.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Name,Price,OffPercentage,StarCount,IsNew,ImageFiles")] Project project)
        {
            if (ModelState.IsValid)
            {
                if (project.ImageFiles != null && project.ImageFiles.Length > 0)
                {
                    _context.Add(project);
                    await _context.SaveChangesAsync();
                    foreach (IFormFile item in project.ImageFiles)
                    {
                        if (!item.IsImage())
                        {
                            ModelState.AddModelError("ImageFiles", item.FileName + "is not an image.");
                            _context.Projects.Remove(_context.Projects.Find(project.Id));
                            await _context.SaveChangesAsync();
                            return View(project);
                        }

                        if (!item.IsValidSize(500))
                        {
                            ModelState.AddModelError("ImageFiles", item.FileName + "is too big.");
                            _context.Projects.Remove(_context.Projects.Find(project.Id));
                            await _context.SaveChangesAsync();
                            return View(project);
                        }

                        ProjectImage pi = new ProjectImage();
                        pi.Image = await item.Upload(_env.WebRootPath, @"img\projects");
                        pi.ProjectId = project.Id;

                        await _context.ProjectImages.AddAsync(pi);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                ModelState.AddModelError("ImageFiles", "At least one image is required");
                return View(project);
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["ProjectImages"] = await _context.ProjectImages.Where(x => x.ProjectId == id).ToListAsync();

            var product = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,SubCategoryId,Price,OffPercentage,StarCount,IsNew,Id,ImageFiles")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (project.ImageFiles != null && project.ImageFiles.Length > 0)
                {
                    if (project.ImageFiles.ImagesAreValid())
                    {

                        List<ProjectImage> images = await _context.ProjectImages.Where(x => x.ProjectId == project.Id).ToListAsync();
                        foreach (ProjectImage item in images)
                        {
                            string filePath = Path.Combine(_env.WebRootPath, @"img\projects", item.Image);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            _context.ProjectImages.Remove(item);
                        }

                        foreach (IFormFile item in project.ImageFiles)
                        {
                            ProjectImage pi = new ProjectImage();
                            pi.Image = await item.Upload(_env.WebRootPath, @"uploads\products");
                            pi.ProjectId = project.Id;
                            await _context.ProjectImages.AddAsync(pi);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("ImageFiles", "Images are not valid.");
                        return View(project);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Projects.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
