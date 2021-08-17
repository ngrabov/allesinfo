using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using allinfo.Models;

namespace allinfo.Data
{
    public interface IWritersRepository
    {
        IQueryable<Writer> GetWritersAsync();
        Task<Writer> GetWriterByIDAsync(int? id);
        void AddWriterAsync(Writer writer);
        void RemoveWriterAsync(Writer writer);
        Task<int> SaveAsync();
    }
}