using allinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace allinfo.Data
{
    public interface IArticlesRepository
    {
        Task<List<Article>> GetArticlesByTagAsync(string tag);
        Task<List<Article>> GetArticlesGeneralAsync();
        Task<List<Article>> GetArticlesByFieldAsync(Field? field);
        Task<List<Article>> GetArticlesForModerationAsync();
        Task<Article> GetArticleByIDAsync(int? id);
        Task<Article> GetArticleByWriterIDAsync(int currentUserID, int? id);
        IQueryable<Writer> GetWritersAsync();
        Task<Writer> GetWriterByIDAsync(int id);
        void AddArticleAsync(Article article);
        void RemoveArticleAsync(Article article);
        Task<int> SaveAsync();
    }
}