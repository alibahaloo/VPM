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

        [Display(Name = "Created At")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Name")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Display(Name = "Parkings")]
        public int VisitorParkingCount { get; set; } = 1; //Total number of visitor parkings in the building

        [Required]
        [Display(Name = "Max Reservations/Unit")]
        public int AllowedReservationPerUnit { get; set; } = 1; //Maximum number of reservations to be made by a single unit

        [Required]
        [Display(Name = "Max Duration (Hrs)")]
        [Range(1, 24)]
        public int AllowedReservationLength { get; set; } = 1; //Maximum allowed time to be reserved at a time (in hours?)

        [Required]
        [Display(Name = "Reservation Interval (Mins)")]
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
