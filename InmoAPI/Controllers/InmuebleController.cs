using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;

using InmoAPI.Models;

namespace InmoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InmuebleController : ControllerBase
    {
        private readonly string cadenaSQL;
        public InmuebleController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("cadenaSQL");
        }

        [HttpGet]
        [Route("Inmuebles")]
        public IActionResult ListarInmuebles()
        {
            List<Inmuebles> result = new List<Inmuebles>();

            try
            {
                using (var conn = new SqlConnection(cadenaSQL))
                {
                    conn.Open();
                    var cmd = new SqlCommand("sp_listar_inmuebles", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Inmuebles() 
                            { 
                                IdInmueble = Convert.ToInt32(reader["id"]),
                                
                            });

                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", inmueble_count = result.Count, inmuebles = result });
            }
            
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message, response = result });
            }
        }

        [HttpGet]
        [Route("GetInmueble/{Idinmueble:int}")]
        public IActionResult Inmuebles(int Idinmueble)
        {
            List<Inmuebles> result = new List<Inmuebles>();
            Inmuebles inmueble = new Inmuebles();

            try
            {
                using (var conn = new SqlConnection(cadenaSQL))
                {
                    conn.Open();
                    var cmd = new SqlCommand("sp_listar_inmuebles", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Inmuebles()
                            {
                                IdInmueble = Convert.ToInt32(reader["id"]),

                            });

                        }
                    }
                }

                inmueble = result.Where(item => item.IdInmueble == Idinmueble).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", inmueble_count = result.Count, inmuebles = result });
            }

            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message, response = result });
            }
        }
    }
}
