using Discord;
using Discord.Interactions;
using FallVerseBotV2.Commands.Admin;

[Group("admin", "admin-related commands that help facilitate the bots usuage on the server")]
public class AdminModule : BaseGroupModule
{
  private readonly ILoggerFactory _loggerFactory;

  public AdminModule(ILogger<BaseGroupModule> logger, ILoggerFactory loggerFactory, BotDbContext db)
      : base(logger, db)
  {
    _loggerFactory = loggerFactory;
    //register handlers here
    _setJoinRoleHandler = new SetJoinRoleHandler(loggerFactory.CreateLogger<SetJoinRoleHandler>(), db);
    _welcomeHandler = new SetWelcomeMessageHandler(loggerFactory.CreateLogger<SetWelcomeMessageHandler>(), db);
    _restrictHandler = new RestrictCommandHandler(loggerFactory.CreateLogger<RestrictCommandHandler>(), db);

  }

  //instantiate modules here
  private readonly SetJoinRoleHandler _setJoinRoleHandler;
  private readonly SetWelcomeMessageHandler _welcomeHandler;
  private readonly RestrictCommandHandler _restrictHandler;


  [SlashCommand("add-join-role", "Set a role to be given to new users when they join.")]
  public async Task AddJoinRole(IRole role) => await _setJoinRoleHandler.Add(Context, role);

  [SlashCommand("remove-join-role", "Remove a role from the default join roles.")]
  public async Task RemoveJoinRole(IRole role) => await _setJoinRoleHandler.Remove(Context, role);

  [SlashCommand("list-join-roles", "List all default roles given to users on join.")]
  public async Task ListJoinRoles() => await _setJoinRoleHandler.List(Context);


  // ─────────── Welcome Message Commands ───────────
  [SlashCommand("set-welcome-message", "Set the welcome message template shown when users join.")]
  public async Task SetWelcomeMessage(string message) => await _welcomeHandler.SetMessage(Context, message);

  [SlashCommand("set-welcome-channel", "Set the channel to send the welcome message in.")]
  public async Task SetWelcomeChannel(ITextChannel channel) => await _welcomeHandler.SetChannel(Context, channel);

  [SlashCommand("show-welcome-settings", "Show the current welcome message & channel settings.")]
  public async Task ShowWelcomeSettings() => await _welcomeHandler.Show(Context);

  [SlashCommand("set-manual-hello", "Sets the message used by the welcome command.")]
  public async Task SetManualWelcomeMessage(string message) => await _welcomeHandler.SetManualMessage(Context, message);

  // ─────────── Misc Admin Commands ───────────
  [SlashCommand("restrict-command", "Restrict a slash command to a specific role.")]
  public async Task RestrictCommand(string commandName, IRole role)
    => await _restrictHandler.Restrict(Context, commandName.ToLowerInvariant(), role);

  [SlashCommand("unrestrict-command", "Remove a role restriction from a command.")]
  public async Task UnrestrictCommand(string commandName, IRole role)
      => await _restrictHandler.Unrestrict(Context, commandName.ToLowerInvariant(), role);

  [SlashCommand("list-restrictions", "Show which commands are currently role-restricted.")]
  public async Task ListCommandRestrictions()
      => await _restrictHandler.List(Context);

}
