﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HamburgersBlog.Models
{
    public class SideDish
    {
        [Key]
        public int SideDishId { get; set; }
        public int Price { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}