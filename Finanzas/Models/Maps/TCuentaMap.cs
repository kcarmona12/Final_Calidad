using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finanzas.Models.Maps
{
    public class TCuentaMap : IEntityTypeConfiguration<Cuenta>
    {
        public void Configure(EntityTypeBuilder<Cuenta> builder)
        {
            builder.ToTable("Cuenta");
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Tipo).
                WithMany().
                HasForeignKey(o => o.TypeId);

            builder.HasMany(o => o.Transaccions).
                WithOne().
                HasForeignKey(o => o.CuentaId);
        }
    }
}
