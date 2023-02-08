using NullGuard;

public class AccessConfiguration
{
    [AllowNull]
    public string? Issuer { get; set; }
    [AllowNull]
    public string? Audience { get; set; }
}