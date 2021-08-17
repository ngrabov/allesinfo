using allinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace allinfo.Data
{
    public interface IProspectsRepository
    {
        Task<List<Prospect>> GetProspectsAsync();
        Task<Team> GetPlayersTeamByID(int id);
        Task<Prospect> GetProspectByIDAsync(int? id);
        Task<Team[]> GetTeamsAsync();
        void AddProspect(Prospect prospect);
        Task<int> SaveAsync();
    }
}