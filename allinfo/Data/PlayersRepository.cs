using allinfo.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace allinfo.Data
{
    public class PlayersRepository : IPlayersRepository
    {
        private NewsContext context;

        public PlayersRepository(NewsContext context)
        {
            this.context = context; 
        }

        public async Task<List<Player>> GetFreeAgentsAsync()
        {
            return await context.Players.Include(c => c.Team).
                Where(c => c.contractLength == 1 || ((int)c.contractOption != 3 && c.contractLength == 2)).OrderByDescending(c => c.NBA2KRating).ThenByDescending(c => c.DOB).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            return await context.Articles.OrderByDescending(c => c.TimeWritten).Take(10).ToListAsync();
        }

        public IQueryable<Player> GetPlayersByTeamAsync(int? tmid)
        {
            return context.Players.Include(c => c.Team).Include(c => c.Nationality).Where(c => c.TeamId == tmid).AsNoTracking().AsQueryable();
        }

        public IQueryable<Player> GetPlayersAsync()
        {
            return context.Players.Include(c => c.Team).Include(c => c.Nationality).AsNoTracking();
        }

        public async Task<Player> GetPlayerByIDAsync(int? id)
        {
            return await context.Players.Include(c => c.Team).Include(c => c.Nationality).FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<List<Article>> GetArticlesByTagAsync(Player player)
        {
            return await context.Articles.Where(c => c.Tags.Contains(player.LastName)).Where(c => c.Tags.Contains(player.TeamName.Substring(0, 5))).OrderByDescending(c => c.TimeWritten).AsNoTracking().ToListAsync();
        }

        public async Task<Team> GetTeamByPlayerIDAsync(int id)
        {
            return await context.Teams.FindAsync(id);
        }

        public async Task<Nationality> GetNationalityAsync(int nationid)
        {
            return await context.Nationalities.FindAsync(nationid);
        }

        
        public async Task<Nationality[]> GetNationalitiesAsync()
        {  
            return await context.Nationalities.OrderBy(c => c.nationalityName).ToArrayAsync();
        }

        public async Task<Team[]> GetTeamsAsync()
        {
            return await context.Teams.OrderBy(c => c.Name).ToArrayAsync();
        }

        public void AddPlayer(Player player)
        {
            context.Add(player);
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync(); 
        }
    }
}