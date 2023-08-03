using System.ComponentModel.DataAnnotations;

namespace ASNClub.ViewModels.Comment
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
   
        public string OwnerName { get; set; } = null!;
       
        public string Text { get; set; } = null!;
        
        public DateTime PostedOn { get; set; }
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Display(Name = "Owner")]
        public Guid OwnerId { get; set; }


    }
}
