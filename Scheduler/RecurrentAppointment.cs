using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler.Tests
{
    public class RecurrentAppointment
    {
        public Periodicity periodicity;
        public IEnumerable<string> attendees { get; set; }
        public string subject { get; set; }
        public string location { get; set; }
        public int durationInMinutes { get; set; }
        private DateTime startDateTime => periodicity._startDateTime;
        private DateTime endDateTime => periodicity._endDateTime;
        public IEnumerable<WeekDay> weekdays => periodicity._weekdays;

        private int DurationInDays =>
            (endDateTime - startDateTime).Days;

        private bool DoesItOccurIn(DayOfWeek dayDayOfWeek) =>
            weekdays.Contains((WeekDay) dayDayOfWeek);

        public IEnumerable<DateTime> GetPeriodicityDates() =>
            Enumerable.Range(0, DurationInDays + 1)
                .Select(i => startDateTime.AddDays(i))
                .Where(day => DoesItOccurIn(day.DayOfWeek));
    }
}