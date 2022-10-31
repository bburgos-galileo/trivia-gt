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

                string correo = HttpContext.Session.GetString("Correo");

                UsuarioBE usuario = listaUsuario.First(c => c.Correo.Equals(correo));

                listaUsuario.Remove(usuario);

                foreach (UsuarioBE usuarioBE in listaUsuario)
                {
                    usuarioBE.Roles = new List<SelectListItem>();
                    usuarioBE.Roles.Add(new SelectListItem { Value = "1", Text = "Jugador" });
                    usuarioBE.Roles.Add(new SelectListItem { Value = "2", Text = "Administrador" });
                }

                return View(listaUsuario);
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