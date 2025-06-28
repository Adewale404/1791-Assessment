namespace LoadExpressApi.Persistence.Logging;

public class LoggerSettings
{
    public string AppName { get; set; } = "1791 Assessment.WebAPI";
    public string ElasticSearchUrl { get; set; } = string.Empty;
    public bool WriteToFile { get; set; } = true;
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
}
