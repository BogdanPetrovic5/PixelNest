using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface IAnalyticsRepository
    {
        ICollection<ResponseAnalyticsLocation> GetAnalyticsLocations(string userGuid);
    }
}
