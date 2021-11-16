using Microsoft.EntityFrameworkCore;
using SSSB.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Repositories
{
    public interface IProductCategoriesRepository
    {
        Task DeleteAsync(ProductCategory productCategory);
        Task<List<ProductCategory>> GetAllAsync();
        Task<ProductCategory> GetAsync(int productCategoryId);
        Task<ProductCategory> InsertAsync(ProductCategory productCategory, string addUserId);
        Task<ProductCategory> UpdateAsync(ProductCategory productCategory);
    }

    public class ProductCategoriesRepository : IProductCategoriesRepository
    {
        private readonly SSSBRestContext _sSSBRestContext;

        public ProductCategoriesRepository(SSSBRestContext sSSBRestContext)
        {
            _sSSBRestContext = sSSBRestContext;
        }

        public async Task<ProductCategory> GetAsync(int productCategoryId)
        {
            var productCategory = await _sSSBRestContext.ProductCategories.FirstOrDefaultAsync(o => o.Id == productCategoryId);
            return productCategory;
            //return new ProductCategory()
            //{
            //    Name = "name",
            //    Description = "desc",
            //    //CreationTimeUtc = DateTime.UtcNow
            //};
        }

        public async Task<List<ProductCategory>> GetAllAsync()
        {
            return await _sSSBRestContext.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> InsertAsync(ProductCategory productCategory, string addUserId)
        {
            productCategory.UserId = addUserId;
            //var a = productCategory.User.Id = user;
            _sSSBRestContext.ProductCategories.Add(productCategory);
            await _sSSBRestContext.SaveChangesAsync();
            return productCategory;
        }

        public async Task<ProductCategory> UpdateAsync(ProductCategory productCategory)
        {
            _sSSBRestContext.ProductCategories.Update(productCategory);
            await _sSSBRestContext.SaveChangesAsync();
            return productCategory;
        }

        public async Task DeleteAsync(ProductCategory productCategory)
        {
            _sSSBRestContext.ProductCategories.Remove(productCategory);
            await _sSSBRestContext.SaveChangesAsync();
        }
    }
}
