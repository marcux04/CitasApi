📌 API REST para Gestión de Citas Médicas

API REST desarrollada en ASP.NET Core 8.0 para la gestión de citas médicas, utilizando MySQL, JWT para autenticación y arquitectura en capas.

📋 Características

✅ CRUD completo para Pacientes

✅ Gestión de Médicos (listar y obtener por ID)

✅ Sistema de Citas (agendar, listar por paciente y cancelar)

✅ Autenticación JWT

✅ Validaciones estrictas (CURP, teléfono, email, contraseñas)

✅ Hash de contraseñas con BCrypt

✅ Documentación Swagger

✅ Arquitectura por capas (Controllers / Services / Repositories)

✅ Manejo de errores estructurado

✅ Conexión a MySQL con Pomelo EF Core

🚀 Tecnologías Utilizadas

.NET 8.0

ASP.NET Core Web API

Entity Framework Core 8

MySQL Server 8

Pomelo.EntityFrameworkCore.MySql

JWT (JSON Web Tokens)

BCrypt.Net-Next

Swagger / Swashbuckle

Visual Studio Code

Navicat (gestión de BD)

Thunder Client / Postman (pruebas)

📁 Estructura del Proyecto
CitasApi/
├── Controllers/
│   ├── AuthController.cs
│   ├── PacientesController.cs
│   ├── MedicosController.cs
│   └── CitasController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── PacienteCreateDto.cs
│   ├── PacienteReadDto.cs
│   ├── PacienteUpdateDto.cs
│   ├── CitaCreateDto.cs
│   ├── CitaReadDto.cs
│   └── LoginDto.cs
├── Helpers/
│   ├── ApiResponse.cs
│   ├── CurpAttribute.cs
│   ├── TelefonoAttribute.cs
│   └── JwtHelper.cs
├── Models/
│   ├── Paciente.cs
│   ├── Medico.cs
│   └── Cita.cs
├── Repositories/
│   ├── IPacienteRepository.cs
│   ├── PacienteRepository.cs
│   ├── IMedicoRepository.cs
│   ├── MedicoRepository.cs
│   ├── ICitaRepository.cs
│   └── CitaRepository.cs
├── Services/
│   ├── IPacienteService.cs
│   ├── PacienteService.cs
│   ├── IMedicoService.cs
│   ├── MedicoService.cs
│   ├── ICitaService.cs
│   └── CitaService.cs
├── Program.cs
├── appsettings.json
└── CitasApi.csproj

⚙️ Requisitos Previos

.NET SDK 8.0+

MySQL Server 8+

Visual Studio Code

Navicat (opcional)

Thunder Client / Postman

🛠️ Instalación y Configuración
1. Clonar el repositorio
git clone https://github.com/tu-usuario/CitasApi.git
cd CitasApi

2. Crear la base de datos en MySQL

Ejecuta el siguiente script:

-- Crear base de datos nueva
CREATE DATABASE CitasMedicas;
USE CitasMedicas;

-- Crear tabla catPacientes
CREATE TABLE catPacientes (
    id INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    curp VARCHAR(18) NOT NULL UNIQUE,
    telefono VARCHAR(10),
    correo VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Crear tabla catMedicos
CREATE TABLE catMedicos (
    id INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    especialidad VARCHAR(50) NOT NULL,
    PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Crear tabla citas
CREATE TABLE citas (
    id INT NOT NULL AUTO_INCREMENT,
    paciente_id INT NOT NULL,
    medico_id INT NOT NULL,
    fecha DATE NOT NULL,
    hora TIME NOT NULL,
    estado VARCHAR(20) DEFAULT 'pendiente',
    PRIMARY KEY (id),
    CONSTRAINT fk_citas_paciente
        FOREIGN KEY (paciente_id) 
        REFERENCES catPacientes(id)
        ON DELETE RESTRICT 
        ON UPDATE CASCADE,
    CONSTRAINT fk_citas_medico
        FOREIGN KEY (medico_id) 
        REFERENCES catMedicos(id)
        ON DELETE RESTRICT 
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Insertar médicos de prueba
INSERT INTO catMedicos (nombre, especialidad) VALUES
('Dr. Roberto Sánchez', 'Cardiología'),
('Dra. Ana Martínez', 'Pediatría'),
('Dr. Luis Fernández', 'Dermatología'),
('Dra. Sofía Ramírez', 'Ginecología'),
('Dr. Jorge Torres', 'Ortopedia');

-- Insertar paciente con contraseña hasheada (Password: MariaSecure01)
INSERT INTO catPacientes (nombre, curp, telefono, correo, password) VALUES
('María López', 'LOPM900505MDFRPR02', '5511223344', 'maria.lopez@example.com', '$2a$11$nV8w6K5Y4t3z2q1w0e9rD.ABCDEFGHIJKLMNOPQRSTUVWXYZ01234');

3. Configurar la cadena de conexión

Editar appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=CitasMedicas;user=TU_USUARIO;password=TU_CONTRASEÑA;"
  }
}

4. Restaurar dependencias
dotnet restore

5. Ejecutar la API
dotnet run

6. Abrir Swagger

👉 http://localhost:5106/swagger

📊 Endpoints Implementados
🔐 Autenticación

POST /api/auth/login

👥 Pacientes (CRUD)

POST /api/pacientes

GET /api/pacientes/{id}

PUT /api/pacientes/{id}

DELETE /api/pacientes/{id}

👨‍⚕️ Médicos

GET /api/medicos

GET /api/medicos/{id}

📅 Citas

POST /api/citas

GET /api/citas/{id}

GET /api/citas/paciente/{id}

DELETE /api/citas/{id}

🔒 Validaciones y Seguridad
Datos

CURP válido

Teléfono (10 dígitos)

Email válido y único

Contraseña con mínimo 6 caracteres

Citas solo en fechas futuras

Seguridad

Hash BCrypt para contraseñas

JWT con expiración

Tokens firmados con clave secreta

🧪 Ejemplos para Thunder Client / Postman
1. Registrar paciente
POST /api/pacientes
{
  "nombre": "Juan Pérez",
  "curp": "PERJ800101HDFRNN09",
  "telefono": "5512345678",
  "correo": "juan@ejemplo.com",
  "password": "Password123"
}

2. Login
POST /api/auth/login
{
  "correo": "juan@ejemplo.com",
  "password": "Password123"
}

3. Agendar cita
POST /api/citas
Authorization: Bearer TOKEN
{
  "pacienteId": 1,
  "medicoId": 1,
  "fecha": "2024-12-20",
  "hora": "10:00:00"
}

🚦 Códigos de Estado

200 OK

201 Created

400 Bad Request

401 Unauthorized

404 Not Found

409 Conflict

500 Server Error

📝 Respuestas
Éxito:
{
  "success": true,
  "message": "Operación exitosa",
  "data": { }
}

Error:
{
  "success": false,
  "error": "Descripción del error"
}

📈 Estado del Proyecto (Avance 50%)
✔️ Implementado

CRUD completo de Pacientes

Login JWT

CRUD parcial de Citas

Consultas de Médicos

Swagger

Conexión MySQL

Validaciones

Respuestas estructuradas

⏳ Pendiente 50→100%

Crear médicos

Autenticación en todos los endpoints

Roles (admin/paciente)

Notificaciones por correo

Reportes

Búsqueda avanzada

Paginación