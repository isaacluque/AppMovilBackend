//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pruebas_SolicitarToken_CambioPassword
    {
        public int RefUsuarios { get; set; }
        public string Dispositivo { get; set; }
        public string Token { get; set; }
        public string Key { get; set; }
        public System.DateTime FechaSolicitud { get; set; }
        public System.DateTime FechaExpiracion { get; set; }
        public Nullable<int> Estado { get; set; }
        public Nullable<int> EmailSend { get; set; }
        public int Id { get; set; }
    
        public virtual Pruebas_EMP Pruebas_EMP { get; set; }
    }
}
