using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleLogin.Models
{
    public class LoginCTX : DbContext
    {
        public LoginCTX(DbContextOptions<LoginCTX> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UsuarioRol>().HasKey(x => new { x.IdUsuario, x.IdRol });
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
    }
}
