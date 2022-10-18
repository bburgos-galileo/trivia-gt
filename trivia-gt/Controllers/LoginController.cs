using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using trivia_gt.DAL;
using trivia_gt.Models;

namespace trivia_gt.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [Produces("application/json")]
        public IActionResult Login(UsuarioBE entidad)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();

            if (entidad.Correo == null)
            {
                return Json(new { success = false, message = "<p class='h5'>El correo es un dato requerido</p>" }, new Newtonsoft.Json.JsonSerializerSettings());
            }

            if (entidad.Clave == null)
            {
                return Json(new { success = false, message = "<p class='h5'>La contraseña es un dato requerido</p>" }, new Newtonsoft.Json.JsonSerializerSettings());
            }

            List<UsuarioBE> listaUsuario = usuarioDAL.Listar(entidad);

            if (listaUsuario.Count == 0)
            {
                return Json(new { success = false, message = "<p class='h5'>El correo ingresado no esta registrado</p>" }, new Newtonsoft.Json.JsonSerializerSettings());
            }

            if (!listaUsuario[0].Clave.Equals(entidad.Clave))
            {
                return Json(new { success = false, message = "<p class='h5'>La clave ingresada es incorrecta</p>" }, new Newtonsoft.Json.JsonSerializerSettings());
            }

            HttpContext.Session.SetInt32("IdUsuario", listaUsuario[0].IdUsuario);
            HttpContext.Session.SetString("Nombres", listaUsuario[0].Nombres + " " + listaUsuario[0].Apellidos);
            HttpContext.Session.SetString("Correo", entidad.Correo);
            HttpContext.Session.SetString("Imagen", listaUsuario[0].url);

            return Json(new { success = true, direccion = "/Home/Index" }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Redirect("/Login/Login");
        }

        private string Mensaje(ModelStateDictionary modelState)
        {
            StringBuilder html = new(string.Empty);

            html.Append("<ul class='text-start'>");

            foreach (var key in modelState.Keys)
            {
                foreach (ModelError error in ModelState[key].Errors)
                {
                    html.Append(@"<li>" + error.ErrorMessage + @"</li>");
                }
            }

            html.Append("</ul>");

            return html.ToString();
        }

    }
}
