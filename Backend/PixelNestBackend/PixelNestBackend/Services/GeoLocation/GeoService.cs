using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using PixelNestBackend.Interfaces.GeoLocation;
using System.Net;

namespace PixelNestBackend.Services.GeoLocation
{
    public class GeoService : IGeoService
    {
        private readonly string _dbPath = Path.Combine(AppContext.BaseDirectory, "Database", "GeoLite2-City.mmdb");
        public string? GetCountryFromIP(string ip)
        {
            using var reader = new DatabaseReader(_dbPath);
            if (!File.Exists(_dbPath))
            {
                Console.WriteLine($"Error: GeoLite2-City.mmdb file not found at {_dbPath}");
                return null;
            }

          
            if (!IPAddress.TryParse(ip, out var parsedIP))
            {
                Console.WriteLine("Invalid IP address format.");
                return null;
            }

            try
            {
                CityResponse response = reader.City(parsedIP);
                Console.WriteLine("City: " + response.City.Name + "\n");
                return response.City.Name;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
