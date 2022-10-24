using Microsoft.AspNetCore.Http;
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
            if (HttpContext.Session.GetString("Correo") == null)
            {
                return Redirect("/Login/Login");
            }

            ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
            ViewBag.Imagen = @"https://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");
            ViewBag.ImagenLibro = @"https://drive.google.com/uc?export=view&id=1JC2aNjxTR3he5mywYJY_lnCwG0duZUpp";

            Int32 dias = (int)HttpContext.Session.GetInt32("DiasConexion");

            if (dias > 1)
            {
                ViewBag.ImagenAnimo = @"https://drive.google.com/uc?export=view&id=1wQ3L1xIfvyfoUueYKik5GTNTM1tYU89w";
            }
            else
            {
                ViewBag.ImagenAnimo = @"https://drive.google.com/uc?export=view&id=1nocxQPDztHwLvbbacQcPUhmC2OmbTegL";
            }

            ViewBag.Fecha = HttpContext.Session.GetString("FechaConexion");
            ViewBag.Visible = true;

            List<PreguntaBE>? lista = Utilities.ObtienePreguntasCache<List<PreguntaBE>>(HttpContext.Session);

            if (lista == null)
            {
                int idUsuario = (int)HttpContext.Session.GetInt32("IdUsuario");

                lista = Utilities.ListarPreguntas(HttpContext.Session, idUsuario);

                Utilities.GrabaPreguntasCache(HttpContext.Session, lista);
            }

            bool existe = lista.Exists(p => !p.respondio);
            
            if (!existe)
            {
                HttpContext.Session.SetString("mensaje", "Niveles Completados");
                return Redirect("/Home/Index");
            }

            PreguntaBE pregunta = lista.First(p => !p.respondio);

            ViewBag.Nivel = pregunta.nivel;

            //calcula el porcentaje

            existe = lista.Exists(p => p.respondio && p.nivel.Equals(pregunta.nivel));

            if (!existe)
            {
                ViewBag.Percentage = 0;
            } else
            {
                int contador = (lista.Where(p => p.respondio && p.nivel.Equals(pregunta.nivel)).ToList().Count * 100) / 5;
                ViewBag.Percentage = contador;
            }

            return View(pregunta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Grabar(PreguntaBE entidad)
        {
            PreguntaDAL preguntaDAL = new PreguntaDAL();

            bool esCorrecta = false;

            if (entidad.idRespuesta == 0)
            {
                return Json(new { success = esCorrecta, message = MensajeError("Se debe seleccionar una pregunta"), direccion = string.Empty }, new Newtonsoft.Json.JsonSerializerSettings());
            }

            List<PreguntaBE>? lista = Utilities.ObtienePreguntasCache<List<PreguntaBE>>(HttpContext.Session);

            //optiene la pregunta
            PreguntaBE? pregunta = lista.First(p => p.idPregunta.Equals(entidad.idPregunta));

            //valida respuesta
            RespuestaBE respuesta = pregunta.respuestas.First(r => r.idRespuesta.Equals(entidad.idRespuesta));

            if (respuesta.is_correct == 1)
            {
                esCorrecta = true;
            }
            else
            {
                HttpContext.Session.Remove("preguntas");

                string actualiza;

                if (pregunta.idPunteo.Equals(0))
                {
                    //agrega la pregunta que se respondio de forma incorrecta;

                    int idUsuario = (int)HttpContext.Session.GetInt32("IdUsuario");

                    pregunta = new PreguntaBE();

                    pregunta.punteo = 0;
                    pregunta.intentos = 1;
                    pregunta.nivel = entidad.idPregunta <= 5 ? 1 : 2;
                    pregunta.idUsuario = idUsuario;
                    pregunta.idPregunta = entidad.idPregunta;
                    pregunta.idEstado = 2;

                    _ = preguntaDAL.Crear(pregunta);

                } else
                {
                    pregunta.intentos++;
                    pregunta.punteo = 0;
                    pregunta.idEstado = 2;

                    _ = preguntaDAL.Actualizar(pregunta);
                }
                
                return Json(new { success = esCorrecta, message = MensajeError("Respuesta incorrecta!!! Clic para continual"), direccion = "/Preguntas/Index" }, new Newtonsoft.Json.JsonSerializerSettings());
            }

            int indexList = lista.IndexOf(pregunta);

            lista.Remove(pregunta);

            pregunta.idRespuesta = entidad.idRespuesta;
            pregunta.respondio = true;
            pregunta.idUsuario = (int)HttpContext.Session.GetInt32("IdUsuario");
            pregunta.punteo = 5;
            pregunta.intentos++;
            pregunta.idEstado = 1;

            lista.Insert(indexList, pregunta);

            if (pregunta.idPunteo.Equals(0))
            {
                _ = preguntaDAL.Crear(pregunta);
            }
            else
            {
                _ = preguntaDAL.Actualizar(pregunta);
            }
            
            Utilities.GrabaPreguntasCache(HttpContext.Session, lista);

            return Json(new { success = esCorrecta, message = Mensaje(), direccion = "/Preguntas/Index" }, new Newtonsoft.Json.JsonSerializerSettings());

        }

        private string Mensaje()
        {
            StringBuilder html = new(string.Empty);

            html.Append("<div class='card align-content-center text-center' style='background-color:#32C682'>");
            html.Append(" <img class='w-25 h-auto rounded card-img-top mx-auto d-block' src='https://drive.google.com/uc?export=view&id=1nocxQPDztHwLvbbacQcPUhmC2OmbTegL'>");
            html.Append("<div class='card-body'>");
            html.Append("<p class='card-text text-white font-weight-bold fs-5'>Respuesta correcta!!! clic para continuar</p>");
            html.Append("</div></div>");

            return html.ToString();
        }

        private string MensajeError(string mensaje)
        {
            StringBuilder html = new(string.Empty);

            html.Append("<div class='card align-content-center text-center' style='background-color:#FA5549'>");
            html.Append(" <img class='w-25 h-auto rounded card-img-top mx-auto d-block' src='https://drive.google.com/uc?export=view&id=1wQ3L1xIfvyfoUueYKik5GTNTM1tYU89w'>");
            html.Append("<div class='card-body'>");
            html.Append("<p class='card-text text-white font-weight-bold fs-5'>" + mensaje + "</p>");
            html.Append("</div></div>");

            return html.ToString();
        }

    }
}
