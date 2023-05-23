using AsterismWay.DTOs;
using AsterismWay.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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
        [Route("collection/closest")]
        public async Task<IActionResult> GetClosest()
        {
            return Ok(await _eventService.GetClosest());
        }

        [HttpGet]
        [Route("get/{id}")]
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
        [Route("delete/{id}")]
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

        [HttpPut]
        [Route("save/file")]
        public async Task<IActionResult> OnFileUploadAsync([FromForm] PhotoFile file)
        {

            if (file.file.Length > 0)
            {
                var filePath = $@"D:\CourseWork\AsterismWay\photos{file.file.FileName}";

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.file.CopyToAsync(stream);
                }

            }

            return Ok();
        }
    }

    public class PhotoFile
    {
        public IFormFile file { get; set; }
    }

}
