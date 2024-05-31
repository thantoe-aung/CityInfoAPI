namespace CityInfoAPI.Services
{
    public class CloudMailService :IMailService
    {
        public string _mailTo = string.Empty;
        public string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            Console.WriteLine($"CloudMail sent successfully.");
        }
    }
}
