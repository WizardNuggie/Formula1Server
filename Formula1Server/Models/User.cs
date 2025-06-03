using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Formula1Server.DTO;
using Microsoft.EntityFrameworkCore;

namespace Formula1Server.Models;

[Index("Username", Name = "UQ__Users__536C85E405E8432D", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(250)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string Username { get; set; } = null!;

    [StringLength(250)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string Password { get; set; } = null!;

    public bool IsAdmin { get; set; }

    [StringLength(250)]
    public string FavDriver { get; set; } = null!;

    [StringLength(250)]
    public string FavConstructor { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public bool IsRecievingNot { get; set; }

    public int UserTypeId { get; set; }

    [InverseProperty("Writer")]
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    [ForeignKey("UserTypeId")]
    [InverseProperty("Users")]
    public virtual UserType UserType { get; set; } = null!;

    public User() { }
    public User(UserDTO u)
    {
        this.UserId = u.Id;
        this.Email = u.Email;
        this.Username = u.Username;
        this.Name = u.Name;
        this.Password = u.Password;
        this.IsAdmin = u.IsAdmin;
        this.FavDriver = u.FavDriver;
        this.FavConstructor = u.FavConstructor;
        this.Birthday = u.Birthday;
        this.UserTypeId = u.UserTypeId;
    }

}
