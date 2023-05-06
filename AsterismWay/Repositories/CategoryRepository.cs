using AsterismWay.Data;
using AsterismWay.Data.Entities;
using AsterismWay.Data.Entities.CategoryEntity;
using AsterismWay.Repositories.Interfaces;

namespace AsterismWay.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        protected readonly AsterismWayDbContext _dbContext;
        public CategoryRepository(AsterismWayDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return _dbContext.Categories.FirstOrDefault(x => x.Name == name);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
