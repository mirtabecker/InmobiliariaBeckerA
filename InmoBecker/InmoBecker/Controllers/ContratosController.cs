using InmoBecker.Models;
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
            public ActionResult Index()
        {
            
                List<Contrato> lista = repositorioContrato.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                ViewData["Error"] = TempData["Error"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View(lista);
           
        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            var i = repositorioContrato.ObtenerPorId(id);
            return View(i);
        }

        // GET: ContratoController/Create
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                    return View(c);
                }
            }

            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(c);
            }
        }

        // GET: ContratoController/Edit/5
        public ActionResult Edit(int id)
        {
            var c = repositorioContrato.ObtenerPorId(id);
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(c);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(c);
            }
        }

        // GET: ContratoController/Delete/5
        public ActionResult Delete(int id)
        {
            var c = repositorioContrato.ObtenerPorId(id);
         
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(c);
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                return View(c);
            }
        }
    }
}
