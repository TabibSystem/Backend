namespace EmailService
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public int Code { get; set; }
        public string NewPassword { get; set; }
    }
}
