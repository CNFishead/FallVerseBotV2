using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;

namespace FallVerseBotV2.Commands.Economy
{
  public class DailyCommand : BaseEconomyModule
  {
    private static readonly Random _rng = new();

    public DailyCommand(ILogger<BaseEconomyModule> logger, BotDbContext db) : base(logger, db) { }

    [SlashCommand("daily", "Claim your daily reward!")]
    public async Task ExecuteAsync()
    {
      try
      {

        await DeferAsync();

        Logger.LogInformation("DailyCommand fired.");
        var discordId = Context.User.Id;
        var username = Context.User.Username;
        var guildId = Context.Guild.Id;

        // Step 0: Get Daily Currency for This Server
        var settings = await Db.ServerSettings
            .Include(s => s.DailyCurrency)
            .FirstOrDefaultAsync(s => s.GuildId == guildId);

        if (settings is null)
        {
          await FollowupAsync("❌ This server has not set a daily currency yet. Use `/setdailycurrency` first.");
          return;
        }

        var dailyCurrency = settings.DailyCurrency;

        // Step 1: Ensure UserRecord Exists
        var userRecord = await Db.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId);
        if (userRecord == null)
        {
          Logger.LogInformation($"Creating new user record for {username} ({discordId})");
          userRecord = new UserRecord { DiscordId = discordId, Username = username };
          Db.Users.Add(userRecord);
          await Db.SaveChangesAsync();
        }

        // Step 2: Ensure UserEconomy Exists (per guild)
        var userEconomy = await Db.Economies
            .FirstOrDefaultAsync(e => e.UserId == userRecord.Id && e.GuildId == guildId);

        if (userEconomy == null)
        {
          Logger.LogInformation($"Creating new economy record for {username} ({discordId})");
          userEconomy = new UserEconomy
          {
            UserId = userRecord.Id,
            GuildId = guildId,
            User = userRecord
          };
          Db.Economies.Add(userEconomy);
          await Db.SaveChangesAsync();
        }

        // Step 3: Check if already claimed today
        var now = DateTime.UtcNow;
        // pattern-match null and compare date in one go
        if (userEconomy.LastClaimed?.Date == now.Date)
        {
          Logger.LogInformation($"User {username} ({discordId}) has already claimed daily today.");

          // Calculate next reset: next UTC midnight
          var nextReset = DateTimeOffset.UtcNow.Date.AddDays(1); // Midnight UTC tomorrow
          var nextResetUnix = ((DateTimeOffset)nextReset).ToUnixTimeSeconds(); // Discord-ready timestamp

          await FollowupAsync($"❌ You've already claimed your daily reward today!\nYou can claim again <t:{nextResetUnix}:F> (**<t:{nextResetUnix}:R>**).");
          return;
        }


        int? daysSinceLastClaim = userEconomy.LastClaimed.HasValue
            ? (now.Date - userEconomy.LastClaimed.Value.Date).Days
            : (int?)null;

        // increment or reset streak in a single expression
        userEconomy.StreakCount = daysSinceLastClaim == 1
            ? userEconomy.StreakCount + 1
            : 1;


        // Step 5: Calculate reward
        int baseAmount = 25;
        double streakMultiplier = GetStreakMultiplier(userEconomy.StreakCount);
        double bonusMultiplier = 1 + (_rng.NextDouble() * 0.2);
        int reward = (int)(baseAmount * streakMultiplier * bonusMultiplier);

        // Step 6: Get/create balance for user + currency + guild
        var balance = await Db.CurrencyBalances
            .FirstOrDefaultAsync(b =>
                b.UserId == userRecord.Id &&
                b.CurrencyTypeId == dailyCurrency.Id &&
                b.GuildId == guildId);

        if (balance == null)
        {
          balance = new UserCurrencyBalance
          {
            UserId = userRecord.Id,
            CurrencyTypeId = dailyCurrency.Id,
            GuildId = guildId,
            Amount = 0
          };
          Db.CurrencyBalances.Add(balance);
        }

        balance.Amount += reward;
        userEconomy.LastClaimed = now;

        await Db.SaveChangesAsync();

        // Step 7: Respond
        var embed = new EmbedBuilder()
            .WithTitle("💰 Daily Reward Claimed!")
            .WithDescription($"You received **{reward} {dailyCurrency.Name}**!")
            .AddField("📈 Streak", $"{userEconomy.StreakCount} days", true)
            .AddField("💵 Total Balance", $"{balance.Amount} {dailyCurrency.Name}", true)
            .AddField("🏆 Streak Tier", GetStreakLabel(userEconomy.StreakCount), true)
            .WithColor(Color.Gold)
            .WithTimestamp(now)
            .Build();

        await FollowupAsync(embed: embed);
      }
      catch (System.Exception ex)
      {
        {
          Logger.LogError(ex, "Error in DailyCommand: {Message}", ex.Message);
          await FollowupAsync("❌ Something went wrong while processing your request.");
          throw; // Rethrow the exception to be handled by the global exception handler
        }
      }
    }

    /// <summary>
    /// Calculates a multiplier based on the current streak length.
    /// The multiplier grows logarithmically with the streak, scaled by
    /// 0.6 and capped at 5.0 to prevent runaway rewards.
    /// </summary>
    /// <param name="streak">The number of consecutive days the user has claimed.</param>
    /// <returns>A value between 1.0 and 5.0 used to scale the base reward.</returns>
    private double GetStreakMultiplier(int streak)
    {
      return Math.Min(1.0 + Math.Log10(streak + 1) * 0.6, 5.0);
    }
    private static string GetStreakLabel(int streak) => streak switch
    {
        >= 1500 => "♾️ Infinite",
        >= 750  => "👑 Immortal",
        >= 365  => "🔥 Eternal",
        >= 180  => "💎 Legendary",
        >= 90   => "🌟 Veteran",
        >= 30   => "💪 Committed",
        >= 7    => "📈 Consistent",
        _       => "🌱 Getting Started",
    };

  }

}
