using System.Security.Claims;
using Ex3.Data.DTO.In;
using Ex3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ex3.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContactsController: ControllerBase
{
    private readonly IContactService _contactsService;
    private readonly ILogger<ContactsController> _logger;
    
    public ContactsController(ILogger<ContactsController> logger, IContactService contactsService)
    {
        _logger = logger;
        _contactsService = contactsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var contacts = await _contactsService.GetAllContacts();
        return Ok(contacts);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var contact = await _contactsService.GetContact(id);
        if (contact != null)
            return Ok(contact);
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CreateContactDto createContactDto)
    {
        var userId = User.Claims.First(c=> c.Type == ClaimTypes.NameIdentifier).Value;
        var newContactId = await _contactsService.PostContact(createContactDto, userId);
        if (newContactId == -1)
            return BadRequest();
        var newContact = await _contactsService.GetContact(newContactId);
        return CreatedAtAction(nameof(Post), new { id = newContactId }, newContact);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromForm] CreateContactDto updateContactDto)
    {
        var contact = await _contactsService.GetContact(id);
        if (contact == null)
            return NotFound();
        await _contactsService.UpdateContact(id, updateContactDto);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var jwtCountry = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Country)?.Value;
        if (jwtCountry != "CU")
            return Unauthorized();
        
        var contact = await _contactsService.GetContact(id);
        if (contact == null)
            return NotFound();
        await _contactsService.DeleteContact(id);
        return NoContent();
    }
}