using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Formula1Server.Models;

public partial class Subject
{
    [Key]
    public int SubjectId { get; set; }

    [StringLength(250)]
    public string SubjectName { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("Subjects")]
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
