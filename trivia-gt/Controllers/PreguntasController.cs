using Microsoft.AspNetCore.Mvc;
using System.Text;
using trivia_gt.DAL;
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
            ViewBag.ImagenLibro = @"http://drive.google.com/uc?export=view&id=1JC2aNjxTR3he5mywYJY_lnCwG0duZUpp";
            ViewBag.ImagenAnimo = @"http://drive.google.com/uc?export=view&id=1nocxQPDztHwLvbbacQcPUhmC2OmbTegL";
            ViewBag.Fecha = DateTime.Now.ToString("dd/MM/yyyy");

            List<PreguntaBE>? lista = Utilities.ObtienePreguntasCache<List<PreguntaBE>>(HttpContext.Session);

            if (lista == null)
            {
                lista = Utilities.ListarPreguntas();

                Utilities.GrabaPreguntasCache(HttpContext.Session, lista);
            }

            PreguntaBE pregunta = lista.First(p => !p.respondio);

            return View(pregunta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Grabar(PreguntaBE entidad)
        {
            bool esCorrecta = false;

            if (entidad.idRespuesta == 0)
            {
                return Json(new { success = esCorrecta, message = MensajeError("Se debe seleccionar una pregunta"), direccion = string.Empty }, new Newtonsoft.Json.JsonSerializerSettings());
            }

            List<PreguntaBE>? lista = Utilities.ObtienePreguntasCache<List<PreguntaBE>>(HttpContext.Session);

            PreguntaBE? pregunta = lista.First(p => p.idPregunta.Equals(entidad.idPregunta));

            //valida respuesta
            RespuestaBE respuesta = pregunta.respuestas.First(r => r.idRespuesta.Equals(entidad.idRespuesta));

            if (respuesta.is_correct == 1)
            {
                esCorrecta = true;
            }
            else
            {
                return Json(new { success = esCorrecta, message = MensajeError("Respuesta incorrecta!!!"), direccion = string.Empty }, new Newtonsoft.Json.JsonSerializerSettings());
            }

            int indexList = lista.IndexOf(pregunta);

            lista.Remove(pregunta);

            pregunta.idRespuesta = entidad.idRespuesta;
            pregunta.respondio = true;
            pregunta.idUsuario = (int)HttpContext.Session.GetInt32("IdUsuario");
            pregunta.punteo = 5;
            pregunta.intentos = 1;
            pregunta.idEstado = 1;

            lista.Insert(indexList, pregunta);

            PreguntaDAL preguntaDAL = new PreguntaDAL();

            _ = preguntaDAL.Crear(pregunta);

            Utilities.GrabaPreguntasCache(HttpContext.Session, lista);

            return Json(new { success = esCorrecta, message = Mensaje(), direccion = "/Preguntas/Index" }, new Newtonsoft.Json.JsonSerializerSettings());

        }

        private string Mensaje()
        {
            StringBuilder html = new(string.Empty);

            html.Append("<div class='card align-content-center text-center' style='background-color:#32C682'>");
            html.Append(" <img class='w-25 h-auto rounded card-img-top mx-auto d-block' src='http://drive.google.com/uc?export=view&id=1nocxQPDztHwLvbbacQcPUhmC2OmbTegL'>");
            html.Append("<div class='card-body'>");
            html.Append("<p class='card-text text-white font-weight-bold fs-5'>Respuesta correcta!!! clic para continuar</p>");
            html.Append("</div></div>");

            return html.ToString();
        }

        private string MensajeError(string mensaje)
        {
            StringBuilder html = new(string.Empty);

            html.Append("<div class='card align-content-center text-center' style='background-color:#FA5549'>");
            html.Append(" <img class='w-25 h-auto rounded card-img-top mx-auto d-block' src='http://drive.google.com/uc?export=view&id=1wQ3L1xIfvyfoUueYKik5GTNTM1tYU89w'>");
            html.Append("<div class='card-body'>");
            html.Append("<p class='card-text text-white font-weight-bold fs-5'>" + mensaje + "</p>");
            html.Append("</div></div>");

            return html.ToString();
        }

    }
}
