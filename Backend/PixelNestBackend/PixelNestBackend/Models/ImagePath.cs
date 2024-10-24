using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Models
{
    public class ImagePath
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PathID { get; set; }
        public int? PostID { get; set; }
        public int? StoryID { get; set; }
        public string Path{ get; set; }
        public string PhotoDisplay { get; set; }
        public Post Post { get; set; }
        public Story Story { get; set; }
    }
}
