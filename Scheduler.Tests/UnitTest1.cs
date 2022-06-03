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
            var subject = "meeting";
            var durationInMinutes = 45;

            var identifier = scheduler.CreateAppointment(new NewAppointment(myDate, subject, durationInMinutes));

            Assert.That(identifier, Is.Not.Null);
        }
        
        [Test]
        public void Creating_an_appointment_persists_the_new_appointment()
        {
            var scheduler = new SchedulerManager();

            var myDate = DateTime.Now;
            var subject = "meeting";
            var durationInMinutes = 45;
            var identifier = scheduler.CreateAppointment(new NewAppointment(myDate, subject, durationInMinutes));
            var appointment = scheduler.GetById(identifier);

            Assert.That(appointment, Is.EqualTo(MakeAppointment(myDate, identifier, subject, durationInMinutes)));
        }
        
        [Test]
        public void Getting_by_id_non_existing_appointment_returns_null()
        {
            var scheduler = new SchedulerManager();
            
            var appointment = scheduler.GetById(1);
            
            Assert.That(appointment, Is.Null);
        }
        
        [Test]
        public void Creating_2_appointments_returns_2_distinct_identifiers()
        {
            var scheduler = new SchedulerManager();
            var myDate = DateTime.Now;
            var subject = "meeting";
            var durationInMinutes = 45;

            var appointment1Id = scheduler.CreateAppointment(new NewAppointment(myDate, subject, durationInMinutes));
            var appointment2Id = scheduler.CreateAppointment(new NewAppointment(myDate, subject, durationInMinutes));
            
            Assert.That(appointment1Id,Is.Not.EqualTo(appointment2Id));
        }

        [Test]
        public void Creating_1_appointment_with_invalid_datetime_return_error()
        {
            var scheduler = new SchedulerManager();
       
            var myDate = new DateTime();
            var subject = "meeting";
            var durationInMinutes = 45;

            //TODO: Create a specific exception for this.
            Assert.Throws<System.Exception>(() => scheduler.CreateAppointment(new NewAppointment(myDate, subject, durationInMinutes)));
        }
        
        [Test]
        public void Getting_appointments_for_today_returns_todays_appointments()
        {
            var scheduler = new SchedulerManager();
            var myDate = DateTime.Now;
            var subject = "meeting";
            var durationInMinutes = 45;

            var appointmentIdentifierToday = scheduler.CreateAppointment(new NewAppointment(myDate, subject, durationInMinutes));
            
            var appointmentIdentifierTomorrow = scheduler.CreateAppointment(new NewAppointment(myDate.AddDays(1), subject, durationInMinutes));
            List<Appointment> appointmentsList = scheduler.GetAppointmentsByDate(myDate);

            // TODO: improve test feedback.
            var containsIdentifier = appointmentsList.Any((a) => a.identifier == appointmentIdentifierToday);
            Assert.That(containsIdentifier,Is.True);
            var notContainsIdentifierTomorrow =
                appointmentsList.Any(a => a.identifier == appointmentIdentifierTomorrow);
            Assert.That(notContainsIdentifierTomorrow,Is.False);
        }

        public Appointment MakeAppointment(DateTime? datetime = null, object identifier = null,String subject = "Meeting", int durationInMinutes = 0)
        {

            return new Appointment { 
                subject = subject,
                datetime = datetime ?? new DateTime(2020, 1, 1),
                identifier = identifier ?? (object) 1,
                durationInMinutes = durationInMinutes,
            };
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
        
        public List<Appointment> GetAppointmentsByDate(DateTime dateTime)
        {
            return appointments.Where(a => a.datetime == dateTime).ToList();
        }
    }

    public class NewAppointment
    {
        public NewAppointment(DateTime myDate, string subject, int durationInMinutes)
        {
            MyDate = myDate;
            Subject = subject;
            DurationInMinutes = durationInMinutes;
        }

        public DateTime MyDate { get; private set; }
        public string Subject { get; private set; }
        public int DurationInMinutes { get; private set; }
    }

    public class SchedulerManager
    { 
        // TODO: create abstraction
        // Violates: DIP, OCP
        private readonly XptoAppointmentClass _xptoAppointmentClass = new XptoAppointmentClass();

        public object CreateAppointment(NewAppointment newAppointment)
        {
            if(newAppointment.MyDate == new DateTime())
                throw new System.Exception();
            var appointment = new Appointment
            {
                datetime = newAppointment.MyDate,
                subject = newAppointment.Subject,
                durationInMinutes = newAppointment.DurationInMinutes,
            };
            appointment.identifier = _xptoAppointmentClass.AddAppointment(appointment);
            return appointment.identifier;
        }

        internal Appointment GetById(object identifier)
        {
            return _xptoAppointmentClass.XptoAppointment(identifier);
        }

        public List<Appointment> GetAppointmentsByDate(DateTime dateTime)
        {
            return _xptoAppointmentClass.GetAppointmentsByDate(dateTime);
        }
    }
}