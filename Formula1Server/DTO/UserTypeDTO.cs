using Formula1Server.Models;

namespace Formula1Server.DTO
{
    public class UserTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserTypeDTO(int id, string name)
        {
            this.Name = name;
            this.Id = id;
        }
        public UserTypeDTO(UserType u)
        {
            this.Id = u.UserTypeId;
            this.Name = u.UserTypeName;
        }
    }
}
