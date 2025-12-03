using CitasApi.Helpers;
using CitasApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}