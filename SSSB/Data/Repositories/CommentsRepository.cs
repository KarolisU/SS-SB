using Microsoft.EntityFrameworkCore;
using SSSB.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Repositories
{
    public interface ICommentsRepository
    {
        Task DeleteAsync(Comment comment);
        Task<List<Comment>> GetAsync(int productCategoryId, int advertisementId);
        Task<Comment> GetAsync(int productCategoryId, int advertisementId, int commentId);
        Task InsertAsync(Comment comment, string addUserId);
        Task UpdateAsync(Comment comment);
    }

    public class CommentsRepository : ICommentsRepository
    {
        private readonly SSSBRestContext _sSSBRestContext;

        public CommentsRepository(SSSBRestContext sSSBRestContext)
        {
            _sSSBRestContext = sSSBRestContext;
        }

        public async Task<Comment> GetAsync(int productCategoryId, int advertisementId, int commentId)
        {
            //return await _sSSBRestContext.Comments.FirstOrDefaultAsync(o => o.Advertisement.ProductCategoryId == productCategoryId && o.AdvertisementId == advertisementId && o.Id == commentId);
            return await _sSSBRestContext.Comments.FirstOrDefaultAsync(o => o.Advertisement.ProductCategory.Id == productCategoryId && o.Advertisement.Id == advertisementId && o.Id == commentId);
        }

        public async Task<List<Comment>> GetAsync(int productCategoryId, int advertisementId)
        {
            //return await _sSSBRestContext.Comments.Where(o => o.ProductCategoryId == productCategoryId && o.AdvertisementId == advertisementId).ToListAsync();
            return await _sSSBRestContext.Comments.Where(o => o.Advertisement.ProductCategory.Id == productCategoryId && o.Advertisement.Id == advertisementId).ToListAsync();
        }

        public async Task InsertAsync(Comment comment, string addUserId)
        {
            comment.UserId = addUserId;
            _sSSBRestContext.Comments.Add(comment);
            await _sSSBRestContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _sSSBRestContext.Comments.Update(comment);
            await _sSSBRestContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
            _sSSBRestContext.Comments.Remove(comment);
            await _sSSBRestContext.SaveChangesAsync();
        }
    }
}
