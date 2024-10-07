using System.ComponentModel.DataAnnotations;

namespace Formula1Server.DTO
{
    public class ArticleDTO
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsBreaking { get; set; }

        public ArticleDTO(string title, string text, bool isBreaking)
        {
            this.Title = title;
            this.Text = text;
            this.IsBreaking = isBreaking;
        }
        public ArticleDTO() { }
    }
}
