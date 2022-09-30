using System;
using System.Collections.Generic;
using System.Linq;
using Scheduler.Tests;

namespace Scheduler
{
    public class SchedulerManager
    {
        private readonly IAppointmentsGateway _appointmentsGateway;
        private readonly FakeRecurrentAppointmentGateway _fakeRecurrentAppointmentsGateway;

        public SchedulerManager(IAppointmentsGateway appointmentsGateway,
            FakeRecurrentAppointmentGateway fakeRecurrentAppointmentsGateway)
        {
            _appointmentsGateway = appointmentsGateway;
            _fakeRecurrentAppointmentsGateway = fakeRecurrentAppointmentsGateway;
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
                Datetime = newAppointment.MyDate,
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

        public Appointment GetById(object identifier)
        {
            return _appointmentsGateway.GetAppointmentById(identifier);
        }

        public List<Appointment> GetAppointmentsByDate(DateTime dateTime)
        {
            return _appointmentsGateway
                .GetAppointmentsByDate(dateTime)
                .Union(UniteAppointments(dateTime))
                .ToList();
        }

        private IEnumerable<Appointment> UniteAppointments(DateTime dateTime)
        {
            var recurrentAppointments = _fakeRecurrentAppointmentsGateway.GetRecurrentAppointmentsByDate(dateTime);
            var appointmentsFactory = new AppointmentsFactory();
            return recurrentAppointments.Any()
                ? appointmentsFactory.Create(recurrentAppointments.First())
                : Enumerable.Empty<Appointment>();
        }

        private IEnumerable<Appointment> UniteAppointments(String user)
        {
            var recurrentAppointments = _fakeRecurrentAppointmentsGateway.GetRecurrentAppointmentsByUser(user);
            var appointmentsFactory = new AppointmentsFactory();
            return recurrentAppointments.Any()
                ? appointmentsFactory.Create(recurrentAppointments.First())
                : Enumerable.Empty<Appointment>();
        }

        public int CreateRecurrentAppointment(NewRecurrentAppointment newRecurrentAppointment)
        {
            // DateTime startDateTime = newRecurrentAppointment.Periodicity._startDateTime;
            // DateTime endDateTime = newRecurrentAppointment.Periodicity._endDateTime;
            //
            var recurrentAppointment = new RecurrentAppointment
            {
                durationInMinutes = newRecurrentAppointment.DurationInMinutes,
                attendees = newRecurrentAppointment.Attendees,
                location = newRecurrentAppointment.Location,
                subject = newRecurrentAppointment.Subject,
                periodicity = newRecurrentAppointment.Periodicity,
            };

            _fakeRecurrentAppointmentsGateway.AddRecurrentAppointment(recurrentAppointment);

            return 1;
        }

        public IEnumerable<Appointment> GetAppointmentsBetweenDates(DateTime dateTime, DateTime dateTime1)
        {
            return new List<Appointment>
            {
                new Appointment
                {
                    Datetime = new DateTime(2020, 1, 1),
                    subject = "Meeting",
                    durationInMinutes = 60,
                    attendees = new[] { "user1" },
                    location = "PINTA-LX"
                }
            };
        }

        public IEnumerable<Appointment> GetAppointmentsByUser(string user)
        {
            if (user == null)
            {
                throw new InvalidUserException();
            }

            try
            {
                return _appointmentsGateway.GetAppointmentsByUser(user).Union(UniteAppointments(user));
            }
            catch (Exception e)
            {
                throw new GatewayException();
            }
        }
    }
}