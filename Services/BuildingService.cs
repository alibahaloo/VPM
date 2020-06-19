using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data;
using VPM.Data.Entities;
using VPM.Data.Queries;

namespace VPM.Services
{
    public class BuildingService
    {
        private readonly ApplicationDbContext _context;

        private readonly List<string> _errors = new List<string> { };
        public BuildingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ServiceResult CheckNeedForDefaultBuilding()
        {
            /*
             * It is not important to have a default building exactly as it was created during init
             * What matters is to have at least 1 building in the system so that the app can be used
             */
            if (_context.Buildings.Count() > 0)
            {
                _errors.Add("At least 1 building detected.");
                return new ServiceResult { Success = false, Errors = _errors };
            } else
            {
                return new ServiceResult { Success = true }; // Buildings found
            }
        }

        public async Task CreateBuildingAsync(Building building)
        {

            try
            {
                _context.Buildings.Add(building);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            
        }

        public async Task DeleteBuildingAsync(Guid buildingId)
        {
            Building building = await _context.Buildings.FindAsync(buildingId);

            if (building != null)
            {

                try
                {
                    _context.Buildings.Remove(building);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    
                    throw;
                }

                
            }
        }

        public async Task<Building> GetDefaultBuildingAsync()
        {
            return await _context.Buildings.FirstOrDefaultAsync();
        }

        public Building GetBuilding(Guid buildingId)
        {
            try
            {
                return _context.Buildings.FirstOrDefault(b => b.Id == buildingId);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<Building> GetBuildingAsync(Guid buildingId)
        {
            try
            {
                return await _context.Buildings.FirstOrDefaultAsync(b => b.Id == buildingId);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<IList<Building>> GetBuildingsAsync(BuildingQuery query = null)
        {
            IQueryable<Building> Repo = _context.Buildings
                .Include(b => b.Reservations);

            if (query != null)
            {
                if (query.Name != null)
                    Repo = Repo.Where(b => b.Name == query.Name);

                if (query.Address != null)
                    Repo = Repo.Where(b => b.Address == query.Address);

                if (!query.ShowAllInterval)
                    Repo = Repo.Where(b => b.ReservationInterval == query.ReservationInterval);

                if (query.ShowActiveOnly)
                    Repo = Repo.Where(b => b.IsActive == true);
            }

            return await Repo.AsNoTracking().ToListAsync();
        }

        public IList<Building> GetBuildings()
        {
            try
            {
                return _context.Buildings.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateBuildingAsync(Building building)
        {
            try
            {
                _context.Attach(building).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
