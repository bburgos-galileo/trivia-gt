using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace trivia_gt.Models
{
    public class ConfiguracionBE
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int idConfiguracion { get; set; }

        [DisplayName("URL de la API")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El camo URL debe de estar lleno")]
        public string? urlApi { get; set; }

        [DisplayName("Numero de Grupo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe de indicar el numero de grupo")]
        public int? noGrupo { get; set; }
    }
}
