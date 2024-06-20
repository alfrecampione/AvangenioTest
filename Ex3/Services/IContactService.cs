using Ex3.Data.DTO.In;
using Ex3.Data.DTO.Out;

namespace Ex3.Services;

public interface IContactService
{
    Task<int> PostContact(CreateContactDto createContactDto);
    
    Task<ContactDto?> GetContact(int id);
    Task<IEnumerable<ContactDto>> GetAllContacts();
    
    Task DeleteContact(int id);
    
    Task UpdateContact(int id, CreateContactDto updateContactDto);
}