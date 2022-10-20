using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using trivia_gt.Models;
using System.Collections.Generic;
using trivia_gt.DAL;
using MySqlX.XDevAPI;


namespace trivia_gt
{
    public static class Utilities
    {
        public static List<PreguntaBE> ListarPreguntas(this ISession session, int idUsuario)
        {
            try
            {
                PreguntaBE pregunta;
                RespuestaBE respuestaBE;
                PreguntaDAL preguntaDAL = new PreguntaDAL();
                List<PreguntaBE> lista = new List<PreguntaBE>();
                List<PreguntaJsonBE> listaJson = new List<PreguntaJsonBE>();

                pregunta = new PreguntaBE
                {
                    idUsuario = idUsuario
                };

                List<PreguntaBE> listaBD = preguntaDAL.Listar(pregunta);

                for (int i = 1; i <= 10; i++)
                {
                    PreguntaJsonBE preguntaJson = ObtenerPregunta(session, i.ToString(), i);

                    bool existe = listaBD.Exists(p => p.idPregunta.Equals(i));

                    if (!existe)
                    {
                        pregunta = new PreguntaBE
                        {
                            idPregunta = i,
                            nivel = i <= 5 ? 1 : 2,
                            pregunta = preguntaJson.pregunta,
                            respuestas = new List<RespuestaBE>()
                        };

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

                    } else
                    {
                        pregunta = new PreguntaBE();
                        pregunta = listaBD.First(p => p.idPregunta.Equals(i));

                        if (pregunta.idEstado.Equals(1))
                        {
                            pregunta.respondio = true;
                        }
                        else
                        {
                            pregunta.pregunta = preguntaJson.pregunta;
                            pregunta.nivel = i <= 5 ? 1 : 2;

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
                        }

                    }


                    lista.Add(pregunta);
                    listaJson.Add(preguntaJson);

                }

                GrabaPreguntasJsonCache(session, listaJson);

                return lista;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public static PreguntaJsonBE ObtenerPregunta(this ISession session, string nivel, int index)
        {
			try
			{
                if (session.GetString("preguntasJson") == null)
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

                    }
                    else
                    {
                        throw new Exception("Error al obtener la pregunta");
                    }
                }
                else
                {
                    return ObtienePreguntaJson(session, index - 1);
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

        public static void GrabaPreguntasJsonCache(this ISession session, object value)
        {
            if (session.GetString("preguntasJson") == null)
            {
                session.SetString("preguntasJson", JsonConvert.SerializeObject(value));
            }
        }

        public static T? ObtienePreguntasCache<T>(this ISession session)
        {
            string? value = session.GetString("preguntas");

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static PreguntaJsonBE ObtienePreguntaJson(this ISession session, int index)
        {
            PreguntaJsonBE preguntaJson = new PreguntaJsonBE();

            string lista = session.GetString("preguntasJson");

            List<PreguntaJsonBE>? respuestaArray = JsonConvert.DeserializeObject<List<PreguntaJsonBE>>(lista);

            preguntaJson = respuestaArray[index];

            return preguntaJson;
        }
    }
}
