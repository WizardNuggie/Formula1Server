﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Formula1Server.Models;
using Formula1Server.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Formula1Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class F1APIController : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private F1DBContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public F1APIController(F1DBContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }
        #region Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] DTO.LoginDTO loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User? modelsUser = context.GetUser(loginDto.Username);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsUser == null || modelsUser.Password != loginDto.Password)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInUser", modelsUser.Username);

                DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] DTO.UserDTO userDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User modelsUser = new()
                {
                    Email = userDto.Email,
                    Username = userDto.Username,
                    Name = userDto.Name,
                    Password = userDto.Password,
                    FavDriver = userDto.FavDriver,
                    FavConstructor = userDto.FavConstructor,
                    Birthday = userDto.Birthday,
                    UserTypeId = 2,
                };
                modelsUser.UserType = this.context.UserTypes.Where(t => t.UserTypeId == modelsUser.UserTypeId).FirstOrDefault();
                if (this.context.Users.Where(u => u.Username == modelsUser.Username).Any())
                {
                    return Conflict("Username already exists");
                }
                context.Users.Add(modelsUser);
                context.SaveChanges();

                //User was added!
                DTO.UserDTO dtoUser = new(modelsUser);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region GetNews
        [HttpGet("GetAllNews")]
        public IActionResult GetAllNews()
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (!loggedInUser.IsAdmin)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion
                List<DTO.ArticleDTO> dtoNews = new();
                List<Article> modelNews = context.Articles.Include(a => a.Subjects).ToList();
                foreach (Article a in modelNews)
                {
                    dtoNews.Add(new DTO.ArticleDTO(a));
                }
                return Ok(dtoNews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetNews")]
        public IActionResult GetNews()
        {
            try
            {
                List<DTO.ArticleDTO> dtoNews = new();
                List<Article> modelNews = context.Articles.Where(a => a.StatusId == 1).Include(a => a.Subjects).ToList();
                foreach (Article a in modelNews)
                {
                    dtoNews.Add(new DTO.ArticleDTO(a));
                }
                return Ok(dtoNews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNewsBySubject")]
        public IActionResult GetNewsBySubjects(int subjectId)
        {
            try
            {
                List<Subject> subjects = context.Subjects.Include(s => s.Articles).ToList();
                Subject subject = subjects.Where(x =>  x.SubjectId == subjectId).FirstOrDefault();
                List<DTO.ArticleDTO> dtoNews = new();
                List<Article> modelNews = subject.Articles.Where(a => a.StatusId == 1).ToList();
                foreach (Article a in modelNews)
                {
                    dtoNews.Add(new DTO.ArticleDTO(a));
                }
                return Ok(dtoNews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNewsByUser")]
        public IActionResult GetNewsByUser(int userId)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                #endregion
                List<DTO.ArticleDTO> dtoNews = new();
                List<Article> modelNews = context.Articles.Where(a => a.Writer.UserId == userId).ToList();
                foreach (Article a in modelNews)
                {
                    dtoNews.Add(new DTO.ArticleDTO(a));
                }
                return Ok(dtoNews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetSubjects
        [HttpGet("GetSubjects")]
        public IActionResult GetSubjects()
        {
            try
            {
                List<DTO.SubjectDTO> dtoSubjects = new();
                List<Subject> modelSubjects = context.Subjects.ToList();
                foreach (Subject s in modelSubjects)
                {
                    dtoSubjects.Add(new DTO.SubjectDTO(s));
                }
                return Ok(dtoSubjects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetUsers

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (!loggedInUser.IsAdmin)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion
                List<DTO.UserDTO> dtoUsers = new();
                List<User> modelUsers = context.Users.Include(u => u.Articles).ToList();
                foreach (User u in modelUsers)
                {
                    dtoUsers.Add(new DTO.UserDTO(u));
                }
                return Ok(dtoUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //comment
        [HttpGet("GetWriterByArticle")]
        public IActionResult GetWriterByArticle(int articleId)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                #endregion
                List<Article> articles = context.Articles.Include(a => a.Writer).ToList();
                User u = articles.Where(a => a.ArticleId == articleId).FirstOrDefault().Writer;
                UserDTO dtoU = new UserDTO(u);
                return Ok(dtoU);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsersByUT")]
        public IActionResult GetUsersByUT(int userTypeId)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (!loggedInUser.IsAdmin)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion
                List<UserType> uts = context.UserTypes.Include(u => u.Users).ThenInclude(us => us.Articles).ToList();
                UserType ut = uts.Where(x => x.UserTypeId == userTypeId).FirstOrDefault();
                List<DTO.UserDTO> dtoUsers = new();
                List<User> modelUsers = ut.Users.ToList();
                foreach (User u in modelUsers)
                {
                    dtoUsers.Add(new DTO.UserDTO(u));
                }
                return Ok(dtoUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetUserTypes

        [HttpGet("GetUserTypes")]
        public IActionResult GetUserTypes()
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (!loggedInUser.IsAdmin)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion
                List<DTO.UserTypeDTO> dtoUserTypes = new();
                List<UserType> modelUserTypes = context.UserTypes.ToList();
                foreach (UserType u in modelUserTypes)
                {
                    dtoUserTypes.Add(new DTO.UserTypeDTO(u));
                }
                return Ok(dtoUserTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Upload Photo

        [HttpPost("UploadArticleImage")]
        public async Task<IActionResult> UploadArticleImage(IFormFile file, [FromQuery] int id)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (loggedInUser.UserTypeId != 1 && !loggedInUser.IsAdmin)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion
                Models.Article? article = context.GetArticle(id);
                context.ChangeTracker.Clear();

                if (article == null)
                {
                    return Unauthorized("Article was not found in the database");
                }

                //Read all files sent
                long imagesSize = 0;

                if (file.Length > 0)
                {
                    //Check the file extention!
                    string[] allowedExtentions = { ".png" };
                    string extention = "";
                    if (file.FileName.LastIndexOf(".") > 0)
                    {
                        extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                    }
                    if (!allowedExtentions.Where(e => e == extention).Any())
                    {
                        //Extention is not supported
                        return BadRequest("Image format has to be .png");
                    }

                    //Build path in the web root (better to a specific folder under the web root
                    string filePath = $"{this.webHostEnvironment.WebRootPath}\\articles\\{id}{extention}";

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);

                        if (IsImage(stream))
                        {
                            imagesSize += stream.Length;
                        }
                        else
                        {
                            //Delete the file if it is not supported!
                            System.IO.File.Delete(filePath);
                            return BadRequest("Selected file is not an image");
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private static bool IsImage(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Upload article

        [HttpPost("UploadArticle")]
        public async Task<IActionResult> UploadArticle([FromBody] DTO.ArticleDTO articleDto)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (loggedInUser.UserTypeId != 1)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion

                Models.Article modelsArticle = new()
                {
                    Title = articleDto.Title,
                    Text = articleDto.Text,
                    IsBreaking = articleDto.IsBreaking,
                    WriterId = loggedInUser.UserId,
                    StatusId = 2,
                    Subjects = new List<Subject>()
                };
                foreach (SubjectDTO s in articleDto.Subjects)
                {
                    modelsArticle.Subjects.Add(context.Subjects.Where(x => x.SubjectId == s.Id).FirstOrDefault());
                }
                context.Articles.Add(modelsArticle);
                context.SaveChanges();

                //User was added!
                DTO.ArticleDTO dtoArticle = new(modelsArticle);
                return Ok(dtoArticle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Photo Path

        private string GetProfileImageVirtualPath(int userId)
        {
            string virtualPath = $"/profileImages/{userId}";
            string path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/profileImages/default.png";
                }
            }

            return virtualPath;
        }

        #endregion

        #region Remove User
        [HttpGet("RemoveUser")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (!loggedInUser.IsAdmin)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion
                User u = context.Users.Where(x => x.UserId == userId).FirstOrDefault();
                if (u != null)
                {
                    context.Users.Remove(u);
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("User was not found in database");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Manage Articles
        [HttpGet("ApproveArticle")]
        public async Task<IActionResult> ApproveArticle(int articleId)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (!loggedInUser.IsAdmin)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion
                Article a = context.Articles.Where(x => x.ArticleId == articleId).FirstOrDefault();
                if (a != null)
                {
                    a.StatusId = 1;
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("Article was not found in database");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DeclineArticle")]
        public async Task<IActionResult> DeclineArticle(int articleId)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                if (!loggedInUser.IsAdmin)
                {
                    return Unauthorized("You do not have the required permissions");
                }
                #endregion
                Article a = context.Articles.Where(x => x.ArticleId == articleId).FirstOrDefault();
                if (a != null)
                {
                    a.StatusId = 3;
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("Article was not found in database");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Edit User
        [HttpPost("EditUserDetails")]
        public async Task<IActionResult> EditUserDetails([FromBody]UserDTO user)
        {
            try
            {
                #region security check
                string? userName = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in!");
                }
                User? loggedInUser = context.GetUser(userName);
                #endregion
                if (this.context.Users.Where(u => u.Username == user.Username).Any())
                {
                    return Conflict("Username already exists");
                }
                User u = context.Users.Where(x => x.UserId == user.Id).FirstOrDefault();
                if (u != null)
                {
                    u.Username = user.Username;
                    u.Email = user.Email;
                    u.Name = user.Name;
                    u.Password = user.Password;
                    u.FavDriver = user.FavDriver;
                    u.FavConstructor = user.FavConstructor;
                    u.Birthday = user.Birthday;
                    u.UserTypeId = user.UserTypeId;
                    u.IsAdmin = user.IsAdmin;
                    context.SaveChanges();
                    return Ok(user);
                }
                else
                {
                    return BadRequest("User was not found in database");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Backup / Restore
        [HttpGet("Backup")]
        public async Task<IActionResult> Backup()
        {
            string path = $"{this.webHostEnvironment.WebRootPath}\\..\\DbScripts\\backup.bak";

            bool success = await BackupDatabaseAsync(path);
            if (success)
            {
                return Ok("Backup was successful");
            }
            else
            {
                return BadRequest("Backup failed");
            }
        }

        [HttpGet("Restore")]
        public async Task<IActionResult> Restore()
        {
            string path = $"{this.webHostEnvironment.WebRootPath}\\..\\DbScripts\\backup.bak";

            bool success = await RestoreDatabaseAsync(path);
            if (success)
            {
                return Ok("Restore was successful");
            }
            else
            {
                return BadRequest("Restore failed");
            }
        }
        //this function backup the database to a specified path
        private async Task<bool> BackupDatabaseAsync(string path)
        {
            try
            {

                //Get the connection string
                string? connectionString = context.Database.GetConnectionString();
                //Get the database name
                string databaseName = context.Database.GetDbConnection().Database;
                //Build the backup command
                string command = $"BACKUP DATABASE {databaseName} TO DISK = '{path}'";
                //Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Open the connection
                    await connection.OpenAsync();
                    //Create a command
                    using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                    {
                        //Execute the command
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //THis function restore the database from a backup in a certain path
        private async Task<bool> RestoreDatabaseAsync(string path)
        {
            try
            {
                //Get the connection string
                string? connectionString = context.Database.GetConnectionString();
                //Get the database name
                string databaseName = context.Database.GetDbConnection().Database;
                //Build the restore command
                string command = $@"
                USE master;
                ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE {databaseName} FROM DISK = '{path}' WITH REPLACE;
                ALTER DATABASE {databaseName} SET MULTI_USER;";

                //Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Open the connection
                    await connection.OpenAsync();
                    //Create a command
                    using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                    {
                        //Execute the command
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion
    }
}
