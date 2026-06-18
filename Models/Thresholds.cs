using SQLite;

namespace DeteksiBanjir.Models;

public class Thresholds
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public double YellowAlertLevel { get; set; }
    public double OrangeAlertLevel { get; set; }
    public double RedAlertLevel { get; set; }
}
