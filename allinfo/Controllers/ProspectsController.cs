using Microsoft.AspNetCore.Mvc;
using allinfo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using allinfo.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using allinfo.ViewModels;

namespace allinfo.Controllers
{
    public class ProspectsController : Controller
    {
        private readonly NewsContext _context;
        private readonly SignInManager<Writer> _signInManager;
        private readonly UserManager<Writer> _userManager;
        public ProspectsController(NewsContext context, SignInManager<Writer> signInManager, UserManager<Writer> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> MockDraft(TeamsViewModel model)
        {
            var vm = new TeamsViewModel();
            var prospects = await _context.Prospects.Include(c => c.Team).OrderBy(c => c.rank).Take(30).AsNoTracking().ToListAsync();
            vm.Prospects = prospects;
            return View(vm);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public IActionResult Create()
        {
            populateTeams();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,playingPosition,age,college,height,report,group,rank," + 
        "shirtNumber,aviUrl,stat1,stat2,stat3,stat4,stat5,stat6,stat7,stat8,Team,TeamId")]Prospect prospect, IFormFile file)
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
                    prospect.aviUrl = webp;
                    prospect.Team = await _context.Teams.FindAsync(prospect.TeamId);

                    _context.Add(prospect);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MockDraft));
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact the system administrator.");
                }
            }
            ModelState.AddModelError("", "Please select a valid image file.");
            return View(prospect);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();

            if(id == null) return NotFound();

            var prospect = await _context.Prospects.FindAsync(id);

            if(prospect == null)
            {
                return NotFound();
            }
            populateTeams();
            return View(prospect);
        }

        [Authorize(Roles ="Administrator,Manager")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, IFormFile file)
        {
            if(id == null)
            {
                return NotFound();
            }
            var prospect = await _context.Prospects.Include(c => c.Team).FirstOrDefaultAsync(c => c.ID == id);
            
            if(file != null)
            {
                if(await TryUpdateModelAsync<Prospect> (prospect, "", s => s.Name, s => s.college, s => s.rank, s => s.report, s => s.age, s => s.group,
                s => s.height, s => s.playingPosition, s => s.shirtNumber, s => s.TeamId, 
                s => s.stat1, s => s.stat2, s => s.stat3, s => s.stat4, s => s.stat5, s => s.stat6, s => s.stat7, s => s.stat8))
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

                        prospect.aviUrl = webp;
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(MockDraft));
                    }
                    catch(DbUpdateException)
                    {
                        ModelState.AddModelError("", "Could not save changes. Try again, and if the problem persists, contact your system administrator.");
                    }
                } 
            }
            ModelState.AddModelError("", "Please select a valid image file.");
            return View(prospect); 
        }
 
        private async void populateTeams(object selectedTeam = null)
        {
            var teams = await _context.Teams.OrderBy(c => c.Name).ToArrayAsync();
            ViewBag.teams = new SelectList(teams, "ID", "Name", selectedTeam);
        }
    }
}