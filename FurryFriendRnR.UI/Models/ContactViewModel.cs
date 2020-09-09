using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FurryFriendRnR.UI.Models
{
    public class ContactViewModel
    {

        [Required(ErrorMessage = "* Please enter your name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "* Please enter your email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "* Please enter a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "* Please enter your number.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "* Please enter a valid number")]
        public string Phone { get; set; }        
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "* Please enter a message.")]
        public string Message { get; set; }

    }
}