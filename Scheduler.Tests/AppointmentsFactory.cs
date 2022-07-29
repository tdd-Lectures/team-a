using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Scheduler.Tests
{
    public class AppointmentsFactory
    {
        public IEnumerable<Appointment> Create(RecurrentAppointment recurrentAppointment)
        {
            if (!recurrentAppointment.weekdays.Any()) throw new InvalidReccurrencyException();
            
            return recurrentAppointment.GetPeriodicityDates()
                .Select(day => new Appointment
                {
                    Datetime = day,
                    attendees = recurrentAppointment.attendees,
                    subject = recurrentAppointment.subject,
                    location = recurrentAppointment.location,
                    durationInMinutes = recurrentAppointment.durationInMinutes
                });
        }
    }

    public class AppointmentsFactoryTests
    {
        [Test]
        public void Receiving_1_recurrent_appointment_within_1_day_period_returns_1_appointment()
        {
            var factory = new AppointmentsFactory();

            var recurrentAppointment = new RecurrentAppointment
            {
                subject = "subject",
                location = "Teams",
                durationInMinutes = 60,
                attendees = new[] {"user1"},
                periodicity = new Periodicity(
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 1),
                    new[]
                    {
                        WeekDay.Wednesday,
                    }
                )
            };
            var appointments = factory.Create(recurrentAppointment).ToArray();

            Assert.That(appointments.Count(), Is.EqualTo(1));
            RecordsAssert.AreEqual(appointments, new[]
            {
                new Appointment
                {
                    Datetime = new DateTime(2020, 1, 1),
                    attendees = new[] {"user1"},
                    subject = "subject",
                    location = "Teams",
                    durationInMinutes = 60
                }
            });
        }

        [Test]
        public void Receiving_1_recurrent_appointment_within_2_day_period_returns_2_appointment()
        {
            var factory = new AppointmentsFactory();

            var recurrentAppointment = new RecurrentAppointment
            {
                subject = "subject",
                location = "Teams",
                durationInMinutes = 60,
                attendees = new[] {"user1"},
                periodicity = new Periodicity(
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 2),
                    new[]
                    {
                        WeekDay.Wednesday,
                        WeekDay.Thursday,
                    }
                )
            };
            var appointments = factory.Create(recurrentAppointment).ToArray();

            RecordsAssert.AreEqual(appointments, new[]
            {
                new Appointment
                {
                    Datetime = new DateTime(2020, 1, 1),
                    attendees = new[] {"user1"},
                    subject = "subject",
                    location = "Teams",
                    durationInMinutes = 60
                },
                new Appointment
                {
                    Datetime = new DateTime(2020, 1, 2),
                    attendees = new[] {"user1"},
                    subject = "subject",
                    location = "Teams",
                    durationInMinutes = 60
                },
            });
        }

        [Test]
        public void Receiving_1_recurrent_appointment_within_3_day_period_returns_3_appointment()
        {
            var factory = new AppointmentsFactory();

            var recurrentAppointment = new RecurrentAppointment
            {
                subject = "subject",
                location = "Teams",
                durationInMinutes = 60,
                attendees = new[] {"user1"},
                periodicity = new Periodicity(
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 3),
                    new[]
                    {
                        WeekDay.Wednesday,
                        WeekDay.Thursday,
                        WeekDay.Friday,
                    }
                )
            };
            var appointments = factory.Create(recurrentAppointment).ToArray();

            RecordsAssert.AreEqual(appointments, new[]
            {
                new Appointment
                {
                    Datetime = new DateTime(2020, 1, 1),
                    attendees = new[] {"user1"},
                    subject = "subject",
                    location = "Teams",
                    durationInMinutes = 60
                },
                new Appointment
                {
                    Datetime = new DateTime(2020, 1, 2),
                    attendees = new[] {"user1"},
                    subject = "subject",
                    location = "Teams",
                    durationInMinutes = 60
                },
                new Appointment
                {
                    Datetime = new DateTime(2020, 1, 3),
                    attendees = new[] {"user1"},
                    subject = "subject",
                    location = "Teams",
                    durationInMinutes = 60
                },
            });
        }

        [Test]
        public void Receiving_1_recurrent_appointment_within_2_day_period_and_weekday_in_period_returns_1_appointment()
        {
            var factory = new AppointmentsFactory();

            var recurrentAppointment = new RecurrentAppointment
            {
                subject = "subject",
                location = "Teams",
                durationInMinutes = 60,
                attendees = new[] {"user1"},
                periodicity = new Periodicity(
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 2),
                    new[]
                    {
                        WeekDay.Wednesday,
                    }
                )
            };
            var appointments = factory.Create(recurrentAppointment).ToArray();

            RecordsAssert.AreEqual(appointments, new[]
            {
                new Appointment
                {
                    Datetime = new DateTime(2020, 1, 1),
                    attendees = new[] {"user1"},
                    subject = "subject",
                    location = "Teams",
                    durationInMinutes = 60
                },
            });
        }
        
        [Test]
        public void Receiving_1_recurrent_appointment_without_weekday_throws_invalid_reccurrency_exception()
        {
            var factory = new AppointmentsFactory();

            var recurrentAppointment = new RecurrentAppointment
            {
                subject = "subject",
                location = "Teams",
                durationInMinutes = 60,
                attendees = new[] {"user1"},
                periodicity = new Periodicity(
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 2),
                    Array.Empty<WeekDay>()
                )
            };
            Assert.Throws<InvalidReccurrencyException>(() => factory.Create(recurrentAppointment));
        }
    }

    public class InvalidReccurrencyException : Exception
    {
    }

    public class RecurrentAppointment
    {
        internal Periodicity periodicity;
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