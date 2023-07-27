using Discord.Interactions;

namespace Bihyung.Commands;

[Group("webcomic", "Create or modify webcomic notifications")]
public class WebcomicModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("info", "Get information about a configured webcomic")]
    public Task InfoAsync()
    {
        // TypeReader pulls Webcomic from db
        // Get total polls amount
        // Get last poll timestamp
        // Link to notification thread

        return Task.CompletedTask;
    }

    [SlashCommand("add", "Add a new webcomic notifier")]
    public Task AddAsync()
    {
        // Ask to specify a website
        // Ask for the comic's name
        // Scrape site's search page and show results
        // 

        return Task.CompletedTask;
    }

    [SlashCommand("remove", "Remove an existing webcomic notifier")]
    public Task RemoveAsync()
    {
        // Confirm user selection is correct
        // Remove notification subscribers, not the webcomic's base model.

        return Task.CompletedTask;
    }

    [SlashCommand("force", "Force an update check for a specific webcomic")]
    public Task ForceAsync()
    {
        return Task.CompletedTask;
    }
}
