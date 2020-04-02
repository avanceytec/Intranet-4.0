using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class InformacionViewModel
    {
        public List<UsuariosViewModel> Usuarios { get; set; }
        public List<SubmodulosViewModel> Submodulos { get; set; }
        public List<ModulosViewModel> Modulos { get; set; }
        public List<RolViewModel> Roles { get; set; }
    }
}

