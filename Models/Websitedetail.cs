using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecipeBlogProject.Models;

public partial class Websitedetail : BaseEntity
{
    
    [DisplayName("Website Text")]
    public string Websitetext { get; set; } = null!;
    [DisplayName("Type")]
    public int Texttype { get; set; }

    public int? AdminId { get; set; }

    public virtual Admin? Admin { get; set; }
}
