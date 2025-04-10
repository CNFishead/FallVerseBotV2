using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FallVerseBotV2.Commands.Economy
{
  public class BalanceCommand : BaseEconomyModule
  {
    public BalanceCommand(ILogger<BaseEconomyModule> logger, BotDbContext db) : base(logger, db) { }

    [SlashCommand("balance", "Check your balance for a specific currency or all.")]
    public async Task CheckBalance(string? currencyName = null)
    {
      try
      {
        await DeferAsync(true);
        var guildId = Context.Guild.Id;
        var userId = Context.User.Id;
        var username = Context.User.Username;

        var userRecord = await Db.Users.FirstOrDefaultAsync(u => u.DiscordId == userId);
        if (userRecord == null)
        {
          await FollowupAsync("❌ You don't have a user record yet. Try using `/daily` first.");
          return;
        }

        // If a specific currency is requested
        if (!string.IsNullOrWhiteSpace(currencyName))
        {
          var currency = await Db.CurrencyTypes
              .FirstOrDefaultAsync(c =>
                  c.GuildId == guildId &&
                  c.Name.ToLower() == currencyName.ToLower());

          if (currency == null)
          {
            await FollowupAsync($"❌ Currency `{currencyName}` does not exist in this server.");
            return;
          }

          var balance = await Db.CurrencyBalances
              .FirstOrDefaultAsync(b =>
                  b.UserId == userRecord.Id &&
                  b.CurrencyTypeId == currency.Id &&
                  b.GuildId == guildId);

          int amount = balance?.Amount ?? 0;
          string formatted = amount.ToString("N0", CultureInfo.InvariantCulture);

          var embed = new EmbedBuilder()
              .WithTitle($"💰 {currency.Name} Balance")
              .WithDescription($"{username}, you currently have **{formatted} {currency.Name}**.")
              .WithColor(Color.Teal)
              .WithTimestamp(DateTime.UtcNow)
              .Build();

          await FollowupAsync(embed: embed);
          return;
        }

        // Otherwise: show all balances for this user in this server
        var balances = await Db.CurrencyBalances
            .Include(b => b.CurrencyType)
            .Where(b => b.UserId == userRecord.Id && b.GuildId == guildId)
            .ToListAsync();

        if (!balances.Any())
        {
          await FollowupAsync("ℹ️ You have no currency balances in this server.");
          return;
        }

        var embedAll = new EmbedBuilder()
            .WithTitle("💼 Your Balances")
            .WithColor(Color.Teal)
            .WithTimestamp(DateTime.UtcNow);

        foreach (var b in balances)
        {
          string formatted = b.Amount.ToString("N0", CultureInfo.InvariantCulture);
          embedAll.AddField(b.CurrencyType.Name, formatted, inline: true);
        }

        await FollowupAsync(embed: embedAll.Build());
      }
      catch (System.Exception ex)
      {
        Logger.LogError(ex, "Error in BalanceCommand: {Message}", ex.Message);
        await FollowupAsync("❌ Something went wrong while processing your request.");
        throw; // Rethrow the exception to be handled by the global exception handler
      }
    }
  }
}
