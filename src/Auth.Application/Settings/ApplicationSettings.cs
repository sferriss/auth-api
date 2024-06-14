namespace Auth.Application.Settings;

public class ApplicationSettings
{
    public string JwtSecret { get; set; } = null!;
    public int WorkFactor { get; set; }
}