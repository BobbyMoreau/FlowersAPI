

using System.ComponentModel.DataAnnotations;

namespace flowers.api.ViewModels
{
    public class FlowerPostView : BaseViewModel
    {
        // [Required(ErrorMessage = "You have to write the flowers name")]
        // public string Name {get; set;}
        [Required(ErrorMessage = "You have to write the flowers color")]
        public string Color { get; set; }  
        [Required(ErrorMessage = "You have to write the flowers height")] 
        public int Height { get; set; }
        
        [Required(ErrorMessage = "You have to choose the flowers family")]
        public int FamilyId { get; set; }
        public string Family { get; set;}
    }
}