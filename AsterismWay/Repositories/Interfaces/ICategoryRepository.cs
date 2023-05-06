using AsterismWay.Data.Entities.CategoryEntity;

namespace AsterismWay.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetCategoryByName(string name);
        Task<int> SaveChangesAsync();
    }
}
