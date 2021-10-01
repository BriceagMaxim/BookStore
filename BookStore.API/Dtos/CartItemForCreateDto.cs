using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Dtos
{
    public class CartItemForCreateDto
    {
        [Required(ErrorMessage = "BookId is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
    }
}