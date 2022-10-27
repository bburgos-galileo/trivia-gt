using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using trivia_gt.DAL;
using trivia_gt.Models;

namespace trivia_gt.Controllers
{
    public class PerfilController : Controller
    {
        [HttpGet]
        public IActionResult Editar()
        {
            ConfiguracionBE usuarioBE = new ConfiguracionBE();
            PerfilDAL perfilDAL = new PerfilDAL();

            List<ConfiguracionBE> _lista = new List<ConfiguracionBE>();

            if (HttpContext.Session.GetString("Correo") == null)
            {
                return Redirect("/Login/Login");
            }

            usuarioBE.Correo = HttpContext.Session.GetString("Correo");
            
            
            _lista = perfilDAL.Listar(usuarioBE);

            usuarioBE = _lista[0];
            usuarioBE.ListaAvatar = new List<AvatarBE>();
            usuarioBE.ListaAvatar.AddRange(CargarAvatar());

            usuarioBE.Roles = new List<SelectListItem>();
            usuarioBE.Roles.Add(new SelectListItem { Value = "1", Text = "Jugador" });
            usuarioBE.Roles.Add(new SelectListItem { Value = "2", Text = "Administrador" });


            ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
            ViewBag.Imagen = @"https://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");
            ViewBag.Visible = false;
            ViewBag.Nivel = 0;
            ViewBag.Percentage = 0;
            ViewBag.ImagenCombo = @"https://drive.google.com/uc?export=view&id=1wQ3L1xIfvyfoUueYKik5GTNTM1tYU89w";

            return View(usuarioBE);
        }

        [HttpPost]
        public IActionResult Editar(ConfiguracionBE entidad)
        {
            PerfilDAL perfilDAL = new();
            ConfiguracionBE usuarioBE = new();
            UsuarioDAL usuarioDAL = new();

            List<ConfiguracionBE> _lista = new List<ConfiguracionBE>();

            usuarioBE.Correo = HttpContext.Session.GetString("Correo");
            entidad.Correo = usuarioBE.Correo;

            if (entidad.IdRol == null)
            {
                entidad.IdRol = 1;
            }

            DateTime fechaNacimiento = Convert.ToDateTime(entidad.FechaNacimiento);
            DateTime startDate = new DateTime(1921, 1, 1, 0, 0, 0);

            if (fechaNacimiento < startDate || fechaNacimiento > DateTime.Now)
            {
                ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
                ViewBag.Imagen = @"https://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");
                ViewBag.Visible = false;
                ViewBag.Nivel = 0;
                ViewBag.Percentage = 0;
                ViewBag.ImagenCombo = @"https://drive.google.com/uc?export=view&id=1wQ3L1xIfvyfoUueYKik5GTNTM1tYU89w";

                entidad.ListaAvatar = CargarAvatar();
                entidad.Roles = new List<SelectListItem>();
                entidad.Roles.Add(new SelectListItem { Value = "1", Text = "Jugador", Selected = true });
                entidad.Roles.Add(new SelectListItem { Value = "2", Text = "Administrador" });

                ModelState.AddModelError("FechaNacimiento", "La fecha de nacimiento no puede ser menor a 01/01/1921 ni mayor a hoy");
                ModelState.Remove("IdRol");
                ModelState.Remove("Correo");

                return View(entidad);
            }

            _ = perfilDAL.Actualizar(entidad);
            _lista = usuarioDAL.Listar(usuarioBE);

            HttpContext.Session.Remove("Imagen");
            HttpContext.Session.Remove("Nombres");
            HttpContext.Session.SetString("Imagen", _lista[0].url);
            HttpContext.Session.SetString("Nombres", _lista[0].Nombres + " " + _lista[0].Apellidos);
            HttpContext.Session.SetString("informacion", "Registro Actualizado");

            return Redirect("/Home/Index");

        }

        [HttpGet]
        public IActionResult Crear()
        {
            ConfiguracionBE usuarioBE = new ConfiguracionBE();
            PerfilDAL perfilDAL = new PerfilDAL();

            usuarioBE.Correo = HttpContext.Session.GetString("Correo");

            List<ConfiguracionBE> _lista = perfilDAL.Listar(usuarioBE);

            usuarioBE.ListaAvatar = CargarAvatar();
            usuarioBE.Roles = new List<SelectListItem>();
            usuarioBE.Roles.Add(new SelectListItem { Value = "1", Text = "Jugador", Selected = true });
            usuarioBE.Roles.Add(new SelectListItem { Value = "2", Text = "Administrador" });
            usuarioBE.IdRol = 1;

            return View(usuarioBE);
        }

        [HttpPost]
        public IActionResult Crear(ConfiguracionBE entidad)
        {
            
            PerfilDAL perfilDAL = new();

            entidad.IdRol = 1;

            DateTime fechaNacimiento = Convert.ToDateTime(entidad.FechaNacimiento);
            DateTime startDate = new DateTime(1921, 1, 1, 0, 0, 0);

            if (fechaNacimiento < startDate || fechaNacimiento > DateTime.Now)
            {
                entidad.ListaAvatar = CargarAvatar();
                entidad.Roles = new List<SelectListItem>();
                entidad.Roles.Add(new SelectListItem { Value = "1", Text = "Jugador", Selected = true });
                entidad.Roles.Add(new SelectListItem { Value = "2", Text = "Administrador" });

                ModelState.AddModelError("FechaNacimiento", "La fecha de nacimiento no puede ser menor a 01/01/1921 ni mayor a hoy");
                ModelState.Remove("IdRol");

                return View(entidad);
            }


            if (CorreoYaRegistrado(entidad.Correo))
            {

                entidad.ListaAvatar = CargarAvatar();
                entidad.Roles = new List<SelectListItem>();
                entidad.Roles.Add(new SelectListItem { Value = "1", Text = "Jugador" , Selected = true });
                entidad.Roles.Add(new SelectListItem { Value = "2", Text = "Administrador"});

                ModelState.AddModelError("Correo", "El correo ingresado ya esta registrado");
                ModelState.Remove("IdRol");

                return View(entidad);
            }

            
            
            _ = perfilDAL.Crear(entidad);

            HttpContext.Session.SetString("informacion", "Registro creado");

            return Redirect("/Login/Login");

        }

        private List<AvatarBE> CargarAvatar()
        {
            AvatarDAL avatarDAL = new AvatarDAL();

            List<AvatarBE> _listaAvatar = avatarDAL.Listar(new AvatarBE());

            return _listaAvatar;

        }

        private bool CorreoYaRegistrado(string correo)
        {
            ConfiguracionBE usuarioBE = new ConfiguracionBE();
            PerfilDAL perfilDAL = new PerfilDAL();

            List<ConfiguracionBE> _lista = new List<ConfiguracionBE>();

            usuarioBE.Correo = correo;

            _lista = perfilDAL.Listar(usuarioBE);

            if (_lista.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
