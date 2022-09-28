CREATE DATABASE AEROPORTO;

DROP DATABASE AEROPORTO;

USE AEROPORTO;

CREATE TABLE CompanhiaAerea(
	CNPJ VARCHAR(14) CONSTRAINT PK_CompanhiaAerea PRIMARY KEY,
	RazaoSocial VARCHAR(50) NOT NULL,
	DataAbertura DATE NOT NULL,
	UltimoVoo DATE NOT NULL,
	DataCadastro DATE NOT NULL, 
	Situacao VARCHAR(10));

CREATE TABLE Aeronave(
	INSCRICAO VARCHAR(5) CONSTRAINT PK_Aeronave PRIMARY KEY,
	Capacidade INT NOT NULL,
	UltimaVenda DATE NOT NULL,
	DataCadastro DATE NOT NULL,
	Situacao VARCHAR(10));

CREATE TABLE CompanhiaPossueAeronave(
	CNPJ VARCHAR(14) CONSTRAINT FK_CompanhiaAerea FOREIGN KEY REFERENCES CompanhiaAerea(CNPJ),
	INSCRICAO VARCHAR(5) CONSTRAINT FK_Aeronave FOREIGN KEY REFERENCES Aeronave(INSCRICAO),
	CONSTRAINT PK_CompanhiaPossueAeronave PRIMARY KEY(CNPJ, INSCRICAO));

CREATE TABLE Voo(
	IDVOO INT IDENTITY CONSTRAINT PK_Voo PRIMARY KEY,
	Destino VARCHAR(3) NOT NULL,
	DataVoo DATE NOT NULL,
	DataCadastro DATE NOT NULL,
	Situacao VARCHAR(10));

CREATE TABLE AeronavePossueVoo(
	INSCRICAO VARCHAR(5) CONSTRAINT FK_AeronaveVoo FOREIGN KEY REFERENCES Aeronave(INSCRICAO),
	IDVOO INT CONSTRAINT FK_Voo FOREIGN KEY REFERENCES Voo(IDVOO),
	Capacidade INT NOT NULL,
	AcentosOcupados INT,
	CONSTRAINT PK_AeronavePossueVoo PRIMARY KEY(INSCRICAO, IDVOO));

CREATE TABLE Passagem(
	IDPASSAGEM INT IDENTITY CONSTRAINT PK_Passagem PRIMARY KEY,
	DataUltimaOperacao DATE NOT NULL,
	ValorPassagem DECIMAL(6,2) NOT NULL,
	Situacao VARCHAR(10));

CREATE TABLE Venda(
	IDVENDA INT IDENTITY CONSTRAINT PK_Venda PRIMARY KEY,
	DataVenda DATE NOT NULL,
	ValorTotalVendas DECIMAL(6,2));

CREATE TABLE VendaPassagem(
	IDPASSAGEM INT CONSTRAINT FK_Passagem FOREIGN KEY REFERENCES Passagem(IDPASSAGEM),
	IDVENDA INT CONSTRAINT FK_Venda FOREIGN KEY REFERENCES Venda(IDVENDA),
	IDITEM INT IDENTITY,
	ValorUnitarioAtual DECIMAL(6,2),
	PRIMARY KEY(IDPASSAGEM, IDVENDA, IDITEM));

CREATE TABLE Passageiro(
	CPF VARCHAR(11) NOT NULL PRIMARY KEY,
	UltimaCompra DATE NOT NULL,
	Nome VARCHAR(50) NOT NULL,
	Nascimento DATE NOT NULL,
	Sexo VARCHAR(10),
	Situacao VARCHAR(10));

CREATE TABLE Restricao(
	CNPJ VARCHAR(14) UNIQUE,
	CPF VARCHAR(11) UNIQUE);

SELECT * FROM CompanhiaAerea;

SELECT companhiaaerea.RazaoSocial, companhiaaerea.CNPJ,
	companhiaaerea.DataAbertura, companhiaaerea.UltimoVoo,
	companhiaaerea.DataCadastro, companhiaaerea.Situacao FROM CompanhiaAerea;

SELECT * FROM CompanhiaAerea;

SELECT * FROM Aeronave;

SELECT * FROM Voo WHERE;

SELECT * FROM AeronavePossueVoo;

SELECT * FROM CompanhiaPossueAeronave;

SELECT companhiaaerea.RazaoSocial, companhiaaerea.CNPJ, aeronave.INSCRICAO,
	aeronave.Capacidade, aeronave.UltimaVenda, aeronave.DataCadastro, aeronave.Situacao
	FROM Aeronave JOIN CompanhiaPossueAeronave ON 
	companhiapossueaeronave.INSCRICAO = aeronave.INSCRICAO
	JOIN CompanhiaAerea ON companhiapossueaeronave.CNPJ = companhiaaerea.CNPJ WHERE aeronave.INSCRICAO

INSERT AeronavePossueVoo(INSCRICAO, IDVOO, Capacidade, AcentosOcupados) VALUES('PTAAA', 1, 150, 1);

UPDATE Voo SET Destino = 'ALC' WHERE IDVOO = 1;