using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WasteBR.Domain.Entities.Clientes;
using WasteBR.Domain.Interfaces.Repository.Clientes;

namespace WasteBR.Infra.Data.Repositories
{
    public class RelClienteEmpresaRepository : Repository<RelClienteEmpresa>, IRelClienteEmpresaRepository
    {
        public RelClienteEmpresaRepository(Context.WasteBRContext context)
          : base(context)
        { }

        public IEnumerable<RelClienteEmpresa> ListarPorContranteId(Guid contratanteId)
        {
            return DbSet.AsNoTracking()
                .Where(e => e.ContratanteId.Equals(contratanteId));
        }

        public RelClienteEmpresa ObterPorCliente(Guid clienteId)
        {
            return DbSet.AsNoTracking().Where(e => e.ClienteId.Equals(clienteId)).FirstOrDefault();
        }

        public RelClienteEmpresa ObterPorCodigo(Guid RelClienteEmpresaId)
        {
            return DbSet.AsNoTracking().Where(e => e.RelClienteEmpresaId.Equals(RelClienteEmpresaId)).FirstOrDefault();
        }

        public IEnumerable<RelClienteEmpresa> Pesquisar(Expression<Func<RelClienteEmpresa, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate).ToList();
        }

        public void RemoveRelacionamento(Guid RelClienteEmpresaId)
        {
            var rsResult = DbSet.AsNoTracking().Where(e => e.RelClienteEmpresaId.Equals(RelClienteEmpresaId)).FirstOrDefault();
            if (rsResult != null)
                DbSet.Remove(rsResult);
        }
    }
}