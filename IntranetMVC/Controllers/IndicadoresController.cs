using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indicadores.Models.ViewModels;
using IntranetMVC.Helpers;
using IntranetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IntranetMVC.Controllers
{
    public class IndicadoresController : Controller
    {
        public async Task<ActionResult> Index()
        {
            databaseAYT03 dbayt03 = new databaseAYT03();
            databaseRH dbRH = new databaseRH();
            var month = 2;
            var year = 2020;            
            //obtiene tareas
            String query_usuarios = "SELECT"
                                    + " ISNULL(o.Name, 'No Asignado') AS Tecnico, "
                                    + " COUNT(*) AS Total, "
                                    + " COUNT(CASE WHEN StatusType = 'open' THEN 1 END) AS Abiertas, "
                                    + " COUNT(CASE WHEN StatusType = 'closed' THEN 1 END) AS Cerradas "
                                    + " INTO ##T1 "
                                    + " FROM Tasks t "
                                    + " FULL JOIN Owners o on t.IdTask = o.IdTask "
                                    + " WHERE "
                                    + " MONTH(EndDate) = " + month + " and "
                                    + " YEAR(EndDate) = " + year
                                    + " GROUP BY o.Name "
                                    + " SELECT "
                                    + " ISNULL(o.Name, 'No Asignado') AS Tecnico, "
                                    + " COUNT(*) AS Vencidas "
                                    + " INTO ##T2 "
                                    + " FROM Tasks t "
                                    + " FULL JOIN Owners o on t.IdTask = o.IdTask "
                                    + " WHERE "
                                    + " StatusType = 'closed' and "
                                    + " StartDate != '1900-01-01' and "
                                    + " EndDate != '1900-01-01' and "
                                    + " CompletedDate > EndDate and "
                                    + " MONTH(EndDate) = " + month + " and "
                                    + " YEAR(EndDate) = " + year
                                    + " GROUP BY o.Name "
                                    + " SELECT "
                                    + " ISNULL(o.Name, 'No Asignado') AS Tecnico, "
                                    + " COUNT(*) AS ATiempo "
                                    + " INTO ##T3 "
                                    + " FROM Tasks t "
                                    + " FULL JOIN Owners o on t.IdTask = o.IdTask "
                                    + " WHERE "
                                    + " StatusType = 'closed' and "
                                    + " StartDate != '1900-01-01' and "
                                    + " EndDate != '1900-01-01' and "
                                    + " CompletedDate <= EndDate and "
                                    + " MONTH(EndDate) = " + month + " and "
                                    + " YEAR(EndDate) = " + year
                                    + " GROUP BY o.Name "
                                    + " SELECT "
                                    + " T1.*, "
                                    + " ISNULL(T2.Vencidas, 0) AS Vencidas, "
                                    + " ISNULL(T3.ATiempo, 0) AS ATiempo, "
                                    + " P.Profile AS Perfil, "
                                    + " 0 As Compartidas "
                                    + " FROM ##T1 T1 "
                                    + " LEFT JOIN ##T2 T2 ON T1.Tecnico = T2.Tecnico "
                                    + " LEFT JOIN ##T3 T3 ON T1.Tecnico = T3.Tecnico "
                                    + " LEFT JOIN ProjectsUsers P ON T1.Tecnico = P.Name "
                                    + " WHERE P.Profile IS NOT NULL and P.Profile <> 0 "
                                    + " GROUP BY T1.Tecnico,T1.Total,T1.Abiertas, T1.Cerradas,T2.Vencidas,T3.Atiempo,P.Profile "
                                    + " ORDER BY P.Profile, T1.Tecnico "
                                    + " DROP TABLE ##T1, ##T2, ##T3 ";  //Activo
            SqlDataReader reader = (SqlDataReader)await dbayt03.query(query_usuarios);
            List<GeneralViewModel> info = new List<GeneralViewModel>();
            List<ResuladoViewModel> resultado = new List<ResuladoViewModel>();
            int porcentaje_total = 100;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string tecnico = reader["Tecnico"].ToString();

                    String query_projects = "SELECT "
                                            + " ISNULL(o.Name, 'No Asignado') AS Tecnico,"
                                            + " COUNT(*) AS TareaPorProyecto"
                                            + " FROM Tasks t"
                                            + " JOIN Projects p ON t.IdProject = p.IdProjects"
                                            + " JOIN Owners o ON t.IdTask = o.IdTask"
                                            + " WHERE"
                                            + " t.EndDate != '1900-01-01' and"
                                            + " MONTH(t.EndDate) = '" + month + "' and"
                                            + " YEAR(t.EndDate) = '" + year + "' and"
                                            + " o.Name = '" + tecnico + "'"
                                            + "GROUP BY p.IdProjects,o.Name";
                    SqlDataReader reader_projects = (SqlDataReader)await dbayt03.query(query_projects);
                    var count_proyectos = 0;
                    if (reader_projects.HasRows == true)
                    {
                        while (reader_projects.Read())
                        {
                            count_proyectos++;
                        }
                        reader_projects.Close();
                    }

                    String query_actividades_asignadas =  "SELECT "
                                                          + " COUNT(*) AS Asignadas_Total,"
                                                          + " COUNT(CASE WHEN status = 'Abierto' THEN 1 END) AS Asignadas_Abiertas,"
                                                          + " COUNT(CASE WHEN status = 'Cerrado' OR status = 'Resuelto' THEN 1 END) AS Asignadas_Cerradas,"
                                                          + " COUNT(CASE WHEN is_overdue = '1' THEN 1 END) AS Asignadas_Vencidas,"
                                                          + " COUNT(CASE WHEN is_overdue = '0' THEN 1 END) AS Asignadas_ATiempo "
                                                          + " FROM TablaTickets "
                                                          + " WHERE grupo = 'Actividades Asignadas' and "
                                                          + " MONTH(created_time) = '" + month + "' and"
                                                          + " YEAR(created_time) = '" + year + "' and"
                                                          + " technician_name = '" + tecnico + "'"
                                                          + " GROUP BY technician_name";
                    SqlDataReader reader_actividades_asignadas = (SqlDataReader)await dbayt03.query(query_actividades_asignadas);
                    var asignadas_total = 0;
                    var asignadas_abiertas = 0;
                    var asignadas_cerradas = 0;
                    var asignadas_vencidas = 0;
                    var asignadas_a_tiempo = 0;

                    if (reader_actividades_asignadas.HasRows == true)
                    {
                        while (reader_actividades_asignadas.Read())
                        {
                            asignadas_total = Convert.ToInt32(reader_actividades_asignadas["Asignadas_Total"]);
                            asignadas_abiertas = Convert.ToInt32(reader_actividades_asignadas["Asignadas_Abiertas"]);
                            asignadas_cerradas = Convert.ToInt32(reader_actividades_asignadas["Asignadas_Cerradas"]);
                            asignadas_vencidas = Convert.ToInt32(reader_actividades_asignadas["Asignadas_Vencidas"]);
                            asignadas_a_tiempo = Convert.ToInt32(reader_actividades_asignadas["Asignadas_ATiempo"]);
                        }
                        reader_actividades_asignadas.Close();
                    }

                    var total = reader["Total"].ToString();
                    var abiertas = reader["Abiertas"].ToString();
                    var cerradas = reader["Cerradas"].ToString();
                    var vencidas = reader["Vencidas"].ToString();
                    var a_tiempo = reader["ATiempo"].ToString();
                    var perfil = reader["Perfil"].ToString();
                    var compartidas = reader["Compartidas"].ToString();
                    var programadas = Int32.Parse(total) - Int32.Parse(compartidas);
                    
                    var porcentaje_cerradas = Math.Round((decimal)(Int32.Parse(cerradas) * porcentaje_total) / Int32.Parse(total));
                    var porcentaje_a_tiempo = "0";
                    var porcenjate_vencidas = "0";
                    if (Int32.Parse(cerradas) > 0)

                    {
                        porcentaje_a_tiempo = Math.Round((decimal)(Int32.Parse(a_tiempo) * porcentaje_total) / Int32.Parse(cerradas)).ToString();
                        porcenjate_vencidas = Math.Round((decimal)(Int32.Parse(vencidas) * porcentaje_total) / Int32.Parse(cerradas)).ToString();

                    }

                    info.Add(new GeneralViewModel
                    {
                        Tecnico = tecnico,
                        Total = programadas.ToString(),
                        Abiertas = abiertas,
                        Cerradas = cerradas,
                        Porcentaje_Cerradas = porcentaje_cerradas.ToString(),
                        Vencidas = vencidas,
                        Porcentaje_Vencidas = porcenjate_vencidas.ToString(),
                        Tiempo = a_tiempo,
                        Porcentaje_Tiempo = porcentaje_a_tiempo.ToString(),
                        Perfil = perfil,
                        Proyectos = count_proyectos.ToString(),
                        Compartidas = compartidas,
                        Asignadas_Totales = asignadas_total,
                        Asignadas_Abiertas = asignadas_abiertas,
                        Asignadas_Cerradas = asignadas_cerradas,
                        Asignadas_Vencidas = asignadas_vencidas,
                        Asignadas_Tiempo = asignadas_a_tiempo
                    }); ;



                }
                reader.Close();
            }

            info.Add(new GeneralViewModel
            {
                Tecnico = "Ricardo Acevedo",
                Total = "0",
                Abiertas = "0",
                Cerradas = "0",
                Porcentaje_Cerradas = "0",
                Vencidas = "0",
                Porcentaje_Vencidas = "0",
                Tiempo = "0",
                Porcentaje_Tiempo = "0",
                Perfil = "7",
                Proyectos = "0",
                Compartidas = "0",
                Asignadas_Totales = 16,
                Asignadas_Abiertas = 0,
                Asignadas_Cerradas = 16,
                Asignadas_Vencidas = 1,
                Asignadas_Tiempo = 15
            });

            info.Add(new GeneralViewModel
            {
                Tecnico = "Lizeth Gardea",
                Total = "0",
                Abiertas = "0",
                Cerradas = "0",
                Porcentaje_Cerradas = "0",
                Vencidas = "0",
                Porcentaje_Vencidas = "0",
                Tiempo = "0",
                Porcentaje_Tiempo = "0",
                Perfil = "3",
                Proyectos = "0",
                Compartidas = "0",
                Asignadas_Totales = 2,
                Asignadas_Abiertas = 1,
                Asignadas_Cerradas = 1,
                Asignadas_Vencidas = 1,
                Asignadas_Tiempo = 1
            });


            //suma todos los resultados de todos los técnicos excepto el de Alejandro
            var abiertas_gerente = 0;
            var cerradas_gerente = 0;
            var vencidas_gerente = 0;
            var tiempo_gerente = 0;
            var total_gerente = 0;
            var proyectos_gerente = 0;
            var compartidas_gerente = 0;
            var asignadas_total_gerente = 0;
            var asignadas_abiertas_gerente = 0;
            var asignadas_cerradas_gerente = 0;
            var asignadas_vencidas_gerente = 0;
            var asignadas_a_tiempo_gerente = 0;

            //suma todos los resultados de los Desarrolladores
            var abiertas_coord_dev = 0;
            var cerradas_coord_dev = 0;
            var vencidas_coord_dev = 0;
            var tiempo_coord_dev = 0;
            var total_coord_dev = 0;
            var proyectos_coord_dev = 0;
            var compartidas_coord_dev = 0;
            var asignadas_total_coord_dev = 0;
            var asignadas_abiertas_coord_dev = 0;
            var asignadas_cerradas_coord_dev = 0;
            var asignadas_vencidas_coord_dev = 0;
            var asignadas_a_tiempo_coord_dev = 0;

            //asigna el color del semaforo
            var color_vencidas = "";
            var color_a_tiempo = "";
            var color_total = "";



            foreach (var val in info)
            {
                if (val.Perfil == "1" || val.Perfil != "2")
                {
                    abiertas_gerente += Int32.Parse(val.Abiertas);
                    cerradas_gerente += Int32.Parse(val.Cerradas);
                    vencidas_gerente += Int32.Parse(val.Vencidas);
                    tiempo_gerente += Int32.Parse(val.Tiempo);
                    total_gerente += Int32.Parse(val.Total);
                    proyectos_gerente += Int32.Parse(val.Proyectos);
                    compartidas_gerente += Int32.Parse(val.Compartidas);
                    asignadas_total_gerente += +val.Asignadas_Totales;
                    asignadas_abiertas_gerente += val.Asignadas_Abiertas;
                    asignadas_cerradas_gerente += val.Asignadas_Cerradas;
                    asignadas_vencidas_gerente += val.Asignadas_Vencidas;
                    asignadas_a_tiempo_gerente += val.Asignadas_Tiempo;

                    if (Int32.Parse(val.Porcentaje_Vencidas) <= 10)
                    {
                        color_vencidas = "#98ee99";
                    }

                    if (Int32.Parse(val.Porcentaje_Vencidas) > 10)
                    {
                        color_vencidas = "#fff59d";
                    }

                    if (Int32.Parse(val.Porcentaje_Vencidas) > 50)
                    {
                        color_vencidas = "#ffa4a2";
                    }

                    if (Int32.Parse(val.Porcentaje_Tiempo) >= 90)
                    {
                        color_a_tiempo = "#98ee99";
                    }

                    if (Int32.Parse(val.Porcentaje_Tiempo) >= 80 && Int32.Parse(val.Porcentaje_Tiempo) < 90)
                    {
                        color_a_tiempo = "#fff59d";
                    }

                    if (Int32.Parse(val.Porcentaje_Tiempo) < 80)
                    {
                        color_a_tiempo = "#ffa4a2";
                    }

                    if (Int32.Parse(val.Porcentaje_Cerradas) >= 90)
                    {
                        color_total = "#98ee99";
                    }

                    if (Int32.Parse(val.Porcentaje_Cerradas) >= 80 && Int32.Parse(val.Porcentaje_Cerradas) < 90)
                    {
                        color_total = "#fff59d";
                    }

                    if (Int32.Parse(val.Porcentaje_Cerradas) < 80)
                    {
                        color_total = "#ffa4a2";
                    }

                    resultado.Add(new ResuladoViewModel
                    {
                        Tecnico =  val.Tecnico,
                        Proyectos = Int32.Parse(val.Proyectos),
                        Compartidas = Int32.Parse(val.Compartidas),
                        Total = Int32.Parse(val.Total),
                        Abiertas = Int32.Parse(val.Abiertas),
                        Cerradas = Int32.Parse(val.Cerradas),
                        Porcentaje_Cerradas = Int32.Parse(val.Porcentaje_Cerradas),
                        Cerradas_class = color_total,
                        Vencidas = Int32.Parse(val.Vencidas),
                        Porcentaje_Vencidas = Int32.Parse(val.Porcentaje_Vencidas),
                        Vencidas_class = color_vencidas,
                        Tiempo = Int32.Parse(val.Tiempo),
                        Porcentaje_Tiempo = Int32.Parse(val.Porcentaje_Tiempo),
                        Tiempo_class = color_a_tiempo,
                        Asignadas_Totales = val.Asignadas_Totales,
                        Asignadas_Abiertas =  val.Asignadas_Abiertas,
                        Asignadas_Cerradas = val.Asignadas_Cerradas,
                        Asignadas_Vencidas = val.Asignadas_Vencidas,
                        Asignadas_Tiempo = val.Asignadas_Tiempo
                    });                                     
                }

                if (val.Perfil == "2" || val.Perfil == "3")
                {
                    //console.log("proyectos: " , data[i].proyectos)
                    abiertas_coord_dev += Int32.Parse(val.Abiertas);
                    cerradas_coord_dev += Int32.Parse(val.Cerradas);
                    vencidas_coord_dev += Int32.Parse(val.Vencidas);
                    tiempo_coord_dev += Int32.Parse(val.Tiempo);
                    total_coord_dev += Int32.Parse(val.Total);
                    proyectos_coord_dev += Int32.Parse(val.Proyectos);
                    compartidas_coord_dev += Int32.Parse(val.Compartidas);
                    asignadas_total_coord_dev += +val.Asignadas_Totales;
                    asignadas_abiertas_coord_dev += val.Asignadas_Abiertas;
                    asignadas_cerradas_coord_dev += val.Asignadas_Cerradas;
                    asignadas_vencidas_coord_dev += val.Asignadas_Vencidas;
                    asignadas_a_tiempo_coord_dev += val.Asignadas_Tiempo;

                    if (Int32.Parse(val.Porcentaje_Vencidas) <= 10)
                    {
                        color_vencidas = "#98ee99";
                    }

                    if (Int32.Parse(val.Porcentaje_Vencidas) > 10)
                    {
                        color_vencidas = "#fff59d";
                    }

                    if (Int32.Parse(val.Porcentaje_Vencidas) > 50)
                    {
                        color_vencidas = "#ffa4a2";
                    }

                    if (Int32.Parse(val.Porcentaje_Tiempo) >= 90)
                    {
                        color_a_tiempo = "#98ee99";
                    }

                    if (Int32.Parse(val.Porcentaje_Tiempo) >= 80 && Int32.Parse(val.Porcentaje_Tiempo) < 90)
                    {
                        color_a_tiempo = "#fff59d";
                    }

                    if (Int32.Parse(val.Porcentaje_Tiempo) < 80)
                    {
                        color_a_tiempo = "#ffa4a2";
                    }

                    if (Int32.Parse(val.Porcentaje_Cerradas) >= 90)
                    {
                        color_total = "#98ee99";
                    }

                    if (Int32.Parse(val.Porcentaje_Cerradas) >= 80 && Int32.Parse(val.Porcentaje_Cerradas) < 90)
                    {
                        color_total = "#fff59d";
                    }

                    if (Int32.Parse(val.Porcentaje_Cerradas) < 80)
                    {
                        color_total = "#ffa4a2";
                    }

                    resultado.Add(new ResuladoViewModel
                    {
                        Tecnico = val.Tecnico,
                        Proyectos = Int32.Parse(val.Proyectos),
                        Compartidas = Int32.Parse(val.Compartidas),
                        Total = Int32.Parse(val.Total),
                        Abiertas = Int32.Parse(val.Abiertas),
                        Cerradas = Int32.Parse(val.Cerradas),
                        Porcentaje_Cerradas = Int32.Parse(val.Porcentaje_Cerradas),
                        Cerradas_class = color_total,
                        Vencidas = Int32.Parse(val.Vencidas),
                        Porcentaje_Vencidas = Int32.Parse(val.Porcentaje_Vencidas),
                        Vencidas_class = color_vencidas,
                        Tiempo = Int32.Parse(val.Tiempo),
                        Porcentaje_Tiempo = Int32.Parse(val.Porcentaje_Tiempo),
                        Tiempo_class = color_a_tiempo,
                        Asignadas_Totales = val.Asignadas_Totales,
                        Asignadas_Abiertas = val.Asignadas_Abiertas,
                        Asignadas_Cerradas = val.Asignadas_Cerradas,
                        Asignadas_Vencidas = val.Asignadas_Vencidas,
                        Asignadas_Tiempo = val.Asignadas_Tiempo
                    });
                }                
            }

            //calcula los datos para Oscar
            //var porcentaje_total = 100;
            var porcentaje_cerradas_gerente = Math.Round(Convert.ToDecimal((+cerradas_gerente * porcentaje_total) / +total_gerente), 2);
            var porcentaje_a_tiempo_gerente = Math.Round(Convert.ToDecimal((+tiempo_gerente * porcentaje_total) / +cerradas_gerente), 2);
            var porcentaje_vencidas_gerente = Math.Round(Convert.ToDecimal((+vencidas_gerente * porcentaje_total) / +cerradas_gerente), 2);

            if (porcentaje_vencidas_gerente <= 10)
            {
                color_vencidas = "#98ee99";
            }

            if (porcentaje_vencidas_gerente > 10)
            {
                color_vencidas = "#fff59d";
            }

            if (porcentaje_vencidas_gerente > 50)
            {
                color_vencidas = "#ffa4a2";
            }

            if (porcentaje_a_tiempo_gerente >= 90)
            {
                color_a_tiempo = "#98ee99";
            }

            if (porcentaje_a_tiempo_gerente >= 80 && porcentaje_a_tiempo_gerente < 90)
            {
                color_a_tiempo = "#fff59d";
            }

            if (porcentaje_a_tiempo_gerente < 80)
            {
                color_a_tiempo = "#ffa4a2";
            }

            if (porcentaje_cerradas_gerente >= 90)
            {
                color_total = "#98ee99";
            }

            if (porcentaje_cerradas_gerente >= 80 && porcentaje_cerradas_gerente < 90)
            {
                color_total = "#fff59d";
            }

            if (porcentaje_cerradas_gerente < 80)
            {
                color_total = "#ffa4a2";
            }

            resultado.Add(new ResuladoViewModel
            {
                Tecnico = "Oscar Loya",
                Proyectos = proyectos_gerente,
                Compartidas = compartidas_gerente,
                Total = total_gerente,
                Abiertas = abiertas_gerente,
                Cerradas = cerradas_gerente,
                Porcentaje_Cerradas = (int)porcentaje_cerradas_gerente,
                Cerradas_class = color_total,
                Vencidas = vencidas_gerente,
                Porcentaje_Vencidas = (int)porcentaje_vencidas_gerente,
                Vencidas_class = color_vencidas,
                Tiempo = tiempo_gerente,
                Porcentaje_Tiempo = (int)porcentaje_a_tiempo_gerente,
                Tiempo_class = color_a_tiempo,
                Asignadas_Totales = asignadas_total_gerente,
                Asignadas_Abiertas = asignadas_abiertas_gerente,
                Asignadas_Cerradas = asignadas_cerradas_gerente,
                Asignadas_Vencidas = asignadas_vencidas_gerente,
                Asignadas_Tiempo = asignadas_a_tiempo_gerente
            });

            var porcentaje_cerradas_coord_dev = Math.Round(Convert.ToDecimal((+cerradas_coord_dev * porcentaje_total) / +total_coord_dev), 2);
            var porcentaje_a_tiempo_coord_dev = Math.Round(Convert.ToDecimal((+tiempo_coord_dev * porcentaje_total) / +cerradas_coord_dev), 2);
            var porcentaje_vencidas_coord_dev = Math.Round(Convert.ToDecimal((+vencidas_coord_dev * porcentaje_total) / +cerradas_coord_dev), 2);

            if (porcentaje_vencidas_coord_dev <= 10)
            {
                color_vencidas = "#98ee99";
            }

            if (porcentaje_vencidas_coord_dev > 10)
            {
                color_vencidas = "#fff59d";
            }

            if (porcentaje_vencidas_coord_dev > 50)
            {
                color_vencidas = "#ffa4a2";
            }

            if (porcentaje_a_tiempo_coord_dev >= 90)
            {
                color_a_tiempo = "#98ee99";
            }

            if (porcentaje_a_tiempo_coord_dev >= 80 && porcentaje_a_tiempo_coord_dev < 90)
            {
                color_a_tiempo = "#fff59d";
            }

            if (porcentaje_a_tiempo_coord_dev < 80)
            {
                color_a_tiempo = "#ffa4a2";
            }

            if (porcentaje_cerradas_coord_dev >= 90)
            {
                color_total = "#98ee99";
            }

            if (porcentaje_cerradas_coord_dev >= 80 && porcentaje_cerradas_coord_dev < 90)
            {
                color_total = "#fff59d";
            }

            if (porcentaje_cerradas_coord_dev < 80)
            {
                color_total = "#ffa4a2";
            }


            resultado.Add(new ResuladoViewModel
            {
                Tecnico = "Alejandro Gamboa",
                Proyectos = proyectos_coord_dev,
                Compartidas = compartidas_coord_dev,
                Total = total_coord_dev,
                Abiertas = abiertas_coord_dev,
                Cerradas = cerradas_coord_dev,
                Porcentaje_Cerradas = (int)porcentaje_cerradas_coord_dev,
                Cerradas_class = color_total,
                Vencidas = vencidas_coord_dev,
                Porcentaje_Vencidas = (int)porcentaje_vencidas_coord_dev,
                Vencidas_class = color_vencidas,
                Tiempo = tiempo_coord_dev,
                Porcentaje_Tiempo = (int)porcentaje_a_tiempo_coord_dev,
                Tiempo_class = color_a_tiempo,
                Asignadas_Totales = asignadas_total_coord_dev,
                Asignadas_Abiertas = asignadas_abiertas_coord_dev,
                Asignadas_Cerradas = asignadas_cerradas_coord_dev,
                Asignadas_Vencidas = asignadas_vencidas_coord_dev,
                Asignadas_Tiempo = asignadas_a_tiempo_coord_dev
            });
            return View(resultado);
        }
    }
}