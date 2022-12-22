using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryManagement.Models
{
    public class CategoryModel
    {
        public int CATEGORY_ID { get; set; }

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Please enter category name")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Category Name should be min 5 and max 20 characters long")]
        public string CATEGORY_NAME { get; set; }
    }
}