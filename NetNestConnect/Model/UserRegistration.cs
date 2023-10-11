using System.ComponentModel.DataAnnotations;

namespace NetNestConnect.Model
{
    public class UserRegistration
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "FirstName must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "LastName must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid mobile number format")]
        public string PhoneNumber { get; set; }

        [StringLength(100, MinimumLength =4 , ErrorMessage = "Password must be between 4 and 100 characters.")]
        public string Password { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;

        public bool? IsDeleted { get; set; } = false;

        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; } 

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

}
