namespace Formula1Server.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string FavDriver { get; set; }
        public string FavConstructor { get; set; }
        public DateOnly Birthday { get; set; }
        public bool IsAdmin { get; set; }
        public int UserTypeId { get; set; }
        public List<ArticleDTO> Articles { get; set; }

        public UserDTO(int id, string email, string username, string name, string password, string driver, string @const, DateOnly bd, int id2, bool isAdmin)
        {
            this.Id = id;
            this.Email = email;
            this.Username = username;
            this.Name = name;
            this.Password = password;
            this.FavDriver = driver;
            this.FavConstructor = @const;
            this.Birthday = bd;
            this.UserTypeId = id2;
            IsAdmin = isAdmin;
        }
        public UserDTO(Models.User u)
        {
            this.Id = u.UserId;
            this.Email = u.Email;
            this.Username = u.Username;
            this.Name = u.Name;
            this.Password = u.Password;
            this.FavDriver = u.FavDriver;
            this.FavConstructor = u.FavConstructor;
            this.Birthday = u.Birthday;
            this.UserTypeId = u.UserTypeId;
            this.IsAdmin = u.IsAdmin;
            this.Articles = new();
            foreach (Models.Article a in u.Articles)
            {
                this.Articles.Add(new ArticleDTO(a));
            }
        }
        public UserDTO() { }
    }
}
