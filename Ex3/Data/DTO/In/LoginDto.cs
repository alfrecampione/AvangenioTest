using System.ComponentModel.DataAnnotations;

namespace Ex3.Data.DTO.In;

public class LoginDto
{
    [Required] public required string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}