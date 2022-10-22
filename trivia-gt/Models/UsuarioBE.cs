using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace trivia_gt.Models
{
    public class UsuarioBE
    {
        [Key]
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string? FechaNacimiento { get; set; }

        [DisplayName("Correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El usuario es un dato requerido")]
        [DataType(DataType.EmailAddress)]
        public string? Correo { get; set; }

        [DisplayName("Avatar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El avatar es un dato requerido")]
        public int? IdAvatar { get; set; }

        [DisplayName("Rol")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Rol es un dato requerido")]
        public int? IdRol { get; set; }

        [DisplayName("Clave")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Password es un dato requerido")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "El password debe contener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string? Clave { get; set; }

        [DisplayName("Confirmar Clave")]
        [Compare("Clave", ErrorMessage = "La clave y la confirmación no coinciden")]
        [DataType(DataType.Password)]
        public string? ConfirmacionClave { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string?  url { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string? fechaUltimaConexion { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string? diasUltimaConexion { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public List<AvatarBE> ListaAvatar { get; set; }
    }

}
