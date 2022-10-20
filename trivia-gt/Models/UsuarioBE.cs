using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace trivia_gt.Models
{
    public class UsuarioBE
    {
        [HiddenInput(DisplayValue = false)]
        public int IdUsuario { get; set; }

        [DisplayName("Nombre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es un dato requerido")]
        public string? Nombres { get; set; }

        [DisplayName("Apellidos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido del usuario es un dato requerido")]
        public string? Apellidos { get; set; }

        [DisplayName("Fecha de nacimiento")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La fecha de nacimiento del usuario es un dato requerido")]
        [DataType(DataType.DateTime)]
        public string? FechaNacimiento { get; set; }

        [DisplayName("Correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El usuario es un dato requerido")]
        [DataType(DataType.EmailAddress)]
        public string? Correo { get; set; }

       

        public string? Clave { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string?  url { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string? fechaUltimaConexion { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string? diasUltimaConexion { get; set; }
    }

}
