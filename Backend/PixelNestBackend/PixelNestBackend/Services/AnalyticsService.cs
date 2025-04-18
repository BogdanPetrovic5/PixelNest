using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;

namespace PixelNestBackend.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _analyticsRepository;
        public AnalyticsService(IAnalyticsRepository analyticsRepository) {
            _analyticsRepository = analyticsRepository;
        }

        public ICollection<ResponseAnalyticsLocation> GetAnalyticsLocations(string userGuid)
        {
            return _analyticsRepository.GetAnalyticsLocations(userGuid);
        }
    }
}
