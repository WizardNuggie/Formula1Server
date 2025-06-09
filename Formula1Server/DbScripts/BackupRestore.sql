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

Create Database AppServer_DB
go



Use master
Go


USE master;
ALTER DATABASE AppServer_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE AppServer_DB FROM DISK = 'C:\Users\user\source\repos\WizardNuggie\Formula1Server\Formula1Server\wwwroot\..\DbScripts\backup.bak' WITH REPLACE, --להחליף את זה לנתיב של קובץ הגיבוי
    MOVE 'AppServer_DB' TO 'C:\Users\user\AppServer_DB.mdf',   --להחליף לנתיב שנמצא על המחשב שלך
    MOVE 'AppServer_DB_log' TO 'C:\Users\user\AppServer_DB_log.ldf';  
ALTER DATABASE AppServer_DB SET MULTI_USER;