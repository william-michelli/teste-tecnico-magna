CREATE DATABASE TaskManagementDb;
USE TaskManagementDb;

CREATE TABLE Tasks (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Title VARCHAR(50) NOT NULL,
  Description VARCHAR(100),
  Status INT NOT NULL,
  CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  EditedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO Tasks (Title, Description, Status)
VALUES
('Comprar arroz', 'Ir ao mercado comprar 2kg de arroz', 0),
('Lavar a louça', 'Louça do jantar de ontem', 2),
('Estudar SQL', 'Revisar joins e subqueries', 1),
('Fazer exercícios', 'Treino de peito e tríceps', 1),
('Pagar contas', 'Luz e internet', 0),
('Atualizar currículo', 'Adicionar experiência recente', 2),
('Limpar o quarto', 'Organizar roupas e aspirar', 0),
('Responder e-mails', 'Pendências do trabalho', 2),
('Preparar marmitas', '', 0),
('Ir na academia', '', 0),
('Mandar feliz aniversário', 'Dar feliz aniversário para o Roberto', 2),
('Comprar ração', '', 0);
