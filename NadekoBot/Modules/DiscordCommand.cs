using Discord.Commands;
using NadekoBot.Modules;

namespace NadekoBot.Classes
{

       internal override void Init(CommandGroupBuilder cgb)
        {
            cgb.CreateCommand(Module.Prefix + "aar")
                .Alias(Module.Prefix + "autoassignrole")
                .Description($"Automaticaly assigns a specified role to every user who joins the server. Type `.aar` to disable, `.aar Role Name` to enable")
                .Parameter("role", ParameterType.Unparsed)
                .AddCheck(new SimpleCheckers.ManageRoles())
                .Do(async e =>
                {
                    if (!e.Server.CurrentUser.ServerPermissions.ManageRoles)
                    {
                        await e.Channel.SendMessage("I do not have the permission to manage roles.");
                    }
                    var r = e.GetArg("role")?.Trim();

                    var config = SpecificConfigurations.Default.Of(e.Server.Id);

                    if (string.IsNullOrWhiteSpace(r)) //if role is not specified, disable
                    {
                        config.AutoAssignedRole = 0;

                        await e.Channel.SendMessage("`Auto assign role on user join is now disabled.`");
                        return;
                    }
                    var role = e.Server.FindRoles(r).FirstOrDefault();

                    if (role == null)
                    {
                        await e.Channel.SendMessage("💢 `Role not found.`");
                        return;
                    }

                    config.AutoAssignedRole = role.Id;
                    await e.Channel.SendMessage("`Auto assigned role is set.`");

                });
        }
    /// <summary>
    /// Base DiscordCommand Class.
    /// Inherit this class to create your own command.
    /// </summary>
    internal abstract class DiscordCommand
    {

        /// <summary>
        /// Parent module
        /// </summary>
        protected DiscordModule Module { get; }

        /// <summary>
        /// Creates a new instance of discord command,
        /// use ": base(module)" in the derived class'
        /// constructor to make sure module is assigned
        /// </summary>
        /// <param name="module">Module this command resides in</param>
        protected DiscordCommand(DiscordModule module)
        {
            this.Module = module;
        }

        /// <summary>
        /// Initializes the CommandBuilder with values using CommandGroupBuilder
        /// </summary>
        internal abstract void Init(CommandGroupBuilder cgb);
    }
}
