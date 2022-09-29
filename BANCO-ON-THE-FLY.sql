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
	IDPASSAGEM INT IDENTITY,
	DataUltimaOperacao DATE NOT NULL,
	ValorPassagem DECIMAL(7,2) NOT NULL,
	Situacao VARCHAR(10),
	IDVOO INT CONSTRAINT FK_PassagemVoo FOREIGN KEY REFERENCES Voo(IDVOO),
	CONSTRAINT PK_Passagem PRIMARY KEY(IDPASSAGEM));
	   	   
CREATE TABLE Venda(
	IDVENDA INT IDENTITY CONSTRAINT PK_Venda PRIMARY KEY,
	DataVenda DATE NOT NULL,
	ValorTotalVendas DECIMAL(6,2));

CREATE TABLE VendaPassagem(
	IDPASSAGEM INT CONSTRAINT FK_VendaPassagem FOREIGN KEY REFERENCES Passagem(IDPASSAGEM),
	IDVENDA INT CONSTRAINT FK_Venda FOREIGN KEY REFERENCES Venda(IDVENDA),
	IDITEM INT IDENTITY,
	ValorUnitarioAtual DECIMAL(7,2),
	PRIMARY KEY(IDPASSAGEM, IDVENDA, IDITEM));

CREATE TABLE Passageiro(
	CPF VARCHAR(11) NOT NULL PRIMARY KEY,	
	Nome VARCHAR(50) NOT NULL,
	Nascimento DATE NOT NULL,
	Sexo VARCHAR(10),
	UltimaCompra DATE NOT NULL,
	Situacao VARCHAR(10));

INSERT Passageiro VALUES('1', '2022/09/28', 'Giovani', '1994/03/24', 'Masculino', 'ATIVO');

SELECT * FROM Passageiro;

DELETE FROM Passageiro;


CREATE TABLE Restricao(
	CNPJ VARCHAR(14) UNIQUE,
	CPF VARCHAR(11) UNIQUE);

SELECT * FROM CompanhiaAerea;

SELECT companhiaaerea.RazaoSocial, companhiaaerea.CNPJ,
	companhiaaerea.DataAbertura, companhiaaerea.UltimoVoo,
	companhiaaerea.DataCadastro, companhiaaerea.Situacao FROM CompanhiaAerea;

SELECT Capacidade FROM AeronavePossueVoo
JOIN Passagem ON passagem.IDVOO = aeronavepossuevoo.IDVOO 
WHERE passagem.IDPASSAGEM = '1';

SELECT aeronavepossuev

SELECT * FROM CompanhiaAerea;

SELECT * FROM Aeronave;

SELECT * FROM Voo;

SELECT * FROM AeronavePossueVoo;

SELECT * FROM CompanhiaPossueAeronave;

SELECT * FROM Passagem;

SELECT * FROM VendaPassagem;

SELECT aeronavepossuevoo.INSCRICAO, passagem.IDVOO, passagem.IDPASSAGEM, vendapassagem.IDITEM,
	vendapassagem.ValorUnitarioAtual FROM VendaPassagem
	JOIN Passagem ON passagem.IDPASSAGEM = vendapassagem.IDPASSAGEM
	JOIN AeronavePossueVoo ON aeronavepossuevoo.IDVOO = passagem.IDVOO
	WHERE vendapassagem.IDITEM = '17';



SELECT passagem.IDVOO, passagem.IDPASSAGEM, passagem.ValorPassagem, passagem.DataUltimaOperacao, passagem.Situacao FROM Passagem;

SELECT aeronavepossuevoo.INSCRICAO, aeronavepossuevoo.IDVOO, passagem.IDPASSAGEM, aeronavepossuevoo.AcentosOcupados, passagem.DataUltimaOperacao, 
	passagem.ValorPassagem, passagem.Situacao FROM Passagem JOIN AeronavePossueVoo 
	ON aeronavepossuevoo.IDVOO = passagem.IDVOO 



INSERT AeronavePossueVoo(INSCRICAO, IDVOO, Capacidade, AcentosOcupados) VALUES('PTAAA', 1, 150, 1);

UPDATE Voo SET Destino = 'ALC' WHERE IDVOO = 1;

DELETE FROM AeronavePossueVoo WHERE IDVOO = 1 AND INSCRICAO = 'PTAAA';

CREATE TRIGGER Conversao 
ON Passagem
