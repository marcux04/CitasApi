using CitasApi.DTOs;
using CitasApi.Helpers;
using CitasApi.Repositories;
using CitasApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Todos los endpoints requieren autenticación
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;
        private readonly ICitaRepository _citaRepository;

        public CitasController(ICitaService citaService, ICitaRepository citaRepository)
        {
            _citaService = citaService;
            _citaRepository = citaRepository;
        }

        /// <summary>
        /// Obtener cita por ID
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <returns>Datos de la cita</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cita = await _citaRepository.GetByIdAsync(id);
                
                if (cita == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Cita no encontrada"));

                var citaReadDto = new CitaReadDto
                {
                    Id = cita.Id,
                    PacienteId = cita.PacienteId,
                    PacienteNombre = cita.Paciente?.Nombre ?? "N/A",
                    MedicoId = cita.MedicoId,
                    MedicoNombre = cita.Medico?.Nombre ?? "N/A",
                    Especialidad = cita.Medico?.Especialidad ?? "N/A",
                    Fecha = cita.Fecha,
                    Hora = cita.Hora,
                    Estado = cita.Estado
                };

                return Ok(ApiResponse<object>.SuccessResponse(citaReadDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Agendar nueva cita
        /// </summary>
        /// <remarks>
        /// Ejemplo de request:
        /// {
        ///     "pacienteId": 1,
        ///     "medicoId": 1,
        ///     "fecha": "2024-12-15",
        ///     "hora": "10:00:00"
        /// }
        /// </remarks>
        /// <param name="citaDto">Datos de la cita</param>
        /// <returns>Cita creada</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Paciente")] // Solo administradores y pacientes
        [ProducesResponseType(typeof(ApiResponse<object>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] CitaCreateDto citaDto)
        {
            try
            {
                var cita = await _citaService.CreateCitaAsync(citaDto);

                var citaReadDto = new CitaReadDto
                {
                    Id = cita.Id,
                    PacienteId = cita.PacienteId,
                    PacienteNombre = cita.Paciente?.Nombre ?? "N/A",
                    MedicoId = cita.MedicoId,
                    MedicoNombre = cita.Medico?.Nombre ?? "N/A",
                    Especialidad = cita.Medico?.Especialidad ?? "N/A",
                    Fecha = cita.Fecha,
                    Hora = cita.Hora,
                    Estado = cita.Estado
                };

                return CreatedAtAction(nameof(GetById), 
                    new { id = cita.Id }, 
                    ApiResponse<object>.SuccessResponse(citaReadDto, "Cita agendada exitosamente"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
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
        /// Obtener citas por paciente
        /// </summary>
        /// <param name="pacienteId">ID del paciente</param>
        /// <returns>Lista de citas del paciente</returns>
        [HttpGet("paciente/{pacienteId}")]
        [Authorize(Roles = "Admin,Paciente")] // Solo administradores y el propio paciente
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> GetByPaciente(int pacienteId)
        {
            try
            {
                var citas = await _citaService.GetCitasByPacienteAsync(pacienteId);
                
                var citasDto = citas.Select(c => new CitaReadDto
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    PacienteNombre = c.Paciente?.Nombre ?? "N/A",
                    MedicoId = c.MedicoId,
                    MedicoNombre = c.Medico?.Nombre ?? "N/A",
                    Especialidad = c.Medico?.Especialidad ?? "N/A",
                    Fecha = c.Fecha,
                    Hora = c.Hora,
                    Estado = c.Estado
                });

                return Ok(ApiResponse<object>.SuccessResponse(citasDto));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtener citas por médico
        /// </summary>
        /// <param name="medicoId">ID del médico</param>
        /// <returns>Lista de citas del médico</returns>
        [HttpGet("medico/{medicoId}")]
        [Authorize(Roles = "Admin,Medico")] // Solo administradores y médicos
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> GetByMedico(int medicoId)
        {
            try
            {
                var citas = await _citaService.GetCitasByMedicoAsync(medicoId);
                
                var citasDto = citas.Select(c => new CitaReadDto
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    PacienteNombre = c.Paciente?.Nombre ?? "N/A",
                    MedicoId = c.MedicoId,
                    MedicoNombre = c.Medico?.Nombre ?? "N/A",
                    Especialidad = c.Medico?.Especialidad ?? "N/A",
                    Fecha = c.Fecha,
                    Hora = c.Hora,
                    Estado = c.Estado
                });

                return Ok(ApiResponse<object>.SuccessResponse(citasDto));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtener citas con filtros
        /// </summary>
        /// <remarks>
        /// Ejemplo de request:
        /// {
        ///     "pacienteId": 1,
        ///     "medicoId": 1,
        ///     "fechaDesde": "2024-12-01",
        ///     "fechaHasta": "2024-12-31",
        ///     "estado": "pendiente"
        /// }
        /// </remarks>
        /// <param name="filtro">Filtros de búsqueda</param>
        /// <returns>Citas filtradas</returns>
        [HttpGet("filtradas")]
        [Authorize(Roles = "Admin")] // Solo administradores
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> GetFiltradas([FromQuery] FiltroCitasDto filtro)
        {
            try
            {
                var citas = await _citaService.GetCitasFiltradasAsync(filtro);
                
                var citasDto = citas.Select(c => new CitaReadDto
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    PacienteNombre = c.Paciente?.Nombre ?? "N/A",
                    MedicoId = c.MedicoId,
                    MedicoNombre = c.Medico?.Nombre ?? "N/A",
                    Especialidad = c.Medico?.Especialidad ?? "N/A",
                    Fecha = c.Fecha,
                    Hora = c.Hora,
                    Estado = c.Estado
                });

                return Ok(ApiResponse<object>.SuccessResponse(citasDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Actualizar cita
        /// </summary>
        /// <remarks>
        /// Ejemplo de request:
        /// {
        ///     "medicoId": 2,
        ///     "fecha": "2024-12-25",
        ///     "hora": "11:00:00"
        /// }
        /// </remarks>
        /// <param name="id">ID de la cita</param>
        /// <param name="citaDto">Datos a actualizar</param>
        /// <returns>Cita actualizada</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Paciente")] // Solo administradores y pacientes
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [Produces("application/json")]
        public async Task<IActionResult> Update(int id, [FromBody] CitaUpdateDto citaDto)
        {
            try
            {
                var cita = await _citaService.UpdateCitaAsync(id, citaDto);
                
                if (cita == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Cita no encontrada"));

                var citaReadDto = new CitaReadDto
                {
                    Id = cita.Id,
                    PacienteId = cita.PacienteId,
                    PacienteNombre = cita.Paciente?.Nombre ?? "N/A",
                    MedicoId = cita.MedicoId,
                    MedicoNombre = cita.Medico?.Nombre ?? "N/A",
                    Especialidad = cita.Medico?.Especialidad ?? "N/A",
                    Fecha = cita.Fecha,
                    Hora = cita.Hora,
                    Estado = cita.Estado
                };

                return Ok(ApiResponse<object>.SuccessResponse(citaReadDto, "Cita actualizada exitosamente"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
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
        /// Cambiar estado de una cita
        /// </summary>
        /// <remarks>
        /// Ejemplo de request:
        /// {
        ///     "estado": "confirmada"
        /// }
        /// </remarks>
        /// <param name="id">ID de la cita</param>
        /// <param name="estadoDto">Nuevo estado</param>
        /// <returns>Cita actualizada</returns>
        [HttpPut("{id}/estado")]
        [Authorize(Roles = "Admin,Medico,Paciente")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDto estadoDto)
        {
            try
            {
                var cita = await _citaService.CambiarEstadoCitaAsync(id, estadoDto.Estado);
                
                if (cita == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Cita no encontrada"));

                var citaReadDto = new CitaReadDto
                {
                    Id = cita.Id,
                    PacienteId = cita.PacienteId,
                    PacienteNombre = cita.Paciente?.Nombre ?? "N/A",
                    MedicoId = cita.MedicoId,
                    MedicoNombre = cita.Medico?.Nombre ?? "N/A",
                    Especialidad = cita.Medico?.Especialidad ?? "N/A",
                    Fecha = cita.Fecha,
                    Hora = cita.Hora,
                    Estado = cita.Estado
                };

                return Ok(ApiResponse<object>.SuccessResponse(citaReadDto, "Estado de cita actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Cancelar cita
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Paciente")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _citaService.DeleteCitaAsync(id);
                
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Cita no encontrada"));

                return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Cita cancelada exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }
    }
}