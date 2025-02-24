using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Formula1Server.Models;

public partial class Status
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string Name { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
