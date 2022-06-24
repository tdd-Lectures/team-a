using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

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
                location = "PINTA-LX"
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

        private static NewAppointment MakeNewAppointment(DateTime myDate, string subject = "meeting",
            int durationInMinutes = 45,
            IEnumerable<string>? attendees = null, string location = "teams")
        {
            return new NewAppointment(myDate, subject, durationInMinutes, attendees ?? new[] {"user"}, location);
        }

        private static SchedulerManager MakeSchedulerManager()
        {
            var scheduler = new SchedulerManager(new AppointmentsGateway());
            return scheduler;
        }
    }

    public class MissingAttendeesException : Exception
    {
    }

    public static class RecordsAssert
    {
        public static void AreEqual<T>(T actual, T expected)
        {
            var compareLogic = new CompareLogic
            {
                Config =
                {
                    IgnoreObjectTypes = true
                }
            };
            var result = compareLogic.Compare(expected, actual);
            if (!result.AreEqual)
                throw new AssertionException(result.DifferencesString);
        }
    }

    public class InvalidDateException : Exception
    {
    }

    public interface IAppointmentsGateway
    {
        int AddAppointment(Appointment appointment);
        Appointment GetAppointmentById(object identifier);
        List<Appointment> GetAppointmentsByDate(DateTime dateTime);
    }

    public class AppointmentsGateway : IAppointmentsGateway
    {
        private List<Appointment> appointments = new List<Appointment>();

        public int AddAppointment(Appointment appointment)
        {
            appointments.Add(appointment);
            return appointments.Count;
        }

        public Appointment? GetAppointmentById(object identifier)
        {
            return appointments.FirstOrDefault(a => a.identifier == identifier);
        }

        public List<Appointment> GetAppointmentsByDate(DateTime dateTime)
        {
            return appointments.Where(a => a.datetime == dateTime).ToList();
        }
    }

    public class NewAppointment
    {
        public NewAppointment(DateTime myDate, string subject, int durationInMinutes, IEnumerable<string> attendees,
            string location)
        {
            MyDate = myDate;
            Subject = subject;
            DurationInMinutes = durationInMinutes;
            Attendees = attendees;
            Location = location;
        }


        public DateTime MyDate { get; private set; }
        public string Subject { get; private set; }
        public int DurationInMinutes { get; private set; }
        public IEnumerable<string> Attendees { get; set; }
        public string Location { get; }
    }

    public class SchedulerManager
    {
        private readonly IAppointmentsGateway _appointmentsGateway;

        public SchedulerManager(IAppointmentsGateway appointmentsGateway)
        {
            _appointmentsGateway = appointmentsGateway;
        }

        public object CreateAppointment(NewAppointment newAppointment)
        {
            EnsureNewAppointmentIsValid(newAppointment);

            var appointment = MapNewAppointmentToAppointment(newAppointment);

            return StoreNewAppointment(appointment);
        }

        private object StoreNewAppointment(Appointment appointment)
        {
            appointment.identifier = _appointmentsGateway.AddAppointment(appointment);
            return appointment.identifier;
        }

        private static Appointment MapNewAppointmentToAppointment(NewAppointment newAppointment)
        {
            return new Appointment
            {
                datetime = newAppointment.MyDate,
                subject = newAppointment.Subject,
                durationInMinutes = newAppointment.DurationInMinutes,
                attendees = newAppointment.Attendees,
                location = newAppointment.Location,
            };
        }

        private static void EnsureNewAppointmentIsValid(NewAppointment newAppointment)
        {
            if (newAppointment.MyDate == new DateTime())
                throw new InvalidDateException();
            if (!newAppointment.Attendees.Any())
                throw new MissingAttendeesException();
        }

        internal Appointment GetById(object identifier)
        {
            return _appointmentsGateway.GetAppointmentById(identifier);
        }

        public List<Appointment> GetAppointmentsByDate(DateTime dateTime)
        {
            return _appointmentsGateway.GetAppointmentsByDate(dateTime);
        }
    }
}