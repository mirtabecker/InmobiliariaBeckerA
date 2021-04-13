using InmoBecker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Controllers
{
    public class InmueblesController : Controller
    {
       private readonly RepositorioInmueble repositorioInmueble;
       private readonly RepositorioPropietario repositorioPropietario;
        private readonly IConfiguration configuration;

        public InmueblesController(IConfiguration configuration)
    {
        this.repositorioInmueble = new RepositorioInmueble(configuration);
        this.repositorioPropietario = new RepositorioPropietario(configuration);
        this.configuration = configuration;
    }

        // GET: InmueblesController
        public ActionResult Index()
        {
            try
            {
                List<Inmueble> lista = repositorioInmueble.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                ViewData["Error"] = TempData["Error"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View(lista);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
            // GET: InmueblesController/Details/5
            public ActionResult Details(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);
            return View(i);
        }

        // GET: InmueblesController/Create
        public ActionResult Create()
        {

            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            return View();
        }

        // POST: InmueblesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create(Inmueble i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorioInmueble.Alta(i);
                    TempData["Id"] = i.IdInmueble;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                    return View(i);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrete = ex.StackTrace;
                return View(i);
            }
        }

        // GET: InmueblesController/Edit/5
        public ActionResult Edit(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: InmueblesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble i)
        {
            try
            {
                i.IdInmueble = id;
                repositorioInmueble.Modificacion(i);
                TempData["Mensaje"] = "Se modificaron los datos";
                return RedirectToAction(nameof(Index));
              
            }
            catch (Exception ex)
            {
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

        // GET: InmueblesController/Delete/5
        public ActionResult Delete(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: InmueblesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble i)
        {
            try
            {
                repositorioInmueble.Eliminar(id);
                TempData["Mensaje"] = "El inmueble se elimino con exito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(i);
            }
        }
    }
}
