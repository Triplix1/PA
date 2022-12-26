using System.ComponentModel.DataAnnotations;

namespace Lab3.Models.ViewModels
{
    public class IdentityViewModel
    {
        [Required(ErrorMessage = "Value cannot be null")]
        public int Id { get; set; }
    }
}
