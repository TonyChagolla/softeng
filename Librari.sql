USE master;
GO

IF EXISTS(SELECT * FROM sysdatabases WHERE name='DBLibrary')
BEGIN 
	RAISERROR('Dropping existing DBLibrary database...', 0, 1)
	DROP DATABASE DBLibrary;
END
GO

CREATE DATABASE DBLibrary;
GO

USE DBLibrary;
GO

IF db_name() <>'DBLibrary'
BEGIN 
	RAISERROR('Use database failed...', 22, 127) WITH LOG
	DROP DATABASE DBLibrary;
END
GO

CREATE TABLE employee
(
	employee_id INT NOT NULL PRIMARY KEY,
	first_name VARCHAR(15) NOT NULL,
	last_name VARCHAR(15) NOT NULL,
	e_user VARCHAR(15) NOT NULL,
	e_password VARCHAR(15) NOT NULL
);
INSERT INTO employee  VALUES(100, 'Tony', 'Chagolla', 'Master0', 'newpassword0');
INSERT INTO employee  VALUES(101, 'Juan', 'Perez', 'Master1', 'newpassword1');
INSERT INTO employee  VALUES(102, 'Pedro', 'Rodriguez', 'Master2', 'newpassword2');
INSERT INTO employee  VALUES(103, 'Miguel', 'Hernandez', 'Master3', 'newpassword3');
INSERT INTO employee  VALUES(104, 'Antonio', 'Hernandez', 'Master4', 'newpassword4');
INSERT INTO employee  VALUES(105, 'Jose', 'Garcia', 'Master5', 'newpassword5');
INSERT INTO employee  VALUES(106, 'Carlos', 'Bustos', 'Master6', 'newpassword6');

GO

CREATE TABLE cliente
(
	cliente_id INT NOT NULL PRIMARY KEY IDENTITY,
	first_name VARCHAR(15) NOT NULL,
	last_name VARCHAR(15) NOT NULL,
	client_address VARCHAR (50) NOT NULL,
	date_inscription SMALLDATETIME NOT NULL
);
INSERT INTO cliente (first_name, last_name, client_address, date_inscription) VALUES('Mary', 'Arroyo', 'Lazaro Cardenas 120, Salamanca, Gto.', GETDATE());
INSERT INTO cliente (first_name, last_name, client_address, date_inscription) VALUES('Luis', 'Gonzales', 'Lazaro Cardenas 120, Salamanca, Gto.', GETDATE());
INSERT INTO cliente (first_name, last_name, client_address, date_inscription) VALUES('Pablo', 'Ramirez', 'Lazaro Cardenas 120, Salamanca, Gto.', GETDATE());
INSERT INTO cliente (first_name, last_name, client_address, date_inscription) VALUES('Jesus', 'Perez', 'Lazaro Cardenas 120, Salamanca, Gto.', GETDATE());
INSERT INTO cliente (first_name, last_name, client_address, date_inscription) VALUES('Raul', 'Jimenez', 'Lazaro Cardenas 120, Salamanca, Gto.', GETDATE());
INSERT INTO cliente (first_name, last_name, client_address, date_inscription) VALUES('Aaron', 'Juarez', 'Lazaro Cardenas 120, Salamanca, Gto.', GETDATE());
INSERT INTO cliente (first_name, last_name, client_address, date_inscription) VALUES('Eduardo', 'Martinez', 'Lazaro Cardenas 120, Salamanca, Gto.', GETDATE());
GO



CREATE TABLE books
(
	book_id INT NOT NULL PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	author VARCHAR(50) NOT NULL,
	pages INT NOT NULL,
	stock INT NOT NULL
);
INSERT INTO books VALUES(1, 'La metamorfosis', 'Franz Kafka', 210, 12);
INSERT INTO books VALUES(2, 'Fahrenheit 451', 'Ray Bradbury', 210, 12);
INSERT INTO books VALUES(3, 'Crimen y castigo', 'Fedor Dostoievski', 210, 12);
INSERT INTO books VALUES(4, '100 años de Soledad', 'Gabriel García Márquez', 210, 12);
INSERT INTO books VALUES(5, 'Las almas muertas', 'Nicolai Gogol', 210, 12);
INSERT INTO books VALUES(6, 'La comedia humana', 'Honoré de Balzac', 210, 12);
INSERT INTO books VALUES(7, 'Trópico de cáncer', 'Henry Miller', 210, 12);
INSERT INTO books VALUES(8, 'La divina comedia', 'Dante Alighieri', 210, 12);
INSERT INTO books VALUES(9, 'Un mundo feliz', 'Aldous Huxley', 210, 12);
INSERT INTO books VALUES(10, 'Frankenstein', 'Mary Shelley', 210, 12);

GO

CREATE TABLE book_borrowed
(
	borrow_id INT NOT NULL PRIMARY KEY IDENTITY,
	book_id INT NOT NULL REFERENCES books(book_id) ON DELETE CASCADE,
	cliente_id INT NOT NULL REFERENCES cliente(cliente_id) ON DELETE CASCADE,
	employee_id INT NOT NULL REFERENCES employee(employee_id) ON DELETE CASCADE,
	date_b SMALLDATETIME NOT NULL,
	date_r SMALLDATETIME
	
);
INSERT INTO book_borrowed (book_id, cliente_id, employee_id, date_b) VALUES(1, 1, 100, GETDATE());
GO

CREATE VIEW client_books AS
SELECT bb.borrow_id AS b_id, c.first_name + ' ' + c.last_name AS c_name, b.name AS b_name, bb.date_b AS b_date, bb.date_r as r_date, c.cliente_id as c_id
FROM book_borrowed bb, books b, cliente c
WHERE bb.book_id = b.book_id and c.cliente_id = bb.cliente_id
GO

CREATE VIEW vw_client AS
SELECT cliente_id, first_name + ' ' + last_name AS name
FROM cliente
GO


