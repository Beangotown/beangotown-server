namespace BeangoTownServer.Monitor;

public class IndicatorOptions
{
    public bool IsEnabled { get; set; }
    public string Application { get; set; } = "BeangoTown";
    public string Module { get; set; } = "Api";
}