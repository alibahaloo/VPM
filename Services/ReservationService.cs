using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VPM.Data;
using VPM.Data.Entities;
using VPM.Data.Queries;

namespace VPM.Services
{
    interface IReservationService
    {
        public Task<ServiceResult> CreateReservationAsync(Reservation reservation);
        public Task<ServiceResult> UpdateReservationAsync(Reservation reservation);
        public Task DeleteReservationAsync(Guid reservationId);
        public Task<IList<Reservation>> GetReservationsAsync(bool includeExpired = false);
        public Task<IList<Reservation>> GetReservationsByUserAsync(string userId, bool includeExpired = false);
        public Task<IList<Reservation>> GetReservationsByBuildingAsync(Guid buildingId, bool includeExpired = false);
        public Task<Reservation> GetReservationAsync(Guid reservationId);

        //public Task<IList<Reservation>> FilterReservationsAsync(ReservationFilter reservationFilter);
    }

    public class ReservationService /*: IReservationService*/
    {
        public IQueryable<Reservation> Repo;

        private readonly ApplicationDbContext _context;

        private readonly List<string> _errors = new List<string> { };

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
            Repo = _context.Reservations
                .Include(r => r.ApplicationUser)
                .Include(r => r.Building);
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

                return new ServiceResult { Success = true };
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

        public bool IsUserOwner(string userId, Reservation reservation)
        {
            return reservation.ApplicationUserId == userId;
        }

        public string GetReservationDuration(Reservation reservation)
        {
            return "2:00";
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

        public async Task<IList<Reservation>> GetReservationsAsync(ReservationQuery query)
        {
            //Filtering based on the given Query
            if (query.BuildingId != Guid.Empty)
            {
                Repo = Repo.Where(r => r.BuildingId == query.BuildingId);
            }

            if (query.Unit != null)
            {
                Repo = Repo.Where(r => r.ApplicationUser.Unit == query.Unit);
            }

            if (query.VehiclePlateNumber != null)
            {
                Repo = Repo.Where(r => r.VehiclePlateNumber == query.VehiclePlateNumber);
            }

            if (query.ApplicationUserId != null)
            {
                Repo = Repo.Where(r => r.ApplicationUserId == query.ApplicationUserId);
            }

            if (!query.ShowExpired)//Only showing the non-expired reservations
            {
                Repo = Repo.Where(r => r.EndTime >= DateTime.Now);
            }

            return await Repo.AsNoTracking().ToListAsync();
        }


    }

}
