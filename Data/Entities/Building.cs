using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VPM.Data.Entities
{
    public enum AllowedReservationIntervals
    {
        [Display(Name = "Quarter Hour")]
        QH = 900,
        [Display(Name = "Half Hour")]
        HH = 1800,
        [Display(Name = "Full Hour")]
        FH = 3600
    }

    public class Building
    {
        private int minuteMaxLength;
        private int minuteStep;
        private int startTimeStep;

        public Guid Id { get; set; }

        /*[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }*/

        [Display(Name = "Created At")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Display(Name = "Available Parkings")]
        public int VisitorParkingCount { get; set; } = 1; //Total number of visitor parkings in the building

        [Required]
        [Display(Name = "Allowed Reservations per unit")]
        public int AllowedReservationPerUnit { get; set; } = 1; //Maximum number of reservations to be made by a single unit

        [Required]
        [Display(Name = "Max reservation duration")]
        [Range(1, 24)]
        public int AllowedReservationLength { get; set; } = 1; //Maximum allowed time to be reserved at a time (in hours?)

        [Required]
        [Display(Name = "Reservations Interval")]
        public AllowedReservationIntervals ReservationInterval { get; set; } = AllowedReservationIntervals.FH; //Used for steps in time picker

        /*
         * Usage:
         * To display input for reservation. Used in Reservation.DurationHour
         */
        [NotMapped]
        public int MinuteMaxLength { 
            get {

                return ReservationInterval switch
                {
                    AllowedReservationIntervals.QH => 45,
                    AllowedReservationIntervals.HH => 30,
                    AllowedReservationIntervals.FH => 0,
                    _ => 0,
                };
            } 
            set => minuteMaxLength = value; }

        [NotMapped]
        public int MinuteStep { 
            get {
                return ReservationInterval switch
                {
                    AllowedReservationIntervals.QH => 15,
                    AllowedReservationIntervals.HH => 30,
                    AllowedReservationIntervals.FH => 1,
                    _ => 0,
                };
            } 
            set => minuteStep = value; }

        [NotMapped]
        public int StartTimeStep { 
            get 
            {
                return ReservationInterval switch
                {
                    AllowedReservationIntervals.QH => 900,
                    AllowedReservationIntervals.HH => 1800,
                    AllowedReservationIntervals.FH => 3600,
                    _ => 0,
                };
            } 
            set => startTimeStep = value; 
        }

        public virtual List<Reservation> Reservations { get; set; }
        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
