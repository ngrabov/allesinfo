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
        private IPlayersRepository playersRepository;
        private readonly SignInManager<Writer> _signInManager;
        private readonly UserManager<Writer> _userManager;

        public PlayersController(IPlayersRepository playersRepository, SignInManager<Writer> signInManager, UserManager<Writer> userManager)
        {
            this.playersRepository = playersRepository; 
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> UpcomingFA()
        {
            var fa = await playersRepository.GetFreeAgentsAsync();
            
            var articles = await playersRepository.GetArticlesAsync();
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
                players = playersRepository.GetPlayersByTeamAsync(model.TeamId);
            }
            else
            {
                players = playersRepository.GetPlayersAsync();
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
            var player = await playersRepository.GetPlayerByIDAsync(id);
            var artcls = await playersRepository.GetArticlesByTagAsync(player);

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
                    player.Team = await playersRepository.GetTeamByPlayerIDAsync(player.TeamId);
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
                    player.Nationality = await playersRepository.GetNationalityAsync(player.NationalityID);
                    playersRepository.AddPlayer(player);
                    await playersRepository.SaveAsync();
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

            var player = await playersRepository.GetPlayerByIDAsync(id);

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
            var player = await playersRepository.GetPlayerByIDAsync(id);
            
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
                        player.Team = await playersRepository.GetTeamByPlayerIDAsync(player.TeamId);
                        player.Team.payroll += player.salary1;
                        player.Nationality = await playersRepository.GetNationalityAsync(player.NationalityID);
                        await playersRepository.SaveAsync();
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
            var teams = await playersRepository.GetTeamsAsync();
            ViewBag.teams = new SelectList(teams, "ID", "Name", selectedTeam);
        }

        private async void populateNations(object selectedNation = null)
        {
            var nations = await playersRepository.GetNationalitiesAsync();
            ViewBag.nations = new SelectList(nations, "ID", "nationalityName", selectedNation);
        }
    }
}