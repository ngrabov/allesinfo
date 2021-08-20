using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data;
using allinfo.Models;
using System.Threading.Tasks;
using AngleSharp;
using System.Globalization;

namespace allinfo.Data
{
    public class HomeRepository : IHomeRepository
    {
        private NewsContext context;

        public HomeRepository(NewsContext context)
        {
            this.context = context;
        }

        public async Task<int> CountArticlesAsync()
        {
            return await context.Articles.Where(c => !c.isModerated).CountAsync();
        }
        
        public async Task<List<Article>> GetArticlesAsync()
        {
            return await context.Articles.Include(c => c.Writer).AsNoTracking().ToListAsync();
        }

        public async Task<List<Player>> GetPageDataAsync()
        {
            List<Player> results = new List<Player>();
            if(System.DateTime.Now.Hour == 14)
            {
                var config = Configuration.Default.WithDefaultLoader();
                var ctx = BrowsingContext.New(config);
                var dogg2 = await ctx.OpenAsync("http://www.espn.com/nba/seasonleaders/_/league/nba/sort/avgPoints");

                var namesrows = dogg2.QuerySelectorAll("td[align='left'] a").Take(50).ToArray();
                List<string> names = new List<string>();
                foreach(var name in namesrows)
                {
                    names.Add(name.TextContent);
                }

                var pointrows = dogg2.QuerySelectorAll("td.sortcell").Take(50).ToArray();
                List<double> point = new List<double>();
                foreach(var item in pointrows)
                {
                    point.Add(double.Parse(item.TextContent, CultureInfo.InvariantCulture));
                }

                context.Players.Include(c => c.Team).Where(c => names.Contains(c.FullName)).ToList().ForEach(d => {
                    results.Add(d);
                    int j = names.IndexOf(d.FullName);
                    d.ppg = point[j];
                });
                
                await SaveAsync();
            }
            else
            {
                results = await context.Players.Include(c => c.Team).Where(c => c.ppg != 0.0).OrderByDescending(c => c.ppg).Take(50).ToListAsync();
            }

            return results.OrderByDescending(c => c.ppg).ToList();
        }

        public async Task<List<string>> GetVideoDataAsync()
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
        
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}