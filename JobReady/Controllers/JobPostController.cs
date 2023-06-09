﻿using JobReady.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobReady;
using System.Transactions;
using System;
using Microsoft.AspNetCore.Authorization;

namespace JobReady.Controllers
{
    [Authorize]
    public class JobPostController : Controller
    {

        private readonly JobReadyContext context;
        private static HashSet<long> jobSkills = new ();

        public JobPostController(JobReadyContext context)
        {
            this.context = context;
        }
        public IActionResult Create()
        {
            var user = (from x in context.UserAccount
                        where x.Id == this.User.Claims.First().Value
                        select new UserAccountDetails()
                        {
                            Id = x.Id,
                            Username = x.UserName,
                            Headline = x.Headline,
                            Type = x.AccountType == UserAccountType.Company ? "company" :
                                          x.AccountType == UserAccountType.Student ? "student" :
                                          x.AccountType == UserAccountType.Instructor ? "instructor" :
                                          "admin",
                        }).FirstOrDefault();
            if (user.AccountType != UserAccountType.Company)
            {
                return RedirectToAction("Index", "Home");
            }
            var userType = (from x in context.UserAccount
                           where x.Id == this.User.Claims.First().Value
                           select x.AccountType).FirstOrDefault();
            ViewData["User"] = userType;
            jobSkills.Clear();
            return View(new JobPostDetails() { Skills = GetSkills(), CreatedBy = user});
        }

        [HttpGet]
        public IEnumerable<SkillDetails> GetSkills()
        {
            var skills = (from x in context.Skill
                          select new SkillDetails()
                          {
                              Id = x.Id,
                              Name = x.Name,
                          }).AsEnumerable();
            return skills;
        }

        [HttpPost]
        public IActionResult AddSkill([FromBody] long skillId)
        {
                jobSkills.Add(skillId);
                return Ok(true);
        }

        [HttpPost]
        public IActionResult FindSkill([FromBody] long skillId)
        {
           bool found = jobSkills.Any(t => t == skillId);
            return Ok(found);
        }

        [HttpPost]
        public IActionResult Post(JobPostDetails details)
        {
            string error  = details.Validate();
            if (ModelState.IsValid && error == null)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var newJobPost = new JobPost
                    {
                        Description = details.Description,
                        Title = details.Title,
                        JobType = details.JobType,
                        IsActive = true,
                        IsRemote = details.IsRemote,
                        CreatedById = this.User.Claims.First().Value,
                        CreatedOn = DateTime.Now,
                    };

                    context.JobPost.Add(newJobPost);
                    context.SaveChanges();

                    foreach(var skill in jobSkills)
                    {
                        var newSkill = new JobSkill()
                        {
                            JobPostId = newJobPost.Id,
                            SkillId = skill,
                        };
                        context.JobSkill.Add(newSkill);
                        context.SaveChanges();
                    }
                scope.Complete();
                }
                jobSkills.Clear();
                return RedirectToAction("Index", "Home");
            }
            TempData["Error"] = error;
            jobSkills.Clear();
            details.CreatedBy =(from x in context.Users
                                where x.UserName == this.User.Identity.Name
                                select new UserAccountDetails()
                                {
                                    Id = x.Id,
                                    Username = x.UserName,
                                    Headline = x.Headline,
                                }).FirstOrDefault();
            details.Skills = GetSkills();
            return View(details);
        }
    }
}
