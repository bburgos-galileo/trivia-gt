using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using trivia_gt.DAL;
using trivia_gt.Models;

namespace trivia_gt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Correo") == null)
            {
                return Redirect("/Login/Login");
            }

            ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
            ViewBag.Imagen = @"https://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");

            int dias = (int)HttpContext.Session.GetInt32("DiasConexion");

            if (dias > 1)
            {
                ViewBag.ImagenAnimo = @"https://drive.google.com/uc?export=view&id=1wQ3L1xIfvyfoUueYKik5GTNTM1tYU89w";
            } else
            {
                ViewBag.ImagenAnimo = @"https://drive.google.com/uc?export=view&id=1nocxQPDztHwLvbbacQcPUhmC2OmbTegL";
            }

            ViewBag.ImagenLibro = @"https://drive.google.com/uc?export=view&id=1JC2aNjxTR3he5mywYJY_lnCwG0duZUpp";
            
            ViewBag.Fecha = HttpContext.Session.GetString("FechaConexion");

            ViewBag.Visible = false;
            ViewBag.Nivel = 0;
            ViewBag.Percentage = 0;
            
            if (HttpContext.Session.GetString("mensaje") == null)
            {
                ViewBag.Mensaje = null;
            } else
            {
                ViewBag.Mensaje = HttpContext.Session.GetString("mensaje");
            }

            if (HttpContext.Session.GetString("informacion") == null)
            {
                ViewBag.Info = null;
            }
            else
            {
                ViewBag.Info = HttpContext.Session.GetString("informacion");
                HttpContext.Session.Remove("informacion");
            }

            ViewBag.IdRol = HttpContext.Session.GetInt32("IdRol");

            if (HttpContext.Session.GetInt32("idRol").Equals(1))
            {
                return View();

            } else
            {

                UsuarioDAL usuarioDAL = new();
                List<UsuarioBE> listaUsuario = usuarioDAL.Listar(new UsuarioBE());

                PunteoBE punteoBE = new();

                PunteoDAL punteoDAL = new();
                List<PunteoBE> listaPunteo = punteoDAL.Listar(new PunteoBE());

                string correo = HttpContext.Session.GetString("Correo");

                UsuarioBE usuario = listaUsuario.First(c => c.Correo.Equals(correo));

                listaUsuario.Remove(usuario);

                foreach (UsuarioBE usuarioBE in listaUsuario)
                {
                    if (listaPunteo.Exists(p => p.Correo.Equals(usuarioBE.Correo)))
                    {
                        usuarioBE.punteoBE = new();
                        usuarioBE.punteoBE = listaPunteo.First(p => p.Correo.Equals(usuarioBE.Correo));

                        switch (int.Parse(usuarioBE.punteoBE.Punteo) switch
                        {
                            5 or 10 => 1,
                            15 or 20 => 2,
                            25 or 30 => 3,
                            35 or 40 or 45 => 4,
                            50 => 5,
                            _ => 0,
                        })
                        {
                            case 1:
                                usuarioBE.punteoBE.estrella1 = 1;
                                usuarioBE.Orden = 1;
                                break;
                            case 2:
                                usuarioBE.punteoBE.estrella1 = 1;
                                usuarioBE.punteoBE.estrella2 = 1;
                                usuarioBE.Orden = 2;
                                break;
                            case 3:
                                usuarioBE.punteoBE.estrella1 = 1;
                                usuarioBE.punteoBE.estrella2 = 1;
                                usuarioBE.punteoBE.estrella3 = 1;
                                usuarioBE.Orden = 3;
                                break;
                            case 4:
                                usuarioBE.punteoBE.estrella1 = 1;
                                usuarioBE.punteoBE.estrella2 = 1;
                                usuarioBE.punteoBE.estrella3 = 1;
                                usuarioBE.punteoBE.estrella4 = 1;
                                usuarioBE.Orden = 4;
                                break;
                            case 5:
                                usuarioBE.punteoBE.estrella1 = 1;
                                usuarioBE.punteoBE.estrella2 = 1;
                                usuarioBE.punteoBE.estrella3 = 1;
                                usuarioBE.punteoBE.estrella4 = 1;
                                usuarioBE.punteoBE.estrella5 = 1;
                                usuarioBE.Orden = 5;
                                break;
                            default:
                                break;
                        }

                    } else
                    {
                        usuarioBE.punteoBE = new();
                        usuarioBE.punteoBE.Punteo = "0";
                    }

                    usuarioBE.Roles = new List<SelectListItem>();
                    usuarioBE.Roles.Add(new SelectListItem { Value = "1", Text = "Jugador" });
                    usuarioBE.Roles.Add(new SelectListItem { Value = "2", Text = "Administrador" });
                }

                return View(listaUsuario.OrderByDescending(o => o.Orden).ToList());
            }

        }


        [HttpPost]
        [Produces("application/json")]
        public IActionResult Actualiza([FromBody] UsuariosBE usuarios)
        {

            UsuarioDAL usuarioDAL = new();
           

            UsuarioBE usuarioBE = new UsuarioBE
            {
                Correo = usuarios.Correo,
                IdRol = usuarios.IdRol,
                IsEditing = true
            };

            usuarioDAL.Actualizar(usuarioBE);

            return Json(new { success = true, direccion = "/Home/Index" }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult Grabar([FromBody] List<UsuariosBE> usuarios)
        {
            UsuarioDAL usuarioDAL = new();

            foreach (UsuariosBE item in usuarios)
            {
                UsuarioBE usuarioBE = new UsuarioBE
                {
                    Correo = item.Correo,
                    IdRol = item.IdRol,
                    IsEditing = true
                };

                usuarioDAL.Actualizar(usuarioBE);
            }

            return Json(new { success = true, direccion = "/Home/Index" }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Construccion()
        {
            if (HttpContext.Session.GetString("Correo") == null)
            {
                return Redirect("/Login/Login");
            }

            ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
            ViewBag.Imagen = @"https://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");

            ViewBag.Visible = false;
            ViewBag.Nivel = 0;
            ViewBag.Percentage = 0;
            ViewBag.Mensaje = null;
            ViewBag.Info = null;
            ViewBag.IdRol = HttpContext.Session.GetInt32("IdRol");

            return View();
        }

    }
}