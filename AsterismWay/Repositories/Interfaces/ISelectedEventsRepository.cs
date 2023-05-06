using AsterismWay.Data.Entities;

namespace AsterismWay.Repositories.Interfaces
{
    public interface ISelectedEventsRepository
    {
        Task<List<SelectedEvents>> GetEventsByUserId(string userId);
        Task<SelectedEvents> AddSelectedEventsAsync(SelectedEvents selectedEvent);
        Task<SelectedEvents> DeleteAsync(SelectedEvents SelectedEvent);
        Task<SelectedEvents> GetSelectedEventById(int id);
        Task<int> SaveChangesAsync();
    }
}
