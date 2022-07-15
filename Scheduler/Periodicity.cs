using System;

namespace Scheduler
{
    public class Periodicity
    {
       public readonly DateTime _startDateTime;
       public readonly DateTime _endDateTime;
       public readonly WeekDay _weekday;

        public Periodicity(DateTime startDateTime, DateTime endDateTime, WeekDay weekday)
        {
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
            _weekday = weekday;
        }
    }
}