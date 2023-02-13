using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApiCore.Data;
using WebApiCore.Models;
using WebApiCore.Repository.IRepository;


namespace WebApiCore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        public UserRepository(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string userName, string password)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            if (userInDb == null) return null;
           
            
            //jwt authentication
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInDb.Id.ToString()),
                    new Claim(ClaimTypes.Role, userInDb.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);
            userInDb.Token = tokenHandler.WriteToken(token);
            userInDb.Password = "";

            return userInDb;

        }

        public bool IsUniqueUser(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
                return true;
            else
                return false;
        }

        public User Register(string userName, string password)
        {
            User user = new User()
            {
                UserName = userName,
                Password = password,
                Role = "Admin"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
