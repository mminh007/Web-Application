using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class MetadataImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MetaImage_ID { get; set; }
        public string ImageName { get; set; }
        public string? ResultJson   { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
