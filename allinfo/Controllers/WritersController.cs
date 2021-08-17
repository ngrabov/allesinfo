using allinfo.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using allinfo.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace allinfo.Controllers
{
    public class WritersController : Controller
    {
        private IWritersRepository writersRepository;
        private readonly UserManager<Writer> _userManager;
        private readonly SignInManager<Writer> _signInManager;

        public WritersController(IWritersRepository writersRepository, UserManager<Writer> userManager, SignInManager<Writer> signInManager)
        {
            this.writersRepository = writersRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;

            if(String.IsNullOrEmpty(sortOrder))
            {
                ViewData["NameSortParm"] = "name_desc";
            }
            else
            {
                ViewData["NameSortParm"] = "";
            }
            
            if(sortOrder == "Date")
            {
                ViewData["DateSortParm"] = "date_desc";
            }
            else
            {
                ViewData["DateSortParm"] = "Date";
            }

            if(sortOrder == "Numb")
            {
                ViewData["NumbSortParm"] = "numb_desc";
            }
            else
            {
                ViewData["NumbSortParm"] = "Numb";
            } 

            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var writers = from s in writersRepository.GetWritersAsync()
                        where s.isAdmin == false && s.isManager 
                        select s;

            if(!String.IsNullOrEmpty(searchString))
            {
                writers = writers.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                        || s.FirstName.ToUpper().Contains(searchString.ToUpper()));
            }

            if (sortOrder == "name_desc")
            {
                writers = writers.OrderByDescending( s => s.LastName);
            }      
            else if (sortOrder == "Date") 
            {  
                writers = writers.OrderBy(s => s.DOB);
            }        
            else if (sortOrder == "date_desc")  
            { 
                writers = writers.OrderByDescending(s => s.DOB);
            } 
            else if (sortOrder == "numb_desc")
            {
                writers = writers.OrderByDescending(s => s.Articles.Count);
            }   
            else if (sortOrder == "Numb")
            {
                writers = writers.OrderBy(s => s.Articles.Count);
            }        
            else
            {
                writers = writers.OrderBy(s => s.LastName);        
            }

            int pageSize = 6;
            return View(await PaginatedList<Writer>.CreateAsync(writers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var writer = await writersRepository.GetWriterByIDAsync(id);
            
            if(writer == null)
            {
                return NotFound();
            }

            return View(writer);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create ([Bind("FirstName,LastName,DOB,Articles,TwitterHandle,Email,isModerator")] Writer writer, string pw)
        {
            try
            {
                writer.Articles = new List<Article>();
                writer.isAdmin = false;
                writer.isManager = true;
                writer.UserName = writer.Email;
                writersRepository.AddWriterAsync(writer);
                var result = await _userManager.CreateAsync(writer, pw);
                await _userManager.AddToRoleAsync(writer, "Manager");
                await writersRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " + 
                "Try again, and if the problem persists, contact your " + 
                "system administrator.");
            }
            return View(writer);
        }

        [Authorize(Roles= "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var writer = await writersRepository.GetWriterByIDAsync(id);
            if(writer == null)
            {
                return NotFound();
            } 
            return View(writer);
        } 

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var writerToUpdate = await writersRepository.GetWriterByIDAsync(id);

            if(await TryUpdateModelAsync<Writer>(
                writerToUpdate,
                "",
                s => s.FirstName, s => s.LastName, s => s.DOB, s => s.TwitterHandle, s => s.Email, s => s.PasswordHash, s => s.UserName, s => s.isModerator
            ))
            {
                try
                {
                    var result = _userManager.UpdateAsync(writerToUpdate);
                    await writersRepository.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + 
                    "Try again, and if the problem persists,  " + 
                    "contact your system administrator. ");
                }
            }
            return View(writerToUpdate);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if(id == null)
            {
                return NotFound();
            }

            var writer = await writersRepository.GetWriterByIDAsync(id);

            if (writer.isAdmin == true)
            {
                return Forbid();
            }

            if (writer == null)
            {
                return NotFound();
            }

            if(saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again, and if the problem persists, " +
                "contact your system administrator.";
            }

            return View(writer);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var writer = await writersRepository.GetWriterByIDAsync(id);
            if(writer == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if(writer.isAdmin)
            {
                return Forbid();
            }

            try
            {
                writersRepository.RemoveWriterAsync(writer);
                await writersRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new {id = id, saveChangesError = true});
            }
        }
    }
}