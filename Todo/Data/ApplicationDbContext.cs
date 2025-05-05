using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Data.Entities;

namespace Todo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoList>(entity =>
            {
                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasOne(d => d.TodoList)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.TodoListId);
            });
        }

        // Show todo lists where the current user is the owner or responsible for at least one item
        public IQueryable<TodoList> RelevantTodoLists(string userId)
        {
            return TodoLists
                .Include(tl => tl.Items)
                .ThenInclude(i => i.ResponsibleParty)
                .Where(tl =>
                    tl.Owner.Id == userId || 
                    tl.Items.Any(i => i.ResponsiblePartyId == userId)
                );
        }
    }
}
