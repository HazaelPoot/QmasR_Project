using QRDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult Enviar(DtoEmail email)
        {
            _emailService.SendEmail(email);
            return StatusCode(StatusCodes.Status200OK, new {message = $"Tu mensaje se envio con exito, la respuesta será enviada su correo: {email.Remitemte}"});
        }
    }
}