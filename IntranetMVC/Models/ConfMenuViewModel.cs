using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class ConfMenuViewModel
    {
        public string Nombre { get; set; }
        //public List<MenuViewModel> Menu {get; set;}
        public List<ModulosViewModel> Modulos { get; set; }        
    }
}
