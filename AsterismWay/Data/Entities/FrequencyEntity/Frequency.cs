namespace AsterismWay.Data.Entities.FrequencyEntity
{
    public class Frequency
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
