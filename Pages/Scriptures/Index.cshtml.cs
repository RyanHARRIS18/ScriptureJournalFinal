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
        public SelectList Books { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AllBooks { get; set; }
        /*  public string SortBy { get; set; }*/

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }


        public async Task OnGetAsync(string sortOrder)
        {
            IQueryable<string> bookQuery = from m in _context.Scripture
                                           orderby m.Book
                                            select m.Book;

            var scripture = from m in _context.Scripture
                            select m;


            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    scripture = scripture.OrderByDescending(m => m.Book);
                    break;
                case "Date":
                    scripture = scripture.OrderBy(m => m.CreationDate);
                    break;
                case "date_desc":
                    scripture = scripture.OrderByDescending(m => m.CreationDate);
                    break;
                default:
                    scripture = scripture.OrderBy(m => m.Book);
                    break;
            }

            if (!string.IsNullOrEmpty(NameSort))
            {
                scripture = scripture.Where(s => s.Note.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                scripture = scripture.Where(s => s.Note.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(AllBooks))
            {
                scripture = scripture.Where(x => x.Book == AllBooks);
            }
            Scripture = await scripture.ToListAsync();

            Books = new SelectList(await bookQuery.Distinct().ToListAsync());

        }
    }
}
