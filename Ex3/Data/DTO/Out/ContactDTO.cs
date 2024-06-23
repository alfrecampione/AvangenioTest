using Ex3.Data.Model;

namespace Ex3.Data.DTO.Out;

public class ContactDto
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age {get; set;}
    public required string Phone { get; set; }
    
    public static ContactDto FromEntity(Contact contact)
    {
        return new ContactDto
        {
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            DateOfBirth = contact.DateOfBirth,
            Age = DateTime.Now.Year - contact.DateOfBirth.Year,
            Phone = contact.Phone
        };
    }
}