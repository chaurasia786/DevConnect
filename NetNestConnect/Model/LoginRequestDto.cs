﻿namespace NetNestConnect.Model
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string recaptchaResponse { get; set; }

    }
}
