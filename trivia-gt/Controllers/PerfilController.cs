using Microsoft.AspNetCore.Mvc;
using trivia_gt.DAL;
using trivia_gt.Models;

namespace trivia_gt.Controllers
{
    public class PerfilController : Controller
    {
        public IActionResult Editar()
        {
            UsuarioBE usuarioBE = new UsuarioBE();
            PerfilDAL perfilDAL = new PerfilDAL();
            usuarioBE.Correo = HttpContext.Session.GetString("Correo");
            List<UsuarioBE> _lista = new List<UsuarioBE>();
            _lista = perfilDAL.Listar(usuarioBE);
            usuarioBE = _lista[0];
            return View(usuarioBE);
        }
    }
}
