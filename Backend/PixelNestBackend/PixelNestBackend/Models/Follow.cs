using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelNestBackend.Models
{
    public class Follow
    {
   
        public int UserFollowerID { get; set; }
        public int UserFollowingID { get; set; }
        public string FollowerUsername { get; set; }
        public string FollowingUsername { get; set; }
        public virtual User UserFollower { get; set; }
        public virtual User UserFollowing { get; set; }
    }
}
