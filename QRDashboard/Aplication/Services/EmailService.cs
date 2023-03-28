using System.Net;
using System.Text;
using System.Net.Mail;
using QRDashboard.Domain.Dtos;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Aplication.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(DtoEmail email)
        {
            try
            {
                //Ingresar en esta variable el correo de la empresa
                var Destinatario = "rzkd.ycvxg54@nefyp.com";

                string template = GetEmailTemplate();
                string emailContent = new StringBuilder(template)
                    .Replace("{{FromAddress}}", email.Remitemte)
                    .Replace("{{ToAddress}}", Destinatario)
                    .Replace("{{Subject}}", email.Asunto)
                    .Replace("{{Body}}", email.Contenido)
                    .ToString();

                MailMessage message = new MailMessage();
                message.Body = emailContent;
                message.IsBodyHtml = true;
                message.From = new MailAddress(email.Remitemte);
                message.To.Add(new MailAddress(Destinatario));
                message.Subject = email.Asunto;

                using var client = new SmtpClient
                (
                    _config.GetSection("Email:Host").Value, 
                    Convert.ToInt32(_config.GetSection("Email:Port").Value)
                );

                client.EnableSsl = true;
                client.Credentials = new NetworkCredential
                (
                    _config.GetSection("Email:UserName").Value, 
                    _config.GetSection("Email:PassWord").Value
                );

                client.Send(message);
            }
            catch
            {
                throw;
            }
        }

        public string GetEmailTemplate()
        {
            string template = @"
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>{{Subject}}</title>
            </head>

            <body>
                <table style='width:100%¿'>
                    <tr>
                        <td align='center' colspan='2'>
                            <h2 style='color:#004DAF'>Q+R Correo Web</h2>
                        </td>
                    </tr>
                    <tr>
                        <td align='left' colspan='2'>
                            <h4>Este correo fue enviado por un usuario desde la pagina web principal</h4>
                        </td>
                    </tr>
                    <tr>
                        <td><h4 style='color:#004DAF;margin:2px'>Remitente:</h4></td>
                        <td>{{FromAddress}}</td>
                    </tr>
                    <tr>
                        <td><h4 style='color:#004DAF;margin:2px'>Asunto:</h4></td>
                        <td>{{Subject}}</td>
                    </tr>
                </table>
                <br>
                <p style='font-size: 15px'>{{Body}}</p>
                <br>
                <a style='border:3px solid #004DAF; border-radius:22px; padding: 5px; text-decoration: none; color: #004DAF; font-weight: bold;' 
                    href='mailto:{{FromAddress}}?Subject=Respuesta%20para:%20{{Subject}}'>Responder
                </a>
                <br>
            </body>
            </html>
            ";

            return template;
        }
    }
}