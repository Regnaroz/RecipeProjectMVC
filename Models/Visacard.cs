using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeBlogProject.Models;

public partial class Visacard : BaseEntity
{
   
    [DisplayName("First Name")]
    [Required]
    public string Firstname { get; set; } = null!;

    [DisplayName("Last Name")]
	[Required]
	public string Lastname { get; set; } = null!;

    [DisplayName("Card Number")]
	[Required]
	public long Cardnumber { get; set; }

	[Required]
	public byte Pin { get; set; }

    [DisplayName("Expiry Date")]
	[Required]
	public string Expirydate { get; set; } = null!;

	[Required]
	public string Cvv { get; set; }

    public int? UserId { get; set; }

    public virtual Systemuser? User { get; set; }
}
