using AsterismWay.DTOs;
using AsterismWay.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AsterismWay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {

        protected readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] EventDto Event)
        {
            try
            {
                return Ok(await _eventService.CreateEventAsync(Event));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("collection")]
        public async Task<IActionResult> GetEvents()
        {
            return Ok(await _eventService.GetEvents());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            return Ok(await _eventService.GetEventById(id));
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventDto dto)
        {
            return Ok(await _eventService.UpdateEventAsync(dto));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            try
            {
                var deletedEvent = await _eventService.DeleteEventAsync(id);

                return Ok(deletedEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
