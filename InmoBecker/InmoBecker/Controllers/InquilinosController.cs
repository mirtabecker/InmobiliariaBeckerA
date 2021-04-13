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
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly IConfiguration configuration;

        public InquilinosController(IConfiguration configuration)
        {
            this.repositorioInquilino = new RepositorioInquilino(configuration);
            this.configuration = configuration;
        }
        // GET: InquilinosController
        public ActionResult Index()
        {
            try 
            {
                List<Inquilino> lista = repositorioInquilino.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View(lista);
            } 
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: InquilinosController/Details/5
        public ActionResult Details(int id)
        {
            var i = repositorioInquilino.ObtenerPorId(id);
            return View(i);
        }

        // GET: InquilinosController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: InquilinosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int res = repositorioInquilino.Alta(i);
                    TempData["Id"] = i.IdInquilino;
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(i);
            }
            catch (Exception ex)
            {
                ViewBag.Errores = ex.Message;
                return View(i);
            }
        }

        // GET: InquilinosController/Edit/5
        public ActionResult Edit(int id)
        {
            var i = repositorioInquilino.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: InquilinosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino i = null;
            try
            {
                i = repositorioInquilino.ObtenerPorId(id);
                i.Nombre = collection["Nombre"];
                i.Apellido = collection["Apellido"];
                i.Dni = collection["Dni"];
                i.Telefono = collection["Telefono"];
                i.Email = collection["Email"];
                i.Garante = collection["Garante"];
                i.TelGarante = collection["TelGarante"];
                repositorioInquilino.Modificar(i);
                TempData["Mensaje"] = "Se guardo correctamente los cambios realizados";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(i);
            }
        }

        // GET: InquilinosController/Delete/5
        public ActionResult Delete(int id)
        {
            var i = repositorioInquilino.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mesaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: InquilinosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino i)
        {
            try
            {
                repositorioInquilino.Eliminar(id);
                TempData["Mensaje"] = "Se Elimino correctamente el Inquilino";
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
