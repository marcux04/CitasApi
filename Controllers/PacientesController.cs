using CitasApi.DTOs;
using CitasApi.Helpers;
using CitasApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        /// <summary>
        /// Registrar un nuevo paciente
        /// </summary>
        /// <remarks>
        /// Ejemplo de request:
        /// {
        ///     "nombre": "Juan Pérez",
        ///     "curp": "PERJ800101HDFRNN09",
        ///     "telefono": "5512345678",
        ///     "correo": "juan@ejemplo.com",
        ///     "password": "Password123"
        /// }
        /// </remarks>
        /// <param name="pacienteDto">Datos del paciente</param>
        /// <returns>Paciente creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<object>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] PacienteCreateDto pacienteDto)
        {
            try
            {
                var paciente = await _pacienteService.CreatePacienteAsync(pacienteDto);
                
                var pacienteReadDto = new PacienteReadDto
                {
                    Id = paciente.Id,
                    Nombre = paciente.Nombre,
                    Curp = paciente.Curp,
                    Telefono = paciente.Telefono,
                    Correo = paciente.Correo,
                    FechaRegistro = DateTime.Now
                };

                return CreatedAtAction(nameof(GetById), 
                    new { id = paciente.Id }, 
                    ApiResponse<object>.SuccessResponse(pacienteReadDto, "Paciente registrado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtener paciente por ID
        /// </summary>
        /// <param name="id">ID del paciente</param>
        /// <returns>Datos del paciente</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [Produces("application/json")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paciente = await _pacienteService.GetPacienteByIdAsync(id);
                
                if (paciente == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Paciente no encontrado"));

                var pacienteReadDto = new PacienteReadDto
                {
                    Id = paciente.Id,
                    Nombre = paciente.Nombre,
                    Curp = paciente.Curp,
                    Telefono = paciente.Telefono,
                    Correo = paciente.Correo,
                    FechaRegistro = DateTime.Now
                };

                return Ok(ApiResponse<object>.SuccessResponse(pacienteReadDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Actualizar paciente
        /// </summary>
        /// <remarks>
        /// Ejemplo de request:
        /// {
        ///     "nombre": "Juan Pérez Actualizado",
        ///     "telefono": "5512345679",
        ///     "correo": "juan.nuevo@ejemplo.com",
        ///     "password": "NuevaPassword123"
        /// }
        /// </remarks>
        /// <param name="id">ID del paciente</param>
        /// <param name="pacienteDto">Datos a actualizar</param>
        /// <returns>Paciente actualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [Produces("application/json")]
        public async Task<IActionResult> Update(int id, [FromBody] PacienteUpdateDto pacienteDto)
        {
            try
            {
                var paciente = await _pacienteService.UpdatePacienteAsync(id, pacienteDto);
                
                if (paciente == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Paciente no encontrado"));

                var pacienteReadDto = new PacienteReadDto
                {
                    Id = paciente.Id,
                    Nombre = paciente.Nombre,
                    Curp = paciente.Curp,
                    Telefono = paciente.Telefono,
                    Correo = paciente.Correo,
                    FechaRegistro = DateTime.Now
                };

                return Ok(ApiResponse<object>.SuccessResponse(pacienteReadDto, "Paciente actualizado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Eliminar paciente
        /// </summary>
        /// <param name="id">ID del paciente</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _pacienteService.DeletePacienteAsync(id);
                
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Paciente no encontrado"));

                return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Paciente eliminado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }
    }
}