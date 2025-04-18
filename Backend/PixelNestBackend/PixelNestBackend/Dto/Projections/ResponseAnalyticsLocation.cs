namespace PixelNestBackend.Dto.Projections
{
    public class ResponseAnalyticsLocation
    {
   
        public string Country { get; set; }
        public int Count { get; set; }
        public List<string> Cities { get; set; }
        
    }
}
