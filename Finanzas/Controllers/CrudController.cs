using System;
using System.Collections.Generic;
using System.Linq;
using Finanzas.Models;
using Finanzas.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finanzas.Controllers
{
    public class CrudController : Controller
    {
        private readonly ICrudRpo _context;
        public CrudController(ICrudRpo context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var cuentas = _context.GetCuentas();

            ViewBag.Types = _context.GetTipos();

            ViewBag.Total = cuentas.Sum(o => o.Amount);
            return View("Index",cuentas);
        }

        [HttpGet]
        public ActionResult Registrar()
        {
            ViewBag.Types = _context.GetTipos();
            return View("Registrar", new Cuenta());
        }

        [HttpPost]
        public ActionResult Registrar(Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                if (cuenta.TypeId == 2)
                {
                    cuenta.Limite = cuenta.Amount;
                    cuenta.Amount = 0;
                }

                if (cuenta.Amount != 0 && cuenta.TypeId != 2)
                {
                    cuenta.Transaccions = new List<Transaccion>
                {
                    new Transaccion
                    {
                        FechaHora = DateTime.Now,
                        Tipo = "Ingreso",
                        Amount = cuenta.Amount,
                        Motivo = "Inicio"
                    }
                };
                }
                _context.SaveCuenta(cuenta);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Types = _context.GetTipos();
                return View("Registrar", cuenta);
            }
        }
    }
}