namespace EBook.Models;
public class CoverType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Cover Type")]
    [MaxLength(60)]
    public string Name { get; set; }
}

