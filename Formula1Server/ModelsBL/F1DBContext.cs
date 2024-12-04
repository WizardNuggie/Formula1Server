using Formula1Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Formula1Server.Models
{
    public partial class F1DBContext : DbContext
    {
        public User GetUser(string username)
        {
            return this.Users.Where(u => u.Username == username)
                .Include(u => u.Articles)
                .FirstOrDefault();
        }
    }
}
