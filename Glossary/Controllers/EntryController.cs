using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glossary.Models;
using System.Web;

namespace Glossary.Controllers
{
    public class EntryController : Controller
    {
        private GlossaryContext _context;

        /// <summary>
        /// Create an instance of the Entry Controller.
        /// </summary>
        public EntryController()
        {
            _context = new GlossaryContext();
        }


        /// <summary>
        /// Create an instance of the Entry Controller.
        /// </summary>
        /// <param name="context">Database set for Entity Framework</param>
        public EntryController(GlossaryContext context)
        {
            _context = context as GlossaryContext;
        }


        /// <summary>
        /// Release memory resources used by the database context.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        /// <summary>
        /// GET: /Entry/Index/ displays the glossary terms sorted alphabetically or reverse alphabetical order.
        /// </summary>
        /// <param name="sortBy">(optional) Indicates whether to sort terms alphabetically or reversed.</param>
        [HandleError]
        public ActionResult Index(string sortBy)
        {
            // Sort terms alphabetically by default unless otherwise specified to order in reverse. 
            if (String.IsNullOrWhiteSpace(sortBy) || sortBy != Sort.SORT_DESCENDING)
            {
                sortBy = Sort.SORT_ASCENDING;
            }

            var query = from entry in _context.Entries
                        select entry;

            // Sort terms as indicated and flip the sort option in the view to allow user the switch back and forth.
            if (sortBy == Sort.SORT_ASCENDING)
            {
                query = query.OrderBy(e => e.Term);
                ViewBag.NameSortParm = Sort.SORT_DESCENDING;
            }
            else if (sortBy == Sort.SORT_DESCENDING)
            {
                query = query.OrderByDescending(e => e.Term);
                ViewBag.NameSortParm = Sort.SORT_ASCENDING;
            }

            return View(query.ToList());
        }


        /// <summary>
        /// GET: /Entry/New/ displays an empty form to create a new entry in the Glossary.
        /// </summary>
        [HandleError]
        public ActionResult New()
        {
            Entry newEntry = new Entry()
            {
                Id = 0,
                Term = "",
                Definition = ""
            };

            return View("EntryForm", newEntry);
        }


        /// <summary>
        /// GET: /Entry/Edit/ displays a form to update an existing entry in the Glossary.
        /// </summary>
        /// <param name="id">Id of the entry to be edited.</param>
        /// <exception cref="HttpException">Thrown when an entry cannot be found for the id</exception>
        [HandleError]
        public ActionResult Edit(int? id)
        {
            // Validate the provided Id. 
            if (!id.HasValue)
            {
                throw new HttpException(404, "Not found");
            }

            Entry entry = _context.Entries.SingleOrDefault(c => c.Id == id);

            // Validate that an entry was found with the provided Id.
            if (entry == null)
            {
                throw new HttpException(404, "Not found");
            }

            return View("EntryForm", entry);
        }


        /// <summary>
        /// POST: /Entry/Save/ creates a new entry or saves changes to an existing entry.
        /// </summary>
        /// <param name="entry">Entry to create or save changes to.</param>
        /// <exception cref="HttpException">Thrown when the entry cannot be found again</exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Save(Entry entry)
        {
            // Stay on the form if validation logic (from annotations) fails.
            if (!ModelState.IsValid)
            {
                return View("EntryForm", entry);
            }

            // Add new entry to database or save changes to an existing entry.
            if (entry.Id == Entry.UNASSIGNED_ID)
            {
                _context.Entries.Add(entry);
            }
            else
            {
                var existingEntity = _context.Entries.SingleOrDefault(c => c.Id == entry.Id);

                // Validate that an entry was found with the provided Id.
                if (existingEntity == null)
                {
                    throw new HttpException(404, "Not found");
                }

                // Only update properties for fields displayed to the user on the form.
                existingEntity.Term = entry.Term;
                existingEntity.Definition = entry.Definition; 
            }

            _context.SaveChanges();

            // Redirect to the main Glossary page displaying the entries.
            return RedirectToAction("Index", "Entry");
        }


        /// <summary>
        /// GET: /Entry/Delete/ deletes an existing entry from the Glossary.
        /// </summary>
        /// <param name="id">Id of the entry to be deleted.</param>
        /// <exception cref="HttpException">Thrown when an entry cannot be found for the id</exception>
        [HandleError]
        public ActionResult Delete(int? id)
        {
            // Validate the provided Id.
            if (!id.HasValue)
            {
                throw new HttpException(404, "Not found");
            }

            Entry entry = _context.Entries.SingleOrDefault(c => c.Id == id);

            // Validate that an entry was found with the provided Id.
            if (entry == null)
            {
                throw new HttpException(404, "Not found");
            }

            _context.Entries.Remove(entry);
            _context.SaveChanges();

            // Redirect to the main Glossary page displaying remaining entries.
            return RedirectToAction("Index", "Entry");
        }

    }


    public sealed class Sort
    {
        public const string SORT_ASCENDING = "ASC";
        public const string SORT_DESCENDING = "DESC";
    }

}