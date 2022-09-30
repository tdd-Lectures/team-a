using System;
using System.Collections.Generic;
using Scheduler.Tests;

namespace Scheduler
{
    public class FakeRecurrentAppointmentGateway
    {
        private List<RecurrentAppointment> _recurrentAppointmentList = new List<RecurrentAppointment>();

        public void AddRecurrentAppointment(RecurrentAppointment recurrentAppointment)
        {
            _recurrentAppointmentList.Add(recurrentAppointment);
        }

        public List<RecurrentAppointment> GetRecurrentAppointmentsByDate(DateTime dateTime)
        {
            return _recurrentAppointmentList;
        }

        public List<RecurrentAppointment> GetRecurrentAppointmentsByUser(string user)
        {
            return _recurrentAppointmentList;
        }
    }
}