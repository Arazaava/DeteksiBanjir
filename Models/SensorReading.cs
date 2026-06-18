using System;
using SQLite;

namespace DeteksiBanjir.Models;

public class SensorReading
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    // in meters or cm
    public double WaterLevel { get; set; }

    // in mm
    public double Rainfall { get; set; }

    // in m3/s
    public double Discharge { get; set; }

    // in Celsius
    public double Temperature { get; set; }

    // in Percentage
    public double Humidity { get; set; }
}
