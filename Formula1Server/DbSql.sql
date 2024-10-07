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
	[Username] NVARCHAR(20) NOT NULL, -- username
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

-- creation of the aricles table --
CREATE TABLE Articles (
	[ArticleId] INT IDENTITY(1,1) PRIMARY KEY, -- PK
	[Title] NVARCHAR(250) NOT NULL, -- the title of the article
	[Text] NVARCHAR(4000) NOT NULL, -- the content of the article
	[IsBreaking] BIT NOT NULL, -- is the article breaking news
)
Go

-- creation of the writers-articles table --
CREATE TABLE WritersArticles (
	[ArticleId] INT NOT NULL -- the id of the article, FK from articles
		FOREIGN KEY(ArticleId) REFERENCES Articles(ArticleId),
	[WriterId] INT NOT NULL -- the id of the writer, FK from writers
		FOREIGN KEY(WriterId) REFERENCES Users(UserId),
	--CONSTRAINT PK_WritersArticles
	PRIMARY KEY(ArticleId,WriterId) -- constraints both the article id and the writer id to the PK
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


INSERT INTO UserTypes VALUES('Driver')
Go
INSERT INTO UserTypes VALUES('Constructor')
Go
INSERT INTO UserTypes VALUES('Writer')
Go
INSERT INTO UserTypes VALUES('Regular')
Go
INSERT INTO Users VALUES('yo@gmail.com','yoyo','yo','1234',0,'Fernando Alonso','Aston Martin','2007/06/10',1,4)
Go

SELECT * FROM Users
Go

--scaffold - DbContext \"Server = (localdb)\MSSQLLocalDB;Initial Catalog=DbSql;User ID=AdminLogin;Password=rokazyo123;\" Microsoft.EntityFrameworkCore.SqlServer - OutPutDir Models - Context F1DBContext - DataAnnotations - force