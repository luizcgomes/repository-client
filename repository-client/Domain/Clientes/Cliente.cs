using System;
using WasteBR.Domain.Entities.CadUnico;
using WasteBR.Domain.Entities.Clientes;
using WasteBR.Domain.Enums;

namespace WasteBR.Domain.Entities
{
    public class Cliente : EntidadeBaseUnico
    {
        public Guid ClienteId { get; set; }
        public long SeqInterno { get; set; }
        public string CodigoExterno { get; set; }

        public string CnpjCpf { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string InscEstadual { get; set; }
        public string InscMunicipal { get; set; }
        public TipoInscricao TipoInscEstadual { get; set; }
        public TpSegmento Segmento { get; set; }

        public Guid RelClienteEmpresaId { get; set; }
        public virtual RelClienteEmpresa relClienteEmpresas { get; set; }

        public string RetemISS { get; set; }

        public Guid MunicipiosIBGEId { get; set; }
        public virtual MunicipiosIBGE MunicipiosIBGE { get; set; }

        public Cliente()
        {
            ClienteId = Guid.NewGuid();
        }

        public string PessoaJuridica()
        {
            CnpjCpf = CnpjCpf.Replace(".", "").Replace("-", "").Replace("/", "");

            return (CnpjCpf.Length > 11) ? "S" : "N";
        }

        public string ClienteAtivo()
        {
            return (Ativo == true) ? "S" : "N";
        }

        public string Fone1ComDDD()
        {
            DDD1 = string.IsNullOrEmpty(DDD1) ? "" : DDD1;
            Fone1 = string.IsNullOrEmpty(Fone1) ? "" : Fone1;

            return "(" + DDD1.Trim() + ") " + Fone1.Trim();
        }

        public string Fone2ComDDD()
        {
            DDD2 = string.IsNullOrEmpty(DDD2) ? "" : DDD2;
            Fone2 = string.IsNullOrEmpty(Fone2) ? "" : Fone2;

            return "(" + DDD2.Trim() + ") " + Fone2.Trim();
        }

        public string Fone3ComDDD()
        {
            DDD3 = string.IsNullOrEmpty(DDD3) ? "" : DDD3;
            Fone3 = string.IsNullOrEmpty(Fone3) ? "" : Fone3;

            return "(" + DDD3.Trim() + ") " + Fone3.Trim();
        }

        public override bool Validado => (!string.IsNullOrEmpty(CnpjCpf) &&
                                          !string.IsNullOrEmpty(RazaoSocial) &&
                                          !string.IsNullOrEmpty(NomeFantasia) &&
                                          !string.IsNullOrEmpty(InscEstadual) &&
                                          !string.IsNullOrEmpty(InscMunicipal) &&
                                          !string.IsNullOrEmpty(Endereco) &&
                                          !string.IsNullOrEmpty(Numero) &&
                                          !string.IsNullOrEmpty(Bairro) &&
                                          !string.IsNullOrEmpty(Cidade) &&
                                          !string.IsNullOrEmpty(UF) &&
                                          !string.IsNullOrEmpty(Cep) &&
                                          !string.IsNullOrEmpty(Fone1) &&
                                          !string.IsNullOrEmpty(Email1));
    }
}