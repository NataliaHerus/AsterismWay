﻿using AsterismWay.Data.Entities.CategoryEntity;
using AsterismWay.Data.Entities.FrequencyEntity;

namespace AsterismWay.Data.Entities
{
    public class Event
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Year { get; set; }
        public Frequency? Frequency { get; set; }
        public int? FrequencyId { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<SelectedEvents>? SelectedEvents { get; set; }
    }
}
