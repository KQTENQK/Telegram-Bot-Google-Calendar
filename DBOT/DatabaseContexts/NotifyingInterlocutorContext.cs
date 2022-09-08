using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DBOT.DatabaseContexts
{
    /// <summary>
    /// Represents record about user in a database.
    /// </summary>
    [Table("TUsersNotif")]
    public class NotifyingInterlocutor
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("User")]
        public long UId { get; set; }
        [Column("IsNotif")]
        public bool IsNotifying { get; set; }

    }

    public class NotifyingInterlocutorContext : DbContext
    {
        public DbSet<NotifyingInterlocutor> Interlocutors { get; set; }

        public NotifyingInterlocutorContext(string connectionString) : base(connectionString) { }
    }
}
