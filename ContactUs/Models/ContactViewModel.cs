using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactUs.Models
{

    public class ContactViewModel : BaseEntity
    {

        [Required(ErrorMessage="Please fill in your first name")]
        [MaxLength(40, ErrorMessage="Sorry, but we can't accept first names longer than 40 characters")]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage="Please fill in your last name")]
        [MaxLength(40, ErrorMessage="Sorry, but we can't accept last names longer than 40 characters")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Phone(ErrorMessage="Please fill in a valid phone number")]
        [MaxLength(16, ErrorMessage="Phone Number can't be more than 16 digits")]
        public string Telephone { get; set; }

        [Required(ErrorMessage="Please fill in your email address")]
        [MaxLength(80, ErrorMessage="Sorry, but we can't accept email addresses longer than 80 characters")]
        [EmailAddress(ErrorMessage="Please enter a valid email address")]
        [Display(Name="Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name="Best Time to Call")]
        [ProperTime(9, 18, 15)]
        public DateTime BestTimeToCall { get; set; }

        public string Captcha { get; set; }

    }

}