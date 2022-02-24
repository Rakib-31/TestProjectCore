using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProjectCore.Model;

namespace TestProjectCore.Repository
{
    public interface INationalParkRepository
    {
        ICollection<NationalParkDto> GetNationalParks();
        NationalParkDto GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNationalPark(NationalParkDto nationalPark);
        bool UpdateNationalPark(NationalParkDto nationalPark);
        bool DeleteNationalPark(NationalParkDto nationalPark);
        bool Save();
    }
}
