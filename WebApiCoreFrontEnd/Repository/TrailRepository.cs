using System.Net.Http;
using WebApiCoreFrontEnd.Models;
using WebApiCoreFrontEnd.Repository.IRepository;

namespace WebApiCoreFrontEnd.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TrailRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
