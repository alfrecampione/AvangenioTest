namespace Ex3.Data.DTO.In;

public class CreateContactDto
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Phone { get; set; }
}