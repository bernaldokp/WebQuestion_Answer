using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MyWebsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyWebsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyWebs
        public async Task<IActionResult> Index()
        {
            return View(await _context.MyWeb.ToListAsync());
        }

        //Get: Questions/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        //Post: Question/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.MyWeb.Where( q => q.Question.Contains(SearchPhrase)).ToListAsync());
        }
        // GET: MyWebs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myWeb = await _context.MyWeb
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myWeb == null)
            {
                return NotFound();
            }

            return View(myWeb);
        }

        [Authorize]
        // GET: MyWebs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyWebs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] MyWeb myWeb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myWeb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myWeb);
        }

        [Authorize]
        // GET: MyWebs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myWeb = await _context.MyWeb.FindAsync(id);
            if (myWeb == null)
            {
                return NotFound();
            }
            return View(myWeb);
        }

        // POST: MyWebs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] MyWeb myWeb)
        {
            if (id != myWeb.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myWeb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyWebExists(myWeb.Id))
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
            return View(myWeb);
        }

        [Authorize]
        // GET: MyWebs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myWeb = await _context.MyWeb
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myWeb == null)
            {
                return NotFound();
            }

            return View(myWeb);
        }

        // POST: MyWebs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myWeb = await _context.MyWeb.FindAsync(id);
            if (myWeb != null)
            {
                _context.MyWeb.Remove(myWeb);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyWebExists(int id)
        {
            return _context.MyWeb.Any(e => e.Id == id);
        }
    }
}
