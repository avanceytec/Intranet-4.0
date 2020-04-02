using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class RolViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Status { get; set; }
        public List<RolMenuViewModel> Menus { get; set; }
    }
}
