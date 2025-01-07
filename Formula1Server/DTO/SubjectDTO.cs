namespace Formula1Server.DTO
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SubjectDTO() { }
        public SubjectDTO(Models.Subject s)
        {
            this.Id = s.SubjectId;
            this.Name = s.SubjectName;
        }
    }
}
