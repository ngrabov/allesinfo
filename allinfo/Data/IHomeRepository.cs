using allinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace allinfo.Data
{
    public interface IHomeRepository
    {
        Task<int> CountArticlesAsync();
        Task<List<Article>> GetArticlesAsync();
        Task<List<Player>> GetPageDataAsync();
        Task<List<string>> GetVideoDataAsync();
        Task<int> SaveAsync();
    }
}