using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Models;
using SQLite;

namespace Books.Data
{
    public class BookDatabase
    {
        readonly SQLiteAsyncConnection database;

        public BookDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Book>().Wait();
        }

        public Task<List<Book>> GetNotesAsync()
        {
            //Get all books.
            return database.Table<Book>().ToListAsync();
        }

        public Task<Book> GetNoteAsync(int id)
        {
            // Get a specific book.
            return database.Table<Book>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveNoteAsync(Book note)
        {
            if (note.ID != 0)
            {
                // Update an existing note.
                return database.UpdateAsync(note);
            }
            else
            {
                // Save a new note.
                return database.InsertAsync(note);
            }
        }

        public Task<int> DeleteNoteAsync(Book note)
        {
            // Delete a note.
            return database.DeleteAsync(note);
        }
    }
}
