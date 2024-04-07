using System;

namespace WasteBR.Domain.Entities.Clientes
{
    public class RelClienteEmpresa : EntidadeBase
    {
        public Guid RelClienteEmpresaId { get; set; }

        public Guid ClienteId { get; set; }
        public virtual Cliente cliente { get; set; }

        public string ListaEmpresasRelacionadas { get; set; }

        public RelClienteEmpresa()
        {
            //ClienteId = Guid.NewGuid();
        }

        public override bool Validado => (!string.IsNullOrEmpty(ListaEmpresasRelacionadas));
    }
}