namespace HomeTask1.Users.WebApi.Models;

public class Project
{
    public string Id { get; set; }
    
    public int UserId { get; set; }
    
    public string Name { get; set; }
    
    public List<Chart> Charts { get; set; } = new();
}

public class Chart
{
    public string Symbol { get; set; }
    public string Timeframe { get; set; }
    public List<Indicator> Indicators { get; set; } = new();
}

public class Indicator
{
    public string Name { get; set; }
    public string Parameters { get; set; }
}