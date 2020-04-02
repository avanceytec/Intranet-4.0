using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntranetMVC.Helpers;
using IntranetMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IntranetMVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Menu(string email)
        {
            databaseAYT03 dbay03 = new databaseAYT03();

            //Informacion del usuario
            email = "sistemas10@avanceytec.com.mx";
            String query_usuario = "SELECT IdRol FROM ITNT_Usuarios WHERE Email = '" + email + "'";
            SqlDataReader reader = (SqlDataReader)await dbay03.query(query_usuario);
            List<ConfMenuViewModel> configuracion = new List<ConfMenuViewModel>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id_rol = Int32.Parse(reader["IdRol"].ToString());                    
                    

                    String query_menus = "SELECT " +
                                            "m.Id, " +
                                            "m.Nombre " +
                                         "FROM " +
                                         "ITNT_Rol_Menu rm " +
                                         "LEFT JOIN ITNT_Menu m ON rm.IdMenu = m.Id " + 
                                         "WHERE rm.IdRol = "+ id_rol + " AND m.Status = 1";

                    SqlDataReader reader_menu = (SqlDataReader)await dbay03.query(query_menus);


                    if (reader_menu.HasRows)
                    {                        
                        while (reader_menu.Read())
                        {                            
                            var id_menu = reader_menu["Id"].ToString();

                            String query_modulos = "SELECT Id, Nombre FROM ITNT_Modulos WHERE IdMenu = " + id_menu + " AND Status = 1";
                            SqlDataReader reader_modulos = (SqlDataReader)await dbay03.query(query_modulos);
                            List<ModulosViewModel> modulos = new List<ModulosViewModel>();                            

                            if (reader_modulos.HasRows)
                            {
                                
                                while (reader_modulos.Read())
                                {
                                    var id_modulo = reader_modulos["Id"].ToString();

                                    String query_submodulos = "SELECT Nombre, Url FROM ITNT_Submodulos WHERE IdModulo = " + id_modulo + " AND Status = 1";
                                    SqlDataReader reader_submodulo = (SqlDataReader)await dbay03.query(query_submodulos);
                                    List<SubmodulosViewModel> submodulos = new List<SubmodulosViewModel>();
                                    if (reader_submodulo.HasRows)
                                    {
                                        while (reader_submodulo.Read())
                                        {
                                            submodulos.Add(new SubmodulosViewModel
                                            {
                                                Nombre = reader_submodulo["Nombre"].ToString(),
                                                Url = reader_submodulo["Url"].ToString()
                                            });
                                        }
                                    }


                                    modulos.Add(new ModulosViewModel
                                    {
                                        Nombre = reader_modulos["Nombre"].ToString(),
                                        Submodulos = submodulos
                                    });
                                }
                            }

                            configuracion.Add(new ConfMenuViewModel
                            {
                                Nombre = reader_menu["Nombre"].ToString(),
                                Modulos = modulos
                            });
                        }
                    }



                }
            }


            return Json( new { data = configuracion });
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(int id)
        {
            databaseAYT03 dbayt03 = new databaseAYT03();
            databaseRH dbRH = new databaseRH();

            //obtiene información general del usuario
            String query_usuarios = "SELECT " +
                                    "u.Id, " +
                                    "u.Nombre, " +
                                    "u.Departamento,  " +
                                    "u.Email, " +
                                    "u.Extension,  " +
                                    "CASE WHEN u.EsGerente = 1 THEN 'True' ELSE 'False' END AS Gerente, " +
                                    "r.Nombre AS Rol " +
                                    "FROM ITNT_Usuarios u " +
                                    "LEFT JOIN ITNT_Roles r on u.IdRol = r.Id " +
                                    "WHERE u.Id = "+id; //Activo
            SqlDataReader reader = (SqlDataReader)await dbayt03.query(query_usuarios);
            List<InfoGeneralViewModel> usuarios = new List<InfoGeneralViewModel>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    usuarios.Add(new InfoGeneralViewModel
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Departamento = reader["Departamento"].ToString(),
                        Email = reader["Email"].ToString(),
                        Extension = reader["Extension"].ToString(),
                        Gerente = reader["Gerente"].ToString(),
                        Rol = reader["Rol"].ToString()
                    }); ;
                }
                reader.Close();
            }


            //obtiene información general de Roles
            String query_roles = "SELECT * FROM ITNT_Roles WHERE Status = 1";
            SqlDataReader reader_roles = (SqlDataReader)await dbayt03.query(query_roles);
            List<RolViewModel> roles = new List<RolViewModel>();

            if (reader_roles.HasRows)
            {
                while (reader_roles.Read())
                {
                    roles.Add(new RolViewModel
                    {
                        Id = Int32.Parse(reader_roles["Id"].ToString()),
                        Nombre = reader_roles["Nombre"].ToString(),
                        Status = Int32.Parse(reader_roles["Status"].ToString())
                    });
                }
                reader_roles.Close();
            }

            ////obtiene información general de la lista de sucursales
            //String query_sucursales = "SELECT " +
            //                          "TB_CODIGO AS Codigo," +
            //                          "TB_ELEMENT AS Sucursal " +
            //                          "FROM NIVEL1 " +
            //                          "WHERE TB_ACTIVO = 'S' AND " +
            //                          "TB_CODIGO NOT IN('14','23','96','97', '98','99') " +
            //                          "ORDER BY TB_ELEMENT ASC"; 
            //SqlDataReader reader_sucursales = (SqlDataReader)await dbRH.query(query_sucursales);
            //List<SucursalesViewModel> sucursales = new List<SucursalesViewModel>();

            //if (reader_sucursales.HasRows)
            //{
            //    while (reader_sucursales.Read())
            //    {
            //        sucursales.Add(new SucursalesViewModel
            //        {                       
            //            Codigo = reader_sucursales["Codigo"].ToString().Replace(" ",""),
            //            Sucursal = reader_sucursales["Sucursal"].ToString()
            //        });
            //    }
            //    reader_sucursales.Close();
            //}

            ////obtiene relacion usuarios - sucursal
            //String query_usuario_sucursal = "SELECT * FROM ITNT_Usuario_Sucursal WHERE IdUsuario = " + id;
            //SqlDataReader reader_usuario_sucursal = (SqlDataReader)await dbayt03.query(query_usuario_sucursal);
            ////List<UsuarioSucursalViewModel> usuario_sucursales = new List<UsuarioSucursalViewModel>();

            //if (reader_usuario_sucursal.HasRows)
            //{
            //    while (reader_usuario_sucursal.Read())
            //    {
            //        var suc = sucursales.FirstOrDefault(d => d.Codigo == reader_usuario_sucursal["IdSucursal"].ToString());
            //        if ( suc != null)
            //        {
            //            suc.IsSelected = true;
            //        }

            //    }
            //    reader_usuario_sucursal.Close();
            //}

            //obtiene todos los usuarios
            String query_usu = "SELECT Id, Nombre FROM  ITNT_Usuarios"; //Activo
            SqlDataReader reader_todos_usuarios = (SqlDataReader)await dbayt03.query(query_usu);
            List<UsuariosViewModel> _usuarios = new List<UsuariosViewModel>();

            if (reader_todos_usuarios.HasRows)
            {
                while (reader_todos_usuarios.Read())
                {
                    _usuarios.Add(new UsuariosViewModel
                    {
                        Id = Int32.Parse(reader_todos_usuarios["Id"].ToString()),
                        Nombre = reader_todos_usuarios["Nombre"].ToString()
                    });
                }
                reader_todos_usuarios.Close();
            }

            //obtiene relacion personal cargo
            String query_personal_cargo = "SELECT " +
                                          "U.Id AS Usuario, " +
                                          "U.Nombre " +
                                          "FROM ITNT_Personal_Cargos PC " +
                                          "LeFT JOIN ITNT_Usuarios U ON PC.IdUsuarioCargo = U.Id " +
                                          "WHERE PC.IdUsuarioPrimario = " + id;
            SqlDataReader reader_personal_cargo = (SqlDataReader)await dbayt03.query(query_personal_cargo);
            //List<UsuarioSucursalViewModel> personal_cargo = new List<UsuarioSucursalViewModel>();

            if (reader_personal_cargo.HasRows)
            {
                while (reader_personal_cargo.Read())
                {
                    var personal_cargo = _usuarios.FirstOrDefault(d => d.Id == Int32.Parse(reader_personal_cargo["Usuario"].ToString()));
                    if (personal_cargo != null)
                    {
                        personal_cargo.IsSelected = true;
                    }
                }
                reader_personal_cargo.Close();
            }




            PermisosViewModel permisos = new PermisosViewModel
            {
                Informacion = usuarios,
                Roles = roles,
                //Sucursales = sucursales,
                PersonalCargo = _usuarios
                
            };

            return Json(new { data = permisos });
            //return View(permisos);
        }

        [HttpPost]
        public async Task<ActionResult> SaveInformation(int id, int rol, int[] personal)
        {
            try
            {
                databaseAYT03 dbayt03 = new databaseAYT03();
                var message = "fallo";

                String query_delete_personal_cargo = "DELETE FROM ITNT_Personal_Cargos WHERE IdUsuarioPrimario = " + id;
                var delete = await dbayt03.query(query_delete_personal_cargo);

                foreach (var p in personal)
                {
                    String query_insert_personal = "INSERT INTO ITNT_Personal_Cargos VALUES (" + id + "," + p + ")";
                    var personal_cargo = await dbayt03.queryInsert(query_insert_personal);
                }

                String query = "UPDATE ITNT_Usuarios SET IdRol = " + rol + " WHERE Id = " + id;
                var a = await dbayt03.queryInsert(query);

                if (a >= 1)
                {
                    message = "exito";
                }

                return Json(new { message });
            }
            catch
            {
                return Json("s");
            }
            
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public static async Task<List<ConfMenuViewModel>> GetPrivileges(string email)
        {
            databaseAYT03 dbay03 = new databaseAYT03();

            //Informacion del usuario
            //email = "sistemas10@avanceytec.com.mx";
            String query_usuario = "SELECT IdRol FROM ITNT_Usuarios WHERE Email = '" + email + "'";
            SqlDataReader reader = (SqlDataReader)await dbay03.query(query_usuario);
            List<ConfMenuViewModel> configuracion = new List<ConfMenuViewModel>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id_rol = Int32.Parse(reader["IdRol"].ToString());


                    String query_menus = "SELECT " +
                                            "m.Id, " +
                                            "m.Nombre " +
                                         "FROM " +
                                         "ITNT_Rol_Menu rm " +
                                         "LEFT JOIN ITNT_Menu m ON rm.IdMenu = m.Id " +
                                         "WHERE rm.IdRol = " + id_rol + " AND m.Status = 1";

                    SqlDataReader reader_menu = (SqlDataReader)await dbay03.query(query_menus);


                    if (reader_menu.HasRows)
                    {
                        while (reader_menu.Read())
                        {
                            var id_menu = reader_menu["Id"].ToString();

                            String query_modulos = "SELECT Id, Nombre FROM ITNT_Modulos WHERE IdMenu = " + id_menu + " AND Status = 1";
                            SqlDataReader reader_modulos = (SqlDataReader)await dbay03.query(query_modulos);
                            List<ModulosViewModel> modulos = new List<ModulosViewModel>();

                            if (reader_modulos.HasRows)
                            {

                                while (reader_modulos.Read())
                                {
                                    var id_modulo = reader_modulos["Id"].ToString();

                                    String query_submodulos = "SELECT Nombre, Url FROM ITNT_Submodulos WHERE IdModulo = " + id_modulo + " AND Status = 1";
                                    SqlDataReader reader_submodulo = (SqlDataReader)await dbay03.query(query_submodulos);
                                    List<SubmodulosViewModel> submodulos = new List<SubmodulosViewModel>();
                                    if (reader_submodulo.HasRows)
                                    {
                                        while (reader_submodulo.Read())
                                        {
                                            submodulos.Add(new SubmodulosViewModel
                                            {
                                                Nombre = reader_submodulo["Nombre"].ToString(),
                                                Url = reader_submodulo["Url"].ToString()
                                            });
                                        }
                                    }


                                    modulos.Add(new ModulosViewModel
                                    {
                                        Nombre = reader_modulos["Nombre"].ToString(),
                                        Submodulos = submodulos
                                    });
                                }
                            }

                            configuracion.Add(new ConfMenuViewModel
                            {
                                Nombre = reader_menu["Nombre"].ToString(),
                                Modulos = modulos
                            });
                        }
                    }



                }
            }


            return configuracion;
        }
    }
}