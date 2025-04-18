using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;

namespace PixelNestBackend.Repository
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly DataContext _dataContext;
        public AnalyticsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public ICollection<ResponseAnalyticsLocation> GetAnalyticsLocations(string userGuid)
        {
            try
            {
                 
                Guid parsedUserGuid = Guid.Parse(userGuid);
                var data = _dataContext.Follow.
                    Where(f => f.UserFollowerGuid == parsedUserGuid || f.UserFollowingGuid == parsedUserGuid)
                    .Include(u => u.UserFollower)
                    .Include(u => u.UserFollowing)
                    .Select(response => new
                    {
                        Country =
                        response.UserFollowerGuid == parsedUserGuid ? 
                        response.UserFollowing.Country : 
                        response.UserFollower.Country,

                        City = response.UserFollowerGuid == parsedUserGuid ? 
                        response.UserFollowing.City : 
                        response.UserFollower.City

                    }).ToList();

                ICollection<ResponseAnalyticsLocation> responseAnalytics = data
                        .GroupBy(x => x.Country)
                        .Select(group => new ResponseAnalyticsLocation
                        {
                            Country = group.Key,
                            Count = group.Count(),
                            Cities = group
                                .Select(x => x.City)
                                .Where(city => city != null)
                                .Distinct()
                                .ToList()
                        })
                        .ToList();

                return responseAnalytics;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
