namespace Formula1Server.DTO
{
    public class SubjectDTO
    {
        public string Name { get; set; }
        public SubjectDTO() { }
        public SubjectDTO(Models.Subject s)
        {
            this.Name = s.SubjectName;
        }
    }
}
