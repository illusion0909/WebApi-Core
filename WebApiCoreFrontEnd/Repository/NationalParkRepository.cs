using System.Net.Http;
using WebApiCoreFrontEnd.Models;
using WebApiCoreFrontEnd.Repository.IRepository;

namespace WebApiCoreFrontEnd.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NationalParkRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
