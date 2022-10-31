namespace WeatherAssignment.DTOs 
{
    public record TableData(string PartitionKey, string RowKey, double WeightedTemperature);
}
