using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Ex3.Data.Model;

public class User : IdentityUser
{
    [MaxLength(128)]
    public required string FirstName { get; set; }
    [MaxLength(128)]
    public required string LastName { get; set; }

    public required string Country { get; set; }

}