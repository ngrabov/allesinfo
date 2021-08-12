using allinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace allinfo.Data
{
    public interface IHomeRepository
    {
        int CountArticles();
        Task<List<Article>> GetArticlesAsync();
        Task<List<string>> GetPageDataAsync();
        Task<List<string>> GetVideoDataAsync();
        Task<int> SaveAsync();
    }
}