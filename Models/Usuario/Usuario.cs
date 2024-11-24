using System.ComponentModel.DataAnnotations;

namespace Terra.Models.Usuario
{
    public class UsuarioData
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contraseña { get; set; } 
    }
}
