using System;
using System.Collections.Generic;

namespace Scheduler.Tests
{
    public record Appointment
    {
        public object identifier{get;set;}
        
        public DateTime datetime{get;set;}

        public String subject;

        public int durationInMinutes { get; set; }
        public IEnumerable<string> attendees { get; set; }
        public string location { get; set; }
    }
}