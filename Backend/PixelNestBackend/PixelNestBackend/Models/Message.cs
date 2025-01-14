using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }

        public int SenderID { get; set; } 
        public int ReceiverID { get; set; }
        public string MessageText { get; set; }
     
        public DateTime DateSent { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }


        
    }
}
