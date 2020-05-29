using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data;
using VPM.Data.Entities;

namespace VPM.ViewComponents
{
    [ViewComponent(Name = "WeeklyView")]
    public class WeeklyViewViewComponents : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public IList<Reservation> ThisWeekReservations { get; set; }
        public IList<DateTime> ThisWeekDays { get; set; }

        public string[] Hours;

        public WeeklyViewViewComponents(ApplicationDbContext context)
        {
            _context = context;
        }

        public string[] HoursQuarter =
        {
            "12:00 AM",
            "12:15 AM",
            "12:30 AM",
            "12:45 AM",
            "1:00 AM",
            "1:15 AM",
            "1:30 AM",
            "1:45 AM",
            "2:00 AM",
            "2:15 AM",
            "2:30 AM",
            "2:45 AM",
            "3:00 AM",
            "3:15 AM",
            "3:30 AM",
            "3:45 AM",
            "4:00 AM",
            "4:15 AM",
            "4:30 AM",
            "4:45 AM",
            "5:00 AM",
            "5:15 AM",
            "5:30 AM",
            "5:45 AM",
            "6:00 AM",
            "6:15 AM",
            "6:30 AM",
            "6:45 AM",
            "7:00 AM",
            "7:15 AM",
            "7:30 AM",
            "7:45 AM",
            "8:00 AM",
            "8:15 AM",
            "8:30 AM",
            "8:45 AM",
            "9:00 AM",
            "9:15 AM",
            "9:30 AM",
            "9:45 AM",
            "10:00 AM",
            "10:15 AM",
            "10:30 AM",
            "10:45 AM",
            "11:00 AM",
            "11:15 AM",
            "11:30 AM",
            "11:45 AM",
            "12:00 PM",
            "12:15 PM",
            "12:30 PM",
            "12:45 PM",
            "1:00 PM",
            "1:15 PM",
            "1:30 PM",
            "1:45 PM",
            "2:00 PM",
            "2:15 PM",
            "2:30 PM",
            "2:45 PM",
            "3:00 PM",
            "3:15 PM",
            "3:30 PM",
            "3:45 PM",
            "4:00 PM",
            "4:15 PM",
            "4:30 PM",
            "4:45 PM",
            "5:00 PM",
            "5:15 PM",
            "5:30 PM",
            "5:45 PM",
            "6:00 PM",
            "6:15 PM",
            "6:30 PM",
            "6:45 PM",
            "7:00 PM",
            "7:15 PM",
            "7:30 PM",
            "7:45 PM",
            "8:00 PM",
            "8:15 PM",
            "8:30 PM",
            "8:45 PM",
            "9:00 PM",
            "9:15 PM",
            "9:30 PM",
            "9:45 PM",
            "10:00 PM",
            "10:15 PM",
            "10:30 PM",
            "10:45 PM",
            "11:00 PM",
            "11:15 PM",
            "11:30 PM",
            "11:45 PM",
        };

        public string[] HoursHalf =
        {
            "12:00 AM",
            "12:30 AM",
            "1:00 AM",
            "1:30 AM",
            "2:00 AM",
            "2:30 AM",
            "3:00 AM",
            "3:30 AM",
            "4:00 AM",
            "4:30 AM",
            "5:00 AM",
            "5:30 AM",
            "6:00 AM",
            "6:30 AM",
            "7:00 AM",
            "7:30 AM",
            "8:00 AM",
            "8:30 AM",
            "9:00 AM",
            "9:30 AM",
            "10:00 AM",
            "10:30 AM",
            "11:00 AM",
            "11:30 AM",
            "12:00 PM",
            "12:30 PM",
            "1:00 PM",
            "1:30 PM",
            "2:00 PM",
            "2:30 PM",
            "3:00 PM",
            "3:30 PM",
            "4:00 PM",
            "4:30 PM",
            "5:00 PM",
            "5:30 PM",
            "6:00 PM",
            "6:30 PM",
            "7:00 PM",
            "7:30 PM",
            "8:00 PM",
            "8:30 PM",
            "9:00 PM",
            "9:30 PM",
            "10:00 PM",
            "10:30 PM",
            "11:00 PM",
            "11:30 PM",
        };

        public string[] HoursFull =
        {
            "12:00 AM",
            "1:00 AM",
            "2:00 AM",
            "3:00 AM",
            "4:00 AM",
            "5:00 AM",
            "6:00 AM",
            "7:00 AM",
            "8:00 AM",
            "9:00 AM",
            "10:00 AM",
            "11:00 AM",
            "12:00 PM",
            "1:00 PM",
            "2:00 PM",
            "3:00 PM",
            "4:00 PM",
            "5:00 PM",
            "6:00 PM",
            "7:00 PM",
            "8:00 PM",
            "9:00 PM",
            "10:00 PM",
            "11:00 PM",
        };

        public string[] GetHours(AllowedReservationIntervals reservationIntervals)
        {
            return reservationIntervals switch
            {
                AllowedReservationIntervals.QH => HoursQuarter,
                AllowedReservationIntervals.HH => HoursHalf,
                AllowedReservationIntervals.FH => HoursFull,
                _ => throw new NotImplementedException(),
            };
        }

        public IList<DateTime> GetWeekDays()
        {
            //Get a list of reservations for the next 6 days (including today)
            DateTime today = DateTime.Today;

            List<DateTime> ThisWeekDays = new List<DateTime>
            {
                today
            };
            for (int i = 1; i < 7; i++)
            {
                ThisWeekDays.Add(today.AddDays(i));
            }

            return ThisWeekDays;
        }

        public IList<Reservation> GetWeekReservations()
        {
            return _context.Reservations.
                    Where(r => r.StartTime >= DateTime.Today && r.EndTime <= DateTime.Today.AddDays(6)).ToList();
        }

        public async Task<IViewComponentResult> InvokeAsync(Building building)
        {

            ThisWeekDays = GetWeekDays();
            ThisWeekReservations = GetWeekReservations();
            Hours = GetHours(building.ReservationInterval);

            return View(this);
        }
    }
}
