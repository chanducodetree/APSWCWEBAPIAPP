using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APSWCWEBAPIAPP.Models
{
    public static class Policies
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Mob = "Mob";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(User).Build();
        }
        public static AuthorizationPolicy MobPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Mob).Build();
        }

    }
  

}
