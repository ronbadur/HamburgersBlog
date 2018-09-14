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
        public string Location { get; set; }
        [Required]
        public double Rate { get; set; }
        public bool IsVeganFriendly { get; set; }
        public bool IsKosher { get; set; }
        public bool IsParkingAvailable { get; set; }

        public virtual ICollection<Hamburger> Hamburgers { get; set; }
    }
}