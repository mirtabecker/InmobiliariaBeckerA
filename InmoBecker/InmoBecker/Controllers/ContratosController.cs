using InmoBecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Controllers
{
    public class ContratosController : Controller
    {
        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioInmueble repositorioInmueble;

        private readonly IConfiguration configuration;

        public ContratosController(IConfiguration configuration)
        {
            this.repositorioContrato = new RepositorioContrato(configuration);
            this.repositorioInmueble = new RepositorioInmueble(configuration);
            this.repositorioInquilino = new RepositorioInquilino(configuration);
            this.configuration = configuration;
        }
        // GET: ContratoController
        [Authorize]
        public ActionResult Index()
        {
                var lista = repositorioContrato.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                ViewData["Error"] = TempData["Error"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                return View(lista);
           
        }

       
        public ActionResult Vigentes()
        {
            try
            {
                var hoy = DateTime.Now;
                var lista = repositorioContrato.ContradosVigentes(hoy);
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            var i = repositorioContrato.ObtenerPorId(id);
            return View(i);
        }

        // GET: ContratoController/Create
        [Authorize]
        public ActionResult Create(string desde, string hasta)
        {
            DateTime inicio = DateTime.Parse(desde);
            DateTime fin = DateTime.Parse(hasta);
            ViewBag.Desde = inicio;
            ViewBag.Hasta = fin;
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            var Inmuebles = repositorioInmueble.BuscarPorFechas(inicio,fin);
            if (Inmuebles.Count == 0)
            {
                TempData["Error"] = "No hay inmuebles disponibles en esa fecha";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Inmuebles = Inmuebles;
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato c)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorioContrato.Alta(c);
                    TempData["Id"] = c.IdContrato;
                    TempData["Mensaje"] = "Se creo el Contrato con exito";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                    ViewBag.Inmuebles = repositorioInmueble.BuscarPorFechas(c.FechaInicio, c.FechaCierre);
                   
                    return View(c);
                }
            }

            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(c);
            }
        }
        [Authorize]
        public ActionResult Renovar(int id)
        {
            var entidad = repositorioContrato.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            return View(entidad);
        }

        public ActionResult VerContratos(int id)
        {
            try
            {
               // ViewBag.Id = id;
                var lista = repositorioContrato.BuscarPorInmueble(id);

                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var c = repositorioContrato.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            return View(c);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato c)
        {
            try
            {
                c.IdContrato = id;
                repositorioContrato.Modificacion(c);
                TempData["Mensaje"] = "Se modificaron los datos";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(c);
            }
        }

        // GET: ContratoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var c = repositorioContrato.ObtenerPorId(id);
            var inicio = c.FechaInicio;
            var final = c.FechaCierre;
            var tiempoContrato = final - inicio;
            var hoy = DateTime.Now;
            if (final - hoy > tiempoContrato / 2)
            {
                ViewBag.Multa = c.Monto * 2;
            }
            else
            {
                ViewBag.Multa = c.Monto;
            }
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(c);
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato c)
        {
            try
            {
                repositorioContrato.Eliminar(id);
                TempData["Mensaje"] = "El contrato se elimino con exito";
                return RedirectToAction(nameof(Index));
            }
            
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(c);
            }
        }
    }
}
