using SQLite;

namespace DeteksiBanjir.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique, MaxLength(50)]
    public string Username { get; set; }

    public string PasswordHash { get; set; }

    // "Admin" or "User"
    public string Role { get; set; }

    public bool IsActive { get; set; }
}
