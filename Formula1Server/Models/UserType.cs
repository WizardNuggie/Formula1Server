using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Formula1Server.Models;

public partial class UserType
{
    [Key]
    public int UserTypeId { get; set; }

    [StringLength(250)]
    public string UserTypeName { get; set; }

    [InverseProperty("UserType")]
    public virtual ICollection<User> Users { get; set; } = [];
}
