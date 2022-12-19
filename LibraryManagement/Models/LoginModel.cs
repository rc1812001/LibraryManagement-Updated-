using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace LibraryManagement.Web.Models
{
    public class LoginModel
    {
        [Display(Name = "User Name")] //used for dynamic changes in MVC
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter UserName")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "UserName should be min 5 and max 20 characters long")]
        public string USER_NAME{ get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Password")]
       // [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$", ErrorMessage = "Please Enter Valid Password")]


        public string PASSWORD { get; set; }

    }
}