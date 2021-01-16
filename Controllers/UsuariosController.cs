using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleLogin.Models;

namespace SimpleLogin.Controllers
{
    public class UsuariosController : Controller
    {
        readonly LoginCTX ctx;
        public UsuariosController(LoginCTX _ctx)
        {
            ctx = _ctx;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await ctx.Usuarios.Include("Roles.Rol").ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Create(string nombre)
        {
            var usuario = new Usuario()
            {
                Nombre = nombre,
                Clave = "123456",
                Sal = "12456",
                Edad = 28
            };
            await ctx.Usuarios.AddAsync(usuario);
            await ctx.SaveChangesAsync();
            return Ok(usuario);
        }
    }
}