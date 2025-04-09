namespace PixelNestBackend.Interfaces.GeoLocation
{
    public interface IGeoService
    {
        string? GetCountryFromIP(string ip);
    }
}
