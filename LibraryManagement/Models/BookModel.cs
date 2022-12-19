using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryManagement.Models
{
    public class BookModel
    {
            public int BOOK_ID { get; set; }

            [Display(Name = "BOOK NAME")] //used for dynamic changes in MVC
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter UserName")]
            public string BOOK_NAME { get; set; }

            [Display(Name = "CATEGORY")] //used for dynamic changes in MVC
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please select CATEGORY")]
            public string CATEGORY { get; set; }

            [Display(Name = "AUTHOR")] //used for dynamic changes in MVC
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter AUTHOR")]
           
            public string AUTHOR { get; set; }


            [Display(Name = "STATUS")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter STATUS")]
           
            public string STATUS { get; set; }

       


        }
    }


