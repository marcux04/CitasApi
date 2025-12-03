CREATE DATABASE CitasMedicas;
USE CitasMedicas;

CREATE TABLE IF NOT EXISTS catPacientes (
    id INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    curp VARCHAR(18) NOT NULL UNIQUE, -- El CURP no debe repetirse
    telefono VARCHAR(10),
    correo VARCHAR(100) NOT NULL UNIQUE, -- El correo no debe repetirse
    password VARCHAR(255) NOT NULL,
    PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS catMedicos (
    id INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(100) NOT NULL,
    especialidad VARCHAR(50) NOT NULL,
    PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS citas (
    id INT NOT NULL AUTO_INCREMENT,
    paciente_id INT NOT NULL,
    medico_id INT NOT NULL,
    fecha DATE NOT NULL,
    hora TIME NOT NULL,
    estado VARCHAR(20) DEFAULT 'pendiente', -- Valor por defecto
    PRIMARY KEY (id),
    -- Definición de Claves Foráneas (Foreign Keys)
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
