-- ==========================================
-- ExploradorBaas - Script MySQL (schema + tablas)
-- Base: explorador_baas
-- Charset recomendado: utf8mb4
-- ==========================================

CREATE DATABASE IF NOT EXISTS `explorador_baas`
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE `explorador_baas`;

-- Tabla principal: personajes
CREATE TABLE IF NOT EXISTS `personajes` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(200) NOT NULL,
  `Estado` VARCHAR(50) NOT NULL,
  `Especie` VARCHAR(100) NOT NULL,
  `Ubicacion` VARCHAR(200) NOT NULL,
  `ImagenUrl` VARCHAR(500) NOT NULL,
  `FechaActualizacionUtc` DATETIME(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabla hija: episodios por personaje (1 a N)
CREATE TABLE IF NOT EXISTS `episodios_personaje` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `PersonajeId` INT NOT NULL,
  `UrlEpisodio` VARCHAR(500) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_episodios_personaje_PersonajeId` (`PersonajeId`),
  CONSTRAINT `FK_episodios_personaje_personajes_PersonajeId`
    FOREIGN KEY (`PersonajeId`) REFERENCES `personajes` (`Id`)
    ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
