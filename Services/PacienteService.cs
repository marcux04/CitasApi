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
                Password = BCrypt.Net.BCrypt.HashPassword(pacienteDto.Password)
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

            // SOLUCIÓN AL PROBLEMA DEL LOGIN:
            // 1. Primero intentar verificar con BCrypt (si la contraseña fue hasheada)
            // 2. Si falla, verificar si la contraseña está en texto plano (para desarrollo/migración)
            // 3. Si coincide en texto plano, actualizar a hash BCrypt
            
            bool passwordValid = false;
            
            // Intentar verificar con BCrypt
            try
            {
                passwordValid = BCrypt.Net.BCrypt.Verify(password, paciente.Password);
            }
            catch (Exception)
            {
                // Si BCrypt falla, podría ser porque la contraseña está en texto plano
                passwordValid = (paciente.Password == password);
                
                // Si la contraseña coincide en texto plano, actualizar a hash
                if (passwordValid)
                {
                    paciente.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    await _repository.UpdateAsync(paciente);
                }
            }

            return passwordValid ? paciente : null;
        }
    }
}