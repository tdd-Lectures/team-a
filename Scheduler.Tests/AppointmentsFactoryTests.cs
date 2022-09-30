using System;
using System.Linq;
using NUnit.Framework;

namespace Scheduler.Tests
{
    public class AppointmentsFactoryTests
    {

        [Test]
        public void Receiving_1_recurrent_appointment_with_no_participant_return_exception()
        {
            var factory = new AppointmentsFactory();

            var recurrentAppointment = new RecurrentAppointment
            {
                subject = "subject",
                location = "Teams",
                durationInMinutes = 60,
                attendees = Array.Empty<string>(),
                periodicity = new Periodicity(
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 1),
                    new[]
                    {
                        WeekDay.Wednesday,
                    }
                )
            };
            var exception = Assert.Throws<Exception>(() => factory.Create(recurrentAppointment).ToArray());
            Assert.That(exception.Message, Is.EqualTo("Attendees required!"));
           
        }
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
        public void Receiving_1_recurrent_appointment_for_6_weeks_returns_6_appointments()
        {
            var factory = new AppointmentsFactory();

            var recurrentAppointment = new RecurrentAppointment()
            {
                subject = "TDD Course",
                location = "Teams",
                durationInMinutes = 120,
                attendees = new[] { "user1" },
                periodicity = new Periodicity(
                    new DateTime(2022, 9, 30),
                    new DateTime(2022, 11, 4),
                    new[]
                    {
                        WeekDay.Friday,
                    }
                )
            };

            var appointments = factory.Create(recurrentAppointment);
            
            Assert.That(appointments.Count(), Is.EqualTo(6));

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
        
        [Test]
        public void Receiving_1_recurrent_appointment_with_weekday_not_in_period_throws_invalid_reccurrency_exception()
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
                        WeekDay.Friday,
                    }
                )
            };
            Assert.Throws<InvalidReccurrencyException>(() => factory.Create(recurrentAppointment));
        }
    }
}