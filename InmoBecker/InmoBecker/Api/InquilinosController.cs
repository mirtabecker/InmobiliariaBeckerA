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
    public class InquilinosController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public InquilinosController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/Inquilino
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inquilino>>> GetInquilino()
        {
            try
            {
                var lista = await contexto.Inquilinos.ToListAsync();
                return Ok(lista);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Inquilino
        [HttpGet("PorPropietario")]
        public async Task<ActionResult> GetPorPropietario()
        {
            try
            {
                //return await _context.Inquilino.ToListAsync();
                var propietario = User.Identity.Name;
                var res = await contexto.Contratos
                        .Include(x => x.Inquilino)
                        .Where(x => x.Inmueble.Duenio.Email == propietario)
                        .Select(x => x.Inquilino)
                        .ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
