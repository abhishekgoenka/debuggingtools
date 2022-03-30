using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ASPNETMVCApplication.Code.Attributes;

namespace ASPNETMVCApplication.Areas.Auth.Models
{
    /// <summary>
    /// Login model class.
    /// </summary>
    /// <remarks>
    /// Properties hold annotation attributes for Display and Validation.
    /// Including custom validation of email and passwords.
    /// </remarks>
    public class LoginModel
    {
        /// <summary>
        /// The user's user name.
        /// </summary>
        [Required(ErrorMessage = "please enter user name.")]
        [DisplayName("username")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }


        /// <summary>
        /// The user's password.
        /// </summary>
        [Required(ErrorMessage = "please enter password.")]
        [DataType(DataType.Password)]
        [DisplayName("password")]
        [ValidatePassword]
        public string Password { get; set; }
    }
}