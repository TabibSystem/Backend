namespace EmailService
{
    public class VerifyCodeRequest
    {
        public string Email { get; set; }
        public int Code { get; set; }
    }
}
