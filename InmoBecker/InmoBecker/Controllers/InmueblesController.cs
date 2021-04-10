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
            List<Inmueble> lista = repositorioInmueble.ObtenerTodos();
            
            return View(lista);
        }


        // GET: InmueblesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                    return View();
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
            return View();
        }

        // POST: InmueblesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmueblesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InmueblesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
