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
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        /// <summary>
        /// Obtener lista de médicos
        /// </summary>
        /// <returns>Lista de médicos</returns>
        [HttpGet]
        [AllowAnonymous] // Público
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var medicos = await _medicoService.GetAllMedicosAsync();
                return Ok(ApiResponse<object>.SuccessResponse(medicos));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtener médico por ID
        /// </summary>
        /// <param name="id">ID del médico</param>
        /// <returns>Datos del médico</returns>
        [HttpGet("{id}")]
        [AllowAnonymous] // Público
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Produces("application/json")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var medico = await _medicoService.GetMedicoByIdAsync(id);
                
                if (medico == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Médico no encontrado"));

                return Ok(ApiResponse<object>.SuccessResponse(medico));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Crear un nuevo médico
        /// </summary>
        /// <remarks>
        /// Solo administradores pueden crear médicos.
        /// Ejemplo de request:
        /// {
        ///     "nombre": "Dr. Nuevo Médico",
        ///     "especialidad": "Neurología"
        /// }
        /// </remarks>
        /// <param name="medicoDto">Datos del médico</param>
        /// <returns>Médico creado</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")] // Solo administradores
        [ProducesResponseType(typeof(ApiResponse<object>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] MedicoCreateDto medicoDto)
        {
            try
            {
                var medico = await _medicoService.CreateMedicoAsync(medicoDto);
                return CreatedAtAction(nameof(GetById), new { id = medico.Id }, 
                    ApiResponse<object>.SuccessResponse(medico, "Médico creado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Actualizar médico
        /// </summary>
        /// <remarks>
        /// Solo administradores pueden actualizar médicos.
        /// Ejemplo de request:
        /// {
        ///     "nombre": "Dr. Médico Actualizado",
        ///     "especialidad": "Cardiología"
        /// }
        /// </remarks>
        /// <param name="id">ID del médico</param>
        /// <param name="medicoDto">Datos a actualizar</param>
        /// <returns>Médico actualizado</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Solo administradores
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        [Produces("application/json")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicoUpdateDto medicoDto)
        {
            try
            {
                var medico = await _medicoService.UpdateMedicoAsync(id, medicoDto);
                
                if (medico == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Médico no encontrado"));

                return Ok(ApiResponse<object>.SuccessResponse(medico, "Médico actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Eliminar médico
        /// </summary>
        /// <remarks>
        /// Solo administradores pueden eliminar médicos.
        /// </remarks>
        /// <param name="id">ID del médico</param>
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
                var result = await _medicoService.DeleteMedicoAsync(id);
                
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Médico no encontrado"));

                return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Médico eliminado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"Error: {ex.Message}"));
            }
        }
    }
}