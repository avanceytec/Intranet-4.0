using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class ModulosViewModel
    {
        public int Id { get; set; }
        public string Menu { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }       
        public int Status { get; set; }     
        public List<SubmodulosViewModel> Submodulos { get; set; }
    }
}
