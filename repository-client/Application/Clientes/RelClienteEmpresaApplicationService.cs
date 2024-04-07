using AutoMapper;
using System;
using System.Collections.Generic;
using WasteBR.Application.Interfaces;
using WasteBR.Application.ViewModels.Clientes;
using WasteBR.Domain.Entities.Clientes;
using WasteBR.Domain.Interfaces.Repository;
using WasteBR.Domain.Interfaces.Services.Clientes;
using WasteBR.Domain.ValueObjects;

namespace WasteBR.Application.Services
{
    public class RelClienteEmpresaApplicationService : ApplicationService, IRelClienteEmpresaApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IRelClienteEmpresaService _service;

        public RelClienteEmpresaApplicationService(IMapper mapper, IRelClienteEmpresaService service, IUnitOfWork uow)
            : base(uow)
        {
            _mapper = mapper;
            _service = service;
        }

        public Retorno Atualizar(RelClienteEmpresaViewModel _entity)
        {
            BeginTransaction();

            var resultado = _service.Atualizar(_mapper.Map<RelClienteEmpresa>(_entity));

            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }

        public Retorno Cadastrar(RelClienteEmpresaViewModel _entity)
        {
            BeginTransaction();

            var resultado = _service.Cadastrar(_mapper.Map<RelClienteEmpresa>(_entity));

            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }

        public IEnumerable<RelClienteEmpresaViewModel> ListarPorContranteId(Guid contratanteId)
            => _mapper.Map<IEnumerable<RelClienteEmpresaViewModel>>(_service.ListarPorContranteId(contratanteId));

        public RelClienteEmpresaViewModel ObterPeloId(Guid relClienteEmpresaId)
            => _mapper.Map<RelClienteEmpresaViewModel>(_service.ObterPeloId(relClienteEmpresaId));

        public IEnumerable<RelClienteEmpresaViewModel> Pesquisar(RelClienteEmpresaViewModel filtro)
        {
            return _mapper.Map<IEnumerable<RelClienteEmpresaViewModel>>(_service.Pesquisar(_mapper.Map<RelClienteEmpresa>(filtro)));
        }

        public RelClienteEmpresaViewModel ObterPorCliente(Guid clienteId)
            => _mapper.Map<RelClienteEmpresaViewModel>(_service.ObterPorCliente(clienteId));

        public void RemoveRelacionamento(Guid RelClienteEmpresaId)
        {
            BeginTransaction();
            _service.RemoveRelacionamento(RelClienteEmpresaId);
            Commit();
        }

        public void Dispose()
        {
            _service.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}