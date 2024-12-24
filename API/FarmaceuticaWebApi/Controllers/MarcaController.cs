using FarmaceuticaBack.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmaceuticaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcaController : Controller
    {
        private readonly IMarcaService _marcaService;

        public MarcaController(IMarcaService marcaService)
        {
            _marcaService = marcaService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMarcas()
        {
            return Ok(await _marcaService.GetMarca());
        }
    }
}
