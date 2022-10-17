using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using trivia_gt.Models;
using System.Collections.Generic;

namespace trivia_gt
{
    public static class Utilities
    {
        public static List<PreguntaBE> ListarPreguntas()
        {
            try
            {
                RespuestaBE respuestaBE;
                List<PreguntaBE> lista = new List<PreguntaBE>();

                for (int i = 1; i < 10; i++)
                {
                    PreguntaJsonBE preguntaJson = ObtenerPregunta(i.ToString());

                    PreguntaBE pregunta = new PreguntaBE();

                    pregunta.idPregunta = i;
                    
                    if (i < 5)
                    {
                        pregunta.nivel = 1;
                    } else
                    {
                        pregunta.nivel = 2;
                    }

                    pregunta.pregunta = preguntaJson.pregunta;

                    pregunta.respuestas = new List<RespuestaBE>();

                    respuestaBE = new RespuestaBE();
                    respuestaBE = preguntaJson.respuesta_1[0];
                    respuestaBE.idRespuesta = 1;
                    pregunta.respuestas.Add(respuestaBE);

                    respuestaBE = new RespuestaBE();
                    respuestaBE = preguntaJson.respuesta_2[0];
                    respuestaBE.idRespuesta = 2;
                    pregunta.respuestas.Add(respuestaBE);

                    respuestaBE = new RespuestaBE();
                    respuestaBE = preguntaJson.respuesta_3[0];
                    respuestaBE.idRespuesta = 3;
                    pregunta.respuestas.Add(respuestaBE);


                    lista.Add(pregunta);


                }

                return lista;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static PreguntaJsonBE ObtenerPregunta(string nivel)
        {
			try
			{
                string apiURL = "http://ec2-44-203-35-246.compute-1.amazonaws.com/preguntas.php?nivel=" + nivel + "&grupo=1";

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.GetAsync(apiURL).Result;
                if (response.IsSuccessStatusCode)
                {
                    string message = response.Content.ReadAsStringAsync().Result;
                    string parsedString = Regex.Unescape(message);
                    byte[] isoBites = Encoding.UTF8.GetBytes(parsedString);
                    string respuesta = Encoding.UTF8.GetString(isoBites, 0, isoBites.Length);

                    var json = respuesta.Replace(":{", ":[{").Replace("},", "}],").Replace("}}", "}]}");

                    List<PreguntaJsonBE>? respuestaArray = JsonConvert.DeserializeObject<List<PreguntaJsonBE>>("[" + json + "]");

                    return respuestaArray[0];

                } else
                {
                    throw new Exception("Error al obtener la pregunta");
                }
            }
			catch (Exception)
			{
                throw new Exception("Error al obtener la pregunta");
            }
        }

        public static void GrabaPreguntasCache(this ISession session, object value)
        {
            session.Remove("preguntas");
            session.SetString("preguntas", JsonConvert.SerializeObject(value));
        }

        public static T? ObtienePreguntasCache<T>(this ISession session)
        {
            var value = session.GetString("preguntas");
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
