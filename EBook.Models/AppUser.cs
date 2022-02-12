namespace EBook.Models;
public class AppUser : IdentityUser
{
    [Required]
    public string Name { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }

    public int? CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    [ValidateNever]
    public Company Company { get; set; }
}