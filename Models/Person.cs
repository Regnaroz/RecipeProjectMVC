using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBlogProject.Models;

public partial class Person : BaseEntity
{
    [Required]
    [DisplayName("First Name")]
    public string Firstname { get; set; } = null!;

    [Required]
    [DisplayName("Last Name")]
    public string Lastname { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Gender { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Phone { get; set; } = null!;
    public string? ImagePath { get; set; } = null;

    [DisplayName("Role")]
    public int? RoleId { get; set; }

    public virtual Userrole? Role { get; set; }
}
