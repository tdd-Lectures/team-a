using System;

namespace Scheduler.Tests
{
    // TODO: we changed from Class to Record
    public record Appointment
    {
        public object identifier{get;set;}
        
        public DateTime datetime{get;set;}

        public String subject;

        public int durationInMinutes { get; set; }
    }
}