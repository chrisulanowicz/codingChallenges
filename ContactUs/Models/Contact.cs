using System;
using System.ComponentModel.DataAnnotations;

namespace ContactUs.Models
{

    public class Contact : BaseEntity
    {

        public int Id { get; set; }
        [MaxLength(40)]
        public string FirstName { get; set; }
        [MaxLength(40)]
        public string LastName { get; set; }
        [MaxLength(16)]
        public string Telephone { get; set; }
        [MaxLength(80)]
        public string EmailAddress { get; set; }
        public DateTime BestTimeToCall { get; set; }

    }

}