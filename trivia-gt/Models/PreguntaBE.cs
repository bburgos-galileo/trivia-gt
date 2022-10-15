namespace trivia_gt.Models
{
    public class PreguntaBE
    {
        public int idPregunta { get; set; }

        public int nivel { get; set; }

        public string pregunta { get; set; }

        public List<RespuestaBE> respuestas { get; set; }

        public int idRespuesta { get; set; }
        public bool respondio { get; set; }
    }
}
