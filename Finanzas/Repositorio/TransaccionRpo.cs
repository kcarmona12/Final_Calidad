using Finanzas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Finanzas.Repositorio
{
    public interface ITransaccionRpo
    {
        List<Transaccion> GetTransaccions(int id);
        Cuenta GetCuentas(int id);
        void SaveTransaccion(Transaccion transaccion);

    }
    public class TransaccionRpo : ITransaccionRpo
    {
        private readonly ContextoFinanzas _context;
        public TransaccionRpo(ContextoFinanzas context)
        {
            _context = context;
        }
        public Cuenta GetCuentas(int id)
        {
            return _context.Cuentas.First(o => o.Id == id);
        }

        public List<Transaccion> GetTransaccions(int id)
        {
            return _context.Transacciones.Where(o => o.CuentaId == id).ToList();
        }

        public void SaveTransaccion(Transaccion transaccion)
        {
            _context.Transacciones.Add(transaccion);
            _context.SaveChanges();
            ModificaMontoCuenta(transaccion.CuentaId);
        }

        private void ModificaMontoCuenta(int cuentaId)
        {
            var cuenta = _context.Cuentas
                .Include(o => o.Transaccions)
                .FirstOrDefault(o => o.Id == cuentaId);

            var total = cuenta.Transaccions.Sum(o => o.Amount);
            cuenta.Amount = total;
            _context.SaveChanges();
        }
    }
}
