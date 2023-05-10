using System.ComponentModel.DataAnnotations.Schema;

internal class Users
{
    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("password")]
    public string? Password { get; set; }
}