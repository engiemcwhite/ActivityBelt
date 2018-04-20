using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace beltexam.Models
{
    public class Rsvp : BaseEntity
    {
        public int RsvpId {get;set;}
        public int UserId {get;set;}
        public User User {get;set;}
        public int ActivityId {get;set;}
        public Activity Activity {get;set;}
    }
}