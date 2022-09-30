using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler.Tests
{
    public class FakeAppointmentsGateway : IAppointmentsGateway
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
            return appointments.Where(a => a.Datetime == dateTime).ToList();
        }

        public IEnumerable<Appointment> GetAppointmentsByUser(string user)
        {
            if (user == "fail")
            {
                throw new Exception();
            }
            return appointments
                .Where((a)=> a.attendees == null || a.attendees.Contains(user));
        }
    }
}