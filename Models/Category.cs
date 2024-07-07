using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecipeBlogProject.Models;

public partial class Category: BaseEntity
{

    [DisplayName("Category Name")]
    public string Categoryname { get; set; } = null!;
    public string? CategoryImagePath { get; set; } = null;

    public virtual ICollection<Recipecategory> Recipecategories { get; set; } = new List<Recipecategory>(); 
}
