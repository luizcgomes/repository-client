using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WasteBR.Domain.Dtos;
using WasteBR.Domain.Dtos.Clientes;
using WasteBR.Domain.Entities;
using WasteBR.Domain.Interfaces.Repository;

namespace WasteBR.Infra.Data.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(Context.WasteBRContext context)
          : base(context)
        { }

        public void ConfirmarCadastro(Guid clienteId)
        {
            var cn = _context.Database.GetDbConnection();
            string SQLQuery = "";

            SQLQuery = @"

            UPDATE Clientes
                SET Ativo = 1
                    , UsuarioAlteracao = 'Ativação de cadastro'
                    , DataAlteracao = getdate()
            Where ClienteId = @clienteId";

            var _rowsAffected = cn.Execute(SQLQuery, new { clienteId = clienteId });
        }

        public void Excluir(Guid clienteId)
        {
            var cn = _context.Database.GetDbConnection();
            string SQLQuery = "";

            SQLQuery = @"

            DELETE FROM Clientes Where ClienteId = @clienteId";

            var _rowsAffected = cn.Execute(SQLQuery, new { clienteId = clienteId });
        }

        public IEnumerable<Cliente> ListarPorContranteId(Guid contratanteId)
        {
            return DbSet
                .Include(e => e.relClienteEmpresas)
                .Where(e => e.ContratanteId.Equals(contratanteId)).AsNoTracking();

            //return DbSet.AsNoTracking().Where(e => e.ContratanteId.Equals(contratanteId));
        }

        public Cliente ObterPorCnpj(string cnpj)
        {
            return DbSet
                .Include(e => e.relClienteEmpresas)
                .Where(e => e.CnpjCpf.Equals(cnpj)).AsNoTracking().FirstOrDefault();
        }

        public Cliente ObterPorCodigo(Guid clienteId)
        {
            return DbSet
                .Include(e => e.relClienteEmpresas)
                .Where(e => e.ClienteId.Equals(clienteId)).AsNoTracking().FirstOrDefault();
        }

        public IEnumerable<Cliente> PesquisarCliente(Expression<Func<Cliente, bool>> predicate)
        {
            return DbSet
                .Include(e => e.relClienteEmpresas)
                .Where(predicate).AsNoTracking().ToList();
        }
        

        public Paginado<ClienteDTO> ObterListaPaginacao(ClienteFiltroDTO _filtro, int pageSize, int pageNumber)
        {
            var strQuery = @" 
            DECLARE @pageNumber     AS int = " + pageNumber + @"
			DECLARE @pageSize		as int  = " + pageSize + @"

            Select distinct
	            ClienteId
	            , SeqInterno
	            , CnpjCpf
	            , RazaoSocial
	            , NomeFantasia
	            , InscEstadual
	            , InscMunicipal
	            , TipoInscEstadual
	            , Segmento
	            , CodigoExterno = isnull(CodigoExterno,'') 
	            , Endereco
	            , Numero
	            , Bairro
	            , Cidade
	            , Cep
	            , UF
	            , Complemento = isnull(Complemento,'')
	            , Pais
	            , Email1 = isnull(Email1,'')
	            , Email2 = isnull(Email2,'')
	            , Email3 =  isnull(Email3,'')
	            , Tipo1  = isnull(Tipo1,0)
	            , Tipo2 = isnull(Tipo2,0)
	            , Tipo3 = isnull(Tipo3,0)
	            , DDD1 = isnull(DDD1,'')
	            , DDD2= isnull(DDD2,'')
	            , DDD3 = isnull(DDD3,'')
	            , Fone1 = isnull(Fone1,'')
	            , Fone2 = isnull(Fone2,'')
	            , Fone3 = isnull(Fone3,'')
	            , Ativo
	            , ContratanteId
	            , MunicipiosIBGEId

            From clientes

            Where SeqInterno  > 0 

                   @ComplementoClausulaWhere

            ORDER BY RazaoSocial

            OFFSET @pageSize * (@pageNumber - 1) ROWS 

            FETCH NEXT @pageSize ROWS ONLY 

            Select count(ClienteId) From clientes
            
            Where SeqInterno  > 0 

                   @ComplementoClausulaWhere

";


            var strClausulaWhere = "";

			if (String.IsNullOrEmpty(_filtro.CPFCNPJ) != true)
				strClausulaWhere += Environment.NewLine + " AND clientes.CnpjCpf = '" + _filtro.CPFCNPJ + "'";

            if (String.IsNullOrEmpty(_filtro.RazaoSocial) != true)
                strClausulaWhere += Environment.NewLine + " AND clientes.RazaoSocial LIKE '%" + _filtro.RazaoSocial + "%'";

            if (String.IsNullOrEmpty(_filtro.NomeFantasia) != true)
                strClausulaWhere += Environment.NewLine + " AND clientes.NomeFantasia LIKE '%" + _filtro.NomeFantasia + "%'";

            if (string.IsNullOrEmpty(strClausulaWhere))
                strQuery = strQuery.Replace("@ComplementoClausulaWhere", "");
            else
                strQuery = strQuery.Replace("@ComplementoClausulaWhere", strClausulaWhere);

            var offSet = pageSize * (pageNumber - 1);

            
            var cn = _context.Database.GetDbConnection();
            var multi = cn.QueryMultiple(strQuery);

            var clientes = multi.Read<ClienteDTO>();
            var total = multi.Read<int>().FirstOrDefault();

            var pagedList = new Paginado<ClienteDTO>()
            {
                List = clientes,
                Count = total
            };

            return pagedList;

        }
    }
}