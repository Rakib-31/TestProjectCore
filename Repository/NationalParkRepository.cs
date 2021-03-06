using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProjectCore.Data;
using TestProjectCore.Model;

namespace TestProjectCore.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;
        public NationalParkRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _db.NationalParks.FirstOrDefault(x => x.Id == nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(x => x.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            return _db.NationalParks.Any(x => x.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public bool NationalParkExists(int id)
        {
            return _db.NationalParks.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
