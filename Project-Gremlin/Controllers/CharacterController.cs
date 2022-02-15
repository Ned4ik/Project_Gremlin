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
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public CharacterController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnviroment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Character> characterList = _db.Character;
            return View(characterList);
        }

        public IActionResult AddCharacter()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCharacter(Character character)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var secondfiles = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnviroment.WebRootPath;

                string upload = webRootPath + ENV.characterPath;
                string Secondupload = webRootPath + ENV.characterPath;
                string fileName = Guid.NewGuid().ToString();
                string secondFileName = Guid.NewGuid().ToString();
                string extentions = Path.GetExtension(files[0].FileName);
                string secondExtentions = Path.GetExtension(secondfiles[1].FileName);


                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extentions), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                using (var SecondfileStream = new FileStream(Path.Combine(Secondupload, secondFileName + secondExtentions), FileMode.Create))
                {
                    secondfiles[1].CopyTo(SecondfileStream);
                }
                character.imageTitle = fileName + extentions;
                character.characterImage = secondFileName + secondExtentions;
                _db.Character.Add(character);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(character);
        }


        public IActionResult EditCharacter(int? Id)
        {
           CharacterVM characterVM = new CharacterVM()
            {
                Character = new Character()
            };
            if (Id == null)
            {
                return View(characterVM);
            }
            else
            {
                characterVM.Character = _db.Character.Find(Id);
                if (characterVM.Character == null)
                {
                    return NotFound();
                }
                return View(characterVM);
            }


        }

        [HttpPost]
        public IActionResult EditCharacter(CharacterVM characterVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var secondfiles = HttpContext.Request.Form.Files;

                string webRootPath = _webHostEnviroment.WebRootPath;

                string upload = webRootPath + ENV.characterPath;
                string secondupload = webRootPath + ENV.characterPath;

                string fileName = Guid.NewGuid().ToString();
                string secondFileName = Guid.NewGuid().ToString();

                string extentions = Path.GetExtension(files[0].FileName);
                string secondExtentions = Path.GetExtension(secondfiles[1].FileName);

                var formObject = _db.Character.AsNoTracking().FirstOrDefault(u => u.Id == characterVM.Character.Id);
                if (files.Count > 0)
                {
                    var oldFile = Path.Combine(upload, formObject.imageTitle);
                    var secondOldFile = Path.Combine(secondupload, formObject.characterImage);

                    if (System.IO.File.Exists(oldFile) && System.IO.File.Exists(secondOldFile))
                    {
                        System.IO.File.Delete(oldFile);
                        System.IO.File.Delete(secondOldFile);
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extentions), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    using (var secondFileStream = new FileStream(Path.Combine(secondupload, secondFileName + secondExtentions), FileMode.Create))
                    {
                        secondfiles[1].CopyTo(secondFileStream);
                    }

                    characterVM.Character.imageTitle = fileName + extentions;
                    characterVM.Character.characterImage = secondFileName + secondExtentions;
                }
                else
                {
                    characterVM.Character.imageTitle  = formObject.imageTitle;
                    characterVM.Character.characterImage = formObject.characterImage;
                }
                _db.Character.Update(characterVM.Character);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(characterVM);
        }


        public IActionResult DeleteCharacter(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Character character = _db.Character.FirstOrDefault(u => u.Id == id);
            if (character == null)
            {
                return NotFound();
            }
            return View(character);
        }

        [HttpPost, ActionName("DeleteCharacter")]
        public IActionResult Delete(int? id)
        {
            var character = _db.Character.Find(id);
            if (character == null)
            {
                return NotFound();
            }
            string upload = _webHostEnviroment.WebRootPath + ENV.characterPath;
            string secondupload = _webHostEnviroment.WebRootPath + ENV.characterPath;

            var oldFile = Path.Combine(upload, character.imageTitle);
            var secondOldFile = Path.Combine(upload, character.characterImage);
            if (System.IO.File.Exists(oldFile) && System.IO.File.Exists(secondOldFile))
            {
                System.IO.File.Delete(oldFile);
                System.IO.File.Delete(secondOldFile);
            }
            _db.Character.Remove(character);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult CharacterDetails(int id)
        {
            CharacterDetailsVM characterDetailsVM = new CharacterDetailsVM()
            {
                Character = _db.Character.Where(u => u.Id == id).FirstOrDefault()
            };
            return View(characterDetailsVM);
        }
    }
}
