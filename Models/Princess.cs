using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HamburgersBlog.Models
{
    public enum HairColor
    {
        Black, Red, Yellow, Brown, Redhead,
    }

    public enum KingdomType
    {
        RoyalBlood, MarriedToRoyalty, NotRoyal
    }

    public class Princess
    {
        [Key]
        public int PrincessID { get; set; }
        [Required]
        public string Name { get; set; }
        public KingdomType RoyaltyType { get; set; }
        [Required]
        public int CreationYear { get; set; }
        public HairColor HairColor { get; set; }
        public string Nationality { get; set; }
        public string MovieName { get; set; }
    }
}