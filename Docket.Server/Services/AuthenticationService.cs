using Docket.Server.Data;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Docket.Server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMongoCollection<User> users;
        private readonly IConfiguration configuration;

        public AuthenticationService(IDocketDatabaseSettings settings, IMongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
            this.configuration = configuration;
        }

        public async Task<User> Login(string id)
        {
            return await users.Find(user => user.id == id).FirstOrDefaultAsync();
        }

        public async Task Register(User user)
        {
            await users.InsertOneAsync(user);
        }

        public void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using(var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.name),
                new Claim(ClaimTypes.Gender, user.gender)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
