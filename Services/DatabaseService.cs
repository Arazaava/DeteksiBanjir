using System.IO;
using System.Threading.Tasks;
using SQLite;
using DeteksiBanjir.Models;

namespace DeteksiBanjir.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "deteksi_banjir.db3");
    }

    public async Task InitAsync()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath);

        await _database.CreateTableAsync<User>();
        await _database.CreateTableAsync<SensorReading>();
        await _database.CreateTableAsync<FloodStatusLog>();
        await _database.CreateTableAsync<Thresholds>();
        await _database.CreateTableAsync<NNParameter>();

        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        // Seed Admin if not exists
        var admin = await _database.Table<User>().Where(u => u.Username == "admin").FirstOrDefaultAsync();
        if (admin == null)
        {
            await _database.InsertAsync(new User
            {
                Username = "admin",
                PasswordHash = "admin123", // In a real app, hash this properly.
                Role = "Admin",
                IsActive = true
            });
        }

        // Seed initial thresholds
        var thresholds = await _database.Table<Thresholds>().FirstOrDefaultAsync();
        if (thresholds == null)
        {
            await _database.InsertAsync(new Thresholds
            {
                YellowAlertLevel = 5.0,
                OrangeAlertLevel = 7.0,
                RedAlertLevel = 10.0
            });
        }
        
        // Seed dummy sensor data
        var sensorCount = await _database.Table<SensorReading>().CountAsync();
        if(sensorCount == 0)
        {
            await _database.InsertAsync(new SensorReading { Timestamp = DateTime.Now.AddHours(-3), WaterLevel = 3.2, Rainfall = 10, Discharge = 50, Temperature = 28, Humidity = 75 });
            await _database.InsertAsync(new SensorReading { Timestamp = DateTime.Now.AddHours(-2), WaterLevel = 4.5, Rainfall = 25, Discharge = 70, Temperature = 27, Humidity = 80 });
            await _database.InsertAsync(new SensorReading { Timestamp = DateTime.Now.AddHours(-1), WaterLevel = 6.8, Rainfall = 50, Discharge = 120, Temperature = 26, Humidity = 85 });
        }
    }

    // Generic CRUD Methods
    public async Task<List<T>> GetAllAsync<T>() where T : new()
    {
        await InitAsync();
        return await _database.Table<T>().ToListAsync();
    }

    public async Task<int> InsertAsync<T>(T item) where T : new()
    {
        await InitAsync();
        return await _database.InsertAsync(item);
    }

    public async Task<int> UpdateAsync<T>(T item) where T : new()
    {
        await InitAsync();
        return await _database.UpdateAsync(item);
    }

    public async Task<int> DeleteAsync<T>(T item) where T : new()
    {
        await InitAsync();
        return await _database.DeleteAsync(item);
    }
    
    // User Methods
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        await InitAsync();
        return await _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
    }
}
