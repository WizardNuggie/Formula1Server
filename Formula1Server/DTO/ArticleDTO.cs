using System.ComponentModel.DataAnnotations;

namespace Formula1Server.DTO
{
    public class ArticleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsBreaking { get; set; }
        public List<SubjectDTO> Subjects { get; set; }

        public ArticleDTO(int id, string title, string text, bool isBreaking, List<SubjectDTO> subjects)
        {
            this.Id = id;
            this.Title = title;
            this.Text = text;
            this.IsBreaking = isBreaking;
            this.Subjects = subjects;
        }
        public ArticleDTO() { }
        public ArticleDTO(Models.Article a)
        {
            this.Id = a.ArticleId;
            this.Title = a.Title;
            this.Text = a.Text;
            this.IsBreaking = a.IsBreaking;
            this.Subjects = new();
            foreach (Models.Subject s in a.Subjects)
            {
                this.Subjects.Add(new SubjectDTO(s));
            }
        }
    }
}
