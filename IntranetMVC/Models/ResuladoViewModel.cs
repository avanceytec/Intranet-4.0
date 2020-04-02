using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetMVC.Models
{
    public class ResuladoViewModel
    {
        public string Tecnico { get; set; }
        public int Proyectos { get; set; }
        public int Total { get; set; }
        public int Abiertas { get; set; }
        public int Cerradas { get; set; }
        public int Porcentaje_Cerradas { get; set; }
        public string Cerradas_class { get; set; }
        public int Vencidas { get; set; }
        public int Porcentaje_Vencidas { get; set; }
        public string Vencidas_class { get; set; }
        public int Tiempo { get; set; }
        public int Porcentaje_Tiempo { get; set; }
        public string Tiempo_class { get; set; }
        public int Compartidas { get; set; }
        public int Asignadas_Totales { get; set; }
        public int Asignadas_Abiertas { get; set; }
        public int Asignadas_Cerradas { get; set; }
        public int Asignadas_Vencidas { get; set; }
        public int Asignadas_Tiempo { get; set; }

        //public int Abiertas { get; set; 
    }
}
