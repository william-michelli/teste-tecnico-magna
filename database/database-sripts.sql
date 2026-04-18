-- Criar banco
CREATE DATABASE TaskManagementDb;
USE TaskManagementDb;

-- Criar tabela
CREATE TABLE Tasks (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Title VARCHAR(50) NOT NULL,
  Description VARCHAR(100),
  Status INT NOT NULL,
  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  EditedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Inserir dados
INSERT INTO Tasks (Title, Description, Status)
VALUES
('Compra arroz', '', 0),
('Organizar gavetas', 'Gavetas da escrivaninha', 1),
('Lavar a louça', '', 0);