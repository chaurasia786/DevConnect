using NetNestConnect.Model;

namespace NetNestConnect.Repository
{
    public interface IUser
    {
        public Task<RegisterResponse> RegisterUser(UserRegistration userRegistration);
        public Task<UserRegistration> GetUserById(int id);  
        public Task<IEnumerable<UserRegistration>> GetAllUsers();
        public string GenerateJWT(string UserId,string Email);
        public Task<DeleteResponse> DeleteUser(int id);
        public Task<UpdatedResponse> UpdateUser(UserRegistration userRegistration); 
        public Task<LoginResponse> Login(string Email, string password); 

    }
}
