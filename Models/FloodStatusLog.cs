using System;
using SQLite;

namespace DeteksiBanjir.Models;

public class FloodStatusLog
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    // "No Alert", "Yellow Alert", "Orange Alert", "Red Alert"
    public string Level { get; set; }

    // "Fuzzy", "NN"
    public string Source { get; set; }
    
    // Detailed output or risk index
    public double RiskScore { get; set; }
}
