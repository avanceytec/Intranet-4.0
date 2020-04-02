using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class UsuariosViewModel
    {
        public int Id { get; set; }
        public string  Usuario  { get; set; }
        public string  Nombre { get; set; }       
        public string  Rol { get; set; }
        public bool IsSelected { get; set; }

    }
}
