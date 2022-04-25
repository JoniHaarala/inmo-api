namespace InmoAPI.Models
{
    public class Archivos
    {
        public int IdArchivo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public IFormFile Archivo { get; set; }
    }
}
