using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VPM.Data;
using VPM.Data.Entities;

namespace VPM.Services
{
    interface IReservationService
    {
        public Task<ServiceResult> CreateReservationAsync(Reservation reservation);
        public Task<ServiceResult> UpdateReservationAsync(Reservation reservation);
        public Task DeleteReservationAsync(Guid reservationId);
        public Task<IList<Reservation>> GetReservationsAsync();
        public Task<IList<Reservation>> GetReservationsByUserAsync(string userId);
        public Task<IList<Reservation>> GetReservationsByBuildingAsync(Guid buildingId);
        public Task<Reservation> GetReservationAsync(Guid reservationId);
    }

    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        private readonly List<string> _errors = new List<string> { };

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Checks the Start/End time to be valid
         * Start time must be before End time
         */
        private bool ValidateStartEndTime(Reservation reservation)
        {
            if (DateTime.Compare(reservation.StartTime, reservation.EndTime) >= 0)
            {
                return false;
            }

            return true;
        }

        /*
         * Checks the reservation length against what is allowed for the building
         */
        private bool ValidateLength(Reservation reservation)
        {
            double ReservationMins = reservation.EndTime.Subtract(reservation.StartTime).TotalMinutes;

            //TODO: On update this would throw null for reservation.Building .. (probably best is to get building from ID here)
            double AllowedReservationLengthMins = reservation.Building.AllowedReservationLength * 60; // Getting minutes

            if (AllowedReservationLengthMins < ReservationMins)
            {
                return false;
            }

            return true;
        }

        /*
         * Checks to see if there are available parkings left for the building
         */
        private bool IsParkingAvailable(Reservation reservation)
        {
            int BuildingReservedCount = _context.Reservations
                .Where(r => r.BuildingId == reservation.BuildingId) //Belonging to the building
                .Where(r => r.StartTime >= reservation.StartTime) //After or on given start time
                .Where(r => r.EndTime <= reservation.EndTime) //Before or on the given end time
                .ToList().Count();
            int BuildingTotalCount = reservation.Building.VisitorParkingCount;

            if (BuildingTotalCount <= BuildingReservedCount)
            {
                return false;
            }

            return true;
        }

        /*
         * Checks to see if unit has reached the limit of allowed reservations per unit (user)
         */
        private bool DoesUnitHaveParking(Reservation reservation)
        {
            int UnitReservedCount = _context.Reservations
                .Where(r => r.BuildingId == reservation.BuildingId) //Belonging to the building
                .Where(r => r.ApplicationUserId == reservation.ApplicationUserId) //Belonging to the usedr
                .Where(r => r.EndTime >= DateTime.Now) //Reservations is still active
                .ToList().Count();
            int UnitAllowedCount = reservation.Building.AllowedReservationPerUnit;

            if (UnitAllowedCount <= UnitReservedCount)
            {
                return false;
            }

            return true;
        }

        private void AdjustTimeValues(Reservation reservation)
        {
            reservation.StartTime = new DateTime(
                reservation.Date.Year,
                reservation.Date.Month,
                reservation.Date.Day,
                reservation.StartTime.Hour,
                reservation.StartTime.Minute,
                reservation.StartTime.Second);

            reservation.EndTime = reservation.StartTime.AddHours(reservation.DurationHour).AddMinutes(reservation.DurationMin);

            //TODO: Set reservation.Building
            //TODO: Set reservation.ApplicationUser
            
        }

        private async Task SetBuildingAndUserAsync(Reservation reservation)
        {
            //Set ApplicationUser
            if (reservation.ApplicationUserId is null || reservation.BuildingId == Guid.Empty)
            {
                //This should never happen, throw exception
                throw new Exception("ApplicationUserId and/or BuildingId null in reservation");
            }

            //Check if related objects are null
            if (reservation.ApplicationUser is null)
                reservation.ApplicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == reservation.ApplicationUserId);
            
            if (reservation.Building is null) 
                reservation.Building = await _context.Buildings.FirstOrDefaultAsync(b => b.Id == reservation.BuildingId);
        }

        private async Task<bool> ValidateReservationAsync(Reservation reservation)
        {
            await SetBuildingAndUserAsync(reservation); //Ensure reservation related objects are load; 

            // Adjusting date on START/END TIME to be consistent for better searching
            AdjustTimeValues(reservation);

            if (!ValidateStartEndTime(reservation))
                _errors.Add("Start time is in valid: Must be after 'End Time'");

            if (!ValidateLength(reservation))
                _errors.Add("Reservations time exceeds allowed time.");

            if (!IsParkingAvailable(reservation))
                _errors.Add("No parking available for the selected date/time");

            //if (!DoesUnitHaveParking(reservation))
            //    errors.Add("Reached reservation limit");

            return _errors.Count <= 0;
        }

        public async Task<ServiceResult> CreateReservationAsync(Reservation reservation)
        {
            try
            {
                if (!await ValidateReservationAsync(reservation))
                {
                    return new ServiceResult { Success = false, Errors = _errors };
                }

                // If all good, then save reservation
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return new ServiceResult { Success = true};
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }
        public async Task DeleteReservationAsync(Guid id)
        {
            try
            {
                Reservation reservation = await _context.Reservations.FindAsync(id);

                if (reservation != null)
                {
                    _context.Reservations.Remove(reservation);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                //Log Ex
                throw ex;
            }
        }

        public async Task<ServiceResult> UpdateReservationAsync(Reservation reservation)
        {
            try
            {
                if (!await ValidateReservationAsync(reservation))
                {
                    return new ServiceResult { Success = false, Errors = _errors };
                }

                _context.Attach(reservation).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                //Log Ex
                throw ex;
            }
        }

        public async Task<IList<Reservation>> GetReservationsAsync()
        {
            try
            {
                return await _context.Reservations
                .Include(r => r.ApplicationUser) //May not be needed
                .Include(r => r.Building) //May not be needed
                .ToListAsync();
            }
            catch (Exception ex)
            {
                //log
                throw ex;
            }
        }

        public async Task<IList<Reservation>> GetReservationsByUserAsync(string ApplicationUserId)
        {
            try
            {
                return await _context.Reservations
                .Include(r => r.ApplicationUser)
                .Include(r => r.Building)
                .Where(r => r.ApplicationUserId == ApplicationUserId)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                //log
                throw ex;
            }
        }

        public async Task<IList<Reservation>> GetReservationsByBuildingAsync(Guid buildingId)
        {
            try
            {
                return await _context.Reservations
                .Include(r => r.ApplicationUser)
                .Include(r => r.Building)
                .Where(r => r.BuildingId == buildingId)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                //log
                throw ex;
            }
        }

        public async Task<Reservation> GetReservationAsync(Guid reservationId)
        {
            try
            {
                return await _context.Reservations
                .Include(r => r.ApplicationUser)
                .Include(r => r.Building)
                .FirstOrDefaultAsync(m => m.Id == reservationId);
            }
            catch (Exception ex)
            {
                //Log
                throw ex;
            }
        }

        public bool IsUserOwner(string userId, Reservation reservation)
        {
            return reservation.ApplicationUserId == userId;
        }

        public string GetReservationDuration(Reservation reservation)
        {
            return "2:00";
        }
    }

}
