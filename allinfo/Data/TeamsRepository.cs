using allinfo.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace allinfo.Data
{
    public class TeamsRepository : ITeamsRepository
    {
        private NewsContext context;

        public TeamsRepository(NewsContext context)
        {
            this.context = context;
        }

        public async Task<List<Team>> GetTeamsByPayrollAsync()
        {
            return await context.Teams.AsNoTracking().OrderByDescending(c => c.payroll).ToListAsync();
        }

        public async Task<Team> GetTeamByAbb(string abb)
        {
            return await context.Teams.AsNoTracking().FirstOrDefaultAsync(c => c.ShortName == abb);
        }

        public async Task<Team> GetTeamByID(int? id)
        {
            return await context.Teams.FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<List<Player>> GetPlayersByTeamID(int? id)
        {
            return await context.Players.AsNoTracking().Where(c => c.TeamId == id).OrderByDescending(c => c.salary1).ToListAsync();
        }

        public async Task<List<Player>> GetPlayersByTeamAbb(string abb)
        {
            return await context.Players.AsNoTracking().Where(c => c.Team.ShortName == abb).OrderByDescending(c => c.salary1).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByTag(string teamname)
        {
            return await context.Articles.AsNoTracking().Where(c => c.Tags.Contains(teamname)).OrderByDescending(c => c.TimeWritten).ToListAsync();
        }

        public int GetAbbsAsync(string a)
        {
            foreach(var i in context.Teams)
            {
                if(i.ShortName == a)
                {
                    return 1;
                }
            }
            return 0;
        } 

        public IQueryable<Team> GetTeamsAsync()
        {
            return context.Teams.AsQueryable();
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}