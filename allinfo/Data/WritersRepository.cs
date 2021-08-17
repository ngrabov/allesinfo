using allinfo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace allinfo.Data
{
    public class WritersRepository : IWritersRepository
    {
        private NewsContext context;

        public WritersRepository(NewsContext context)
        {
            this.context = context;
        }

        public IQueryable<Writer> GetWritersAsync()
        {
            return context.Writers.Include(w => w.Articles).AsQueryable();
        }

        public async Task<Writer> GetWriterByIDAsync(int? id)
        {
            return await context.Writers.Include(w => w.Articles).FirstOrDefaultAsync(m => m.Id == id);
        }

        public void AddWriterAsync(Writer writer)
        {
            context.Add(writer);
        }

        public void RemoveWriterAsync(Writer writer)
        {
            context.Writers.Remove(writer);
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}