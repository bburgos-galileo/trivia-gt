namespace trivia_gt.Models
{
    public class PreguntaBE
    {
        public int idPunteo { get; set; }

        public int punteo { get; set; }

        public int intentos { get; set; }

        public int nivel { get; set; }

        public int idUsuario { get; set; }

        public int idPregunta { get; set; }

        public int idEstado { get; set; }

        public string pregunta { get; set; }

        public List<RespuestaBE> respuestas { get; set; }

        public int idRespuesta { get; set; }
        public bool respondio { get; set; }
    }
}
