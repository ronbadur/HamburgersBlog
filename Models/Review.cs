using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HamburgersBlog.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int RestaurantId{ get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public string Content { get; set; }
    }
}