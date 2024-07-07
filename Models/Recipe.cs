using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBlogProject.Models;

public partial class Recipe : BaseEntity
{
   
    [DisplayName("Recipe Name")]
    [Required]
    public string Receipename { get; set; } = null!;

    [Required]
    public double? Price { get; set; }

    [Required]
    public string Ingredients { get; set; } = null!;
    public string? Details { get; set; } = null;

   
    public string ImagePath { get; set; } = string.Empty;

    [NotMapped]
    [DisplayName("Recipe Image")]
    [Required]
    public virtual IFormFile? ImageFile { get; set; }

    public bool Isapproved { get; set; }

    public int? ChefId { get; set; }

    public virtual Chef? Chef { get; set; }

    public virtual ICollection<Recipecategory> Recipecategories { get; set; } = new List<Recipecategory>();

    public virtual ICollection<Recipepayment> Recipepayments { get; set; } = new List<Recipepayment>();
}
