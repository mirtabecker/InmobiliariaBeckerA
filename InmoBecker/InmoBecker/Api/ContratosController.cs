using InmoBecker.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly DataContext contexto;
        

        public ContratosController(DataContext contexto)
        {
            this.contexto = contexto;
           
        }

        // GET: api/<ContratosController>
        [HttpGet("inmueblesConContrato")]
        public async Task<IActionResult> InmueblesConContrato()
        {

            try
            {

                var usuario = User.Identity.Name;
                var contratosVigentes = contexto.Contratos

              .Include(x => x.Inquilino)
              .Include(x => x.Inmueble)
              .Where(c => c.Inmueble.Duenio.Email == usuario).ToList();
               
                return Ok(contratosVigentes);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        // GET api/<controller>/5
        [HttpGet("obtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var contratoPorId = contexto.Contratos
                .Include(x => x.Inquilino)
                .Include(x => x.Inmueble)
                 .Where(c => c.Inmueble.Duenio.Email == usuario)
                 .Single(e => e.IdContrato == id);
                return Ok(contratoPorId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



       


    }
}
