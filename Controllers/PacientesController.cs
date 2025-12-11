using CitasApi.DTOs;
using CitasApi.Helpers;
using CitasApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Todos los endpoints requieren autenticación
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        /// <summary>
        /// Obtener todos los pacientes (solo administradores)
        /// </summary>
        /// <returns>Lista de pacientes</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")] // Solo administradores
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var pacientes = await _pacienteService.GetAllPacientesAsync();
                return Ok(ApiResponse<object>.SuccessResponse(pacientes));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Buscar pacientes con filtros (solo administradores)
        /// </summary>
        /// <remarks>
        /// Ejemplo de request como query string:
        /// /api/pacientes/buscar?nombre=Juan&amp;curp=PERJ
        /// </remarks>
        /// <param name="filtro">Filtros de búsqueda</param>
        /// <returns>Pacientes filtrados</returns>
        [HttpGet("buscar")]
        [Authorize(Roles = "Admin")] // Solo administradores
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        [Produces("application/json")]
        public async Task<IActionResult> Buscar([FromQuery] FiltroPacientesDto filtro)
        {
            try
            {
                var pacientes = await _pacienteService.GetPacientesFiltradosAsync(filtro);
                return Ok(ApiResponse<object>.SuccessResponse(pacientes));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Registrar un nuevo paciente (público)
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
        [AllowAnonymous] // Público - cualquiera puede registrarse
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
        [Authorize(Roles = "Admin,Paciente")] // Solo admin o el propio paciente
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        [Produces("application/json")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // Verificar que el usuario tiene permiso para ver este paciente
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
        [Authorize(Roles = "Admin,Paciente")] // Solo admin o el propio paciente
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
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
        /// Cambiar rol de paciente (solo administradores)
        /// </summary>
        /// <remarks>
        /// Ejemplo de request:
        /// {
        ///     "nuevoRol": "Admin"
        /// }
        /// </remarks>
        /// <param name="id">ID del paciente</param>
        /// <param name="nuevoRol">Nuevo rol (Admin, Paciente, Medico)</param>
        /// <returns>Paciente actualizado</returns>
        [HttpPut("{id}/cambiar-rol")]
        [Authorize(Roles = "Admin")] // Solo administradores
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> CambiarRol(int id, [FromBody] string nuevoRol)
        {
            try
            {
                var paciente = await _pacienteService.CambiarRolAsync(id, nuevoRol);
                
                if (paciente == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Paciente no encontrado"));

                return Ok(ApiResponse<object>.SuccessResponse(new 
                { 
                    paciente.Id, 
                    paciente.Nombre, 
                    Rol = paciente.Rol 
                }, "Rol actualizado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
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
        [Authorize(Roles = "Admin")] // Solo administradores
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
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