using System;
using System.Collections.Generic;

namespace Scheduler
{
    public class Periodicity
    {
       public readonly DateTime _startDateTime;
       public readonly DateTime _endDateTime;
       public readonly IEnumerable<WeekDay> _weekdays;

        public Periodicity(DateTime startDateTime, DateTime endDateTime, IEnumerable<WeekDay> weekdays)
        {
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
            _weekdays = weekdays;
        }
    }
}