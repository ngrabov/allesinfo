using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using allinfo.Data;
using allinfo.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;

namespace allinfo.Controllers
{
    public class TeamsController : Controller
    {
        private ITeamsRepository teamsRepository;

        public TeamsController(ITeamsRepository teamsRepository)
        {
            this.teamsRepository = teamsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var teams = new List<Team>();

            var group = from team in await teamsRepository.GetTeamsAsync().ToListAsync() 
                        group team by team.division into divTeam //Team.division, Team
                        orderby divTeam.Key
                        select divTeam;

            foreach(var grouping in group)
            {
                foreach(var team in grouping.OrderBy(c => c.Name))
                {
                    teams.Add(team);
                }
            }

            return View(teams);
        }

        public async Task<IActionResult> Details(int? id, string abb)
        {
            Team team;
            List<Team> teams;
            List<Player> players;
            double avg = 0.0;
            teams = await teamsRepository.GetTeamsByPayrollAsync();
            ViewData["Payrolls"] = teams;
            if(id != null)
            {
                team = await teamsRepository.GetTeamByID(id); 
                players = await teamsRepository.GetPlayersByTeamID(id);
            }
            else if(teamsRepository.GetAbbsAsync(abb) == 1)
            {
                team = await teamsRepository.GetTeamByAbb(abb);
                players = await teamsRepository.GetPlayersByTeamAbb(abb);
            }
            else
            {
                return NotFound();
            }

            foreach(var item in players)
            {
                avg += item.age;
            }
            avg /= players.Count();
            ViewData["Avgs"] = avg;
            ViewData["Salary"] = players;
            var articls = await teamsRepository.GetArticlesByTag(team.Name.Substring(0, 5));
            ViewData["Artcls"] = articls;
            return View(team);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return NotFound();
            var team = await teamsRepository.GetTeamByID(id);
            if(team == null) return NotFound();
            return View(team);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, IFormFile file)
        {
            if(id == null) return NotFound();
            var team = await teamsRepository.GetTeamByID(id); 

            if(file != null)
            {
                if(await TryUpdateModelAsync<Team>(team, "", c => c.Name, c => c.Arena, c => c.AvatarURL, c => c.division, c => c.HeadCoach))
                { 
                    try
                    {
                        team.AvatarURL = await UploadMe(file);
                        await teamsRepository.SaveAsync(); 
                        return RedirectToAction(nameof(Index));
                    }
                    catch(DbUpdateException)
                    {
                        ModelState.AddModelError("", "Image could not be uploaded. But keep trying.");
                    }
                }
            }

            ModelState.AddModelError("", "No image added. Please try again.");
            return View(team);
        }

        public async Task<string> UploadMe(IFormFile files)
        {
            string ext = files.FileName;
            string pth = Path.GetTempFileName();
            string nx = Path.GetFileNameWithoutExtension(ext);
            string webp = nx + ".webp";

            Account account = new Account("dsjavparg", "351856923196787", "rANgXE5HEDwuyV3QThqfJiRs8Zg");
            Cloudinary cloudinary = new Cloudinary(account);

            using(var stream = System.IO.File.Create(pth))
            {
                await files.CopyToAsync(stream);
            }

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(pth), 
                PublicId = nx,
                Format = "webp"
            };

            ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
            return webp;
        }
    }
}