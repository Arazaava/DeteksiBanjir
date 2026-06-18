using System.Threading.Tasks;
using DeteksiBanjir.Models;

namespace DeteksiBanjir.Services;

public class AuthService
{
    private readonly DatabaseService _databaseService;

    public User CurrentUser { get; private set; }

    public AuthService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _databaseService.GetUserByUsernameAsync(username);
        
        // In a real application, you would hash the provided password and compare.
        // For demonstration, we compare directly.
        if (user != null && user.PasswordHash == password && user.IsActive)
        {
            CurrentUser = user;
            return true;
        }

        return false;
    }

    public void Logout()
    {
        CurrentUser = null;
    }

    public bool IsLoggedIn() => CurrentUser != null;
    public bool IsAdmin() => CurrentUser?.Role == "Admin";
}
