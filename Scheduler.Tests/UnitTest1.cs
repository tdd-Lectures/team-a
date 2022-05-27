using System;
using System.Collections.Generic;
using System.Linq;
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
            // TODO: Extract SchedulerManager creation from tests.
            var scheduler = new SchedulerManager();

            var myDate = DateTime.Now;
            var identifier = scheduler.CreateAppointment(myDate);

            Assert.That(identifier, Is.Not.Null);
        }
        
        [Test]
        public void Creating_an_appointment_persists_the_new_appointment()
        {
            var scheduler = new SchedulerManager();

            var myDate = DateTime.Now;
            var identifier = scheduler.CreateAppointment(myDate);
            // TODO: rename: appt -> appointment
            var appt = scheduler.GetById(identifier);

            Assert.That(appt, Is.Not.Null);
            Assert.That(appt, Is.TypeOf<Appointment>()); // TODO: Type Check
            Assert.That(appt.identifier, Is.EqualTo(identifier));
        }
        
        [Test]
        public void Getting_by_id_non_existing_appointment_returns_null()
        {
            var scheduler = new SchedulerManager();
            
            var appt = scheduler.GetById(1);
            
            Assert.That(appt, Is.Null);
        }
        
        [Test]
        public void Creating_2_appointments_returns_2_distinct_identifiers()
        {
            var scheduler = new SchedulerManager();
            var myDate = DateTime.Now;
            var appointment1Id = scheduler.CreateAppointment(myDate);
            var appointment2Id = scheduler.CreateAppointment(myDate);
            
            Assert.That(appointment1Id,Is.Not.EqualTo(appointment2Id));
        }

        [Test]
        public void Creating_1_appointment_with_invalid_datetime_return_error()
        {
            var scheduler = new SchedulerManager();
       
            var myDate = new DateTime();
            //TODO: Create a specific exception for this.
            Assert.Throws<System.Exception>(() => scheduler.CreateAppointment(myDate));
        }
        
        [Test]
        public void Getting_appointments_for_today_returns_todays_appointments()
        {
            var scheduler = new SchedulerManager();
            var myDate = DateTime.Now;
            var identifier = scheduler.CreateAppointment(myDate);
            List<Appointment> appointmentsList = scheduler.GetAppointmentsByDate();

            // TODO: improve test feedback.
            var containsIdentifier = appointmentsList.Any((a) => a.identifier == identifier);
            Assert.That(containsIdentifier,Is.True);
        }
    }

    // TODO: rename to something useful
    public class XptoAppointmentClass
    {
        private List<Appointment> appointments = new List<Appointment>();

        public int AddAppointment(Appointment appointment)
        {
            appointments.Add(appointment);
            return appointments.Count;
        }

        // TODO: rename to something useful
        public Appointment? XptoAppointment(object identifier)
        {
            return appointments.FirstOrDefault(a => a.identifier == identifier);
        }
    }

    public class SchedulerManager
    { 
        // TODO: create abstraction
        // Violates: DIP, OCP
        private readonly XptoAppointmentClass _xptoAppointmentClass = new XptoAppointmentClass();

        public object CreateAppointment(DateTime myDate)
        {
            if(myDate == new DateTime())
                throw new System.Exception();
            var appointment = new Appointment();
            appointment.identifier = _xptoAppointmentClass.AddAppointment(appointment);
            return appointment.identifier;
        }

        internal Appointment GetById(object identifier)
        {
            return _xptoAppointmentClass.XptoAppointment(identifier);
        }

        public List<Appointment> GetAppointmentsByDate()
        {
            throw new NotImplementedException();
        }
    }
}