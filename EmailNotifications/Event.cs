using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifications
{
    public class Event
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Frequency { get; set; }
        public string Category { get; set; }
    }
}
