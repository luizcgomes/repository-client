using AutoMapper;
using System;
using System.Collections.Generic;
using WasteBR.Application.Interfaces;
using WasteBR.Application.ViewModels.SegmentoCliente;
using WasteBR.Domain.Entities;
using WasteBR.Domain.Interfaces.Repository;
using WasteBR.Domain.Interfaces.Services;
using WasteBR.Domain.ValueObjects;

namespace WasteBR.Application.Services
{
    public class SegmentoClienteApplicationService : ApplicationService, ISegmentoClienteApplicationService
    {
        private readonly IMapper _mapper;
        private readonly ISegmentoClienteService _segmentoClienteService;

        public SegmentoClienteApplicationService(IMapper mapper, ISegmentoClienteService segmentoClienteService, IUnitOfWork uow)
            : base(uow)
        {
            _segmentoClienteService = segmentoClienteService;
            _mapper = mapper;
        }

        public Retorno Atualizar(SegmentoClienteViewModel segmento)
        {
            BeginTransaction();

            var resultado = _segmentoClienteService.Atualizar(_mapper.Map<SegmentoCliente>(segmento));

            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }

        public Retorno Cadastrar(SegmentoClienteViewModel segmento)
        {
            BeginTransaction();

            var resultado = _segmentoClienteService.Cadastrar(_mapper.Map<SegmentoCliente>(segmento));

            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }

        public IEnumerable<SegmentoClienteViewModel> ListarPorContranteId(Guid contratanteId)
            => _mapper.Map<IEnumerable<SegmentoClienteViewModel>>(_segmentoClienteService.ListarPorContranteId(contratanteId));

        public SegmentoClienteViewModel ObterPeloCodigo(int codigo)
            => _mapper.Map<SegmentoClienteViewModel>(_segmentoClienteService.ObterPeloCodigo(codigo));

        public SegmentoClienteViewModel ObterPeloId(Guid segmentoId)
            => _mapper.Map<SegmentoClienteViewModel>(_segmentoClienteService.ObterPeloId(segmentoId));

        public void Dispose()
        {
            _segmentoClienteService.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}