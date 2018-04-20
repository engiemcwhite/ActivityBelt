using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace beltexam.Models
{
    public class Activity : BaseEntity
    {
        public int ActivityId {get;set;}
        public string Title { get; set; }
        // public string Time {get;set;}
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        // public int ThisDur {get; set;}
        // public string Unit {get; set;}
        public string Desc { get; set;}
        public int UserId { get; set; }

        public DateTime CreatedAt {get; set;}
        public User User { get; set; }
        public List<Rsvp> Participants { get; set; }

        public Activity(){
            Participants = new List<Rsvp>();
        }
    }
}