using Microsoft.Extensions.Hosting;

namespace AsterismWay.Data.Entities
{
    public class SelectedEvents
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}
