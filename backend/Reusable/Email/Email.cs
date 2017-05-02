using System.Collections.Generic;
using System.Net.Mail;

namespace Reusable.Email
{
    public class EmailService
    {
        private SmtpClient server;
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        
        public List<string> To = new List<string>();
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailService(string EmailServer, int EmailPort)
        {
            server = new SmtpClient(EmailServer, EmailPort);
        }

        public CommonResponse SendMail()
        {
            CommonResponse response = new CommonResponse();

            try
            {

                server.EnableSsl = true;
                server.DeliveryMethod = SmtpDeliveryMethod.Network;
                server.UseDefaultCredentials = false;
                server.Credentials = new System.Net.NetworkCredential(EmailAddress, Password);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(From, From);

                foreach (var to in To)
                {
                    message.To.Add(new MailAddress(to));
                }
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;

                message.Body = Body;

                server.Send(message);
            }
            catch (System.Exception ex)
            {
                return response.Error(ex.Message);
            }

            return response.Success();
        }


    }
}
