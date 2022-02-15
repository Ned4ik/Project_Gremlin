using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project_Gremlin.Data;
using Project_Gremlin.Models;
using Project_Gremlin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment webHostEnviroment)
        {
            _logger = logger;
            _db = db;
            _webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                MiniHistories = _db.MiniHistory
            };
            return View(homeVM);
        }

        public IActionResult AddHistory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddHistory(MiniHistory miniHistory)
        {
           if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnviroment.WebRootPath;

                string upload = webRootPath + ENV.miniHistoryPath;
                string fileName = Guid.NewGuid().ToString();
                string extentions = Path.GetExtension(files[0].FileName);

                using(var fileStream = new FileStream(Path.Combine(upload, fileName + extentions), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                miniHistory.historyImage = fileName + extentions;
                _db.MiniHistory.Add(miniHistory);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(miniHistory);
        }

        public IActionResult EditHistory(int? Id)
        {
            MiniHistoryVM miniHistoryVM = new MiniHistoryVM()
            {
                MiniHistory = new MiniHistory()
            };
            if (Id == null)
            {
                return View(miniHistoryVM);
            }
            else
            {
                miniHistoryVM.MiniHistory = _db.MiniHistory.Find(Id);
                if (miniHistoryVM.MiniHistory == null)
                {
                    return NotFound();
                }
                return View(miniHistoryVM);
            }
            
            
        }

        [HttpPost]
        public IActionResult EditHistory(MiniHistoryVM miniHistoryVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnviroment.WebRootPath;

                string upload = webRootPath + ENV.miniHistoryPath;
                string fileName = Guid.NewGuid().ToString();
                string extentions = Path.GetExtension(files[0].FileName);
                var formObject = _db.MiniHistory.AsNoTracking().FirstOrDefault(u => u.Id == miniHistoryVM.MiniHistory.Id);
                if (files.Count > 0)
                {
                    var oldFile = Path.Combine(upload, formObject.historyImage);
                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extentions), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    miniHistoryVM.MiniHistory.historyImage = fileName + extentions;
                }
                else
                {
                    miniHistoryVM.MiniHistory.historyImage = formObject.historyImage;
                }
                _db.MiniHistory.Update(miniHistoryVM.MiniHistory);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }    
            return View(miniHistoryVM);
        }

        public IActionResult DeleteHistory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            MiniHistory miniHistory = _db.MiniHistory.FirstOrDefault(u => u.Id == id);
            if (miniHistory == null)
            {
                return NotFound();
            }
            return View(miniHistory);
        }

        [HttpPost, ActionName("DeleteHistory")]
        public IActionResult Delete(int? id)
        {
            var history = _db.MiniHistory.Find(id);
            if (history == null)
            {
                return NotFound();
            }
            string upload = _webHostEnviroment.WebRootPath + ENV.miniHistoryPath;
            var oldFile = Path.Combine(upload, history.historyImage);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            _db.MiniHistory.Remove(history);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult HistoryDetails(int id)
        {
            HistoryDetailsVM historyDetailsVM = new HistoryDetailsVM()
            {
                MiniHistory = _db.MiniHistory.Where(u => u.Id == id).FirstOrDefault()
            };
            return View(historyDetailsVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
