using Microsoft.AspNetCore.Authorization;

namespace Sakila.Api.Authorization
{
    
    public static class AuthorizationPolicyBuilderExtensions
    {
        // Metoda rozszerzająca do sprawdzania uprawnień
        public static AuthorizationPolicyBuilder RequirePermission(this AuthorizationPolicyBuilder builder, string permission)
        {
            builder.RequireClaim("Permission", permission);

            return builder;
        }
    }
}
