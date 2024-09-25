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

-- creation of the user types table --
CREATE TABLE UserTypes (
	[UserTypeId] INT IDENTITY(1,1) PRIMARY KEY, -- PK
	[UserTypeName] NVARCHAR (250) -- Name of the user type
)

-- creation of the users table --
CREATE TABLE Users (
	[UserId] INT IDENTITY(1,1) PRIMARY KEY, -- PK user ID
	[Email] NVARCHAR(250), -- user email
	[Username] NVARCHAR(20), -- username
	[Name] NVARCHAR(250), -- name of the user
	[Password] NVARCHAR(20), -- password
	[IsAdmin] BIT, -- defines whether the user is an admin or not
	[FavDriver] NVARCHAR(250), -- the user's favoite driver
	[FavConstructor] NVARCHAR(250), -- the user's favorite constructor
	[Birthday] DATE,
	[IsRecievingNot] BIT, -- whether the user wants to recieve notifications or not
	[UserTypeId] INT -- the id of the user type, FK from UserTypes table
		FOREIGN KEY(UserTypeId) REFERENCES UserTypes(UserTypeId)
)

-- creation of the aricles table --
CREATE TABLE Articles (
	[ArticleId] INT IDENTITY(1,1) PRIMARY KEY, -- PK
	[Title] NVARCHAR(250), -- the title of the article
	[Text] NVARCHAR(4000), -- the content of the article
	[IsBreaking] BIT, -- is the article breaking news
)

-- creation of the writers-articles table --
CREATE TABLE WritersArticles (
	[ArticleId] INT -- the id of the article, FK from articles
		FOREIGN KEY(ArticleId) REFERENCES Articles(ArticleId),
	[WriterId] INT -- the id of the writer, FK from writers
		FOREIGN KEY(WriterId) REFERENCES Users(UserId),
	--CONSTRAINT PK_WritersArticles
	PRIMARY KEY(ArticleId,WriterId) -- constraints both the article id and the writer id to the PK
)

-- creation of the subjects table --
CREATE TABLE Subjects (
	[SubjectId] INT IDENTITY(1,1) PRIMARY KEY, -- PK
	[SubjectName] NVARCHAR(250), -- the name of the subject
)

-- creation of the subjects-articles table --
CREATE TABLE ArticlesSubjects (
	[ArticleId] INT -- the id of the article, FK from articles
		FOREIGN KEY(ArticleId) REFERENCES Articles(ArticleId),
	[SubjectId] INT -- the id of the subject, FK from subjects
		FOREIGN KEY(SubjectId) REFERENCES Subjects(SubjectId),
	--CONSTRAINT PK_ArticlesSubjects
	PRIMARY KEY(ArticleId,SubjectId) -- constraints both the article id and the subject id to the PK
)



INSERT INTO UserTypes VALUES('Driver')
INSERT INTO UserTypes VALUES('Constructor')
INSERT INTO UserTypes VALUES('Writer')
INSERT INTO UserTypes VALUES('Regular')
INSERT INTO Users VALUES('yo@gmail.com','yoyo','yo','1234',0,'Fernando Alonso','Aston Martin','2007/06/10',1,4)

SELECT * FROM Users