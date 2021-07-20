using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using allinfo.Data;
using Microsoft.AspNetCore.Authorization;
using allinfo.Models;
using allinfo.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace allinfo.Controllers
{
    public class PlayersController : Controller
    {
        private readonly NewsContext _context;
        private readonly SignInManager<Writer> _signInManager;
        private readonly UserManager<Writer> _userManager;

        public PlayersController(NewsContext context, SignInManager<Writer> signInManager, UserManager<Writer> userManager)
        {
            _context = context; 
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> UpcomingFA()
        {
            var fa = await _context.Players.Include(c => c.Team).
                Where(c => c.contractLength == 1 || ((int)c.contractOption != 3 && c.contractLength == 2)).OrderByDescending(c => c.NBA2KRating).ThenByDescending(c => c.DOB).ToListAsync();
            
            var articles = await _context.Articles.OrderByDescending(c => c.TimeWritten).Take(10).ToListAsync();
            ViewData["SShow"] = articles;
            return View(fa);
        }

        [HttpGet]
        public async Task<IActionResult> Index(TeamsViewModel model, string sortOrder, string currentFilter, int TeamId, Position position, int minage, int maxage)
        {
            var vm = new TeamsViewModel();
            populateTeams();

            if(String.IsNullOrEmpty(sortOrder))
            {
                ViewData["NameSortParm"] = "";
            }
            else
            {
                ViewData["NameSortParm"] = "";
            }
            
            if(sortOrder == "Slry")
            {
                ViewData["SlrySortParm"] = "Slry_desc";
            }
            else
            {
                ViewData["SlrySortParm"] = "Slry";
            }

            if(sortOrder == "2RTG")
            {
                ViewData["2RTGSortParm"] = "2RTG_desc";
            }
            else
            {
                ViewData["2RTGSortParm"] = "2RTG";
            } 

            if(sortOrder == "PtsG")
            {
                ViewData["PtsGSortParm"] = "PtsG_desc";
            }
            else
            {
                ViewData["PtsGSortParm"] = "PtsG";
            } 
            IQueryable<Player> players;

            if(model.TeamId != null) 
            {
                players = _context.Players.Include(c => c.Team).Include(c => c.Nationality).Where(c => c.TeamId == model.TeamId).AsNoTracking();
            }
            else
            {
                players = _context.Players.Include(c => c.Team).Include(c => c.Nationality).AsNoTracking();
            }

            if(model.position != null)
            {
                players = players.Where(c => c.playingPosition == model.position);
                ViewData["Position"] = model.position;
            }

            if(model.minage != null)
            {
                players = players.Where(c => c.DOB.Year >= model.minage);
            }

            if(model.maxage != null)
            {
                players = players.Where(c => c.DOB.Year <= model.maxage);
            }

            if (sortOrder == "PtsG_desc")
            {
                players = players.OrderByDescending( s => s.ppg);
            }      
            else if (sortOrder == "PtsG")
            {
                players = players.OrderBy( s => s.ppg);
            }
            else if (sortOrder == "Slry") 
            {  
                players = players.OrderBy(s => s.salary1);
            }        
            else if (sortOrder == "Slry_desc")  
            { 
                players = players.OrderByDescending(s => s.salary1);
            } 
            else if (sortOrder == "2RTG_desc")
            {
                players = players.OrderByDescending(s => s.NBA2KRating);
            }   
            else if (sortOrder == "2RTG")
            {
                players = players.OrderBy(s => s.NBA2KRating);
            }        
            else
            {
                players = players.OrderBy(s => s.ID);        
            }
            ViewData["Counter"] = players.Count();
            ViewData["CurrentFilter"] = model.TeamId;
            ViewData["Position"] = model.position;
            ViewData["minage"] = model.minage;
            ViewData["maxage"] = model.maxage;

            List<int> age = new List<int>();
            for(int l = 1980; l < DateTime.Now.Year - 18; l++)
            {
                age.Add(l);
            }
            ViewBag.ages = new SelectList (age);
            vm.Players = await players.ToListAsync();

            return View(vm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var player = await _context.Players.Include(c => c.Team).Include(c => c.Nationality).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);
            var artcls = await _context.Articles.Where(c => c.Tags.Contains(player.LastName)).Where(c => c.Tags.Contains(player.TeamName.Substring(0, 5))).OrderByDescending(c => c.TimeWritten).AsNoTracking().ToListAsync();

            ViewData["Art"] = artcls;
            if(player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public IActionResult Create()
        {
            populateTeams();
            populateNations();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DOB,Team,TeamId,AvatarURL,Nationality,NationalityID,playingPosition,Accolades,height,NBA2KRating,shirtNumber,salary1,salary2,salary3,salary4,salary5,contractOption,faType")] Player player, IFormFile file)
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

                    player.AvatarURL = webp;
                    player.Team = await _context.Teams.FindAsync(player.TeamId);
                    player.Team.payroll += player.salary1;
                    player.contractLength = 5;
                    var salaries = new double[]{player.salary2, player.salary3, player.salary4, player.salary5};
                    int i;
                    for(i = 0; i < 4; i++)
                    {
                        if(salaries[i] == 0)
                        {
                            player.contractLength = i + 1;
                            break;
                        }
                    }
                    player.Nationality = await _context.Nationalities.FindAsync(player.NationalityID);
                    _context.Add(player);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the issue persists, contact your system administrator.");
                }
            }
            ModelState.AddModelError("", "Please select a valid image file for avatar.");
            return View(player);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();

            if(id == null) return NotFound();

            var player = await _context.Players.FindAsync(id);

            if(player == null)
            {
                return NotFound();
            }
            populateNations();
            populateTeams();
            return View(player);
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
            var player = await _context.Players.Include(c => c.Team).Include(c => c.Nationality).FirstOrDefaultAsync(c => c.ID == id);
            
            if(file != null)
            {
                if(await TryUpdateModelAsync<Player> (player, "", s => s.contractLength, s => s.Accolades, s => s.NBA2KRating, s => s.FirstName, s => s.AvatarURL, s => s.LastName, s => s.DOB, s => s.NationalityID, s => s.contractOption,
                s => s.height, s => s.ppg, s => s.playingPosition, s => s.shirtNumber, s => s.TeamId, s => s.salary1, s => s.salary2, s => s.salary3, s => s.salary4, s => s.salary5, s => s.contractDetails))
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

                        player.contractLength = 5;
                        var salaries = new double[]{player.salary2, player.salary3, player.salary4, player.salary5};
                        int i;
                        for(i = 0; i < 4; i++)
                        {
                            if(salaries[i] == 0)
                            {
                                player.contractLength = i + 1;
                                break;
                            }
                        }

                        player.AvatarURL = webp;
                        player.Team.payroll -= player.salary1;
                        player.Team = await _context.Teams.FindAsync(player.TeamId);
                        player.Team.payroll += player.salary1;
                        player.Nationality = await _context.Nationalities.FindAsync(player.NationalityID);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch(DbUpdateException)
                    {
                        ModelState.AddModelError("", "Could not save changes. Try again, and if the problem persists, contact your system administrator.");
                    }
                } 
            }
            ModelState.AddModelError("", "Please select a valid image file.");
            return View(player);
        }

        // delete

        private async void populateTeams(object selectedTeam = null)
        {
            var teams = await _context.Teams.OrderBy(c => c.Name).ToArrayAsync();
            ViewBag.teams = new SelectList(teams, "ID", "Name", selectedTeam);
        }

        private async void populateNations(object selectedNation = null)
        {
            var nations = await _context.Nationalities.OrderBy(c => c.nationalityName).ToArrayAsync();
            ViewBag.nations = new SelectList(nations, "ID", "nationalityName", selectedNation);
        }
    }
}