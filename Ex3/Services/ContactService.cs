using Ex3.Data.DTO.In;
using Ex3.Data.DTO.Out;
using Ex3.Data.Model;
using Ex3.DataAccess.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Ex3.Services;

public class ContactService(IDataRepository repository) : IContactService
{
    private readonly IDataRepository _repository = repository;

    public Task DeleteContact(int id)
    {
        var contact = _repository.Set<Contact>().First(c => c.Id == id);

        _repository.Set<Contact>().Remove(contact);
        return _repository.Save(default);
    }

    public Task<IEnumerable<ContactDto>> GetAllContacts()
    {
        var contacts = _repository.Set<Contact>().ToList();
        return Task.FromResult(contacts.Select(c => ContactDto.FromEntity(c)));
    }

    public Task<ContactDto?> GetContact(int id)
    {
        var contact = _repository.Set<Contact>().FirstOrDefault(c => c.Id == id);
        if (contact == null)
            return Task.FromResult<ContactDto?>(null);

        return Task.FromResult<ContactDto?>(ContactDto.FromEntity(contact));
    }

    public async Task<int> PostContact(CreateContactDto createContactDto, string userID)
    {
        if ((DateTime.Today.Year - createContactDto.DateOfBirth.Year) < 18)
            return -1;
        var contact = new Contact
        {
            FirstName = createContactDto.FirstName,
            LastName = createContactDto.LastName,
            DateOfBirth = createContactDto.DateOfBirth,
            Email = createContactDto.Email,
            Phone = createContactDto.Phone,
            Owner = userID
        };
        await _repository.Set<Contact>().Create(contact);
        await _repository.Save(default);

        return contact.Id;
    }

    public Task UpdateContact(int id, CreateContactDto updateContactDto)
    {
        var contact = _repository.Set<Contact>().First(c => c.Id == id);

        contact.FirstName = updateContactDto.FirstName;
        contact.LastName = updateContactDto.LastName;
        contact.DateOfBirth = updateContactDto.DateOfBirth;
        contact.Email = updateContactDto.Email;
        contact.Phone = updateContactDto.Phone;

        return _repository.Save(default);
    }
}
