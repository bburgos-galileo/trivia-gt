using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace trivia_gt.Models
{
    public class ConfiguracionBE
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int IdUsuario { get; set; }

        [DisplayName("Nombre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es un dato requerido")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+$", ErrorMessage = "El nombre ingresado no es valido")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El nombre debe contener entre 2 y 30 caracteres")]
        public string? Nombres { get; set; }

        [DisplayName("Apellidos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido del usuario es un dato requerido")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+$", ErrorMessage = "El apellido ingresado no es valido")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El apellido debe contener entre 2 y 30 caracteres")]
        public string? Apellidos { get; set; }

        [DisplayName("Fecha de nacimiento")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La fecha de nacimiento del usuario es un dato requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, HtmlEncode = false)]
        public string? FechaNacimiento { get; set; }

        [DisplayName("Correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El correo es un dato requerido")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "La dirección de correo electrónico ingresada no es válida")]
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

        public List<SelectListItem>? Roles { get; set; }

        public List<AvatarBE>? ListaAvatar { get; set; }
    }

}
