using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
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

        public Appointment GetById(object identifier)
        {
            return _appointmentsGateway.GetAppointmentById(identifier);
        }

        public List<Appointment> GetAppointmentsByDate(DateTime dateTime)
        {
            return _appointmentsGateway.GetAppointmentsByDate(dateTime);
        }

        public int CreateRecurrentAppointment(NewRecurrentAppointment newRecurrentAppointment)
        {
            // DateTime startDateTime = newRecurrentAppointment.Periodicity._startDateTime;
            // DateTime endDateTime = newRecurrentAppointment.Periodicity._endDateTime;
            //
            return 1;
        }
    }
}