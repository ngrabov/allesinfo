using allinfo.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace allinfo.Data
{
    public interface IPlayersRepository
    {
        Task<List<Player>> GetFreeAgentsAsync();
        Task<List<Article>> GetArticlesAsync();
        IQueryable<Player> GetPlayersByTeamAsync(int? tmid);
        IQueryable<Player> GetPlayersAsync();
        Task<Player> GetPlayerByIDAsync(int? id);
        Task<List<Article>> GetArticlesByTagAsync(Player player);
        Task<Team> GetTeamByPlayerIDAsync(int id);
        Task<Nationality> GetNationalityAsync(int nationid);
        Task<Nationality[]> GetNationalitiesAsync();
        Task<Team[]> GetTeamsAsync();
        void AddPlayer(Player player);
        Task<int> SaveAsync();
    }
}