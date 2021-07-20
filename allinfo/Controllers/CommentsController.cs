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
        private readonly NewsContext _context;
        private readonly UserManager<Writer> _userManager;
        private readonly SignInManager<Writer> _signInManager;

        public CommentsController(NewsContext context, UserManager<Writer> userManager, SignInManager<Writer> signInManager)
        {
            _context = context;
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
                _context.Add(comment);
                
                await _context.SaveChangesAsync();
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
                var comment = await _context.Comments.FindAsync(id);
                comment.TimeWritten = DateTime.Now;
                comment.Text = txt;
                await _context.SaveChangesAsync();
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
            var comment = await _context.Comments.FindAsync(mtx);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Redirect($"../Articles/Read/{artid}");
        }
    }
}