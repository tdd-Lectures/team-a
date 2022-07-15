using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Scheduler.Tests
{
    // API
    // CreateAppointment(...)
    // GetAppointments(...)
    public class Tests
    {
        [Test]
        public void Creating_an_appointment_returns_the_new_appointment_identifier()
        {
            var scheduler = MakeSchedulerManager();

            var myDate = DateTime.Now;

            var identifier = scheduler.CreateAppointment(MakeNewAppointment(myDate));

            Assert.That(identifier, Is.Not.Null);
        }

        [Test]
        public void Creating_an_appointment_persists_the_new_appointment()
        {
            var scheduler = MakeSchedulerManager();

            var identifier = scheduler.CreateAppointment(MakeNewAppointment(
                new DateTime(2020, 1, 1),
                "Meeting",
                60,
                new[] {"user1"},
                "PINTA-LX"
            ));

            var appointment = scheduler.GetById(identifier);

            RecordsAssert.AreEqual(appointment, new Appointment
            {
                identifier = 1,
                datetime = new DateTime(2020, 1, 1),
                subject = "Meeting",
                durationInMinutes = 60,
                attendees = new[] {"user1"},
                location = "PINTA-LX",
            });
        }

        [Test]
        public void Getting_by_id_non_existing_appointment_returns_null()
        {
            var scheduler = MakeSchedulerManager();

            var appointment = scheduler.GetById(1);

            Assert.That(appointment, Is.Null);
        }

        [Test]
        public void Creating_2_appointments_returns_2_distinct_identifiers()
        {
            var scheduler = MakeSchedulerManager();
            var myDate = DateTime.Now;

            var appointment1Id = scheduler.CreateAppointment(MakeNewAppointment(myDate));
            var appointment2Id = scheduler.CreateAppointment(MakeNewAppointment(myDate));

            Assert.That(appointment1Id, Is.Not.EqualTo(appointment2Id));
        }

        [Test]
        public void Creating_1_appointment_with_invalid_datetime_return_error()
        {
            var scheduler = MakeSchedulerManager();

            var myDate = new DateTime();

            Assert.Throws<InvalidDateException>(() =>
                scheduler.CreateAppointment(MakeNewAppointment(myDate)));
        }

        [Test]
        public void Creating_1_appointment_with_empty_attendees_return_error()
        {
            var scheduler = MakeSchedulerManager();

            var myDate = DateTime.Now;

            Assert.Throws<MissingAttendeesException>(() =>
                scheduler.CreateAppointment(MakeNewAppointment(myDate, attendees: Array.Empty<string>())));
        }

        [Test]
        public void Getting_appointments_for_today_returns_todays_appointments()
        {
            var scheduler = MakeSchedulerManager();
            var myDate = DateTime.Now;
            var subject = "meeting";
            var durationInMinutes = 45;
            var attendees = new[] {"user"};
            var location = "teams";

            scheduler.CreateAppointment(MakeNewAppointment(myDate, subject, durationInMinutes));
            scheduler.CreateAppointment(MakeNewAppointment(myDate.AddDays(1), subject, durationInMinutes));

            List<Appointment> appointmentsList = scheduler.GetAppointmentsByDate(myDate);

            RecordsAssert.AreEqual<IEnumerable<Appointment>>(appointmentsList, new[]
            {
                new Appointment
                {
                    identifier = 1,
                    datetime = myDate,
                    subject = subject,
                    durationInMinutes = durationInMinutes,
                    attendees = attendees,
                    location = location
                }
            });
        }

        [Test]
        public void Creating_recurrent_appointment_returns_1_recurrent_appointment_identifier()
        {
            var scheduler = MakeSchedulerManager();

            var recurrentAppointmentIdentifier = scheduler.CreateRecurrentAppointment(MakeNewRecurrentAppointment(
                new DateTime(2020, 1, 1),
                "Meeting",
                60,
                new[] {"user1"},
                "PINTA-LX",
                periodicity: new Periodicity(
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 16),
                    WeekDay.Monday
                )
            ));

            Assert.That(recurrentAppointmentIdentifier, Is.Not.Null);
        }

        private NewRecurrentAppointment MakeNewRecurrentAppointment(DateTime startDateTime, string subject,
            int durationInMinutes, string[] attendees, string location, Periodicity periodicity)
        {
            return new NewRecurrentAppointment(
                startDateTime, subject, 
                durationInMinutes, 
                attendees ?? new[] {"user"},
                location, 
                periodicity);
        }

        private static NewAppointment MakeNewAppointment(DateTime myDate, string subject = "meeting",
            int durationInMinutes = 45,
            IEnumerable<string> attendees = null, string location = "teams")
        {
            return new NewAppointment
            {
                MyDate = myDate,
                Subject = subject,
                DurationInMinutes = durationInMinutes,
                Attendees = attendees ?? new[] {"user"},
                Location = location
            };
        }

        private static SchedulerManager MakeSchedulerManager()
        {
            var scheduler = new SchedulerManager(new AppointmentsGateway());
            return scheduler;
        }
    }
}