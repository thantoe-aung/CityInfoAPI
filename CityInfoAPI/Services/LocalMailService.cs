namespace CityInfoAPI.Services
{
    public class LocalMailService : IMailService
    {
        public string _mailTo = string.Empty;
        public string _mailFrom = string.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

      

        public void Send(string subject, string message)
        {
            Console.WriteLine($"LocalMail sent successfully.");
        }
    }
}
