using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using trivia_gt.DAL;
using trivia_gt.Models;

namespace trivia_gt.Controllers
{
    public class ConfiguracionController : Controller
    {
        [HttpGet]
        public IActionResult Editar()
        {
            ConfiguracionBE configuracionBE = new ConfiguracionBE();
            ConfiguracionDAL configuracionDAL = new ConfiguracionDAL();

            List<ConfiguracionBE> _lista = new List<ConfiguracionBE>();

            if (HttpContext.Session.GetString("Correo") == null)
            {
                return Redirect("/Login/Login");
            }

            _lista = configuracionDAL.Listar(new ConfiguracionBE());

            if (_lista.Count > 0)
            {
                configuracionBE = _lista[0];
            }

            ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
            ViewBag.IdRol = HttpContext.Session.GetInt32("IdRol");
            ViewBag.Imagen = @"https://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");
            ViewBag.Visible = false;
            ViewBag.Nivel = 0;
            ViewBag.Percentage = 0;
            ViewBag.ImagenCombo = @"https://drive.google.com/uc?export=view&id=1wQ3L1xIfvyfoUueYKik5GTNTM1tYU89w";

            return View(configuracionBE);
        }

        [HttpPost]
        public IActionResult Editar(ConfiguracionBE entidad)
        {
            ConfiguracionDAL configuracionDAL = new ConfiguracionDAL();

            if (!IsWebsiteUp_Get(entidad.urlApi))
            {
                ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
                ViewBag.IdRol = HttpContext.Session.GetInt32("IdRol");
                ViewBag.Imagen = @"https://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");
                ViewBag.Visible = false;
                ViewBag.Nivel = 0;
                ViewBag.Percentage = 0;
                ViewBag.ImagenCombo = @"https://drive.google.com/uc?export=view&id=1wQ3L1xIfvyfoUueYKik5GTNTM1tYU89w";

                ModelState.AddModelError("urlApi", "La url ingresada no es valida");

                return View(entidad);

            }

            if (entidad.idConfiguracion == 0)
            {
                configuracionDAL.Crear(entidad);
            }
            else
            {
                configuracionDAL.Actualizar(entidad);
            }

            return Redirect("/Login/Logout");
        }

        private bool IsWebsiteUp_Get(string url)
        {
            url += "1&grupo=1";

            try
            {

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage httpResponse = httpClient.GetAsync(url).Result;
                return httpResponse.IsSuccessStatusCode;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
