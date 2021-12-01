using Finanzas.Controllers;
using Finanzas.Models;
using Finanzas.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finanzas_Test
{
    [TestFixture]
    internal class TransaccionTest
    {
        [Test]
        public void IndexTest()
        {
            var repo = new Mock<ITransaccionRpo>();
            repo.Setup(o => o.GetTransaccions(5)).Returns(new List<Transaccion>());
            repo.Setup(o => o.GetCuentas(5)).Returns(new Cuenta());

            var controller = new TransaccionController(repo.Object);
            var view = controller.Index(5) as ViewResult;

            Assert.AreEqual("Index", view.ViewName);
        }

        [Test]
        public void CrearTest()
        {
            var repo = new Mock<ITransaccionRpo>();

            var controller = new TransaccionController(repo.Object);
            var view = controller.Crear(5) as ViewResult;

            Assert.AreEqual("Crear", view.ViewName);
        }

        [Test]
        public void PostCrearIngresoTest()
        {
            var repo = new Mock<ITransaccionRpo>();
            repo.Setup(o => o.SaveTransaccion(new Transaccion() {CuentaId=5, Tipo="Ingreso", Amount= 100m}));
            repo.Setup(o => o.GetCuentas(5)).Returns(new Cuenta() { Id = 5, Amount = 500m , Limite = 500 });

            var controller = new TransaccionController(repo.Object);
            var view = controller.Crear(new Transaccion() { Tipo = "Ingreso", Amount = 100m , Motivo="Pago del mes"}) as RedirectToActionResult;

            Assert.AreEqual("Index", view.ActionName);
        }

        [Test]
        public void PostCrearEgresoTest()
        {
            var repo = new Mock<ITransaccionRpo>();
            repo.Setup(o => o.GetCuentas(5)).Returns(new Cuenta() { Id = 5,Name="hola", Amount = 500m, Limite = 100m });
            repo.Setup(o => o.SaveTransaccion(new Transaccion()));
            
            var controller = new TransaccionController(repo.Object);
            var view = controller.Crear(new Transaccion() { CuentaId = 5,Tipo = "Gasto", Amount = 10m }) as RedirectToActionResult;

            Assert.AreEqual("Index", view.ActionName);
        }

        [Test]
        public void PostCrearEgresoEXCEDIDOTest()
        {
            var repo = new Mock<ITransaccionRpo>();
            repo.Setup(o => o.GetCuentas(5)).Returns(new Cuenta() { Id = 5, Name = "hola", Amount = 500m, Limite = 100m });
            repo.Setup(o => o.SaveTransaccion(new Transaccion()));

            var controller = new TransaccionController(repo.Object);
            var view = controller.Crear(new Transaccion() { CuentaId = 5, Tipo = "Gasto", Amount = 9000m }) as ViewResult;

            Assert.AreEqual("Crear", view.ViewName);
        }
    }
}
