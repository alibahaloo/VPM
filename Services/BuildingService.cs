using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPM.Data;
using VPM.Data.Entities;

namespace VPM.Services
{
    public class BuildingService
    {
        private readonly ApplicationDbContext _context;
        public BuildingService(ApplicationDbContext context)
        {
            _context = context;
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

        public async Task<IList<Building>> GetBuildingsAsync()
        {
            try
            {
                return await _context.Buildings.ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
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
