using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ScriptureJournal.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Data.ScriptureJournalContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<Data.ScriptureJournalContext>>()))
            {
                // Look for any movies.
                if (context.Scripture.Any())
                {
                    return;   // DB has been seeded
                }

        context.Scripture.AddRange(
                    new Scripture
                    {
                        Book = "Nephi",
                        Chapter = "13",
                        Verse = "3",
                        CreationDate = DateTime.Parse("1989-2-12"),
                        Note = "note1"
                    },

                    new Scripture
                    {
                        Book = "Nephi",
                        Verse = "3:3",
                        CreationDate = DateTime.Parse("1989-2-12"),
                        Note = "note2"
                    },

                    new Scripture
                    {
                        Book = "Nephi",
                        Chapter = "1",
                        Verse = "1-3",
                        CreationDate = DateTime.Parse("1989-2-12"),
                        Note = "note3"
                    },

                    new Scripture
                    {
                        Book = "Nephi",
                        Chapter = "13",
                        Verse = "13-31",
                        CreationDate = DateTime.Parse("1989-2-12"),
                        Note = "note4"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}