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
    public class InmueblesController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public InmueblesController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/<InmueblesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuarios = User.Identity.Name;
                return Ok(contexto.Inmuebles.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuarios));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // GET: api/<InmueblesController/obtenerPorId>
        [HttpGet("obtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {

            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmuebles
                    .Include(e => e.Duenio)
                    .Where(e => e.Duenio.Email == usuario).Single(e => e.IdInmueble == id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPut("modificarDisponible")]
        public async Task<IActionResult> ModificarDisponible(int id, [FromBody] Inmueble entidad)
        {
            try
            {
                var usuario = User.Identity.Name;
                id = entidad.IdInmueble;


                var control = contexto.Inmuebles.Include(i => i.Duenio.Email)
                    .Where(x => x.IdInmueble == id && x.Duenio.Email == usuario);


                if (ModelState.IsValid && control != null)
                {

                    if (entidad.Estado == 1)
                    {
                        entidad.Estado = 2;
                        // contexto.Attach(entidad);
                        // contexto.Entry(entidad).Property("disponible").IsModified== true;
                        contexto.Inmuebles
                        .Update(entidad).Property("Disponible");

                    }

                    else
                    {
                        entidad.Estado = 1;
                        contexto.Inmuebles
                        .Update(entidad).Property("Disponible");
                    }

                    contexto.SaveChanges();

                }
                return Ok(entidad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<controller>
        [HttpPut]
        public async Task<IActionResult> Put(int id, Inmueble entidad)
        {
            try
            {
                if (ModelState.IsValid && contexto.Inmuebles.AsNoTracking().Include(e => e.Duenio).FirstOrDefault(e => e.IdInmueble == id && e.Duenio.Email == User.Identity.Name) != null)
                {
                    entidad.IdInmueble = id;
                    contexto.Inmuebles.Update(entidad).Property("Disponible");
                    contexto.SaveChanges();

                }
                return Ok(entidad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}
