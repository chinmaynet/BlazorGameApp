using BlazorGameApp.Server.Data;
using BlazorGameApp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorGameApp.Server.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {   
            var response =new  ServiceResponse<string>();
            var user =await  _context.Users.FirstOrDefaultAsync(user=>user.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User Not Found";
            }
            else if (!VerifyPassswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password";
            }
            else {
                response.Data = CreateToken(user);
                response.Success = true;
                response.Message = "User Login Successfully";
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password, int startUnitId)
        {

            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int> { Success = false, Message = "User Already Exists."};
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

             _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await AddStartUnitId(user, startUnitId);
            return new ServiceResponse<int> { Data = user.Id, Success = true, Message = "Registration Sucessful!" };
        }

        public async  Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()))) {
                return true;
            }
            return false;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new HMACSHA512())
            {
                //while (hmac.Key.Length * 8 < 512)
                //{
                //    hmac.Key = new HMACSHA512().Key;
                //}

                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassswordHash(string password,  byte[] passwordHash, byte[] passwordSalt) {



            using (var hmac = new HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


                //if (computedHash != passwordHash) {
                //    return false;
                //}
                //return true;

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            
                return true;
            }
        }


      
        /// Creates a JSON Web Token (JWT) for the given user, containing relevant claims.
        /// </summary>
        /// <param name="user">The user for whom the token is created.</param>
        /// <returns>The generated JWT as a string.</returns>
        private string CreateToken(User user)
        {
            // Define the claims to be included in the JWT, such as user ID and username.
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            // Retrieve the secret key used for signing the JWT from app settings.
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            // Create signing credentials using the secret key and HMACSHA512 signature algorithm.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Define the JWT with specified claims, expiration time, and signing credentials.
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            // Generate the JWT string representation.
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private async Task AddStartUnitId(User user, int startUnitId) {
            var unit = await _context.Units.FirstOrDefaultAsync<Unit>(u => u.Id == startUnitId);
            _context.UserUnits.Add(new UserUnit
            {
                UnitId = unit.Id,
                UserId = user.Id,
                HitPoints = unit.HitPoints,
            });
            await _context.SaveChangesAsync();  
        }
    }
}
