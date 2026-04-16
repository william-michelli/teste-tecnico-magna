-- Criar banco
CREATE DATABASE TaskManagementDb;
USE TaskManagementDb;

-- Criar tabela
CREATE TABLE Tasks (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Title VARCHAR(200) NOT NULL,
  Description VARCHAR(1000),
  Status INT NOT NULL,
  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Inserir dados de exemplo
INSERT INTO Tasks (Title, Description, Status)
VALUES
('Organizar backlog', 'Revisar as tarefas abertas e priorizar.', 0),
('Implementar API', 'Criar endpoints para CRUD de tarefas.', 1),
('Enviar relatório', 'Enviar relatório semanal para o time.', 0);