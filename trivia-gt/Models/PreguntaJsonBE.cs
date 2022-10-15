using Microsoft.AspNetCore.Components.Forms;

namespace trivia_gt.Models
{
    public class PreguntaJsonBE
    {

        public string pregunta { get; set; }

        public List<RespuestaBE> respuesta_1 { get; set; }
        public List<RespuestaBE> respuesta_2 { get; set; }
        public List<RespuestaBE> respuesta_3 { get; set; }


    }
}
