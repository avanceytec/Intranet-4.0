using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class SubmodulosViewModel
    {

        public int Id { get; set; }
        public string Modulo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }       
        public int Status { get; set; }       
    }
}
