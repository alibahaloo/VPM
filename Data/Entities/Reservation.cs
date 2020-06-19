using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VPM.Data.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }

        [Display(Name = "Created At")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Plate #")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string VehiclePlateNumber { get; set; }

        [Required]
        [Display(Name = "From")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "To")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime EndTime { get; set; }

        public string ApplicationUserId { get; set; }

        [Display(Name = "Resident")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public Guid BuildingId { get; set; }
        public virtual Building Building { get; set; }

        [Required]
        [NotMapped]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get => StartTime; set => date = value; }

        private int durationHour;

        [Required]
        [NotMapped]
        [Display(Name = "Hour")]
        [Range(0, 24)]
        public int DurationHour
        {
            get
            {
                //Object is not new
                if (durationHour == 0)
                {
                    var diff = EndTime - StartTime; //Calculate the diff
                    return diff.Hours;
                }


                return durationHour;
            }

            set => durationHour = value;
        }

        private int durationMin;
        private DateTime date;

        [Required]
        [NotMapped]
        [Display(Name = "Minute")]
        [Range(0, 60)]
        public int DurationMin
        {
            get
            {
                //Object is not new
                if (durationMin == 0)
                {
                    var diff = EndTime - StartTime; //Calculate the diff
                    return diff.Minutes;
                }


                return durationMin;
            }

            set => durationMin = value;
        }
    }
}
