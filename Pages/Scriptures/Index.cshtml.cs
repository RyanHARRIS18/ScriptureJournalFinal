using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScriptureJournal.Data;
using ScriptureJournal.Models;

namespace ScriptureJournal
{
    public class IndexModel : PageModel
    {
        private readonly ScriptureJournal.Data.ScriptureJournalContext _context;

        public IndexModel(ScriptureJournal.Data.ScriptureJournalContext context)
        {
            _context = context;
        }

        public IList<Scripture> Scripture { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        /*SEARCH BY BOOKS LIKE Genres*/

        [BindProperty(SupportsGet = true)]

        public string ScriptureBook { get; set; }
        public string Books { get; set; }

        public SelectList Titles { get; set; }
        /*  public string SortBy { get; set; }*/
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";


            IQueryable<string> titleQuery = from s in _context.Scripture
                                          orderby s.Book
                                          select  s.Book;

            var books = from m in _context.Scripture
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Note.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(ScriptureBook))
            {
                books = books.Where(s => s.Book.Contains(ScriptureBook));
            }

            if (!string.IsNullOrEmpty(sortOrder)){
               switch (sortOrder)
                {
                    case "name_desc":
                        books = books.OrderByDescending(m => m.Book);
                        break;
                    case "Date":
                        books = books.OrderBy(m => m.CreationDate);
                        break;
                    case "date_desc":
                        books = books.OrderByDescending(m => m.CreationDate);
                        break;
                    default:
                        books = books.OrderBy(m => m.Book);
                        break;
                }
            }
            Titles = new SelectList(await titleQuery.Distinct().ToListAsync());

            Scripture = await books.AsNoTracking().ToListAsync();

        }
    }
}
