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

        [DisplayName("Nombre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido del usuario es un dato requerido")]
        public string? Apellidos { get; set; }

        [DisplayName("Nombre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La fecha de nacimiento del usuario es un dato requerido")]
        [DataType(DataType.DateTime)]
        public string? FechaNacimiento { get; set; }

        [DisplayName("Correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El usuario es un dato requerido")]
        [DataType(DataType.EmailAddress)]
        public string? Correo { get; set; }

        [DisplayName("Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El password es un dato requerido")]
        [StringLength(100, ErrorMessage = "La clave debe contener de 5 a 100 caracteres", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string? Clave { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string?  url { get; set; }
    }

}
