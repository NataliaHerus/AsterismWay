using AsterismWay.Data;
using AsterismWay.Data.Entities;
using AsterismWay.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsterismWay.Repositories
{
    public class SelectedEventsRepository : ISelectedEventsRepository
    {
        protected readonly AsterismWayDbContext _dbContext;
        public SelectedEventsRepository(AsterismWayDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SelectedEvents> AddSelectedEventsAsync(SelectedEvents selectedEvent)
        {
            var lastEvent = await _dbContext.SelectedEvents.OrderBy(e => e.Id).LastOrDefaultAsync();
            selectedEvent.Id = lastEvent.Id + 1;
            await _dbContext.SelectedEvents.AddAsync(selectedEvent);
            await _dbContext.SaveChangesAsync();
            return selectedEvent;
        }

        public async Task<SelectedEvents> DeleteAsync(SelectedEvents SelectedEvent)
        {
            await Task.Run(() => _dbContext.SelectedEvents.Remove(SelectedEvent));
            return SelectedEvent;
        }
        public async Task<List<SelectedEvents>> GetEventsByUserId(string userId)
        {
            return  _dbContext.SelectedEvents.Where(x => x.UserId == userId).ToList();
        }

        public async Task<SelectedEvents> GetEvent(string userId, int eventId)
        {
            return await _dbContext.SelectedEvents.FirstOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId);
        }

        public async Task<SelectedEvents> GetSelectedEventById(int id)
        {
            return _dbContext.SelectedEvents.FirstOrDefault(x => x.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
