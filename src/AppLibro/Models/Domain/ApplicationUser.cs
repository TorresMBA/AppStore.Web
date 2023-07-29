using Microsoft.AspNetCore.Identity;

namespace AppLibro.Models.Domain
{
    /**
    Se genera esta clara para poder heredar todas las propiedades de la Tabla/Clase
    'AspNetUsers' generada por identity y para que eso suceda se crea una clase y herada
    de 'IdentityUser'
    */
    public class ApplicationUser : IdentityUser
    {
        public string? Nombre { get; set; }        
    }
}