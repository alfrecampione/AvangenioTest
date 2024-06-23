using System.ComponentModel.DataAnnotations;

namespace Ex3.Data.DTO.In;

public class RegisterDto
{
    [Required] public required string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public required string Email { get; set; }


    [Required] public required string CountryCode { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}