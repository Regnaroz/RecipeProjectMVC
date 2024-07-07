using System;
using System.Collections.Generic;

namespace RecipeBlogProject.Models;

public partial class Recipecategory : BaseEntity
{
  

    public int? RecipeId { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Recipe? Recipe { get; set; }
}
