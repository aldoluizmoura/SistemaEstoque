using SistemaEstoque.Infra.Entidades.Validações;
using SistemaEstoque.Infra.Enums;
using SistemaEstoque.Infra.Exceptions;

namespace SistemaEstoque.Infra.Entidades
{
    public class Documento : Entity
    {
        public TipoDocumento Tipo { get; private set; }
        public string Numero { get; private set; }

        //EF relations
        public Fabricante Fabricante { get; set; }
        public Usuario Usuario { get; set; }

        public Documento() { }

        public Documento(string numero)
        {
            if (!Validar(numero)) 
                throw new EntidadeExcepetions("Documento inválido");

            Numero = numero;
            Tipo = DefinirTipoDocumento(numero);
        }

        public TipoDocumento DefinirTipoDocumento(string numero)
        {
            if (CpfCnpjValidation.IsCpf(numero)) 
                return TipoDocumento.CPF;

            return TipoDocumento.CNPJ;
        }

        private static bool Validar(string cpfCnpj)
        {
            return CpfCnpjValidation.IsCpf(cpfCnpj) || CpfCnpjValidation.IsCnpj(cpfCnpj);
        }
    }
}
