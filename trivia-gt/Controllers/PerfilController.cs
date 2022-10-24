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
            UsuarioBE usuarioBE = new UsuarioBE();
            PerfilDAL perfilDAL = new PerfilDAL();
            AvatarDAL avatarDAL = new AvatarDAL();

            List<UsuarioBE> _lista = new List<UsuarioBE>();
            List<AvatarBE> _listaAvatar = new List<AvatarBE>();

            usuarioBE.Correo = HttpContext.Session.GetString("Correo");
            
            
            _lista = perfilDAL.Listar(usuarioBE);
            _listaAvatar = avatarDAL.Listar(new AvatarBE());

            usuarioBE = _lista[0];
            usuarioBE.ListaAvatar = new List<AvatarBE>();

            bool seleccionar = false;

            foreach (AvatarBE item in _listaAvatar)
            {

                usuarioBE.ListaAvatar.Add(new AvatarBE { IdAvatar = item.IdAvatar, Tag = item.Tag, URL = @"https://drive.google.com/uc?export=view&id=" + item.URL });
            }

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
        public IActionResult Editar(UsuarioBE entidad)
        {
            PerfilDAL perfilDAL = new PerfilDAL();
            UsuarioBE usuarioBE = new UsuarioBE();
            UsuarioDAL usuarioDAL = new UsuarioDAL();

            List<UsuarioBE> _lista = new List<UsuarioBE>();

            usuarioBE.Correo = HttpContext.Session.GetString("Correo");

            _ = perfilDAL.Actualizar(entidad);
            _lista = usuarioDAL.Listar(usuarioBE);

            HttpContext.Session.Remove("Imagen");
            HttpContext.Session.SetString("Imagen", _lista[0].url);
            HttpContext.Session.SetString("informacion", "Registro Actualizado");

 


            return Redirect("/Home/Index");

        }

        [HttpGet]
        public IActionResult Crear()
        {
            UsuarioBE usuarioBE = new UsuarioBE();
            PerfilDAL perfilDAL = new PerfilDAL();
            AvatarDAL avatarDAL = new AvatarDAL();

            List<UsuarioBE> _lista = new List<UsuarioBE>();
            List<AvatarBE> _listaAvatar = new List<AvatarBE>();

            usuarioBE.Correo = HttpContext.Session.GetString("Correo");


            _lista = perfilDAL.Listar(usuarioBE);
            _listaAvatar = avatarDAL.Listar(new AvatarBE());

            foreach (AvatarBE item in _listaAvatar)
            {

                usuarioBE.ListaAvatar.Add(new AvatarBE { IdAvatar = item.IdAvatar, Tag = item.Tag, URL = @"https://drive.google.com/uc?export=view&id=" + item.URL });
            }

            usuarioBE.Roles = new List<SelectListItem>();

            usuarioBE.Roles.Add(new SelectListItem { Value = "1", Text = "Jugador" });
            usuarioBE.Roles.Add(new SelectListItem { Value = "2", Text = "Administrador" });

            return View(new UsuarioBE());
        }

        [HttpPost]
        public IActionResult Crear(UsuarioBE entidad)
        {
            PerfilDAL perfilDAL = new PerfilDAL();

            _ = perfilDAL.Crear(entidad);

            return Redirect("/Login/Login");

        }
    }
}
