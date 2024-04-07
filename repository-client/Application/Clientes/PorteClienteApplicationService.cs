using AutoMapper;
using System;
using System.Collections.Generic;
using WasteBR.Application.Interfaces;
using WasteBR.Application.ViewModels.PorteCliente;
using WasteBR.Domain.Entities;
using WasteBR.Domain.Interfaces.Repository;
using WasteBR.Domain.Interfaces.Services;
using WasteBR.Domain.ValueObjects;

namespace WasteBR.Application.Services
{
    public class PorteClienteApplicationService : ApplicationService, IPorteClienteApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IPorteClienteService _porteClienteService;

        public PorteClienteApplicationService(IMapper mapper, IPorteClienteService porteClienteService, IUnitOfWork uow)
            : base(uow)
        {
            _porteClienteService = porteClienteService;
            _mapper = mapper;
        }

        public IEnumerable<PorteClienteViewModel> ListarPorContranteId(Guid contratanteId)
                 => _mapper.Map<IEnumerable<PorteClienteViewModel>>(_porteClienteService.ListarPorContranteId(contratanteId));

        public PorteClienteViewModel ObterPeloCodigo(int codigo)
        {
            return _mapper.Map<PorteClienteViewModel>(_porteClienteService.ObterPeloCodigo(codigo));
        }

        public PorteClienteViewModel ObterPeloId(Guid porteId)
        {
            return _mapper.Map<PorteClienteViewModel>(_porteClienteService.ObterPeloId(porteId));
        }

        public Retorno Cadastrar(PorteClienteViewModel porte)
        {
            BeginTransaction();

            var resultado = _porteClienteService.Cadastrar(_mapper.Map<PorteCliente>(porte));

            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }

        public Retorno Atualizar(PorteClienteViewModel porte)
        {
            BeginTransaction();

            var resultado = _porteClienteService.Atualizar(_mapper.Map<PorteCliente>(porte));

            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }

        public void Dispose()
        {
            _porteClienteService.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}