﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 1000, ErrorMessage ="Display Order must be between 1-1000")]
        public int DisplayOrder { get; set; }

    }
}
