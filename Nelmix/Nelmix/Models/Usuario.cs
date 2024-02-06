namespace Nelmix.Models
{
    
    public partial class Usuario
    {
        public int UserId { get; set; }

        public string Nombre { get; set; }

        public int Edad { get; set; }

        public string Email { get; set; }

        public string? Contraseña { get; set; }

        public int EstadoId { get; set; }

        public int AdultoAsignadoId { get; set; }
    }
}
