using allinfo.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace allinfo.Data
{
    public class ProspectsRepository : IProspectsRepository
    {
        private NewsContext context;

        public ProspectsRepository(NewsContext context)
        {
            this.context = context;
        }

        public async Task<List<Prospect>> GetProspectsAsync()
        {
            return await context.Prospects.Include(c => c.Team).OrderBy(c => c.rank).Take(30).AsNoTracking().ToListAsync();
        }

        public async Task<Team> GetPlayersTeamByID(int id)
        {
            return await context.Teams.FindAsync(id);
        }

        public async Task<Prospect> GetProspectByIDAsync(int? id)
        {
            return await context.Prospects.Include(c => c.Team).FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<Team[]> GetTeamsAsync()
        {
            return await context.Teams.OrderBy(c => c.Name).ToArrayAsync();
        }

        public void AddProspect(Prospect prospect)
        {
            context.Add(prospect);
        }

        public Task<int> SaveAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}