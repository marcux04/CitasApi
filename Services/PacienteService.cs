using CitasApi.DTOs;
using CitasApi.Models;
using CitasApi.Repositories;

namespace CitasApi.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Paciente>> GetAllPacientesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Paciente>> GetPacientesFiltradosAsync(FiltroPacientesDto filtro)
        {
            return await _repository.GetPacientesFiltradosAsync(filtro);
        }

        public async Task<Paciente> CreatePacienteAsync(PacienteCreateDto pacienteDto)
        {
            // Verificar CURP única
            if (await _repository.ExistsByCurpAsync(pacienteDto.Curp))
                throw new InvalidOperationException("La CURP ya está registrada");

            // Verificar correo único
            if (await _repository.ExistsByEmailAsync(pacienteDto.Correo))
                throw new InvalidOperationException("El correo ya está registrado");

            var paciente = new Paciente
            {
                Nombre = pacienteDto.Nombre,
                Curp = pacienteDto.Curp,
                Telefono = pacienteDto.Telefono,
                Correo = pacienteDto.Correo,
                Password = BCrypt.Net.BCrypt.HashPassword(pacienteDto.Password),
                Rol = "Paciente" // Rol por defecto
            };

            return await _repository.CreateAsync(paciente);
        }

        public async Task<Paciente?> GetPacienteByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Paciente?> UpdatePacienteAsync(int id, PacienteUpdateDto pacienteDto)
        {
            var paciente = await _repository.GetByIdAsync(id);
            if (paciente == null)
                return null;

            if (!string.IsNullOrEmpty(pacienteDto.Nombre))
                paciente.Nombre = pacienteDto.Nombre;

            if (!string.IsNullOrEmpty(pacienteDto.Telefono))
                paciente.Telefono = pacienteDto.Telefono;

            if (!string.IsNullOrEmpty(pacienteDto.Correo) && pacienteDto.Correo != paciente.Correo)
            {
                if (await _repository.ExistsByEmailAsync(pacienteDto.Correo))
                    throw new InvalidOperationException("El correo ya está registrado");
                
                paciente.Correo = pacienteDto.Correo;
            }

            if (!string.IsNullOrEmpty(pacienteDto.Password))
                paciente.Password = BCrypt.Net.BCrypt.HashPassword(pacienteDto.Password);

            await _repository.UpdateAsync(paciente);
            return paciente;
        }

        public async Task<bool> DeletePacienteAsync(int id)
        {
            var paciente = await _repository.GetByIdAsync(id);
            if (paciente == null)
                return false;

            await _repository.DeleteAsync(paciente);
            return true;
        }

        public async Task<Paciente?> AuthenticateAsync(string email, string password)
        {
            var paciente = await _repository.GetByEmailAsync(email);
            if (paciente == null)
                return null;

            bool passwordValid = false;
            
            try
            {
                passwordValid = BCrypt.Net.BCrypt.Verify(password, paciente.Password);
            }
            catch (Exception)
            {
                passwordValid = (paciente.Password == password);
                
                if (passwordValid)
                {
                    paciente.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    await _repository.UpdateAsync(paciente);
                }
            }

            return passwordValid ? paciente : null;
        }

        public async Task<Paciente?> CambiarRolAsync(int id, string nuevoRol)
        {
            var paciente = await _repository.GetByIdAsync(id);
            if (paciente == null)
                return null;

            var rolesPermitidos = new[] { "Paciente", "Admin", "Medico" };
            if (!rolesPermitidos.Contains(nuevoRol))
                throw new InvalidOperationException($"Rol no válido. Roles permitidos: {string.Join(", ", rolesPermitidos)}");

            paciente.Rol = nuevoRol;
            await _repository.UpdateAsync(paciente);
            return paciente;
        }
    }
}