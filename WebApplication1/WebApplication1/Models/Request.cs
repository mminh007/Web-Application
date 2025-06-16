using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public User? User { get; set; }

        public int MetaImage_Id { get; set; }
        public MetadataImage? MetaImage { get; set; }

        public DateTime RequestedAt { get; set; }
    }
}
