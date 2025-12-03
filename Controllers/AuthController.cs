using CitasApi.DTOs;
using CitasApi.Helpers;
using CitasApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IPacienteService pacienteService, JwtHelper jwtHelper)
        {
            _pacienteService = pacienteService;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Iniciar sesión como paciente
        /// </summary>
        /// <remarks>
        /// Ejemplo de request:
        /// {
        ///     "correo": "paciente@ejemplo.com",
        ///     "password": "Password123"
        /// }
        /// </remarks>
        /// <param name="loginDto">Credenciales de acceso</param>
        /// <returns>Token JWT y datos del paciente</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var paciente = await _pacienteService.AuthenticateAsync(loginDto.Correo, loginDto.Password);
                
                if (paciente == null)
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Credenciales inválidas"));

                var token = _jwtHelper.GenerateToken(paciente.Correo, paciente.Id);

                return Ok(ApiResponse<object>.SuccessResponse(new
                {
                    Token = token,
                    Paciente = new
                    {
                        paciente.Id,
                        paciente.Nombre,
                        paciente.Correo,
                        paciente.Telefono
                    }
                }, "Inicio de sesión exitoso"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }
    }
}