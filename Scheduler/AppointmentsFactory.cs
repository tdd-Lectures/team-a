using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler.Tests
{
    public class AppointmentsFactory
    {
        public IEnumerable<Appointment> Create(RecurrentAppointment recurrentAppointment)
        {
            if (!recurrentAppointment.weekdays.Any()) throw new InvalidReccurrencyException();
            ValidateAttendees(recurrentAppointment);
            var appointments = recurrentAppointment.GetPeriodicityDates()
                .Select(day => new Appointment
                {
                    Datetime = day,
                    attendees = recurrentAppointment.attendees,
                    subject = recurrentAppointment.subject,
                    location = recurrentAppointment.location,
                    durationInMinutes = recurrentAppointment.durationInMinutes
                });
            return xpto(appointments);
        }

        private static void ValidateAttendees(RecurrentAppointment recurrentAppointment)
        {
            if (recurrentAppointment.attendees == Array.Empty<string>())
            {
                throw new Exception("Attendees required!");
            }
        }

        private static IEnumerable<Appointment> xpto(IEnumerable<Appointment> appointments)
        {
            var enumerable = appointments.ToList();
            if (!enumerable.Any()) throw new InvalidReccurrencyException();
            return enumerable;
        }
    }
}