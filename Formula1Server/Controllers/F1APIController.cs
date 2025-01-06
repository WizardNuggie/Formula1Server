using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Formula1Server.Models;

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
                    UserTypeId = 4,
                };
                modelsUser.UserType = this.context.UserTypes.Where(t => t.UserTypeId == modelsUser.UserTypeId).FirstOrDefault();
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
        [HttpGet("GetNews")]
        public IActionResult GetNews()
        {
            try
            {
                List<DTO.ArticleDTO> dtoNews = new();
                List<Article> modelNews = context.Articles.ToList();
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
                    dtoSubjects.Add(new DTO.SubjectDTO() { Name = s.SubjectName});
                }
                return Ok(dtoSubjects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
