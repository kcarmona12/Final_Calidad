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
    internal class CrudTest
    {
        [Test]
        public void IndexTest()
        {
            var repo = new Mock<ICrudRpo>();
            repo.Setup(o => o.GetCuentas()).Returns(new List<Cuenta>());
            repo.Setup(o => o.GetTipos()).Returns(new List<Tipos>());

            var controller = new CrudController(repo.Object);
            var view = controller.Index() as ViewResult;

            Assert.AreEqual("Index", view.ViewName);
        }
        [Test]
        public void RegistrarTest()
        {
            var repo = new Mock<ICrudRpo>();
            repo.Setup(o => o.GetTipos()).Returns(new List<Tipos>());

            var controller = new CrudController(repo.Object);
            var view = controller.Registrar() as ViewResult;

            Assert.AreEqual("Registrar", view.ViewName);
        }
        [Test]
        public void RegistrarTransaccionPost()
        {
            var repo = new Mock<ICrudRpo>();
            repo.Setup(o => o.SaveCuenta(new Cuenta() ));

            var controller = new CrudController(repo.Object);
            var view = controller.Registrar(new Cuenta()) as RedirectToActionResult;

            Assert.AreEqual("Index", view.ActionName);
        }
    }
}
