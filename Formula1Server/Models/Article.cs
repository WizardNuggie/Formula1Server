using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Formula1Server.Models;

public partial class Article
{
    [Key]
    public int ArticleId { get; set; }

    [StringLength(250)]
    public string Title { get; set; } = null!;

    [StringLength(4000)]
    public string Text { get; set; } = null!;

    public bool IsBreaking { get; set; }

    public int WriterId { get; set; }

    [ForeignKey("WriterId")]
    [InverseProperty("Articles")]
    public virtual User Writer { get; set; } = null!;

    [ForeignKey("ArticleId")]
    [InverseProperty("Articles")]
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
