using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Web.Models
{
    public class UserModel
    {
        public int USER_ID { get; set; }

        [Display(Name = "User Name")] //used for dynamic changes in MVC
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter UserName")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "UserName should be min 5 and max 20 characters long")]  //Validation using Regex
        public string USER_NAME { get; set; }

        [Display(Name = "First Name")] //used for dynamic changes in MVC
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter UserName")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "UserName should be min 5 and max 20 characters long")]  //Validation using Regex
        public string FIRST_NAME { get; set; }

        [Display(Name = "Last Name")] //used for dynamic changes in MVC
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter UserName")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "UserName should be min 5 and max 20 characters long")]  //Validation using Regex
        public string LAST_NAME { get; set; }


        [Display(Name = "Mobile")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Mobile")]
        [RegularExpression(("^\\+?[1-9][0-9]{7,14}$"), ErrorMessage = "Please enter valid Mobile No s")] //Validation using Regex
        public string MOBILE { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Email")]
        public string EMAIL_ID { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Password")]
        [StringLength(10,MinimumLength =8,ErrorMessage = "Password should be min 8 and max 20 characters long")]
        
        public string PASSWORD { get; set; }


        [Display(Name = "ConfirmPassword")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Password")]
        [System.ComponentModel.DataAnnotations.Compare("PASSWORD", ErrorMessage = "Password and ConfirmPassword Doesnt Match")] //Validation
        public string CONFIRMPASSWORD { get; set; }


        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please enter Gender")]
        public string GENDER { get; set; }

        [Display(Name = "IS_ACTIVE")]
        [Required(ErrorMessage = "Please select ststus")]
        public bool IS_ACTIVE { get; set; }

    }
}