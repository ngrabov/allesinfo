using System.Threading.Tasks;
using allinfo.Models;

namespace allinfo.Data
{
    public class CommentsRepository : ICommentsRepository
    {
        private NewsContext context;

        public CommentsRepository(NewsContext context)
        {
            this.context = context;
        }

        public void AddCommentAsync(Comment comment)
        {
            context.Add(comment);
        }

        public void RemoveCommentAsync(Comment comment)
        {
            context.Comments.Remove(comment);
        }

        public async Task<Comment> GetCommentByIDAsync(int id)
        {
            return await context.Comments.FindAsync(id);
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}