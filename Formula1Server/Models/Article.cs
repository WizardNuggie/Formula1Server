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
    public string Title { get; set; }

    [StringLength(4000)]
    public string Text { get; set; }

    public bool IsBreaking { get; set; }

    [ForeignKey("ArticleId")]
    [InverseProperty("Articles")]
    public virtual ICollection<Subject> Subjects { get; set; } = [];

    [ForeignKey("ArticleId")]
    [InverseProperty("Articles")]
    public virtual ICollection<User> Writers { get; set; } = [];
}
