using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Scheduler.Tests
{
    public class AppointmentsFactory
    {
        public IEnumerable<Appointment> Create(RecurrentAppointment recurrentAppointment)
        {
            yield return new Appointment
            {
                Datetime = recurrentAppointment.periodicity._startDateTime
            };
        }
    }

    public class AppointmentsFactoryTests
    {
        [Test]
        public void Receiving_1_recurrent_appointment_within_1_day_period_returns_1_appointment()
        {
            var factory = new AppointmentsFactory();

            var recurrentAppointment = new RecurrentAppointment
            {
                periodicity = new Periodicity(
                    new DateTime(2020, 1, 1),
                    new DateTime(2020, 1, 1),
                    new[]
                    {
                        WeekDay.Wednesday,
                    }
                )
            };
            var appointments = factory.Create(recurrentAppointment).ToArray();
            
            Assert.That(appointments.Count(), Is.EqualTo(1));
            RecordsAssert.AreEqual(appointments, new []
            {
                new Appointment
                {
                    Datetime = new DateTime(2020,1,1)
                }
            });
        }
    }

    public class RecurrentAppointment
    {
        internal Periodicity periodicity;
    }
}