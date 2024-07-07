using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecipeBlogProject.Models;

public partial class Recipepayment : BaseEntity
{
 

    public int? RecipeId { get; set; }

    public int? UserId { get; set; }
    [DisplayName("Total Amount")]
    public double? Totalamount { get; set; }
    [DisplayName("Payment File Path")]
    public string Paymentfilepath { get; set; } = null!;

    public virtual Recipe? Recipe { get; set; }

    public virtual Systemuser? User { get; set; }
}
