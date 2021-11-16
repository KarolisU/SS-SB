using Microsoft.EntityFrameworkCore;
using SSSB.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Repositories
{
    public interface IAdvertisementsRepository
    {
        Task DeleteAsync(Advertisement advertisement);
        Task<List<Advertisement>> GetAsync(int productCategoryId);
        Task<Advertisement> GetAsync(int productCategoryId, int advertisementId);
        Task InsertAsync(Advertisement advertisement, string addUserId);
        Task UpdateAsync(Advertisement advertisement);
    }

    public class AdvertisementsRepository : IAdvertisementsRepository
    {
        private readonly SSSBRestContext _sSSBRestContext;

        public AdvertisementsRepository(SSSBRestContext sSSBRestContext)
        {
            _sSSBRestContext = sSSBRestContext;
        }

        public async Task<Advertisement> GetAsync(int productCategoryId, int advertisementId)
        {
            //return await _sSSBRestContext.Advertisements.FirstOrDefaultAsync(o => o.ProductCategoryId == productCategoryId && o.Id == advertisementId);
            return await _sSSBRestContext.Advertisements.FirstOrDefaultAsync(o => o.ProductCategory.Id == productCategoryId && o.Id == advertisementId);
        }

        public async Task<List<Advertisement>> GetAsync(int productCategoryId)
        {
            //return await _sSSBRestContext.Advertisements.Where(o => o.ProductCategoryId == productCategoryId).ToListAsync();
            return await _sSSBRestContext.Advertisements.Where(o => o.ProductCategory.Id == productCategoryId).ToListAsync();
        }

        public async Task InsertAsync(Advertisement advertisement, string addUserId)
        {
            advertisement.UserId = addUserId;
            _sSSBRestContext.Advertisements.Add(advertisement);
            await _sSSBRestContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Advertisement advertisement)
        {
            _sSSBRestContext.Advertisements.Update(advertisement);
            await _sSSBRestContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Advertisement advertisement)
        {
            _sSSBRestContext.Advertisements.Remove(advertisement);
            await _sSSBRestContext.SaveChangesAsync();
        }
    }
}
