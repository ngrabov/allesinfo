using System.Collections.Generic;
using System.Threading.Tasks;
using allinfo.Models;
using System.Linq;

namespace allinfo.Data
{
    public interface ITeamsRepository
    {
        IQueryable<Team> GetTeamsAsync();
        Task<List<Team>> GetTeamsByPayrollAsync();
        Task<Team> GetTeamByAbb(string abb);
        Task<Team> GetTeamByID(int? id);
        Task<List<Player>> GetPlayersByTeamID(int? id);
        Task<List<Player>> GetPlayersByTeamAbb(string abb);
        Task<List<Article>> GetArticlesByTag(string teamname);
        int GetAbbsAsync(string a);
        Task<int> SaveAsync();
    }
}