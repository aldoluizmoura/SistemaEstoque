using System.Security.Claims;

namespace SistemaEstoque.API.Autenticação
{
    public static class AuthExtension
    {
        public static Claim PegarUsuarioIdDoContext(HttpContext context)
        {
            return context.User.FindFirst(ClaimTypes.NameIdentifier);
        }
    }
}
