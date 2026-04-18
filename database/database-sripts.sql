-- Criar banco
CREATE DATABASE TaskManagementDb;
USE TaskManagementDb;

-- Criar tabela
CREATE TABLE Tasks (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Title VARCHAR(200) NOT NULL,
  Description VARCHAR(1000),
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