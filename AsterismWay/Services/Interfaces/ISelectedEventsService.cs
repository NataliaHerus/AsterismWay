﻿using AsterismWay.DTOs;

namespace AsterismWay.Services.Interfaces
{
    public interface ISelectedEventsService
    {
        Task<IEnumerable<EventDto>> GetEventsByUser();
        Task<SelectedEventsDto> DeleteEventAsync(int id);
        Task<SelectedEventsDto> CreateSelectedEventAsync(SelectedEventsDto dto);
    }
}
