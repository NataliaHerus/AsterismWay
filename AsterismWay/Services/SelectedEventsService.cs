using AsterismWay.Data;
using AsterismWay.Data.Entities;
using AsterismWay.DTOs;
using AsterismWay.Repositories.Interfaces;
using AsterismWay.Services.Interfaces;
using AutoMapper;

namespace AsterismWay.Services
{
    public class SelectedEventsService : ISelectedEventsService
    {
        protected readonly IEventRepository _eventRepository;
        protected readonly ISelectedEventsRepository _selectedEventsRepository;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public SelectedEventsService(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IEventRepository eventRepository, ISelectedEventsRepository selectedEventsRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _selectedEventsRepository = selectedEventsRepository;
        }
        public async Task<IEnumerable<EventDto>> GetEventsByUser()
        {
            string userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var selectedEvents = await _selectedEventsRepository.GetEventsByUserId(userId);
            List<EventDto> events = new List<EventDto>();
            foreach(var selectedEvent in selectedEvents)
            {
                events.Add(_mapper.Map<EventDto> ( await _eventRepository.GetEventById(selectedEvent.EventId)));
            }
            return events;
        }

        public async Task<SelectedEventsDto> CreateSelectedEventAsync(SelectedEventsDto dto)
        {
            var Event = _mapper.Map<SelectedEvents>(dto);
            await _selectedEventsRepository.AddSelectedEventsAsync(Event);
            await _selectedEventsRepository.SaveChangesAsync();
            return _mapper.Map<SelectedEventsDto>(Event);
        }

        public async Task<SelectedEventsDto> DeleteEventAsync(int id)
        {
            var Event = await _selectedEventsRepository.GetSelectedEventById(id);
            var deletedEvent = await _selectedEventsRepository.DeleteAsync(Event);
            await _selectedEventsRepository.SaveChangesAsync();

            return _mapper.Map<SelectedEventsDto>(deletedEvent);
        }
    }
}
