using allinfo.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace allinfo.Data
{
    public interface ICommentsRepository
    {
        void AddCommentAsync(Comment comment);
        void RemoveCommentAsync(Comment comment);
        Task<Comment> GetCommentByIDAsync(int id);
        Task<int> SaveAsync();
    }
}