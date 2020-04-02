using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class PermisosViewModel
    {

        public List<InfoGeneralViewModel> Informacion { get; set; }
        public List<RolViewModel> Roles { get; set; }
        //public List<SucursalesViewModel> Sucursales { get; set; }
        public List<UsuariosViewModel> PersonalCargo { get; set; }        
    }
}
