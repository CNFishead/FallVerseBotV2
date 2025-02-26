using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("UserEconomy")]
public class UserEconomy
{
    [Key] // Primary Key
    [ForeignKey(nameof(User))] // Foreign Key linking to UserRecord.Id
    public int UserId { get; set; }

    public int CurrencyAmount { get; set; } = 0;
    public DateTime? LastClaimed { get; set; }
    public int StreakCount { get; set; } = 0;

    // 🔹 Navigation property (required means UserEconomy cannot exist without UserRecord)
    public virtual required UserRecord User { get; set; }
}
