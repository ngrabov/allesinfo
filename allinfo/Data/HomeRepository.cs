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

        public async Task<List<string>> GetPageDataAsync()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var ctx = BrowsingContext.New(config);
            var dogg2 = await ctx.OpenAsync("http://www.espn.com/nba/seasonleaders/_/league/nba/sort/avgPoints");
            var namesrows = dogg2.QuerySelectorAll("td[align='left'] a").Take(50).ToArray();
            var pointrows = dogg2.QuerySelectorAll("td.sortcell").Take(50).ToArray();

            List<string> results = new List<string>();
            IQueryable<Player> playerz = context.Players.Include(c => c.Team).AsQueryable();
            int i;
            for(i = 0; i < namesrows.Count() ; i++)
            {
                Player player = await playerz.Where(c => c.FullName == namesrows[i].TextContent).FirstOrDefaultAsync();
                if(player != null)
                {
                    player.ppg = double.Parse(pointrows[i].TextContent, CultureInfo.InvariantCulture);
                    results.Add(namesrows[i].TextContent);
                    results.Add(pointrows[i].TextContent);
                    results.Add(player.ID.ToString());
                    results.Add(player.AvatarURL);
                    results.Add(player.Team.AvatarURL);
                }
            }
            await SaveAsync();

            return results;
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