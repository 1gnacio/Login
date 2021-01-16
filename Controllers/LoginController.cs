using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SimpleLogin.Helper;
using SimpleLogin.Models;
using SimpleLogin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleLogin.Controllers
{
    public class LoginController : Controller
    {
        LoginCTX ctx;
        public LoginController(LoginCTX _ctx)
        {
            ctx = _ctx;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Login");
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home");
            }
            else
            {
                return View();
            }
        }
        [BindProperty]
        public UsuarioVM Usuario_ { get; set; }
        public async Task<IActionResult> Login()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new JObject()
                {   
                    { "StatusCode", 400 },
                    { "Message", "Complete todos los campos" }
                });
            }
            else
            {
                var result = await ctx.Usuarios.Include("Roles.Rol").Where(x => x.Nombre == Usuario_.Nombre).SingleOrDefaultAsync();
                if(result == null)
                {
                    return NotFound(new JObject()
                    {
                        { "StatusCode", 404 },
                        { "Message", "Usuario no encontrado" }
                    });
                }
                else
                {
                    if (HashHelper.CheckHash(Usuario_.Clave, result.Clave, result.Sal))
                    {
                        if (result.Roles.Count == 0)
                        {
                            var response = new JObject()
                            {
                                { "StatusCode", 403 },
                                { "Message", "El usuario no tiene acceso al sistema" }
                            };
                            return StatusCode(403, response);
                        }

                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.IdUsuario.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Name, result.Nombre));
                        
                        foreach(var r in result.Roles)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, r.Rol.Descripcion));
                        }
                        
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                            new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddDays(1), IsPersistent = true });
                        return Ok(result);
                    }
                    else
                    {
                        var response = new JObject()
                        {
                            { "StatusCode", 403 },
                            { "Message", "Usuario o contraseña no valido" }
                        };
                        return StatusCode(403, response);
                    }
                }
            }
        }
    }
}
