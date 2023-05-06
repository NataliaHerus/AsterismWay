using AsterismWay.Data;
using AsterismWay.Data.Entities;
using AsterismWay.Data.Entities.CategoryEntity;
using AsterismWay.Data.Entities.FrequencyEntity;
using AsterismWay.DTOs;
using AsterismWay.Repositories;
using AsterismWay.Repositories.Interfaces;
using AsterismWay.Services.Interfaces;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AsterismWay.Services
{
    public class EventService: IEventService
    {
        protected readonly IEventRepository _eventRepository;
        protected readonly ISelectedEventsRepository _selectedEventsRepository;
        protected readonly ICategoryRepository _categoryRepository;
        protected readonly IFrequencyRepository _frequencyRepository;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public EventService(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IEventRepository eventRepository, ISelectedEventsRepository selectedEventsRepository,
            ICategoryRepository categoryRepository, IFrequencyRepository frequencyRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _selectedEventsRepository = selectedEventsRepository;
            _categoryRepository = categoryRepository;
            _frequencyRepository = frequencyRepository;
        }

        public async Task<EventDto> CreateEventAsync(EventDto dto)
        {
            var Event = _mapper.Map<Event>(dto);
            Frequency frequency = await _frequencyRepository.GetFrequencyByName(Event.Frequency.Name);
            Event.Frequency = frequency;
            Category category = await _categoryRepository.GetCategoryByName(Event.Category.Name);
            Event.Category = category;
            await _eventRepository.AddEventAsync(Event);
            await _eventRepository.SaveChangesAsync();
            return _mapper.Map<EventDto>(Event);
        }

        public async Task<EventDto> UpdateEventAsync(EventDto dto)
        {
            var updatedEvent = _mapper.Map<Event>(dto);
            Frequency frequency = await _frequencyRepository.GetFrequencyByName(dto.Frequency.Name);
            updatedEvent.Frequency = frequency;
            Category category = await _categoryRepository.GetCategoryByName(dto.Category.Name);
            updatedEvent.Category = category;
            await _eventRepository.UpdateEventAsync(updatedEvent);
            await _eventRepository.SaveChangesAsync();
            return _mapper.Map<EventDto>(updatedEvent);
        }

        public async Task<EventDto> DeleteEventAsync(int id)
        {
            var Event =  await _eventRepository.GetEventById(id);
            if (Event == null)
            {
                throw new ArgumentException("Event not found");
            }
            var deletedEvent = await _eventRepository.DeleteAsync(Event);
            await _eventRepository.SaveChangesAsync();

            return _mapper.Map<EventDto>(deletedEvent);
        }
        public async Task<EventDto> GetEventById(int id)
        {
            var Event = await _eventRepository.GetEventById(id);
            return _mapper.Map<EventDto>(Event);
        }

        public async Task<List<EventDto>> GetEvents()
        {
            return _mapper.Map<List<EventDto>>(await _eventRepository.GetAllAsync());
        }
    }

    
}
