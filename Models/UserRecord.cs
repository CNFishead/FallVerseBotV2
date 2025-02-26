using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("UserRecord")]
public class UserRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incrementing PK
    public int Id { get; set; }

    [Required]
    public ulong DiscordId { get; set; } // Unique Discord user ID

    [Required]
    public string Username { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow; // Default to now

    // 🔹 Navigation property for one-to-one relationship
    public virtual UserEconomy? Economy { get; set; }
}
