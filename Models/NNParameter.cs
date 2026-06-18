using SQLite;

namespace DeteksiBanjir.Models;

public class NNParameter
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string LayerName { get; set; }

    // Store serialized JSON array of weights
    public string WeightsJson { get; set; }

    // Store serialized JSON array of biases
    public string BiasesJson { get; set; }
}
