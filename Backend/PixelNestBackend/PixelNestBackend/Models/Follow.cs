using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelNestBackend.Models
{
    public class Follow
    {
   
        public int UserFollowerID { get; set; }
        public int UserFollowingID { get; set; }
        
        public Guid UserFollowerGuid { get; set; }
        public Guid UserFollowingGuid { get; set; }
        public virtual User UserFollower { get; set; }
        public virtual User UserFollowing { get; set; }
       
    }
}
