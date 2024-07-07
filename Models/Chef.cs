using System;
using System.Collections.Generic;

namespace RecipeBlogProject.Models;

public partial class Chef: BaseEntity
{ 
    public int? PersonId { get; set; }
    public virtual Person? Person { get; set; }
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
