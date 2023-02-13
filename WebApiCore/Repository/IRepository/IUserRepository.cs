using WebApiCore.Models;

namespace WebApiCore.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string userName);
        User Authenticate(string userName, string password);
        User Register(string userName, string password);
    }
}
