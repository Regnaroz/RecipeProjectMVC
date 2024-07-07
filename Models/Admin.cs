using System;
using System.Collections.Generic;

namespace RecipeBlogProject.Models;

public partial class Admin: BaseEntity
{
    public int? PersonId { get; set; }

    public virtual Person? Person { get; set; }

    public virtual ICollection<Websitedetail> Websitedetails { get; set; } = new List<Websitedetail>();
}
