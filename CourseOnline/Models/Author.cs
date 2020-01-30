using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter author name")]
        public string AuthorName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter avatar")]
        public string Avatar { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter phone number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
            ErrorMessage = "Entered phone format is not valid.")]
        public string Phone { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        
    }
}