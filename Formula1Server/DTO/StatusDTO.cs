using Formula1Server.Models;

namespace Formula1Server.DTO
{
    public class StatusDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public StatusDTO() { }
        public StatusDTO(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public StatusDTO(Status s)
        {
            this.Id = s.Id;
            this.Name = s.Name;
        }
    }
}
