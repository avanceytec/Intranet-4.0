using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntranetMVC.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using IntranetMVC.Models;
using Newtonsoft.Json.Linq;

namespace IntranetMVC.Controllers
{
    public class ConfiguracionController : Controller
    {
        // GET: Configuration
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> GetUsuarios()
        {

            databaseAYT03 dbayt03 = new databaseAYT03();

            //obtiene información general de la lista de usuarios
            String query_usuarios = "SELECT " +
                                    "u.Id, " +
                                    "u.Usuario, " +
                                    "u.Nombre, " +
                                    "ISNULL(r.Nombre, 'NA') AS Rol " +
                                    "FROM ITNT_Usuarios u " +
                                    "LEFT JOIN ITNT_Roles r ON u.IdRol = r.Id "; //+
                                    //"WHERE Status = 1 "; //Activo
            SqlDataReader reader = (SqlDataReader)await dbayt03.query(query_usuarios);
            List<UsuariosViewModel> usuarios = new List<UsuariosViewModel>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    usuarios.Add(new UsuariosViewModel
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        Usuario = reader["Usuario"].ToString(),
                        Nombre = reader["Nombre"].ToString(),
                        Rol = reader["Rol"].ToString()
                    });
                }
                reader.Close();
            }
            
            return Json(new {data =  usuarios});            
        }

        public async Task<ActionResult> GetRoles()
        {
            databaseAYT03 dbayt03 = new databaseAYT03();

            //obtiene información general de Roles
            String query_roles = "SELECT * FROM ITNT_Roles";//" WHERE Status = 1";
            SqlDataReader reader_roles = (SqlDataReader)await dbayt03.query(query_roles);
            List<RolViewModel> roles = new List<RolViewModel>();

            if (reader_roles.HasRows)
            {
                while (reader_roles.Read())
                {
                    var id = Int32.Parse(reader_roles["Id"].ToString());

                    String query_rol_menu = "SELECT rm.IdMenu, m.Nombre FROM ITNT_Rol_Menu rm LEFT JOIN ITNT_Menu m ON rm.IdMenu = m.Id WHERE rm.IdRol = " + id;
                    SqlDataReader reader_rol_menu = (SqlDataReader)await dbayt03.query(query_rol_menu);
                    List<RolMenuViewModel> rol_menu = new List<RolMenuViewModel>();
                    if (reader_rol_menu.HasRows)
                    {
                        while (reader_rol_menu.Read())
                        {
                            rol_menu.Add(new RolMenuViewModel
                            {
                                IdMenu = Int32.Parse(reader_rol_menu["IdMenu"].ToString()),
                                Nombre = reader_rol_menu["Nombre"].ToString()
                            });
                        }
                    }

                    roles.Add(new RolViewModel
                    {
                        Id = id,
                        Nombre = reader_roles["Nombre"].ToString(),
                        Status = Int32.Parse(reader_roles["Status"].ToString()),
                        Menus = rol_menu
                    });
                }
                reader_roles.Close();
            }         
            return Json(new { data = roles});
        }

        public async Task<ActionResult> GetModulos()
        {
            databaseAYT03 dbayt03 = new databaseAYT03();
            //obtiene información general de Módulo 
            String query_modulos = "SELECT " +
                                   "m.Id, " +
                                   "mn.Nombre AS Menu, " +
                                   "m.Nombre, " +
                                   "m.Descripcion, " +
                                   "m.Status " +
                                   "FROM ITNT_Modulos m " +
                                   "LEFT JOIN ITNT_Menu mn ON m.IdMenu = mn.Id ";// +
                                   //"WHERE mn.Status = 1 AND m.Status = 1";
            SqlDataReader reader_modulos = (SqlDataReader)await dbayt03.query(query_modulos);
            List<ModulosViewModel> modulos = new List<ModulosViewModel>();

            if (reader_modulos.HasRows)
            {
                while (reader_modulos.Read())
                {
                    modulos.Add(new ModulosViewModel
                    {
                        Id = Int32.Parse(reader_modulos["Id"].ToString()),
                        Menu = reader_modulos["Menu"].ToString(),
                        Nombre = reader_modulos["Nombre"].ToString(),
                        Descripcion = reader_modulos["Descripcion"].ToString(),
                        Status = Int32.Parse(reader_modulos["Status"].ToString())
                    });
                }
                reader_modulos.Close();
            }
            return Json(new { data = modulos });

        }

        public async Task<ActionResult> GetSubModulos()
        {
            databaseAYT03 dbayt03 = new databaseAYT03();
            //obtiene información general de Submódulo s
            String query_submodulos = "SELECT " +
                                      "s.Id, " +
                                      "m.Nombre AS Modulo, " +
                                      "s.Nombre,  " +
                                      "s.Descripcion, " +
                                      "s.Url, " +
                                      "s.Status " +
                                      "FROM ITNT_Submodulos s " +
                                      "LEFT JOIN ITNT_Modulos m ON s.IdModulo = m.Id "; //+
                                      //"WHERE s.Status = 1 AND m.Status = 1";
            SqlDataReader reader_submodulos = (SqlDataReader)await dbayt03.query(query_submodulos);


            List<SubmodulosViewModel> submodulos = new List<SubmodulosViewModel>();

            if (reader_submodulos.HasRows)
            {
                while (reader_submodulos.Read())
                {
                    submodulos.Add(new SubmodulosViewModel
                    {
                        Id = Int32.Parse(reader_submodulos["Id"].ToString()),
                        Modulo = reader_submodulos["Modulo"].ToString(),
                        Nombre = reader_submodulos["Nombre"].ToString(),
                        Descripcion = reader_submodulos["Descripcion"].ToString(),
                        Url = reader_submodulos["Url"].ToString(),
                        Status = Int32.Parse(reader_submodulos["Status"].ToString())
                    });
                }
                reader_submodulos.Close();
            }
            return Json(new { data = submodulos });
        }


        public async Task<ActionResult> GetMenu()
        {
            databaseAYT03 dbayt03 = new databaseAYT03();
            //obtiene información general de los Menú
            String query = "SELECT * FROM ITNT_Menu";
            SqlDataReader reader = (SqlDataReader)await dbayt03.query(query);
            List<MenuViewModel> menu = new List<MenuViewModel>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    menu.Add(new MenuViewModel
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),                        
                        Nombre = reader["Nombre"].ToString(),
                        Status = Int32.Parse(reader["Status"].ToString())
                    });
                }
                reader.Close();
            }
            return Json(new { data = menu });

        }

        public async Task<ActionResult> GetModulo()
        {
            databaseAYT03 dbayt03 = new databaseAYT03();
            
            String query = "SELECT * FROM ITNT_Modulos WHERE Status = 1";
            SqlDataReader reader = (SqlDataReader)await dbayt03.query(query);
            List<ModulosViewModel> menu = new List<ModulosViewModel>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    menu.Add(new ModulosViewModel
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        Nombre = reader["Nombre"].ToString(),                       
                    });
                }
                reader.Close();
            }
            return Json(new { data = menu });

        }

        // POST: Configuration/Edit/5
        [HttpPost]       
        public async Task<ActionResult> Edit(string accion, string rol, string nombre_rol, int status_rol, string[] rol_menu)
        {
            try
            {

                databaseAYT03 dbayt03 = new databaseAYT03();
                var message = "fallo";

                String query_delete_menu = "DELETE FROM ITNT_Rol_Menu WHERE IdRol = " + rol;
                var delete = await dbayt03.query(query_delete_menu);

                foreach (var m in rol_menu)
                {
                    String query_insert_rol_menu = "INSERT INTO ITNT_Rol_Menu VALUES (" + rol + "," + m + ")";
                    var rol_menu_ = await dbayt03.queryInsert(query_insert_rol_menu);
                }


                if (accion == "actualizar")
                {
                    String query = "UPDATE ITNT_Roles SET Nombre = '" + nombre_rol + "', Status = " + status_rol + " WHERE Id = " + rol;
                    var a = await dbayt03.queryInsert(query);

                    if (a >= 1)
                    {
                        message = "exito";
                    }

                }
                else
                {
                    String query_add = "INSERT INTO ITNT_Roles VALUES ('" + nombre_rol + "', " + status_rol + ")";
                    var add = await dbayt03.queryInsert(query_add);

                    if (add >= 1)
                    {
                        message = "exito";
                    }
                }

                return Json(new { message });                
            }
            catch
            {
                return View();
            }
        }

        // POST: Configuration/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int rol)
        {
            try
            {                
                databaseAYT03 dbayt03 = new databaseAYT03();
                
                String query_delete = "DELETE FROM ITNT_Roles WHERE Id = " + rol;
                SqlDataReader reader = (SqlDataReader)await dbayt03.query(query_delete);

                String delete_rol_menu = "DELETE FROM ITNT_Rol_Menu WHERE IdRol = " + rol; 
                return Json(rol);               
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<ActionResult> GuardarMenu(string accion, int menu, string nombre, int status)
        {
            try
            {
                var message = "fallo";
                /*var accion = collection["accion"];
                var id_menu = collection["menu"];
                var nombre = collection["nombre"];                
                var status = collection["status"];*/

                databaseAYT03 dbayt03 = new databaseAYT03();

                if (accion == "actualizar")
                {

                    String query = "UPDATE ITNT_Menu SET Nombre = '" + nombre + "', Status = " + status + " WHERE Id = " + menu;
                    var a = await dbayt03.queryInsert(query);

                    if (a >= 1)
                    {
                        message = "exito";
                    }
                }
                else
                {
                    String query_add = "INSERT INTO ITNT_Menu VALUES('" + nombre + "', " + status + ")";
                    var add = await dbayt03.queryInsert(query_add);

                    if (add >= 1)
                    {
                        message = "exito";
                    }
                }


                return Json(new { message });
                //return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        [HttpPost]        
        public async Task<ActionResult> GuardarModulo(string accion, int modulo, string nombre, string descripcion, int menu, int status)
        {
            try
            {
                var message = "fallo";
                /*var accion = collection["accion"];
                var id_modulo = collection["modulo"];
                var nombre = collection["nombre"];
                var descripcion = collection["descripcion"];
                var menu = collection["menu"];
                var status = collection["status"];*/

                databaseAYT03 dbayt03 = new databaseAYT03();

                if (accion == "actualizar")
                {
                    
                    String query = "UPDATE ITNT_Modulos SET IdMenu = " + menu + ", Nombre = '" + nombre + "', Descripcion = '" + descripcion + "', Status = " + status + " WHERE Id = " + modulo;
                    var a = await dbayt03.queryInsert(query);

                    if (a >= 1)
                    {
                        message = "exito";
                    }
                }
                else
                {
                    String query_add = "INSERT INTO ITNT_Modulos VALUES(" + menu +", '" + nombre + "', '" + descripcion + "', " + status + ")";
                    var add = await dbayt03.queryInsert(query_add);

                    if (add >= 1)
                    {
                        message = "exito";
                    }
                }

                
                return Json(new {message });
                //return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<ActionResult> GuardarSubmodulo(string accion, int submodulo, string nombre, string descripcion, int modulo, string url, int status)
        {
            try
            {
                var message = "fallo";                
                databaseAYT03 dbayt03 = new databaseAYT03();

                if (accion == "actualizar")
                {

                    String query = "UPDATE ITNT_Submodulos SET IdModulo = " + modulo + ", Nombre = '" + nombre + "', Descripcion = '" + descripcion + "', Url = '" + url + "', Status = " + status + " WHERE Id = " + submodulo;
                    var a = await dbayt03.queryInsert(query);

                    if (a >= 1)
                    {
                        message = "exito";
                    }
                }
                else
                {
                    String query_add = "INSERT INTO ITNT_Submodulos VALUES(" + modulo + ", '" + nombre + "', '" + descripcion + "', '" + url + "', " + status + ")";
                    var add = await dbayt03.queryInsert(query_add);

                    if (add >= 1)
                    {
                        message = "exito";
                    }
                }
                return Json(new { message });
                //return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}