using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using allinfo.Data;
using allinfo.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace allinfo.Controllers
{
    public class CommentsController : Controller
    {
        private ICommentsRepository commentsRepository;
        private readonly UserManager<Writer> _userManager;
        private readonly SignInManager<Writer> _signInManager;

        public CommentsController(ICommentsRepository commentsRepository, UserManager<Writer> userManager, SignInManager<Writer> signInManager)
        {
            this.commentsRepository = commentsRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,TimeWritten,UserID,ArticleID")] Comment comment, int artid, string mtx)
        {
            try{
                comment.Text = mtx;
                comment.TimeWritten = DateTime.Now;
                comment.UserID = _userManager.GetUserId(User);
                var currentUser = await _userManager.GetUserAsync(User);
                comment.UserFullName = currentUser.FullName;
                comment.ArticleID = artid;
                commentsRepository.AddCommentAsync(comment);
                await commentsRepository.SaveAsync();
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " + 
                "Check your input, and if the problem persists, contact your " + 
                "system administrator.");
            }
            return Redirect($"../Articles/Read/{artid}");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int artid, string txt)
        {
            try
            {
                var comment = await commentsRepository.GetCommentByIDAsync(id);
                comment.TimeWritten = DateTime.Now;
                comment.Text = txt;
                await commentsRepository.SaveAsync();
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " + 
                "Check your input, and if the problem persists, contact your " + 
                "system administrator.");
            }
            
            return Redirect($"../Articles/Read/{artid}");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int mtx, int artid)
        {
            var comment = await commentsRepository.GetCommentByIDAsync(mtx);
            commentsRepository.RemoveCommentAsync(comment);
            await commentsRepository.SaveAsync();
            return Redirect($"../Articles/Read/{artid}");
        }
    }
}