using FarmaceuticaBack.Data.Models;
using FarmaceuticaBack.Models;
using FarmaceuticaBack.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmaceuticaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicamentoController : Controller
    {
        private readonly IMedicamentoService _service;
        public MedicamentoController(IMedicamentoService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet("Filter")]
        public async Task<IActionResult> GetByFilter([FromQuery] MedicamentoFiltro filtro)
        {

          
                return Ok(await _service.GetByFiltro(filtro));
            

        }

        [HttpGet("Id")]

        public async Task<IActionResult> GetMedicamentoById([FromQuery] int id)
        {
            try
            {
                var med = await _service.GetMedicamentoById(id);
                if (med != null)
                {
                    return Ok(med);
                }
                else
                {
                    return NotFound("No se encontró el medicamento.");
                }

            }
            catch (Exception exc)
            {

                return StatusCode(500, "Error" + exc); ;
            }

        }

        [Authorize]
        [HttpGet("GetForSave/id")]

        public async Task<IActionResult> GetMedicamentoSaveDTOById([FromQuery] int id)
        {
            try
            {
                var med = await _service.GetMedicamentoSaveDTOById(id);
                if (med != null)
                {
                    return Ok(med);
                }
                else
                {
                    return NotFound("No se encontró el medicamento.");
                }

            }
            catch (Exception exc)
            {

                return StatusCode(500, "Error" + exc); ;
            }

        }


        [HttpGet("LastId")]

        public async Task<IActionResult> GetLastId()
        {
            return Ok(await _service.GetLastId());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMedicamentos()
        {
            try
            {
                var list = await _service.GetAll();
                if (list.Count > 0)
                {
                    return Ok(list);
                }
                return StatusCode(500, "No hay medicamentos.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error en el servidor" + ex);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMedicamentos([FromQuery] int? id)
        {
            try
            {
                if (id != 0 && id != null)
                {
                    var baja = await _service.Delete(id);
                    return Ok("Se dio la baja con exito");
                }
                return StatusCode(500, "Debe ingresar un codigo o Uno de valor distinto a 0");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error en el servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveMedicamento([FromBody] MedicamentoSaveDTO oMedicamento)
        {
            var result = IsValid(oMedicamento);
            if (result == null)
            {
                var response = await _service.Save(oMedicamento);
                if (response)
                    return Ok(new { success = true, message = "Medicamento registrado." });
                return StatusCode(500, "Error en el servidor");
            }
            return BadRequest(IsValid(oMedicamento));
        }
        
        [HttpPut]
        public async Task<IActionResult> PutMedicamento([FromBody] MedicamentoSaveDTO oMedicamento)
        {
            try
            {
                var result = IsValid(oMedicamento);
                if (result == null)
                {
                    var obj = await _service.Update(oMedicamento);
                    return Ok(new { success = true, message = "Medicamento editado." });
                }
                return BadRequest( new { success = false,message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hubo un problema en el servidor." + ex);
            }
        }
        private string? IsValid(MedicamentoSaveDTO oMedicamento)
        {
            string? result = null;
            if (oMedicamento.IdMedicamento == 0 || oMedicamento.IdMonodroga == 0 || oMedicamento.IdLaboratorio == 0 || oMedicamento.IdMarca == 0 || oMedicamento.IdPresentacion == 0)
                result = "El campo debe ser mayor a 0";
            if (oMedicamento.NombreMedicamento.Length > 100 || string.IsNullOrEmpty(oMedicamento.NombreMedicamento))
                result = "La cantidad de caracrteres que debe tener debe ser entre 1 y 100";
            if (oMedicamento.VentaLibre == null)
                result = "No se aceptan valores nulose en este campo";
            if (oMedicamento.Descripcion.Length > 255 || string.IsNullOrEmpty(oMedicamento.Descripcion))
                result = "La lomgitud del campo debe tener de 1 a 255 caracteres";
            if (oMedicamento.Precio == null)
                result = "Debe ingresar un precio";


            return result;
        }
    }
}
