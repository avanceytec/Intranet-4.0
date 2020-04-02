using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indicadores.Models.ViewModels
{
    public class GeneralViewModel
    {

        public string Tecnico { get; set; }
        public string Total { get; set; }
        public string Abiertas { get; set; }
        public string Cerradas { get; set; }
        public string Porcentaje_Cerradas { get; set; }
        public string Vencidas { get; set; }
        public string Porcentaje_Vencidas { get; set; }
        public string Tiempo { get; set; }
        public string Porcentaje_Tiempo { get; set; }
        public string Perfil { get; set; }
        public string Proyectos { get; set; }
        public string Compartidas { get; set; }
        public int Asignadas_Totales { get; set; }
        public int Asignadas_Abiertas { get; set; }
        public int Asignadas_Cerradas { get; set; }
        public int Asignadas_Vencidas { get; set; }
        public int Asignadas_Tiempo { get; set; }

    }
}
