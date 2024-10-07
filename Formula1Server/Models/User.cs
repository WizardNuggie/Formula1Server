using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Formula1Server.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(250)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Username { get; set; }

    [StringLength(250)]
    public string Name { get; set; }

    [StringLength(20)]
    public string Password { get; set; }

    public bool IsAdmin { get; set; }

    [StringLength(250)]
    public string FavDriver { get; set; }

    [StringLength(250)]
    public string FavConstructor { get; set; }

    public DateOnly Birthday { get; set; }

    public bool IsRecievingNot { get; set; }

    public int UserTypeId { get; set; }

    [ForeignKey("UserTypeId")]
    [InverseProperty("Users")]
    public virtual UserType UserType { get; set; }

    [ForeignKey("WriterId")]
    [InverseProperty("Writers")]
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
