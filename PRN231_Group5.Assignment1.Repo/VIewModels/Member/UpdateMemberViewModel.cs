using System.ComponentModel.DataAnnotations;

namespace PRN231_Group5.Assignment1.Repo.VIewModels.Member;

public class UpdateMemberViewModel
{
    [Required(ErrorMessage = "Company Name is required.")]
    [StringLength(40, ErrorMessage = "Company Name can't be longer than 40 characters.")]
    public string CompanyName { get; set; } = null!;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(15, ErrorMessage = "City can't be longer than 15 characters.")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(15, ErrorMessage = "Country can't be longer than 15 characters.")]
    public string Country { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(30, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 30 characters.")]
    public string Password { get; set; } = null!;
}