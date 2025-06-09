Use master
Go

-- Create a login for the admin user
CREATE LOGIN  [AdminLogin] WITH PASSWORD = 'rokazyo123';
Go

-- Create a user in the DB database for the login
CREATE USER [AdminLogin] FOR LOGIN [AdminLogin];
Go

--so user can restore the DB!
ALTER SERVER ROLE sysadmin ADD MEMBER [AdminLogin];
Go

Create Database Dbsql
go



Use master
Go


USE master;
ALTER DATABASE Dbsql SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE Dbsql FROM DISK = 'C:\Users\user\source\repos\WizardNuggie\Formula1Server\Formula1Server\wwwroot\..\DbScripts\backup.bak' WITH REPLACE, --להחליף את זה לנתיב של קובץ הגיבוי
    MOVE 'Dbsql' TO 'C:\Users\user\Dbsql.mdf',   --להחליף לנתיב שנמצא על המחשב שלך
    MOVE 'Dbsql_log' TO 'C:\Users\user\Dbsql.ldf';  
ALTER DATABASE Dbsql SET MULTI_USER;