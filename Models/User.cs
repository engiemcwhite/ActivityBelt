using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace beltexam.Models
{
    public abstract class BaseEntity {}
    public class User : BaseEntity
    {
        public int UserId {get;set;}
        public string FirstName { get; set; }
        public string LastName {get;set;}
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public List<Activity> Activities { get; set; }

        public List<Rsvp> Rsvps { get; set; }

        public User(){
            Activities = new List<Activity>();
            Rsvps = new List<Rsvp>();
        }
    }

    
}