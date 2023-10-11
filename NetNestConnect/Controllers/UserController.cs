using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetNestConnect.Model;
using NetNestConnect.Repository;

namespace NetNestConnect.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUser _userCore;
        private IEmailService _emailService;
        public UserController(IUser userCore, IEmailService emailService)
        {
            _userCore = userCore;
            _emailService = emailService;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]

        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequestDto loginRequest)
        {
            var client = new HttpClient();
            //var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Lf0oI8oAAAAAC2GuQc7tnSICf9OWjy5thSNdon-&response={loginRequest.recaptchaResponse}");

            //var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret='6LeQCiYTbbbDDD1xK2-U4cAaBCDoENFhTRDKyy79god'&response={loginRequest.recaptchaResponse}");
            //if (!response.Contains("\"success\": true"))
            //{
            //    return BadRequest("reCAPTCHA verification failed");
            //}

            var login = await _userCore.Login(loginRequest.Email, loginRequest.Password);   
            if (login.IsSuccess)
            {
                login.Token = _userCore.GenerateJWT(login.UserId, loginRequest.Email);
                return Ok(login);
            }
            else
            {
                return BadRequest(login);
            }
        }

       

        [HttpPost]
     
        [Route("RegisterUser")]
        public async Task<ActionResult<RegisterResponse>> UserRegister(UserRegistration userRegistration)
        {
            var response = await _userCore.RegisterUser(userRegistration);
            if (response.IsAdded)
            {
                // Send registration success email
                var subject = "Registration Successful";
                var content = "Dear " + userRegistration.FirstName + ",\n\nThank you for registering. Your registration was successful!";
                await _emailService.SendEmailAsync(userRegistration.Email, subject, content);

                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpGet]
        [Route("AllUsers")]
        public async Task<ActionResult<IEnumerable<UserRegistration>>> GetUsers()
        {
            var usersList = await _userCore.GetAllUsers();
            return Ok(usersList);
        }
        [HttpGet]
        public async Task<ActionResult<UserRegistration>> GetUserById(int id)
        {
            var user = await _userCore.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete]
        public async Task<ActionResult<UserRegistration>> DeleteUser(int id)
        {
            var deleteResponse=await _userCore.DeleteUser(id);
            if (deleteResponse.IsDeleted)
            {
                return Ok(deleteResponse);
            }
            else
            {
                return NotFound(deleteResponse);
            }
        }
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult<UpdatedResponse>> UpdateUser(UserRegistration userRegistration)
        {
            var updatedResponse = await _userCore.UpdateUser(userRegistration);
            if (updatedResponse.IsUpdated)
            {
                return Ok(updatedResponse);
            }
            else
            {
                return NotFound(updatedResponse);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("Logout")]
        public IActionResult Logout()
        {

            return Ok(new { message = "Logged out successfully" });
        }


    }


   
}
