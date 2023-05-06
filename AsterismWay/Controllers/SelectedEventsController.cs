using AsterismWay.DTOs;
using AsterismWay.Services;
using AsterismWay.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsterismWay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SelectedEventsController : ControllerBase
    {
        protected readonly ISelectedEventsService _selectedEventsService;
        public SelectedEventsController(ISelectedEventsService selectedEventsService)
        {
            _selectedEventsService = selectedEventsService;
        }

        [HttpGet]
        [Route("collection/byUser")]
        public async Task<IActionResult> GetEventsByUser()
        {
            return Ok(await _selectedEventsService.GetEventsByUser());
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] SelectedEventsDto Event)
        {
            try
            {
                return Ok(await _selectedEventsService.CreateSelectedEventAsync(Event));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSelectedEvent([FromRoute] int id)
        {
            try
            {
                var deletedEvent = await _selectedEventsService.DeleteEventAsync(id);

                return Ok(deletedEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
