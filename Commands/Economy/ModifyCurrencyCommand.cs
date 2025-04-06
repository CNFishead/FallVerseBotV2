using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace FallVerseBotV2.Commands.Economy
{
  public class ModifyCurrencyCommand : BaseEconomyModule
  {
    public ModifyCurrencyCommand(ILogger<BaseEconomyModule> logger, BotDbContext db) : base(logger, db) { }

    [SlashCommand("modifycurrency", "Modify a user's currency of a specific type.")]
    public async Task ModifyCurrency(SocketUser user, string currencyName, int amount)
    {
      var guildId = Context.Guild.Id;
      var discordId = user.Id;
      var username = user.Username;

      // Fetch user record
      var userRecord = await Db.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId);
      if (userRecord == null)
      {
        await RespondAsync("❌ That user has no record in the database.");
        return;
      }

      // Look up currency type scoped to this guild
      var currencyType = await Db.CurrencyTypes
          .FirstOrDefaultAsync(c =>
              c.GuildId == guildId &&
              c.Name.ToLower() == currencyName.ToLower());

      if (currencyType == null)
      {
        await RespondAsync($"❌ Currency type `{currencyName}` does not exist in this server.");
        return;
      }

      // Look up or create user balance
      var balance = await Db.CurrencyBalances
          .FirstOrDefaultAsync(b =>
              b.UserId == userRecord.Id &&
              b.CurrencyTypeId == currencyType.Id &&
              b.GuildId == guildId);

      if (balance == null)
      {
        balance = new UserCurrencyBalance
        {
          UserId = userRecord.Id,
          CurrencyTypeId = currencyType.Id,
          GuildId = guildId,
          Amount = 0
        };
        Db.CurrencyBalances.Add(balance);
      }

      // Prevent negative balances
      if (balance.Amount + amount < 0)
      {
        await RespondAsync("❌ This operation would result in a negative balance.");
        return;
      }

      balance.Amount += amount;
      await Db.SaveChangesAsync();
      await RespondAsync($"✅ {user.Mention} now has {balance.Amount:N0} {currencyType.Name}.");

    }
  }
}
