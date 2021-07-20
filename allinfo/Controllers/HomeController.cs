using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using allinfo.Models;
using allinfo.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AngleSharp;
using AngleSharp.Html;
using AngleSharp.Dom;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace allinfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsContext _context;

        public HomeController(NewsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var artquery = _context.Articles.Where(c => !c.isModerated).Count().ToString();
            ViewBag.cnt = artquery;
            var articles = _context.Articles.Include(c => c.Writer).AsNoTracking();
            ViewData["Scrap"] = await GetPageData();
            ViewData["Video"] = await GetVideoData();
            return View(await articles.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<List<string>> GetPageData()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var ctx = BrowsingContext.New(config);
            var dogg2 = await ctx.OpenAsync("http://www.espn.com/nba/seasonleaders/_/league/nba/sort/avgPoints");
            var namesrows = dogg2.QuerySelectorAll("td[align='left'] a").Take(30).ToArray();
            var pointrows = dogg2.QuerySelectorAll("td.sortcell").Take(30).ToArray();

            List<string> results = new List<string>();
            int i;
            for(i = 0; i < namesrows.Count() ; i++)
            {
                Player player = await _context.Players.Where(c => c.FullName == namesrows[i].TextContent).FirstOrDefaultAsync();
                if(player != null)
                {
                    player.ppg = double.Parse(pointrows[i].TextContent, CultureInfo.InvariantCulture);
                    results.Add(namesrows[i].TextContent);
                    results.Add(pointrows[i].TextContent);
                    results.Add(player.ID.ToString());
                }
                else
                {
                    i++;
                }
            }
            await _context.SaveChangesAsync();

            return results;
        }

        public async Task<List<string>> GetVideoData()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var ctx = BrowsingContext.New(config);
            var dogg1 = await ctx.OpenAsync("https://www.foxsports.com/nba/highlights");
            var videorows = dogg1.QuerySelectorAll("iframe").Select(m => m.GetAttribute("title")).Take(15).ToArray();
            var linksrows = dogg1.QuerySelectorAll("iframe").Select(m => m.GetAttribute("data-src")).Take(15).ToArray(); //full url

            List<string> videos = new List<string>();
            int j;
            for( j = 0; j < videorows.Count(); j++)
            {
                videos.Add(videorows[j]); //captions
                videos.Add(linksrows[j].Remove(0, 30)); //videoid
                videos.Add("https://img.youtube.com/vi/" + linksrows[j].Remove(0, 30) + "/sddefault.jpg"); //thumbnail
            }

            return videos;
        }
    }
}
