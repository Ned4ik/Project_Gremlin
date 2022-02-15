using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Gremlin.Data;
using Project_Gremlin.Models;
using Project_Gremlin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Controllers
{
    public class LoreController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public LoreController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnviroment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Lore> loreList = _db.Lore;
            return View(loreList);
        }

        public IActionResult AddLore()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLore(Lore lore)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnviroment.WebRootPath;
                string upload = webRootPath + ENV.LorePath;            
                string fileName = Guid.NewGuid().ToString();
                string extentions = Path.GetExtension(files[0].FileName);
               
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extentions), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                lore.ImageTitle = fileName + extentions;
                _db.Lore.Add(lore);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lore);
        }


        public IActionResult EditLore(int? Id)
        {
            LoreVM loreVM = new LoreVM()
            {
                Lore = new Lore()
            };
            if (Id == null)
            {
                return View(loreVM);
            }
            else
            {
                loreVM.Lore = _db.Lore.Find(Id);
                if (loreVM.Lore == null)
                {
                    return NotFound();
                }
                return View(loreVM);
            }


        }

        [HttpPost]
        public IActionResult EditLore(LoreVM loreVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                string webRootPath = _webHostEnviroment.WebRootPath;
                string upload = webRootPath + ENV.LorePath;
                string fileName = Guid.NewGuid().ToString();
                string extentions = Path.GetExtension(files[0].FileName);

                var formObject = _db.Lore.AsNoTracking().FirstOrDefault(u => u.Id == loreVM.Lore.Id);
                if (files.Count > 0)
                {
                    var oldFile = Path.Combine(upload, formObject.ImageTitle);


                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extentions), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    loreVM.Lore.ImageTitle = fileName + extentions;
                }
                else
                {
                    loreVM.Lore.ImageTitle = formObject.ImageTitle;
                }
                _db.Lore.Update(loreVM.Lore);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loreVM);
        }


        public IActionResult DeleteLore(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Lore lore = _db.Lore.FirstOrDefault(u => u.Id == id);
            if (lore == null)
            {
                return NotFound();
            }
            return View(lore);
        }

        [HttpPost, ActionName("DeleteLore")]
        public IActionResult Delete(int? id)
        {
            var lore = _db.Lore.Find(id);
            if (lore == null)
            {
                return NotFound();
            }
            string upload = _webHostEnviroment.WebRootPath + ENV.characterPath;

            var oldFile = Path.Combine(upload, lore.ImageTitle);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            _db.Lore.Remove(lore);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult LoreDetails(int id)
        {
            LoreDetailsVM loreDetailsVM = new LoreDetailsVM()
            {
                Lore = _db.Lore.Where(u => u.Id == id).FirstOrDefault()
            };
            return View(loreDetailsVM);
        }
    }
   
}

