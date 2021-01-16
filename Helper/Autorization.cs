using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleLogin.Helper
{
    public class Autorization
    {
    }

    public class EdadMinima : IAuthorizationRequirement
    {
        public int Edad { get; set; }
        public EdadMinima(int _Edad)
        {
            Edad = _Edad;
        }
    }

    public class EdadMinimaHandler : AuthorizationHandler<EdadMinima>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EdadMinima requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "Edad"))
            {
                return Task.CompletedTask;
            }
            int edad = Convert.ToInt32(context.User.FindFirst(x => x.Type == "Edad").Value);
            if (edad >= requirement.Edad)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
