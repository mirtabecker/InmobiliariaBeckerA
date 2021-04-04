using InmoBecker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly IConfiguration configuration;

        public PropietariosController(IConfiguration configuration)
        {
           this.repositorioPropietario = new RepositorioPropietario(configuration);
           this.configuration = configuration;
        }
        // GET: PropietariosController
        public ActionResult Index()
        {
            List<Propietario> lista = repositorioPropietario.ObtenerTodos();
            return View(lista);
        }

        // GET: PropietariosController/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorioPropietario.ObtenerPorId(id);
            return View(p);
        }

        // GET: PropietariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)

        {
            try
            {
                int res = repositorioPropietario.Alta(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            
        }

        // GET: PropietariosController/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repositorioPropietario.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        // POST: PropietariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = null;
            try
            {
                p = repositorioPropietario.ObtenerPorId(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Email = collection["Email"];
                p.Telefono = collection["Telefono"];
                repositorioPropietario.Modificar(p);
                TempData["Mensaje"] = "Se guardo correctamente el Propietario";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }

        // GET: PropietariosController/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorioPropietario.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mesaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        // POST: PropietariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario entidad)
        {
            try
            {
                repositorioPropietario.Eliminar(id);
                TempData["Mensaje"] = "Se Elimino correctamente el propietario";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }
    }
}
