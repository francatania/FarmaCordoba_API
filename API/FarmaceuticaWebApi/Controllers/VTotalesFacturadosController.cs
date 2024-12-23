﻿using FarmaceuticaBack.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FarmaceuticaWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VTotalesFacturadosController : Controller
    {
        private readonly IVTotalesFacturadosService _service;
        public VTotalesFacturadosController(IVTotalesFacturadosService service)
        {
            _service = service; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var lst = await _service.GetAll();

            if(lst.Count > 0)
            {
                return Ok(lst);
            }
            else
            {
                return BadRequest("No hay resultados.");
            }
        }

        [HttpGet("Filter")]

        public async Task<IActionResult> GetByFilter([FromQuery] int id, int year, int month)
        {
            if(id == 0 && year == 0 && month == 0)
            {
                return Ok(await _service.GetAll());
            }
            else
            {
                var lst = await _service.GetByFilter(id, year, month);


                return Ok(lst);
            }



        }
    }
}
