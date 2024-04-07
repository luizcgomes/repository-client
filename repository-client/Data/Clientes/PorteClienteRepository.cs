using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WasteBR.Domain.Entities;
using WasteBR.Domain.Interfaces.Repository;

namespace WasteBR.Infra.Data.Repositories
{
    public class PorteClienteRepository : Repository<PorteCliente>, IPorteClienteRepository
    {
        public PorteClienteRepository(Context.WasteBRContext context)
            : base(context)
        { }

        public IEnumerable<PorteCliente> ListarPorContranteId(Guid contratanteId)
            => DbSet.AsNoTracking().Where(p => p.ContratanteId.Equals(contratanteId));

        public PorteCliente ObterPeloCodigo(int codigo)
            => DbSet.AsNoTracking().Where(x => x.Codigo == codigo).FirstOrDefault();
    }
}