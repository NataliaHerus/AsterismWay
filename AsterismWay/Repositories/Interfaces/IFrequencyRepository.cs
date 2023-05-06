using AsterismWay.Data.Entities.CategoryEntity;
using AsterismWay.Data.Entities.FrequencyEntity;

namespace AsterismWay.Repositories.Interfaces
{
    public interface IFrequencyRepository
    {
        Task<Frequency> GetByIdAsync(int id);

        Task<Frequency> GetFrequencyByName(string name);
        Task<int> SaveChangesAsync();
    }
}
