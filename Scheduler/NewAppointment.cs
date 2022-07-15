using System;
using System.Collections.Generic;

namespace Scheduler
{
    public record NewAppointment
    {
        public DateTime MyDate { get; init; }
        public string Subject { get; init; }
        public int DurationInMinutes { get; init; }
        public IEnumerable<string> Attendees { get; init; }
        public string Location { get; init; }
        public Periodicity Periodicity { get; init;  }
    }
}