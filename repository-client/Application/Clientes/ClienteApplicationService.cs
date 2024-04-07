using AutoMapper;
using System;
using System.Collections.Generic;
using WasteBR.Application.Interfaces;
using WasteBR.Application.ViewModels.Cliente;
using WasteBR.Domain.Dtos;
using WasteBR.Domain.Dtos.Clientes;
using WasteBR.Domain.Entities;
using WasteBR.Domain.Interfaces.Repository;
using WasteBR.Domain.Interfaces.Services;
using WasteBR.Domain.ValueObjects;
using WasteBR.Infra.Services.Interface;
using WasteBR.Infra.Services.Model;

namespace WasteBR.Application.Services
{
    public class ClienteApplicationService : ApplicationService, IClienteApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IClienteService _clienteService;
        private readonly IWSCorreios _wscorreios;

        public ClienteApplicationService(IMapper mapper, IClienteService clienteService, IUnitOfWork uow, IWSCorreios wsCorreios)
            : base(uow)
        {
            _mapper = mapper;
            _clienteService = clienteService;
            _wscorreios = wsCorreios;
        }

        private ClienteViewModel ViewToEntity(ClienteViewModel _entity)
        {
            _entity.relClienteEmpresas.ContratanteId = _entity.ContratanteId;
            _entity.relClienteEmpresas.ClienteId = _entity.ClienteId;
            _entity.relClienteEmpresas.ListaEmpresasRelacionadas = _entity.ListaIDEmpresasSelecionadas;
            _entity.relClienteEmpresas.Ativo = _entity.Ativo;

            _entity.RazaoSocial = _entity.RazaoSocial.Replace("'", "´");
            _entity.NomeFantasia = _entity.NomeFantasia.Replace("'", "´");
            _entity.Endereco = _entity.Endereco.Replace("'", "´");
            _entity.Bairro = _entity.Bairro.Replace("'", "´");

            


            return _entity;
        }

        public Retorno Atualizar(ClienteViewModel cliente)
        {
            BeginTransaction();

            cliente = ViewToEntity(cliente);

            var resultado = _clienteService.Atualizar(_mapper.Map<Cliente>(cliente));

            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }

        public Retorno Cadastrar(ClienteViewModel cliente)
        {
            BeginTransaction();

            cliente = ViewToEntity(cliente);

            var resultado = _clienteService.Cadastrar(_mapper.Map<Cliente>(cliente));

            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }

        public IEnumerable<ClienteViewModel> ListarPorContranteId(Guid contratanteId)
         => _mapper.Map<IEnumerable<ClienteViewModel>>(_clienteService.ListarPorContranteId(contratanteId));

        public ClienteViewModel ObterPeloId(Guid ClienteId)
        => _mapper.Map<ClienteViewModel>(_clienteService.ObterPeloId(ClienteId));

        public Endereco ObterEnderecoPorCep(string cep)
        {
            var enderecoCorreios = _wscorreios.ObterEnderecoPorCep(cep);
            if (enderecoCorreios == null)
                return enderecoCorreios;
            return enderecoCorreios;
        }

        public void Dispose()
        {
            _clienteService.Dispose();
            GC.SuppressFinalize(this);
        }
        public Paginado<ClienteDTO> ObterListaPaginacao(string SearchData, string TipoFiltro, int pageSize, int pageNumber)
        {

            var _filtro = new ClienteFiltroDTO();

            if (TipoFiltro.Trim().ToUpper() == "EMPRESA")
            {
                SearchData = (string.IsNullOrWhiteSpace(SearchData) ? "" : SearchData);
                _filtro.Empresa = SearchData;
            }
            else if (TipoFiltro.Trim().ToUpper() == "CNPJCPF")
            {
                SearchData = (string.IsNullOrWhiteSpace(SearchData) ? "" : SearchData);
                _filtro.CPFCNPJ = SearchData;
            }
            else if (TipoFiltro.Trim().ToUpper() == "RAZAOSOCIAL")
            {
                SearchData = (string.IsNullOrWhiteSpace(SearchData) ? "" : SearchData);
                _filtro.RazaoSocial = SearchData;

            }
            else if (TipoFiltro.Trim().ToUpper() == "NOMEFANTASIA")
            {
                SearchData = (string.IsNullOrWhiteSpace(SearchData) ? "" : SearchData);
                _filtro.NomeFantasia = SearchData;

            }

            return _clienteService.ObterListaPaginacao(_filtro, pageSize, pageNumber);
        }
        public IEnumerable<ClienteViewModel> PesquisarCliente(ClientePesquisarViewModel filtro)
        {
            filtro.RazaoSocial = (String.IsNullOrEmpty(filtro.RazaoSocial) == false) ? filtro.RazaoSocial : "";
            filtro.CPFCNPJ = (String.IsNullOrEmpty(filtro.CPFCNPJ) == false) ? filtro.CPFCNPJ : "";
            filtro.NomeFantasia = (String.IsNullOrEmpty(filtro.NomeFantasia) == false) ? filtro.NomeFantasia : "";
            filtro.Empresa = (String.IsNullOrEmpty(filtro.Empresa) == false) ? filtro.Empresa : "";

            var filtroCliente = new Cliente
            {
                ContratanteId = filtro.ContratanteId,
                CnpjCpf = filtro.CPFCNPJ,
                RazaoSocial = filtro.RazaoSocial,
                NomeFantasia = filtro.NomeFantasia,
                relClienteEmpresas = new Domain.Entities.Clientes.RelClienteEmpresa
                {
                    ListaEmpresasRelacionadas = filtro.Empresa
                }
            };

            return _mapper.Map<IEnumerable<ClienteViewModel>>(_clienteService.PesquisarCliente(filtroCliente));
        }

        public RootObject ObterGeoLocalizacao(string Endereco)
        {
            throw new NotImplementedException();
        }

        public Retorno EnviarEmail(ConfiguracaoSMTP _configSMTP)
        {
            return _wscorreios.EnviarEmail(_configSMTP);
        }

        public Retorno ConfirmarCadastro(Guid clienteId)
        {
            return _clienteService.ConfirmarCadastro(clienteId);
        }

        public Retorno Excluir(ClienteViewModel cliente)
        {
            BeginTransaction();
            
            var resultado = _clienteService.Excluir(_mapper.Map<Cliente>(cliente));
            if (!resultado.HouveFalha)
                Commit();

            return resultado;
        }
    }
}