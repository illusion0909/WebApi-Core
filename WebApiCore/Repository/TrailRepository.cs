using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApiCore.Data;
using WebApiCore.Models;
using WebApiCore.Repository.IRepository;

namespace WebApiCore.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _context;
        public TrailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateTrail(Trail trail)
        {
            _context.Trails.Add(trail);
            return save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _context.Trails.Remove(trail);
            return save();
        }

        public Trail GetTrail(int trailId)
        {
            return _context.Trails.Find(trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _context.Trails.Include(t => t.NationalPark).ToList();///list show karne ke liye simple foregin key work nahi karegi
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return _context.Trails.Include(t => t.NationalPark).Where(t => t.NationalParkId == nationalParkId).ToList();
        }

        public bool save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool TrailExists(int trailId)
        {
            return _context.Trails.Any(t => t.Id == trailId);
        }

        public bool TrailExists(string trailName)
        {
            return _context.Trails.Any(t => t.Name == trailName);
        }

        public bool UpdateTrail(Trail trail)
        {
            _context.Trails.Update(trail);
            return save();
        }
    }
}
