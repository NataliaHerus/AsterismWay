using AsterismWay.DTOs;

namespace AsterismWay.Services.Interfaces
{
    public interface IEventService
    {
        Task<EventDto> CreateEventAsync(EventDto dto);
        Task<EventDto> DeleteEventAsync(int id);
        Task<EventDto> UpdateEventAsync(EventDto dto);
        Task<EventDto> GetEventById(int id);
        Task<List<EventDto>> GetEvents();
        Task<List<EventDto>> GetClosest();
    }
}
