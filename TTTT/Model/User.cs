﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TTTT.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
