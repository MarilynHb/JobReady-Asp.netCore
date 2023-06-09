﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobReady;
using Microsoft.AspNetCore.Authorization;

namespace JobReady.Controllers
{
    [Authorize]
    public class IndustryController : Controller
    {
        private readonly JobReadyContext context;

        public IndustryController(JobReadyContext context)
        {
            this.context = context;
        }

        private bool IsAdmin()
        {
            var userType = (from x in context.Users
                            where x.Id == this.User.Claims.First().Value
                            select x.AccountType).FirstOrDefault();
            return (userType == UserAccountType.Admin);
        }
        // GET: Industry
        public async Task<IActionResult> Index()
        {
              if(!IsAdmin()) return RedirectToAction("Index", "Home");
              return View(await context.Industry.ToListAsync());
        }

        // GET: Industry/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (id == null || context.Industry == null)
            {
                return NotFound();
            }

            var industry = await context.Industry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (industry == null)
            {
                return NotFound();
            }

            return View(industry);
        }

        // GET: Industry/Create
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Industry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Industry industry)
        {
            if (ModelState.IsValid)
            {
                context.Add(industry);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(industry);
        }

        // GET: Industry/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
              if(!IsAdmin()) return RedirectToAction("Index", "Home");
            if (id == null || context.Industry == null)
            {
                return NotFound();
            }

            var industry = await context.Industry.FindAsync(id);
            if (industry == null)
            {
                return NotFound();
            }
            return View(industry);
        }

        // POST: Industry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Industry industry)
        {
            if (id != industry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(industry);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndustryExists(industry.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(industry);
        }

        // GET: Industry/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if(!IsAdmin()) return RedirectToAction("Index", "Home");
            if (id == null || context.Industry == null)
            {
                return NotFound();
            }

            var industry = await context.Industry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (industry == null)
            {
                return NotFound();
            }

            return View(industry);
        }

        // POST: Industry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (context.Industry == null)
            {
                return Problem("Entity set 'JobReadyContext.Industries'  is null.");
            }
            var industry = await context.Industry.FindAsync(id);
            if (industry != null)
            {
                context.Industry.Remove(industry);
            }
            
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndustryExists(long id)
        {
          return context.Industry.Any(e => e.Id == id);
        }
    }
}
