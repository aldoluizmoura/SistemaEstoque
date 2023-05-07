using System.Globalization;

namespace SistemaEstoque.Infra.Entidades.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizarString(string value)
        {
            return CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(value);
        }
    }
}
