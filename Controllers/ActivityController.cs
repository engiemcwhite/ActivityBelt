using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using beltexam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace beltexam.Controllers
{
    public class ActivityController : Controller
    {

        private BeltContext _context;
 
        public ActivityController(BeltContext context)
        {
            _context = context;
        }
 
        [HttpGet]
        [Route("/Home")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("id") == null){
                return Redirect("/");
            }

            ViewBag.AllActivities = _context.Activities.Include(b => b.Participants).Include(d =>d.User).Where(a => a.StartTime + a.Duration > DateTime.Now).OrderBy(c => c.StartTime).ToList();
            ViewBag.Id = HttpContext.Session.GetInt32("id");
            ViewBag.Name = _context.Users.SingleOrDefault(a => a.UserId == HttpContext.Session.GetInt32("id"));
            return View();
            // Other code
        }

        [HttpPost]
        [Route("/newactivity")]
        public IActionResult NewActivity(PlanViewModel model)
        {
            if(HttpContext.Session.GetInt32("id") == null){
                return Redirect("/");
            }   
            if(ModelState.IsValid){
                TimeSpan duration;
                if(model.Unit == "days"){
                    duration = new TimeSpan(model.Duration,0,0,0);
                }
                else if(model.Unit == "hours"){
                    duration = new TimeSpan(0,model.Duration,0,0);
                }
                else{
                    duration = new TimeSpan(0, 0, model.Duration, 0);
                }
                DateTime starttime = model.Date+model.Time;
                if(starttime < DateTime.Now){
                    ViewBag.TimeError = 2;
                    return View("New");
                }
                Activity NewAct = new Activity {
                    Title = model.Title,
                    Desc = model.Desc,
                    StartTime = starttime,
                    Duration = duration,
                    // ThisDur = model.Duration,
                    // Unit = model.Unit,
                    CreatedAt = DateTime.Now,
                    UserId = (int)HttpContext.Session.GetInt32("id"),
                };
                _context.Add(NewAct);
                _context.SaveChanges();
                Activity thisactivity = _context.Activities.OrderByDescending(a=>a.CreatedAt).FirstOrDefault();
                int num = thisactivity.ActivityId;
                return Redirect("/activity/"+num);
            }
            return View("New");
        }

        [HttpGet]
        [Route("/delete/{num}")]
        public IActionResult Delete(int num){
            if(HttpContext.Session.GetInt32("id") == null){
                return Redirect("/");
            }
            Activity thisact = _context.Activities.Include(b => b.Participants).SingleOrDefault(a => a.ActivityId == num);
            foreach(Rsvp part in thisact.Participants){
                _context.Rsvps.Remove(part);
            }
            _context.Activities.Remove(thisact);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

       [HttpGet]
       [Route("/rsvp/{num}")]
       public IActionResult Rsvp(int num){
           if(HttpContext.Session.GetInt32("id") == null){
                return Redirect("/");
            }
           List<Activity> allActs = _context.Activities.Include(a => a.Participants).ToList();
           Activity thisAct = _context.Activities.SingleOrDefault(a => a.ActivityId == num);
           ViewBag.Sched = 0;
           foreach(var act in allActs){
               foreach(var part in act.Participants){
                   if(part.UserId == HttpContext.Session.GetInt32("id")){
                       if(act.StartTime < thisAct.StartTime && thisAct.StartTime < act.StartTime+act.Duration ){
                           ViewBag.SchedError = 1;   //Check to see if desired activity starts during any of your current activities.
                           ViewBag.AllActivities = _context.Activities.Include(b => b.Participants).Include(d =>d.User).Where(a => a.StartTime + a.Duration > DateTime.Now).OrderBy(c=>c.StartTime).ToList();
                           ViewBag.Name = _context.Users.SingleOrDefault(a => a.UserId == HttpContext.Session.GetInt32("id"));
                           ViewBag.Id = HttpContext.Session.GetInt32("id");
                           return View("Index");
                       }
                       if(thisAct.StartTime < act.StartTime && thisAct.StartTime+thisAct.Duration > act.StartTime){
                           ViewBag.SchedError = 1;  //Check to see if any current activities start during your desired activity.
                           ViewBag.AllActivities = _context.Activities.Include(b => b.Participants).Include(d =>d.User).Where(a => a.StartTime + a.Duration > DateTime.Now).OrderBy(c=>c.StartTime).ToList();
                           ViewBag.Name = _context.Users.SingleOrDefault(a => a.UserId == HttpContext.Session.GetInt32("id"));
                           ViewBag.Id = HttpContext.Session.GetInt32("id");
                           return View("Index");
                       }
                   }
               }
           }
           Rsvp newRsvp = new Rsvp {
               UserId = (int)HttpContext.Session.GetInt32("id"),
               ActivityId = num
           };
           _context.Rsvps.Add(newRsvp);
           _context.SaveChanges();
           return RedirectToAction("Index");
       }

       [HttpGet]
       [Route("/unrsvp/{num}")]
       public IActionResult Unrsvp(int num){
           if(HttpContext.Session.GetInt32("id") == null){
                return Redirect("/");
            }
           Activity thisact = _context.Activities.Include(a => a.Participants).SingleOrDefault(a => a.ActivityId == num);
           foreach(Rsvp part in thisact.Participants){
               if(part.UserId == (int)HttpContext.Session.GetInt32("id")){
                   _context.Rsvps.Remove(part);
               }
           }
            _context.SaveChanges();
           return RedirectToAction("Index");
       }

       [HttpGet]
       [Route("/activity/{num}")]
       public IActionResult Activity(int num){
           if(HttpContext.Session.GetInt32("id") == null){
                return Redirect("/");
            }
            ViewBag.Id = HttpContext.Session.GetInt32("id");
           ViewBag.Activity = _context.Activities.Include(c => c.Participants).ThenInclude(a => a.User).Include(b => b.User).SingleOrDefault(d => d.ActivityId == num);
           return View();
       }

       [HttpGet]
       [Route("/new")]
       public IActionResult New(){
           if(HttpContext.Session.GetInt32("id") == null){
                return Redirect("/");
            }
           return View();
       }
    }
}