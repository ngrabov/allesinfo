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
    public class ArticlesRepository : IArticlesRepository
    {
        private NewsContext context;

        public ArticlesRepository(NewsContext context)
        {
            this.context = context;
        }

        public async Task<List<Article>> GetArticlesByTagAsync(string tag)
        {
            return await context.Articles.Where(c => c.Tags.Contains(tag)).Include(c => c.Writer).AsNoTracking().ToListAsync();
        }

        public async Task<List<Article>> GetArticlesGeneralAsync()
        {
            return await context.Articles.Include(c => c.Writer).AsNoTracking().ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByFieldAsync(Field? field)
        {
            return await context.Articles.Where(d => d.Field == field).Include(c => c.Writer).AsNoTracking().ToListAsync();
        }

        public async Task<List<Article>> GetArticlesForModerationAsync()
        {
            return await context.Articles.Where(d => !d.isModerated).Include(c => c.Writer).AsNoTracking().ToListAsync();
        }

        public async Task<Article> GetArticleByIDAsync(int? id)
        {
            return await context.Articles.Include(c => c.Writer).FirstOrDefaultAsync(m => m.ArticleID == id);
        }

        public async Task<Article> GetArticleByWriterIDAsync(int currentUserID, int? id)
        {
            return await context.Articles.Where(x => x.WriterId == currentUserID).AsNoTracking().FirstOrDefaultAsync(m => m.ArticleID == id);
        }

        public async Task<Writer> GetWriterByIDAsync(int id)
        {
            return await context.Writers.FindAsync(id);
        }

        public void AddArticleAsync(Article article)
        {
            context.Add(article);
        }

        public void RemoveArticleAsync(Article article)
        {
            context.Articles.Remove(article);
        }

        public IQueryable<Writer> GetWritersAsync()
        {
            return context.Writers.AsQueryable();
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}