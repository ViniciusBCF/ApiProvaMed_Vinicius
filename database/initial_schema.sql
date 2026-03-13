IF DB_ID('ApiMedTestContatosDb') IS NULL
BEGIN
    CREATE DATABASE ApiMedTestContatosDb;
END
GO

USE ApiMedTestContatosDb;
GO

IF OBJECT_ID('dbo.Contatos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Contatos
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        NomeContato NVARCHAR(128) NOT NULL,
        DataNascimento DATETIME2 NOT NULL,
        Sexo INT NOT NULL,
        StatusContato INT NOT NULL,
        DataCriacao DATETIME2 NOT NULL,
        DataAtualizacao DATETIME2 NULL,
        CONSTRAINT PK_Contatos PRIMARY KEY (Id)
    );
END
GO
