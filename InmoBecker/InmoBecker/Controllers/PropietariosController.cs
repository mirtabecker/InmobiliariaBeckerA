using InmoBecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly IConfiguration configuration;

        public PropietariosController(IConfiguration configuration)
        {
            this.repositorioPropietario = new RepositorioPropietario(configuration);
            this.repositorioInmueble = new RepositorioInmueble(configuration);
            this.configuration = configuration;
        }
        // GET: PropietariosController
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                List<Propietario> lista = repositorioPropietario.ObtenerTodos();
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

        // GET: Propietario/Buscar/5
        [Authorize]
        public ActionResult Buscar(string q)
        {
            try
            {
                var lista = repositorioPropietario.BuscarPorNombre(q);
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (lista.Count == 0)
                {
                    var propietario = repositorioPropietario.ObtenerPorEmail(q);
                    if (propietario == null)
                    {
                        TempData["Mensaje"] = "No se han encontrado propietarios con ese nombre o email";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return View("Details", propietario);
                }
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }

        // GET: PropietariosController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        { 
            var p = repositorioPropietario.ObtenerPorId(id);
            return View(p);
        }

        // GET: PropietariosController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Propietario p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: p.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    p.Clave = hashed;
                    repositorioPropietario.Alta(p);
                    TempData["Id"] = p.IdPropietario;
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(p);
            }
            catch (Exception ex)
            {
                ViewBag.Errores = ex.Message;
                return View(p);
            }

        }

        // GET: PropietariosController/Edit/5
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(int id,Propietario p)
        {
            try
            {
                p.IdPropietario = id;
                repositorioPropietario.Modificar(p);
                TempData["Mensaje"] = "Datos guardados correctamente";
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
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var p = repositorioPropietario.ObtenerPorId(id);
            ViewBag.Error = TempData["Error"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        // POST: PropietariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Propietario p)
        {
            try
            {
                repositorioPropietario.Eliminar(id);
                TempData["Mensaje"] = "PROPIETARIO eliminado con exito";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede eliminar este PROPIRTARIO, porque tiene un INMUEBLE ASOCIADO." : "Ocurrio Error";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return View(p);
            }
        }
    }
}