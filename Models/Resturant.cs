using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HamburgersBlog.Models
{
    public class Resturant
    {
        [Key]
        public int ResturantId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public int PrincessID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public virtual Princess Princess { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}