using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using allinfo.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using allinfo.Models;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Collections.Generic;

namespace allinfo.Controllers
{
    public class ArticlesController : Controller
    {
        private IArticlesRepository articlesRepository;
        private readonly UserManager<Writer> _userManager;
        private readonly SignInManager<Writer> _signInManager;

        public ArticlesController(IArticlesRepository articlesRepository, UserManager<Writer> userManager, SignInManager<Writer> signInManager)
        {
            this.articlesRepository = articlesRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(Field? field, string tag)
        {
            Task<List<Article>> articles;

            if(tag != null)
            {
                articles = articlesRepository.GetArticlesByTagAsync(tag);
                return View(await articles);
            }
            
            if(field == null)
            {
                articles = articlesRepository.GetArticlesGeneralAsync();
                return View(await articles);
            }

            articles = articlesRepository.GetArticlesByFieldAsync(field);
            return View(await articles);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Moderation()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null || (!currentUser.isModerator && !currentUser.isAdmin))
            {
                return Forbid();
            }
            var articles = articlesRepository.GetArticlesForModerationAsync();
            return View(await articles);
        }

        [HttpPost,ActionName("Moderation")]
        [Authorize(Roles = "Administrator,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModerationPost()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null || (!currentUser.isModerator && !currentUser.isAdmin))
            {
                return Forbid();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Read(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var article = await articlesRepository.GetArticleByIDAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public IActionResult Create()
        {
            PopulateWritersDropdownList();
            return View();
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Headline,Text,WriterId,Writer,Field,TimeWritten,HeadImageURL,SubHeadline,isImportant,Tags,isModerated")] Article article, IFormFile file)
        {
            if(file != null)
            {
                try
                {
                    string ext = file.FileName;
                    string pth = Path.GetTempFileName();
                    string nx = Path.GetFileNameWithoutExtension(ext);
                    string webp = nx + ".webp";

                    Account account = new Account("dsjavparg", "351856923196787", "rANgXE5HEDwuyV3QThqfJiRs8Zg");
                    Cloudinary cloudinary = new Cloudinary(account);

                    using(var stream = System.IO.File.Create(pth))
                    {
                        await file.CopyToAsync(stream);
                    }

                    ImageUploadParams uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(pth), 
                        PublicId = nx,
                        Format = "webp"
                    };

                    ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

                    article.HeadImageURL = webp;
                    article.TimeWritten = DateTime.Now;
                    article.Writer = await articlesRepository.GetWriterByIDAsync(article.WriterId);
                    articlesRepository.AddArticleAsync(article);
                    await articlesRepository.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + 
                    "Try again, and if the problem persists, contact your " + 
                    "system administrator.");
                } 
            }
            ModelState.AddModelError("", "Please choose a valid image file and writer for the article.");
            PopulateWritersDropdownList(article.WriterId);
            return View(article);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int? id, Field field)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();

            if(id == null)
            {
                return NotFound();
            }

            Article article;
            if(currentUser.isAdmin || currentUser.isModerator)
            {
                article = await articlesRepository.GetArticleByIDAsync(id);

                if(article == null)
                {
                    return Forbid();
                }

                return View(article);
            }

            article = await articlesRepository.GetArticleByWriterIDAsync(currentUser.Id, id); 

            if(article == null)
            {
                return Forbid();
            }
            return View(article);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var articleToUpdate = await articlesRepository.GetArticleByIDAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);
            if(await TryUpdateModelAsync<Article>(
                articleToUpdate,
                "",
                c => c.Headline, c => c.WriterId, c => c.Text, c => c.Field, c => c.SubHeadline, c => c.isImportant, c => c.Tags, c => c.isModerated
                ))
            {
                try
                {
                    if(!currentUser.isModerator && ! await _userManager.IsInRoleAsync(currentUser, "Administrator"))
                    {
                        articleToUpdate.isModerated = false;
                    } 
                    
                    await articlesRepository.SaveAsync();
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + 
                    "Try again, and if the problem persists, contact your " +
                    "system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateWritersDropdownList(articleToUpdate.WriterId);
            return View(articleToUpdate);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            if (currentUser == null) return Challenge();

            if(id == null)
            {
                return NotFound();
            }

            Article article;
            if(currentUser.isAdmin)
            {
                article = await articlesRepository.GetArticleByIDAsync(id);

                if(article == null)
                {
                    return Forbid();
                }

                return View(article);
            }

            article = await articlesRepository.GetArticleByWriterIDAsync(currentUser.Id, id);
            if(article == null)
            {
                return Forbid();
            }

            return View(article);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await articlesRepository.GetArticleByIDAsync(id);
            articlesRepository.RemoveArticleAsync(article);
            await articlesRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async void PopulateWritersDropdownList(object selectedWriter = null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            var writersQuery = from s in articlesRepository.GetWritersAsync()
                                where s.Id == currentUser.Id
                                orderby s.FirstName
                                select s;

            if(_signInManager.IsSignedIn(User) && currentUser!= null && await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            {
                writersQuery = from s in articlesRepository.GetWritersAsync()
                                where s.isManager == true
                                orderby s.FirstName
                                select s;
            }

            ViewBag.WriterId = new SelectList(writersQuery.AsNoTracking(), "Id", "FullName", selectedWriter);
        }
    }
}