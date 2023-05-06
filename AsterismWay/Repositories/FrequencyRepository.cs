using AsterismWay.Data.Entities.CategoryEntity;
using AsterismWay.Data;
using AsterismWay.Repositories.Interfaces;
using AsterismWay.Data.Entities.FrequencyEntity;

namespace AsterismWay.Repositories
{
    public class FrequencyRepository : IFrequencyRepository
    {
        protected readonly AsterismWayDbContext _dbContext;
        public FrequencyRepository(AsterismWayDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Frequency> GetByIdAsync(int id)
        {
            return await _dbContext.Frequencies.FindAsync(id);
        }

        public async Task<Frequency> GetFrequencyByName(string name)
        {
            return _dbContext.Frequencies.FirstOrDefault(x => x.Name == name);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
