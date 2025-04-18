using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface IAnalyticsService
    {
        ICollection<ResponseAnalyticsLocation> GetAnalyticsLocations(string userGuid);
    }
}
