using Finanzas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Finanzas.Repositorio
{
    public interface ICrudRpo
    {
        List<Cuenta> GetCuentas();
        List<Tipos> GetTipos();
        void SaveCuenta(Cuenta cuenta);
    }
    public class CrudRpo : ICrudRpo
    {
        private readonly ContextoFinanzas _context;
        public CrudRpo(ContextoFinanzas context)
        {
            _context = context;
        }

        public List<Cuenta> GetCuentas()
        {
            return _context.Cuentas
                .Include(o => o.Tipo)
                .ToList();
        }

        public List<Tipos> GetTipos()
        {
            return _context.Types.ToList();
        }

        public void SaveCuenta(Cuenta cuenta)
        {
            _context.Cuentas.Add(cuenta);
            _context.SaveChanges();
        }
    }
}
