using InmoBecker.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly DataContext contexto;


        public PagosController(DataContext contexto)
        {
            this.contexto = contexto;

        }

        // GET api/<PagoController>
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)

        {
            try
            {
                var usuario = User.Identity.Name;
                var res = await contexto.Pagos
                .Include(e => e.Alquiler)
                .Where(e => e.Alquiler.Inmueble.Duenio.Email == usuario && e.ContratoId == id)
                .Select(x => new
                {
                    x.IdPago,
                    x.NroPago,
                    x.Alquiler,
                    x.Importe,
                    x.Fecha

                })
                .ToListAsync();
                if (res == null)
                {
                    return NotFound("No hay pagos");
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }


}
