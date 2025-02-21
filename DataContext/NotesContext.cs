using Microsoft.EntityFrameworkCore;
using NotesApi.Models;

namespace NotesApi.Data
{
    public class NotesContext : DbContext
    {
        public NotesContext(DbContextOptions<NotesContext> options) : base(options)
        {
        }

        // DbSet representing the Notes table
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; } // ✅ Add this line
    }
}
