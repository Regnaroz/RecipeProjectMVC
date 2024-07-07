using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBlogProject.Models;

public partial class Systemuser : BaseEntity
{
    public int? PersonId { get; set; }

    public virtual Person? Person { get; set; }

    public virtual ICollection<Recipepayment> Recipepayments { get; set; } = new List<Recipepayment>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Visacard> Visacards { get; set; } = new List<Visacard>();
}
