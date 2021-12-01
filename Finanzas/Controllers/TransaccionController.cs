using System;
using System.Collections.Generic;
using System.Linq;
using Finanzas.Models;
using Finanzas.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finanzas.Controllers
{
    public class TransaccionController : Controller
    {
        private readonly ITransaccionRpo _context;
        public TransaccionController(ITransaccionRpo context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Index(int id)
        {
            var transacciones = _context.GetTransaccions(id);
            ViewBag.Cuenta = _context.GetCuentas(id);
            return View("Index", transacciones);
        }

        [HttpGet]
        public ActionResult Crear(int id)
        {
            ViewBag.Tipos = new List<string> { "Gasto", "Ingreso" };
            ViewBag.CuentaId = id;
            return View("Crear");
        }

        [HttpPost]
        public ActionResult Crear(Transaccion transaccion)
        {
            var cuenta = _context.GetCuentas(transaccion.CuentaId);

            if (transaccion.Tipo == "Gasto" && (cuenta.Amount + cuenta.Limite) < transaccion.Amount)
                ModelState.AddModelError("LIMITE","SUPERADO");

            if (ModelState.IsValid)
            {
                if (transaccion.Tipo == "Gasto")
                        transaccion.Amount *= -1;

                _context.SaveTransaccion(transaccion);
                return RedirectToAction("Index", new { id = transaccion.CuentaId });
            }
            else
            {
                ViewBag.Tipos = new List<string> { "Gasto", "Ingreso" };
                ViewBag.CuentaId = transaccion.CuentaId;
                return View("Crear", transaccion);
            }
        }
    }
}
