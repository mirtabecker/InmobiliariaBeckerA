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
        [Authorize]
        public ActionResult Index()
        {
                var lista = repositorioInmueble.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                ViewData["Error"] = TempData["Error"];
                if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
                return View(lista);
            
        }
        [Authorize]
        public ActionResult InmueblesPorPropietario(int id)
        {
                var lista = repositorioInmueble.BuscarPorPropietario(id);
                //ViewBag.Id = id;
                return View(lista); 
        }

        // GET: InmueblesController/Details/5
        public ActionResult Details(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);
            return View(i);
        }

        // GET: InmueblesController/Create
        [Authorize]
        public ActionResult Create()
        {

            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            ViewBag.Estados = Inmueble.ObtenerEstados();
            return View();
        }

        // POST: InmueblesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public  ActionResult Create(Inmueble i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorioInmueble.Alta(i);
                    TempData["Id"] = i.IdInmueble;
                    TempData["Mensaje"] = "Se dio de alta con exito al inmueble";
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
        [Authorize]
        public ActionResult Edit(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            ViewBag.Estados = Inmueble.ObtenerEstados();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: InmueblesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

        // GET: InmueblesController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);
            ViewBag.Error = TempData["Error"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: InmueblesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble i)
        {
            try
            {
                repositorioInmueble.Eliminar(id);
                TempData["Mensaje"] = "El Inmueble se elimino con exito";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede eliminar este INMUEBLE, porque tiene un CONTRATO ASOCIADO" : "Ocurrio Error";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }
    }
}
