namespace NetNestConnect.Model
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }

    }
}
