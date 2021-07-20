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
        private readonly NewsContext _context;

        public TeamsController(NewsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teams = new List<Team>();

            var group = from team in await _context.Teams.ToListAsync()
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
            IQueryable<Player> players;
            double avg = 0.0;
            teams = await _context.Teams.AsNoTracking().OrderByDescending(c => c.payroll).ToListAsync();
            ViewData["Payrolls"] = teams;
            if(id != null)
            {
                team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);
                players = _context.Players.AsNoTracking().Where(c => c.TeamId == id).OrderByDescending(c => c.salary1);
            }
            else
            {
                team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(c => c.ShortName == abb);
                players = _context.Players.AsNoTracking().Where(c => c.Team.ShortName == abb).OrderByDescending(c => c.salary1);
            }
            //avg = await players.AverageAsync(s => s.age);
            foreach(var item in players)
            {
                avg += item.age;
            }
            avg /= players.Count();
            ViewData["Avgs"] = avg;
            ViewData["Salary"] = await players.ToListAsync();
            var articls = await _context.Articles.AsNoTracking().Where(c => c.Tags.Contains(team.Name.Substring(0, 5))).OrderByDescending(c => c.TimeWritten).ToListAsync();
            ViewData["Artcls"] = articls;
            return View(team);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return NotFound();
            var team = await _context.Teams.FirstOrDefaultAsync(c => c.ID == id);
            if(team == null) return NotFound();
            return View(team);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, IFormFile file)
        {
            if(id == null) return NotFound();
            var team = await _context.Teams.FirstOrDefaultAsync(c => c.ID == id);

            if(file != null)
            {
                if(await TryUpdateModelAsync<Team>(team, "", c => c.Name, c => c.Arena, c => c.AvatarURL, c => c.division, c => c.HeadCoach))
                { 
                    try
                    {
                        team.AvatarURL = await UploadMe(file);
                        await _context.SaveChangesAsync();
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