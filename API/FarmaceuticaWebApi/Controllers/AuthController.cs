using FarmaceuticaBack.Models;
using FarmaceuticaBack.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FarmaceuticaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IPersonalEstablecimientoService _service;

        public AuthController(IPersonalEstablecimientoService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _service.Login(request.Username, request.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Credenciales incorrectas.");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Personal oPersonal)
        {
            var result = await _service.Register(oPersonal);

            if (!result)
            {
                return BadRequest("No se pudo registrar el usuario.");
            }

            return Ok("Usuario registrado exitosamente.");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

}
