using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetNestConnect.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetNestConnect.Repository
{
    public class UserCore : IUser
    {
        private DatabaseContext _dbContext;
        IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserCore(DatabaseContext databaseContext,IConfiguration configuration, IHttpContextAccessor httpContextAccessor) { 
            _configuration = configuration; 
            _dbContext = databaseContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeleteResponse> DeleteUser(int id)
        {
            var deleteResponse = new DeleteResponse();
            var user =await _dbContext.Registrations.FindAsync(id);
            
            
            if (user == null)
            {
                deleteResponse.IsDeleted = false;
                deleteResponse.Message = "User Not Found";
            }
            else
            {
                string userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                int curUserId= userId==null?0:Convert.ToInt32(userId);
                user.DeletedBy = curUserId;
                user.DeletedOn = DateTime.UtcNow;
                user.IsDeleted = true;
                _dbContext.Registrations.Update(user);
                await _dbContext.SaveChangesAsync();
                deleteResponse.IsDeleted = true;
                deleteResponse.Message = "User Deleted Successfullyy";

            }
            return deleteResponse;

        }

        public string GenerateJWT(string userId, string username)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username)
        }),
                Expires = DateTime.UtcNow.AddHours(7), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<IEnumerable<UserRegistration>> GetAllUsers()
        {
            var ele = await _dbContext.Registrations
                          .Where(r => r.IsDeleted == null || r.IsDeleted == false).OrderByDescending(x=>x.Id)
                          .ToListAsync();

            return ele;
        }
        public async Task<UserRegistration> GetUserById(int id)
        {
           return await _dbContext.Registrations
                .FindAsync(id);

        }

        public async Task<LoginResponse> Login(string Email, string password)
        {
            try
            {

           
            var loginResponse = new LoginResponse();
            var user = await _dbContext.Registrations.Where(r => r.IsDeleted == null || r.IsDeleted == false).FirstOrDefaultAsync(x => x.Email == Email && x.Password == password);
            if (user != null) {
                loginResponse.IsSuccess = true;
                loginResponse.UserId = user.Id.ToString();
                loginResponse.Message = "Loged In SuccessFully";
            }
            else
            {
                loginResponse.IsSuccess = false;
                loginResponse.Message = "Login Failed";
            }
            return loginResponse;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<RegisterResponse> RegisterUser(UserRegistration userRegistration)
        {
            var user=await _dbContext.Registrations.FirstOrDefaultAsync(x=>x.Email==userRegistration.Email);
            RegisterResponse registerResponse=new RegisterResponse();
            if(user==null)
            {
             await  _dbContext.Registrations.AddAsync(userRegistration);
             await _dbContext.SaveChangesAsync();
                registerResponse.IsAdded = true;
                registerResponse.Message = "User Added Successfully";
            }
            else
            {
                registerResponse.IsAdded = false;   
                registerResponse.Message="Email Alreeady Present";
            }
            return registerResponse;
        }

        public async Task<UpdatedResponse> UpdateUser(UserRegistration userRegistration)
        {
            var updatedResponse=new UpdatedResponse();  
            var user = await _dbContext.Registrations.FindAsync(userRegistration.Id);
            if (user != null)
            {
                user.FirstName = userRegistration.FirstName;
                user.LastName = userRegistration.LastName;
                user.Email = userRegistration.Email;
                user.Password = userRegistration.Password;
                user.PhoneNumber = userRegistration.PhoneNumber;
                user.ModifiedBy = userRegistration.ModifiedBy;
                user.ModifiedOn = userRegistration.ModifiedOn;
                 _dbContext.Registrations.Update(user);
                await _dbContext.SaveChangesAsync(); 
                updatedResponse.IsUpdated = true;
                updatedResponse.Message = "User Details Updated";
            }
            else
            {
                updatedResponse.IsUpdated = false;
                updatedResponse.Message = "Oops Something Went Wrong";
            }
            return updatedResponse; 

        }
    }
}
