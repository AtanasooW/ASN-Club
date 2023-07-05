namespace ASNClub.Data.Models.Product
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static Common.EntityValidationConstants.Comment;
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string OwnerName { get; set; } = null!;
        [Required]
        [StringLength(TextMaxLength, MinimumLength = TextMinLength)]
        public string Text { get; set; } = null!;
        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required]
        [ForeignKey("Owner")]
        public Guid OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; } = null!;

    }
}
