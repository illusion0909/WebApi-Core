using System.Collections.Generic;

namespace WebApiCoreFrontEnd.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParkList { get; set; }
        public IEnumerable<Trail> TrailList { get; set; }
    }
}
