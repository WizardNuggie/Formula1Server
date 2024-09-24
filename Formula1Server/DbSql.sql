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
	[UserTypeId] INT PRIMARY KEY, -- PK
	[UserTypeName] NVARCHAR (250) -- Name of the user type
)

-- creation of the users table --
CREATE TABLE Users (
	[Email] NVARCHAR (250) PRIMARY KEY, -- PK, user email
	[UserId] INT IDENTITY(1,1), -- user ID
	[Username] NVARCHAR(20), -- username
	[Name] NVARCHAR (250), -- name of the user
	[Password] NVARCHAR (20), -- password
	[IsAdmin] BOOL, -- defines whether the user is an admin or not
	[UserTypeId] INT, -- the id of the user type, FK from UserTypes table
	FOREIGN KEY (UserTypeId) REFERENCES UserTypes(UserTypeId)
)

-- creation of the aricles table --
CREATE TABLE Articles (
	[ArticleId] INT IDENTITY (1,1) PRIMARY KEY, -- PK
	
)