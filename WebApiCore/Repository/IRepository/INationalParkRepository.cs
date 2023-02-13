using System.Collections;
using System.Collections.Generic;
using WebApiCore.Models;

namespace WebApiCore.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int nationalParkId);
        bool NationalParkExists(int nationalParkId);
        bool NationalParkExists(string nationalParkName);
        bool CreateNationalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(NationalPark nationalPark);
        bool Save();

    }
}
