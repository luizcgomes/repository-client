using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WasteBR.Domain.Entities;
using WasteBR.Domain.Interfaces.Repository;

namespace WasteBR.Infra.Data.Repositories
{
    public class SegmentoClienteRepository : Repository<SegmentoCliente>, ISegmentoClienteRepository
    {
        public SegmentoClienteRepository(Context.WasteBRContext context)
            : base(context)
        { }

        public IEnumerable<SegmentoCliente> ListarPorContranteId(Guid contratanteId)
            => DbSet.AsNoTracking().Where(p => p.ContratanteId.Equals(contratanteId));

        public SegmentoCliente ObterPeloCodigo(int codigo)
            => DbSet.AsNoTracking().Where(x => x.Codigo == codigo).FirstOrDefault();
    }
}