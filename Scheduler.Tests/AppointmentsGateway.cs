using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler.Tests
{
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
}