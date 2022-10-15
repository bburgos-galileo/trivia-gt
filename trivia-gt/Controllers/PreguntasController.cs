using Microsoft.AspNetCore.Mvc;
using trivia_gt.Models;

namespace trivia_gt.Controllers
{
    public class PreguntasController : Controller
    {
        // GET: PreguntasController
        public ActionResult Index()
        {
            ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
            ViewBag.Imagen = @"http://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");

            List<PreguntaBE>? lista = Utilities.ListarPreguntas();

            Utilities.GrabaPreguntasCache(HttpContext.Session, lista);

            PreguntaBE pregunta = lista.First(p => !p.respondio);

            return View(pregunta);
        }


        // POST: PreguntasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grabar(PreguntaBE entidad)
        {
            bool esCorrecta = false;

            List<PreguntaBE>? lista = Utilities.ObtienePreguntasCache<List<PreguntaBE>>(HttpContext.Session);

            PreguntaBE? pregunta = lista.First(p => p.idPregunta.Equals(entidad.idPregunta));

            //valida respuesta
            RespuestaBE respuesta = pregunta.respuestas.First(r => r.idRespuesta.Equals(entidad.idRespuesta));

            if (respuesta.is_correct == 1)
            {
                esCorrecta = true;
            }

            int? indexList = lista.IndexOf(pregunta);

            lista.Remove(pregunta);



            return View();
        }

    }
}
