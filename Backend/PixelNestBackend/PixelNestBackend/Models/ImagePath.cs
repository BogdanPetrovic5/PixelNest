using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Models
{
    public class ImagePath
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PathID { get; set; }
        public int PostID { get; set; }
        public string Path{ get; set; }
       
        public Post Post { get; set; }
    }
}
