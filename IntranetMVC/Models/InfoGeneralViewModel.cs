using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class InfoGeneralViewModel
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public String Departamento { get; set; }
        public String Email { get; set; }
        public String Extension { get; set; }
        public string Gerente { get; set; }
        public String Rol { get; set; }
    }
}
