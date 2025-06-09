Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'Dbsql')
BEGIN
    DROP DATABASE Dbsql;
END
Go
Create Database Dbsql
Go
Use Dbsql
Go

-- Create a login for the admin user
CREATE LOGIN [AdminLogin] WITH PASSWORD = 'rokazyo123';
Go

-- Create a user in the DbSql database for the login
CREATE USER [AdminLogin] FOR LOGIN [AdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [AdminLogin];
Go

-- creation of the user types table --
CREATE TABLE UserTypes (
	[UserTypeId] INT IDENTITY(1,1) PRIMARY KEY, -- PK
	[UserTypeName] NVARCHAR (250) NOT NULL -- Name of the user type
)
Go

-- creation of the users table --
CREATE TABLE Users (
	[UserId] INT IDENTITY(1,1) PRIMARY KEY, -- PK user ID
	[Email] NVARCHAR(250) NOT NULL, -- user email
	[Username] NVARCHAR(20) UNIQUE NOT NULL, -- username
	[Name] NVARCHAR(250) NOT NULL, -- name of the user
	[Password] NVARCHAR(20) NOT NULL, -- password
	[IsAdmin] BIT NOT NULL, -- defines whether the user is an admin or not
	[FavDriver] NVARCHAR(250) NOT NULL, -- the user's favoite driver
	[FavConstructor] NVARCHAR(250) NOT NULL, -- the user's favorite constructor
	[Birthday] DATE NOT NULL,
	[IsRecievingNot] BIT NOT NULL, -- whether the user wants to recieve notifications or not
	[UserTypeId] INT NOT NULL -- the id of the user type, FK from UserTypes table
		FOREIGN KEY(UserTypeId) REFERENCES UserTypes(UserTypeId)
)
Go

-- creation of the statuses table --
CREATE TABLE Statuses (
	[Id] INT IDENTITY(1,1) PRIMARY KEY, -- PK
	[Name] NVARCHAR(250) NOT NULL
)
Go

-- creation of the aricles table --
CREATE TABLE Articles (
	[ArticleId] INT IDENTITY(1,1) PRIMARY KEY, -- PK
	[Title] NVARCHAR(250) NOT NULL, -- the title of the article
	[Text] NVARCHAR(4000) NOT NULL, -- the content of the article
	[IsBreaking] BIT NOT NULL, -- is the article breaking news
	[WriterId] INT NOT NULL -- the id of the writer, FK from writers
		FOREIGN KEY(WriterId) REFERENCES Users(UserId),
	[StatusId] INT NOT NULL -- the id of the status, FK from statuses
		FOREIGN KEY(StatusId) REFERENCES Statuses(Id)
)
Go

-- creation of the subjects table --
CREATE TABLE Subjects (
	[SubjectId] INT IDENTITY(1,1) PRIMARY KEY, -- PK
	[SubjectName] NVARCHAR(250) NOT NULL, -- the name of the subject
)
Go

-- creation of the subjects-articles table --
CREATE TABLE ArticlesSubjects (
	[ArticleId] INT NOT NULL -- the id of the article, FK from articles
		FOREIGN KEY(ArticleId) REFERENCES Articles(ArticleId),
	[SubjectId] INT NOT NULL -- the id of the subject, FK from subjects
		FOREIGN KEY(SubjectId) REFERENCES Subjects(SubjectId),
	--CONSTRAINT PK_ArticlesSubjects
	PRIMARY KEY(ArticleId,SubjectId) -- constraints both the article id and the subject id to the PK
)
Go


INSERT INTO UserTypes VALUES('Writer')
Go
INSERT INTO UserTypes VALUES('Regular')
Go

INSERT INTO Users VALUES('regular@gmail.com','regular1','Ro','regular123',0,'Fernando Alonso','Aston Martin','2007/06/10',1, 2)
Go
INSERT INTO Users VALUES('writer@gmail.com', 'writer1', 'Shahar Dabush', 'writer123', 0, 'Fernando Alonso','Aston Martin','2007/09/29', 1, 1)
Go
INSERT INTO Users VALUES('admin@gmail.com', 'admin1', 'Ofek Rom', 'admin123', 1, 'Fernando Alonso','Aston Martin','2007/03/12', 0, 2)
Go

INSERT INTO Statuses VALUES('Approved')
Go
INSERT INTO Statuses VALUES('Pending')
Go
INSERT INTO Statuses VALUES('Declined')
Go

INSERT INTO Articles VALUES('Test', 'This is the test article', 1, 2, 1)
Go
INSERT INTO Articles VALUES('Test', 'This is the second test article', 0, 2, 1)
Go


INSERT INTO Subjects VALUES('Drivers')
Go
INSERT INTO Subjects VALUES('Constructors')
Go
INSERT INTO Subjects VALUES('Races')
Go
INSERT INTO Subjects VALUES('F2')
Go
INSERT INTO Subjects VALUES('Technical')
Go
INSERT INTO Subjects VALUES('General')
Go

INSERT INTO ArticlesSubjects VALUES(1, 1)
Go
INSERT INTO ArticlesSubjects VALUES(1, 3)
Go
INSERT INTO ArticlesSubjects VALUES(1, 4)
Go
INSERT INTO ArticlesSubjects VALUES(2, 2)
Go

SELECT * FROM UserTypes
Go
SELECT * FROM Users
Go
SELECT * FROM Statuses
Go
SELECT * FROM Articles
Go
SELECT * FROM Subjects
Go
SELECT * FROM ArticlesSubjects
Go

--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=DbSql;User ID=AdminLogin;Password=rokazyo123;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context F1DBContext -DataAnnotations -force
--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=DbSql;User ID=AdminLogin;Password=rokazyo123;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context F1DBContext -DataAnnotations -force