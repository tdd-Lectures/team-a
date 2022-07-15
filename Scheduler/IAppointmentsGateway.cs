using System;
using System.Collections.Generic;

namespace Scheduler
{
    public interface IAppointmentsGateway
    {
        int AddAppointment(Appointment appointment);
        Appointment GetAppointmentById(object identifier);
        List<Appointment> GetAppointmentsByDate(DateTime dateTime);
    }
}