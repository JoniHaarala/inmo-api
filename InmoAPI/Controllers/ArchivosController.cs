using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;
using InmoAPI.Models;

namespace InmoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        private readonly string rutaServidor;
        private readonly string CadenaSQL;

        public ArchivosController(IConfiguration config)
        {
            rutaServidor = config.GetSection("filePath").GetSection("rutaServidor").Value;
            CadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpPost]
        [Route("subir")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public IActionResult Subir([FromForm] Archivos request)
        {
            string rutaArchivo = Path.Combine(CadenaSQL, request.Archivo.FileName);

            try
            {
                using (FileStream fs = System.IO.File.Create(rutaArchivo))
                {
                    //creamos un nuevo archivo en el directorio del server
                    request.Archivo.CopyTo(fs);
                    fs.Flush();
                }

                // creamos la coneccion a la bd y realizamos el sp
                using (var conn = new SqlConnection(rutaArchivo))
                {
                    conn.Open();
                    var cmd = new SqlCommand("sp_guardar_archivo", conn);
                    cmd.Parameters.AddWithValue("Nombre", request.Nombre);
                    cmd.Parameters.AddWithValue("Direccion", request.Direccion);
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "documento guardado" });

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }
    }
}
