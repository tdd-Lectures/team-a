using System;

namespace Scheduler
{
    public class NewRecurrentAppointment
    {
        public DateTime StartDateTime { get; }
        public string Subject { get; }
        public int DurationInMinutes { get; }
        public string[] Attendees { get; }
        public string Location { get; }
        public Periodicity Periodicity { get; }

        public NewRecurrentAppointment(DateTime startDateTime, string subject, int durationInMinutes, string[] attendees, string location, Periodicity periodicity)
        {
            StartDateTime = startDateTime;
            Subject = subject;
            DurationInMinutes = durationInMinutes;
            Attendees = attendees;
            Location = location;
            Periodicity = periodicity;
        }
    }
}