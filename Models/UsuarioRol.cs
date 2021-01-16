using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleLogin.Models
{
    [Table("UsuarioRol")]
    public class UsuarioRol
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; }
    }
}
