using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ex3.Data.Model;

public class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(128)]
    public required string FirstName { get; set; }
    [MaxLength(128)]
    public string? LastName { get; set; }
    [MaxLength(128)]
    public required string Email { get; set; }
    public required DateTime DateOfBirth { get; set; }
    [MaxLength(20)]
    public required string Phone { get; set; }

    public string? Owner { get; set; }
}