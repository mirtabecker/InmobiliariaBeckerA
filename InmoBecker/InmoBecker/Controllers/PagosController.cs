﻿using InmoBecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Controllers
{
    public class PagosController : Controller
    {
        private readonly RepositorioPago repositorioPago;
        private readonly RepositorioContrato repositorioContrato;

        private readonly IConfiguration configuration;

        public PagosController(IConfiguration configuration)
        {
            this.repositorioPago = new RepositorioPago(configuration);
           this.repositorioContrato = new RepositorioContrato(configuration);
           
            this.configuration = configuration;
        }

        // GET: PagosController
        [Authorize]
        public ActionResult Index(int id)
        {
            ViewBag.Contrato = repositorioContrato.ObtenerPorId(id);
            var lista = repositorioPago.ObtenerTodos(id);
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: PagosController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PagosController/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.Contrato = repositorioContrato.ObtenerPorId(id);
            ViewBag.Id = TempData["Id"];
            List<Pago> pagos = repositorioPago.ObtenerTodos(id);
            ViewBag.Pago = pagos.Count();
            
            return View();
        }

        // POST: PagosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorioPago.Alta(p);
                   TempData["Id"] = p.IdPago;
                    return RedirectToAction("Index", "Pagos", new {Id = p.ContratoId});
                }
                else
                {
                    ViewBag.Contrato = repositorioContrato.ObtenerPorId(p.ContratoId);
                    return View(p);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(p);
            }
        }

        // GET: PagosController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var e = repositorioPago.Obtener(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(e);
        }

        // POST: PagosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {
                pago.IdPago = id;
                repositorioPago.Modificacion(pago);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction("Index", "Pagos", new { id = pago.ContratoId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pago);
            }
        }

        // GET: PagosController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
           
                var d = repositorioPago.ObtenerPorId(id);
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                return View(d);
            
        }

        // POST: PagosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
                repositorioPago.Eliminar(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction("Index", "Contratos");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pago);
            }
        }
    }
}
