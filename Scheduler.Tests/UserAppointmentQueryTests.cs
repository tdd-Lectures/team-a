using System;
using System.Linq;
using NUnit.Framework;

namespace Scheduler.Tests
{
    public class UserAppointmentQueryTests
    {
        private static SchedulerManager CreateSchedulerManager()
        {
            var fakeAppointmentsGateway = new FakeAppointmentsGateway();
            fakeAppointmentsGateway.AddAppointment(new Appointment
            {
                attendees = new []{"userWithOneAppointment","userWithTwoAppointments"}
            });
            fakeAppointmentsGateway.AddAppointment(new Appointment
            {
                attendees = new []{"userWithTwoAppointments"}
            });

            var fakeRecurrentAppointmentGateway = new FakeRecurrentAppointmentGateway();
            fakeRecurrentAppointmentGateway.AddRecurrentAppointment(new RecurrentAppointment());
            return new SchedulerManager(fakeAppointmentsGateway, fakeRecurrentAppointmentGateway);
        }
        
        [Test]
        public void Getting_user_appointments_for_a_user_with_no_appointments_returns_empty_list()
        {
            SchedulerManager schedulerManager = CreateSchedulerManager();
            var appointments = schedulerManager.GetAppointmentsByUser("user1");

            Assert.That(appointments, Is.Empty);
        }


        [Test]
        public void Getting_user_appointments_for_a_user_with_1_appointment_returns_1_appointment()
        {
            SchedulerManager schedulerManager = CreateSchedulerManager();
            var appointments = schedulerManager.GetAppointmentsByUser("userWithOneAppointment");

            Assert.That(appointments.Count(), Is.EqualTo(1));
        }
        
        [Test]
        public void Getting_user_appointments_for_a_user_with_2_appointment_returns_2_appointment()
        {
            SchedulerManager schedulerManager = CreateSchedulerManager();
            var appointments = schedulerManager.GetAppointmentsByUser("userWithTwoAppointments");

            Assert.That(appointments.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Getting_user_appointments_from_failing_datasource_throws_exception()
        {
            SchedulerManager schedulerManager = CreateSchedulerManager();
            Assert.Throws<GatewayException>(() => schedulerManager.GetAppointmentsByUser("fail"));
        }
        
        [Test]
        public void Getting_user_appointments_for_null_user_throws_exception()
        {
            SchedulerManager schedulerManager = CreateSchedulerManager();
            Assert.Throws<InvalidUserException>(() => schedulerManager.GetAppointmentsByUser(null));
        }
        
        [Test]
        public void Getting_user_appointments_for_user_with_recurrent_appointment_returns_appointments()
        {
            SchedulerManager schedulerManager = CreateSchedulerManager();
            
            var appointments = schedulerManager.GetAppointmentsByUser("userWithRecurrentAppointment");

            Assert.That(appointments.Count(), Is.EqualTo(1));
        }
    }
}